using GigaSpaces.Core.Persistency;

using System.Collections;
using NHibernate;
using NHibernate.Proxy;
using NHibernate.Metadata;
using NLog;
using NHibernate.Criterion;

namespace CustomExternalDataSource.ExternalDataSource
{
    internal class MyIDataEnumerator : IDataEnumerator
    {
        private readonly Query _persistencyQuery;
        private readonly string _entityType;
        private readonly ISessionFactory _sessionFactory;
        private readonly int _fetchSize;
        private readonly bool _performOrderById;
        private readonly int _offset;
        private readonly int _chunkSize;

        private IQuery _query;
        private NHibernate.ISession _session;
        private ITransaction _tx;
        private ICriteria _criteria;
        private IEnumerator _currentEnumerator;

        private int _currentFetchPosition;
        private object _current;
        private bool _isExhausted;
        private int _totalEnumeratedObject;

        private InitialLoadWithPartitioning _initialLoadWithPartitioning;

        private static readonly NLog.Logger Logger = NLog.LogManager.GetCurrentClassLogger();

        // doesn't appear to be used in initialload, so initial load with partitioning does not apply
        public MyIDataEnumerator(Query persistencyQuery, ISessionFactory sessionFactory, int fetchSize, bool performOrderById)
            : this((string)null, sessionFactory, fetchSize, performOrderById, new InitialLoadWithPartitioning(false, "", 0, 0))
        {
            _persistencyQuery = persistencyQuery;
            
        }

        public MyIDataEnumerator(string entityType, ISessionFactory sessionFactory, int fetchSize, bool performOrderById, InitialLoadWithPartitioning initialLoadWithPartitioning
            )
            : this(entityType, sessionFactory, fetchSize, performOrderById, 0, int.MaxValue, initialLoadWithPartitioning)
        {
        }

        public MyIDataEnumerator(string entityType, ISessionFactory sessionFactory, int fetchSize, bool performOrderById,
            int offset, int chunkSize, InitialLoadWithPartitioning initalLoadWithPartitioning)
        {
            _entityType = entityType;
            _sessionFactory = sessionFactory;
            _performOrderById = performOrderById;
            _fetchSize = fetchSize;
            _offset = offset;
            _chunkSize = chunkSize;
            _initialLoadWithPartitioning = initalLoadWithPartitioning;
        }

        public void Dispose()
        {
            if (_session != null && _session.IsOpen)
            {
                try
                {
                    if (_tx != null && _tx.IsActive)
                        _tx.Commit();
                }
                catch (Exception e)
                {
                    Console.WriteLine("Dispose Exception 69 " + e.ToString());
                }
                finally
                {
                    _session.Flush();
                    _session.Close();
                }
            }
        }

        public bool MoveNext()
        {
            return AssignNextElement();
        }

        public void Reset()
        {
            throw new NotImplementedException();
        }

        public object Current
        {
            get
            {
                if (_current is INHibernateProxy)
                {
                    INHibernateProxy proxy = _current as INHibernateProxy;
                    _current = proxy.HibernateLazyInitializer.GetImplementation();
                }
                return _current;
            }
        }

        /// <summary>
        /// Initializes the session that will be used to enumerate
        /// </summary>
        private void InitSession()
        {
            _session = _sessionFactory.OpenSession();
            _session.FlushMode = FlushMode.Never;
            //           _session.FlushMode = FlushMode.Auto;
            _tx = _session.BeginTransaction();
            try
            {
                //Handle the case of entity type (InitialLoad)
                if (_entityType != null)
                {
                    _criteria = _session.CreateCriteria(_entityType);
                    _criteria.SetCacheable(false);

                    if (_initialLoadWithPartitioning.InitialLoadWithSpaceRouting == true)
                    {
                        string modFilterSql = String.Format("(({0} % {1}) + 1) = {2}",
                                _initialLoadWithPartitioning.RoutingPropertyName, _initialLoadWithPartitioning.NumberOfInstances, _initialLoadWithPartitioning.InstanceId);

                        string msg = "modFilterSql is: " + modFilterSql;
                        Logger.Info(msg);
                        Console.WriteLine(msg);

                        _criteria.Add(Expression.Sql(modFilterSql));

                    }
                    //If perform by id, add order to the criteria
                    if (_performOrderById)
                    {
                        IClassMetadata metadata = _sessionFactory.GetClassMetadata(_entityType);
                        string idPropName = metadata.IdentifierPropertyName;
                        if (idPropName != null)
                            _criteria.AddOrder(NHibernate.Criterion.Order.Asc(idPropName));
                    }
                }
                //Handle the case of Persistency.Query (GetEnumerator)
                else if (_persistencyQuery != null)
                {
                    string select = _persistencyQuery.SqlQuery;
                    _query = _session.CreateQuery(select);
                    object[] preparedValues = _persistencyQuery.Parameters;
                    if (preparedValues != null)
                    {
                        for (int i = 0; i < preparedValues.Length; i++)
                        {
                            _query.SetParameter(i, preparedValues[i]);
                        }
                    }
                    _query.SetCacheable(false);
                    _query.SetFlushMode(FlushMode.Never);
                }
                else throw new Exception("NHibernateDataEnumerator must receive an Entity Type or a Query");
            }
            catch (Exception ex)
            {
                if (_tx != null && _tx.IsActive)
                    _tx.Rollback();
                if (_session.IsOpen)
                {
                    _session.Flush();
                    _session.Close();
                }
                Console.WriteLine("InitSession Exception149 : " + ex.ToString());
                throw new Exception("Error while constructing an enumerator", ex);
            }
        }

        /// <summary>
        /// Gets the next element from the enumerator and returns true if such exists
        /// </summary>
        /// <returns></returns>
        private bool AssignNextElement()
        {
            if (_session == null)
                InitSession();
            //If enumerated over chunk size of objects set the state of this enumerator as exhausted
            if (_totalEnumeratedObject == _chunkSize)
                _isExhausted = true;
            if (_isExhausted)
            {
                return false;
            }
            if (_currentEnumerator == null || !_currentEnumerator.MoveNext())
            {
                int firstResultPosition = _offset + _currentFetchPosition * _fetchSize;
                int maxResultSize = Math.Min(_fetchSize, _chunkSize);
                if (_query != null)
                {
                    _query.SetFirstResult(firstResultPosition);
                    _query.SetMaxResults(maxResultSize);
                    _currentEnumerator = _query.List().GetEnumerator();
                    _currentFetchPosition++;
                }
                else
                { // criteria != null
                    _criteria.SetFirstResult(firstResultPosition);
                    _criteria.SetMaxResults(maxResultSize);
                    _currentEnumerator = _criteria.List().GetEnumerator();
                    _currentFetchPosition++;
                }
                if (!_currentEnumerator.MoveNext())
                {
                    _current = null;
                    _isExhausted = true;
                    return false;
                }
            }
            _totalEnumeratedObject++;
            _current = _currentEnumerator.Current;
            return true;
        }
    }

    public class InitialLoadWithPartitioning
    {
        private readonly bool _initialLoadWithSpaceRouting;
        private readonly string _routingPropertyName;
        private readonly int _numberOfInstances;
        private readonly int _instanceId;

        public InitialLoadWithPartitioning(bool initialLoadWithSpaceRouting, string routingPropertyName, int numberOfInstances, int instanceId) {
            _initialLoadWithSpaceRouting = initialLoadWithSpaceRouting;
            _routingPropertyName = routingPropertyName;
            _numberOfInstances = numberOfInstances;
            _instanceId = instanceId;
        }

        public bool InitialLoadWithSpaceRouting { get { return _initialLoadWithSpaceRouting; } }

        public string RoutingPropertyName { get { return _routingPropertyName; } }

        public int NumberOfInstances { get { return _numberOfInstances; } }

        public int InstanceId { get { return _instanceId; } }

    }
}