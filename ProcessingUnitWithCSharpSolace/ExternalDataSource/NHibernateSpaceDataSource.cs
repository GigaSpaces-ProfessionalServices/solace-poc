using GigaSpaces.Core;
using GigaSpaces.Core.Metadata;
using GigaSpaces.Core.Persistency;
using GigaSpaces.XAP.ProcessingUnit.Containers;
using NHibernate;
using NHibernate.Criterion;
using NHibernate.Metadata;
using NHibernate.Persister.Entity;
using System.Reflection;
using System.Xml.Linq;
using NHibernateCfg = NHibernate.Cfg;

namespace CustomExternalDataSource.ExternalDataSource
{
    /// <summary>
    /// GigaSpaces implementation of IExternalDataSource using NHibernate
    /// </summary>
    public class NHibernateSpaceDataSource : AbstractExternalDataSource
    {
        public const string NHibernateConfigProperty = "nhibernate-config-file";
        public const string NHibernateHbmDirectory = "nhibernate-hbm-dir";
        public const string NHibernateConnectionStringProperty = "nhibernate-connection-string";

        /// <summary>
        /// Create a new NHibernate External Data Source
        /// </summary>
        public NHibernateSpaceDataSource()
        {
        }

        /// <summary>
        /// Create a new NHibernate External Data Source
        /// </summary>
        /// <param name="sessionFactory">NHibernate Session Factory that will be used</param>
        public NHibernateSpaceDataSource(ISessionFactory sessionFactory)
        {
            SessionFactory = sessionFactory;
        }

        /// <summary>
        /// Gets the session factory used to interoperate with nHibernate.
        /// </summary>
        public ISessionFactory SessionFactory { get; protected set; }

        /// <summary>
        /// Gets or sets the InitialLoad operation thread pool size. The InitialLoad operation uses the <see cref="ConcurrentMultiDataEnumerator"/>.
        /// This property allows to control the thread pool size of the concurrent multi data iterator. Defaults to
        /// 10.
        ///
        /// Note, this usually will map one to one to the number of open connections against the database.
        /// </summary>
        public int InitialLoadThreadPoolSize { get; set; }

        /// <summary>
        /// Gets or sets whether to perform InitialLoad ordered by id, this flag indicates if the generated query will order the results by
        /// the id. By default it is set to false, in some cases it might result in better initial load performance.
        /// </summary>
        public bool PerformOrderById { get; set; }

        /// <summary>
        /// Gets or sets a list of entries that will be used to perform the InitialLoad operation. By default, will
        /// try and build a sensible list based on NHiberante meta data.
        ///
        /// Note, sometimes an explicit list should be provided. For example, if we have a class A and class B, and
        /// A has a relationship to B which is not component. If in the space, we only wish to have A, and have B just
        /// as a field in A (and not as an Entry), then we need to explcitly set the list just to A. By default, if
        /// we won't set it, it will result in two entries existing in the Space, A and B, with A having a field of B
        /// as well.
        /// </summary>
        public string[] InitialLoadEntries { get; set; }

        /// <summary>
        /// Gets or sets the initial load chunk size.
        /// By default, the initial load process will chunk large tables and will iterate over the table (entity) per
        /// chunk (concurrently). This setting allows to control the chunk size to split the table by. By default, set
        /// 100,000. Batching can be disabled by setting -1.
        /// </summary>
        public int InitialLoadChunkSize { get; set; }

        /// <summary>
        /// Gets or sets the enumerator fetch size
        /// </summary>
        public int EnumeratorLoadFetchSize { get; set; }

        /// <summary>
        /// The default behavior when performing initial load is each partition loads the contents of the entire
        /// database, and only the rows that match the Space objects routing key are kept. The rest are discarded.
        /// If set to true, it will attempt to select based on routing key
        /// (routing field % number of partitions) + 1 == partition_id
        /// </summary>
        public bool InitiallLoadWithSpaceRouting { get; set; }

        public string AssemblyFileName { get; set; }
        
        private Assembly? _assembly;

        public int? InstanceId { get; protected set; }

        public int? NumberOfInstances { get; protected set; }

        private static readonly NLog.Logger Logger = NLog.LogManager.GetCurrentClassLogger();


        /// <summary>
        /// Initialize and configure the data source using given properties.
        /// Called when space is started.
        /// </summary>
        /// <param name="properties">Propeties to initialize by.</param>
        public override void Init(Dictionary<string, string> properties)
        {
            base.Init(properties);

            EnumeratorLoadFetchSize = GetIntProperty("EnumeratorLoadFetchSize", 10000);
            if (EnumeratorLoadFetchSize < 1)
                throw new ArgumentException("EnumeratorLoadFetchSize must be a positive number");

            InitialLoadChunkSize = GetIntProperty("InitialLoadChunkSize", 100000);
            if (InitialLoadChunkSize < 1 && InitialLoadChunkSize != -1)
                throw new ArgumentException("InitialLoadChunkSize must be a positive number or -1");

            InitialLoadThreadPoolSize = GetIntProperty("InitialLoadThreadPoolSize", 10);
            if (InitialLoadThreadPoolSize < 1)
                throw new ArgumentException("InitialLoadThreadPoolSize must be a positive number");

            PerformOrderById = GetBoolProperty("PerformOrderById", PerformOrderById);
            //UseMerge = GetBoolProperty("UseMerge", UseMerge);

            Logger.Info("EnumeratorLoadFetchSize is: " + EnumeratorLoadFetchSize);

            Logger.Info("InitialLoadChunkSize is: " + InitialLoadChunkSize);

            Logger.Info("InitialLoadThreadPoolSize is: " + InitialLoadThreadPoolSize);

            Logger.Info("PerformOrderById is: " + PerformOrderById);


            InitiallLoadWithSpaceRouting = GetBoolProperty("InitiallLoadWithSpaceRouting", InitiallLoadWithSpaceRouting);

            Logger.Info("InitiallLoadWithSpaceRouting is: " + InitiallLoadWithSpaceRouting);

            AssemblyFileName = GetFileProperty("AssemblyFileName");

            Logger.Info("AssemblyFileName is: " + AssemblyFileName);

            if (ProcessingUnitContainer.Current != null)
            {
                if (ProcessingUnitContainer.Current.ClusterInfo != null)
                {
                    InstanceId = ProcessingUnitContainer.Current.ClusterInfo.InstanceId;
                    NumberOfInstances = ProcessingUnitContainer.Current.ClusterInfo.NumberOfInstances;
                    Logger.Info("ProcessingUnitContainer.Current.ClusterInfo.InstanceId : " + ProcessingUnitContainer.Current.ClusterInfo.InstanceId);
                    Logger.Info("ProcessingUnitContainer.Current.ClusterInfo.NumberOfInstances : " + ProcessingUnitContainer.Current.ClusterInfo.NumberOfInstances);
                }
                else
                {
                    Logger.Info("ProcessingUnitContainer.Current.ClusterInfo is null.");
                }
            }
            else
            {
                Logger.Info("ProcessingUnitContainer.Current is null ");
            }
            /*
            if (InitiallLoadWithSpaceRouting == true) {
                if (InstanceId == null || InstanceId == 0 || NumberOfInstances == null || NumberOfInstances == 0 || NumberOfInstances == 1)
                {
                    Logger.Info("The initial load is not being run in a partitioned environment. Setting InitialLoadWithSpaceRouting to false.");
                    InitiallLoadWithSpaceRouting = false;
                }
            }
            */
            // only configure a session factory if it is not injected			
            if (SessionFactory == null)
                SessionFactory = CreateSessionFactory();
            /*
            // only extract managed entries if it wasn't injected
            if (ManagedEntries == null)
            {
                List<string> managedEntriesList = new List<string>();
                IDictionary<string, IClassMetadata> allClassMetadata = SessionFactory.GetAllClassMetadata();
                foreach (string type in allClassMetadata.Keys)
                    managedEntriesList.Add(type);
                ManagedEntries = managedEntriesList.ToArray();
            }
            //Initialized Managed Entries set to be used later for faster search
            _managedEntriesSet = new HashSet<string>(ManagedEntries);
             */
            // only extract initial load entries if it wasn't injected
            if (InitialLoadEntries == null)
            {
                List<string> initialLoadEntriesList = new List<string>();
                IDictionary<string, IClassMetadata> allClassMetadata = SessionFactory.GetAllClassMetadata();
                foreach (KeyValuePair<string, IClassMetadata> entry in allClassMetadata)
                {
                    AbstractEntityPersister entityPersister = (AbstractEntityPersister)entry.Value;
                    string mappedSuperClass = entityPersister.MappedSuperclass;
                    if (mappedSuperClass != null)
                    {
                        IClassMetadata superClassMetadata = allClassMetadata[mappedSuperClass];
                        if (superClassMetadata.GetMappedClass(EntityMode.Map) != null)
                        {
                            //Filter out those who have their super classes mapped
                            continue;
                        }
                    }
                    initialLoadEntriesList.Add(entry.Key);
                }
                InitialLoadEntries = initialLoadEntriesList.ToArray();
            }
        }

        /// <summary>
        /// Close the data source and clean any used resources.
        /// Called before space shutdown.
        /// </summary>
        public override void Shutdown()
        {
            try
            {
                if (SessionFactory != null)
                    SessionFactory.Close();
            }
            catch
            {
            }

            base.Shutdown();
        }


        ///<summary>
        ///Create an enumerator over all objects that match the given <see cref="T:GigaSpaces.Core.Persistency.Query" />.		            
        ///</summary>
        ///<param name="query">The Query used for matching.</param>
        ///<returns>
        ///Enumerator over all objects that match the given <see cref="T:GigaSpaces.Core.Persistency.Query" />.
        ///</returns>
        ///
        public override IDataEnumerator GetEnumerator(Query query)
        {
            return new MyIDataEnumerator(query, SessionFactory, EnumeratorLoadFetchSize, PerformOrderById);
        }

        /// <summary>
        /// Creates and returns an enumerator over all the entries that should be loaded into space.
        /// </summary>
        /// <returns>Enumerator over all the entries that should be loaded into space.</returns>
        public override IDataEnumerator InitialLoad()
        {
            // this proxy is used only to determine the space routing field
            ISpaceProxy registerHelperProxy = new EmbeddedSpaceFactory("registerHelper").Create();

            List<IDataEnumerator> enumerators = new List<IDataEnumerator>();
            foreach (string initialLoadEntity in InitialLoadEntries)
            {
                Logger.Info("Processing {0} in InitialLoad", initialLoadEntity);
                InitialLoadWithPartitioning initialLoadWithPartitioning = null;
                if (InitiallLoadWithSpaceRouting == false)
                    initialLoadWithPartitioning = new InitialLoadWithPartitioning(false, "", 0, 0);
                else
                {
                    initialLoadWithPartitioning = augmentInitialLoadQuery(initialLoadEntity, registerHelperProxy);
                }

                if (InitialLoadChunkSize == -1)
                    enumerators.Add(GetEnumerator(initialLoadEntity, 0, int.MaxValue, initialLoadWithPartitioning));
                    
                else
                    enumerators.AddRange(DivideToChunks(initialLoadEntity, initialLoadWithPartitioning));
            }
            registerHelperProxy = null;
            return new ConcurrentMultiDataEnumerator(enumerators, EnumeratorLoadFetchSize, InitialLoadThreadPoolSize);
        }
        /// <summary>
        /// Create an enumerator over all objects that match the given <see cref="T:GigaSpaces.Core.Persistency.Query" />.		            
        /// </summary>
        /// <param name="entityType">The entity type to match.</param>
        /// <param name="from">Base index to load from. If null, loads everything.</param>
        /// <param name="maxResults">Maximum results to return</param>
        /// <returns>
        /// Enumerator over all objects that match the given <see cref="T:GigaSpaces.Core.Persistency.Query" />.
        /// </returns>
        protected virtual IDataEnumerator GetEnumerator(String entityType, int from, int maxResults, InitialLoadWithPartitioning initialLoadWithPartitioning)
        {
            return new MyIDataEnumerator(entityType, SessionFactory, EnumeratorLoadFetchSize, PerformOrderById, from, maxResults, initialLoadWithPartitioning);
        }
        protected virtual IEnumerable<IDataEnumerator> DivideToChunks(string entityType, InitialLoadWithPartitioning initialLoadWithPartitioning)
        {
            if (entityType == null)
                throw new ArgumentException("DivideToChunks must receive an Entity Type");

            NHibernate.ISession session = SessionFactory.OpenSession();
            NHibernate.ITransaction tx = session.BeginTransaction();
            try
            {
                //Get number of rows of the current entity in the data base
                ICriteria criteria = session.CreateCriteria(entityType);
                
                if ( initialLoadWithPartitioning.InitialLoadWithSpaceRouting == true )
                {
                    // not sure if this sql syntax is platform independent - other databases such as Oracle don't have a mod operator. They use a MOD() function
                    string modFilterSql = String.Format("(({0} % {1}) + 1) = {2}",
                        initialLoadWithPartitioning.RoutingPropertyName, initialLoadWithPartitioning.NumberOfInstances, initialLoadWithPartitioning.InstanceId);

                    string msg = "modFilterSql is: " + modFilterSql;
                    Logger.Info(msg);
                    Console.WriteLine(msg);

                    criteria.Add(Expression.Sql(modFilterSql));
                }

                criteria.SetProjection(Projections.RowCount());
                int count = (int)criteria.UniqueResult();
                //Create enumerators for each chunk
                List<IDataEnumerator> enumerators = new List<IDataEnumerator>();
                for (int from = 0; from < count; from += InitialLoadChunkSize)
                    enumerators.Add(GetEnumerator(entityType, from, InitialLoadChunkSize, initialLoadWithPartitioning));
                return enumerators;
            }
            finally
            {
                if (tx != null && tx.IsActive)
                    tx.Commit();
                CloseSession(session);
            }
        }


        private InitialLoadWithPartitioning augmentInitialLoadQuery(string entityType, ISpaceProxy registerHelperProxy)
        {
            
            initAssembly();
            
            Type entryType = getTypeFromTypeName(entityType, _assembly);

            registerHelperProxy.TypeManager.RegisterTypeDescriptor(entryType);

            ISpaceTypeDescriptor spaceTypeDescriptor = registerHelperProxy.TypeManager.GetTypeDescriptor(entryType);

            string routingPropertyName = spaceTypeDescriptor.RoutingPropertyName;

            // the db column name mapped to the routing property name
            var classMetaData = SessionFactory.GetClassMetadata(entryType) as NHibernate.Persister.Entity.AbstractEntityPersister;
            string[] columnNames = classMetaData.GetPropertyColumnNames(routingPropertyName);

            string routingColumnName = columnNames[0];

            PropertyInfo propertyInfo = entryType.GetProperty(routingPropertyName);

            Type propertyType = propertyInfo.PropertyType;
            Type propertyTypeToEval = null;

            if (Nullable.GetUnderlyingType(propertyType) != null)
            {
                propertyTypeToEval = Nullable.GetUnderlyingType(propertyType);
            }
            else
            {
                propertyTypeToEval = propertyType;
            }

            // only handle cases where it is simple to determine the modulo value
            // string is not a good candidate, because we would first need to calculate the hashcode then do a modulus
            if (propertyTypeToEval != typeof(byte) &&
                propertyTypeToEval != typeof(int)  &&
                propertyTypeToEval != typeof(long))
            {
                String s = String.Format("The type: {0} has a space routing property: {1} and is not a numeric type so it cannot be used to augment initial load", entryType, routingPropertyName );
                Logger.Info(s);
                return new InitialLoadWithPartitioning(false, "", 0, 0);
            }

            Console.WriteLine("here");

            //criteria.Add(Restrictions.Eq(routingPropertyName, ));
            /*
            initAssembly();
            
            ISpaceProxy spaceProxy = new EmbeddedSpaceFactory("spaceName").Create();

            Type entryType = getTypeFromTypeName(entityType, _assembly);

            spaceProxy.TypeManager.RegisterTypeDescriptor(entryType);

            ISpaceTypeDescriptor spaceTypeDescriptor = spaceProxy.TypeManager.GetTypeDescriptor(entryType);

            string routingPropertyName = spaceTypeDescriptor.RoutingPropertyName;

             */

            return new InitialLoadWithPartitioning(true, routingColumnName, NumberOfInstances.Value, InstanceId.Value);
        
        }
        /*
        void setSpaceProxyIfNull()
        {
            if (spaceProxy == null)
            {
                SpaceProxyFactory factory = new SpaceProxyFactory(SpaceName);

                try
                {
                    spaceProxy = factory.Create();
                }
                catch (Exception e)
                {
                    Logger.Error(e.Message, e);
                    Console.WriteLine("Space verification failed. Please check & try again...");
                }
            }
        }
        
        private ISpaceTypeDescriptor registerAndGetTypeDescriptor(Type entryType)
        {
            setSpaceProxyIfNull();
            spaceProxy.TypeManager.RegisterTypeDescriptor(entryType);
            return spaceProxy.TypeManager.GetTypeDescriptor(entryType);
        } */
        private Type getTypeFromTypeName(string typeName, Assembly assembly)
        {
            Type type = assembly.GetType(typeName);
            
            return type;
        }
        private void initAssembly()
        {
            if (_assembly == null)
            {
                try
                {
                    if (File.Exists(AssemblyFileName))
                    {
                        _assembly = Assembly.LoadFrom(AssemblyFileName);
                    }
                    else
                    {
                        Logger.Warn(String.Format("Cannot load assembly. AssemblyFileName:{0} is null or not valid.", AssemblyFileName));                        
                    }
                }
                catch (Exception ex)
                {
                    Logger.Warn("Could not load AssemblyFileName: {0}", AssemblyFileName, ex);
                }

                if (_assembly == null)
                {
                    _assembly = Assembly.GetExecutingAssembly();
                }
                Logger.Info("Assembly set to {0}", _assembly.FullName);
            }
        }

        /// <summary>
        /// Closes an NHibernate session
        /// </summary>
        /// <param name="session">Session to close</param>
        private static void CloseSession(NHibernate.ISession session)
        {
            if (session.IsOpen)
                session.Flush();
                session.Close();
        }

        private ISessionFactory CreateSessionFactory()
        {
            try
            {
                var config = new NHibernateCfg.Configuration();
                string configFile = null;
                string hbmFolder = null;
                try
                {
                    configFile = GetFileProperty(NHibernateConfigProperty);
                }
                catch (NullReferenceException nre)
                {
                    Logger.Warn("NullReferenceException when getting hibernate config file.", nre);
                }

                if (configFile == null)
                    config = config.Configure();
                else
                {
                    configFile = Path.GetFullPath(Environment.ExpandEnvironmentVariables(configFile));
                    config = config.Configure(configFile);
                }

                try
                {
                    hbmFolder = GetFileProperty(NHibernateHbmDirectory);
                } catch(NullReferenceException nre)
                {
                    Logger.Warn("NullReferenceException when getting hibernate hbm directory.", nre);
                }
                if (!String.IsNullOrEmpty(hbmFolder))
                {
                    hbmFolder = Path.GetFullPath(Environment.ExpandEnvironmentVariables(hbmFolder));
                    config.AddDirectory(new DirectoryInfo(hbmFolder));
                }

                var connectionString = GetProperty(NHibernateConnectionStringProperty);
                if (!String.IsNullOrEmpty(connectionString))
                    config.SetProperty(NHibernateCfg.Environment.ConnectionString, connectionString);

                return config.BuildSessionFactory();
            }
            catch (Exception e)
            {
                string msg = "CreateSessionFactory404 exception1" + e.ToString();
                Console.WriteLine(msg);
                Logger.Warn(msg);
                throw new Exception("Error creating NHibernate Session Factory", e);
            }
        }

        
        public override void ExecuteBulk(IList<BulkItem> bulk)
        {
            throw new NotImplementedException();
        }
        

    }
}