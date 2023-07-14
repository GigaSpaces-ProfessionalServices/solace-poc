using GigaSpaces.Core;
using GigaSpaces.Core.Persistency;


namespace CustomExternalDataSource.ExternalDataSource
{
    public class SolaceExternalDataSource : AbstractExternalDataSource
    {
        const int MaxRetries = 1;

        private static readonly NLog.Logger Logger = NLog.LogManager.GetCurrentClassLogger();
        public SolaceExternalDataSource()
        {
        }

        public override void ExecuteBulk(IList<BulkItem> bulk)
        {
            ExecuteBulk(bulk, 0);
        }
        protected virtual void ExecuteBulk(IList<BulkItem> bulk, int retries)
        {
            try
            {
                foreach (BulkItem bulkItem in bulk)
                {
                    ExecuteBulkItem(bulkItem, retries);
                }
            }
            catch (Exception e)
            {
                if (retries >= MaxRetries)
                    throw new Exception("Can't execute bulk store.", e);
                ExecuteBulk(bulk, retries + 1);
            }
        }
        protected virtual void ExecuteBulkItem(BulkItem bulkItem, int retries)
        {
            object entry = bulkItem.Item;


            switch (bulkItem.Operation)
            {
                case BulkOperation.Remove:
                    //session.Delete(session.Merge(entry));
                    Logger.Info("Remove called for: {0}", entry);
                    
                    break;
                case BulkOperation.Write:
                    Logger.Info("Write called for: {0}", entry);
                    break;
                case BulkOperation.Update:
                    Logger.Info("Update called for: {0}", entry);
                    /*
                    if (retries > 0 || UseMerge)
                        session.Merge(entry);
                    else
                    {
                        try
                        {
                            session.SaveOrUpdate(entry);
                        }
                        catch (HibernateException)
                        {
                            session.Merge(entry);
                        }
                    }
                    */
                    break;
                default:
                    break;
            }
        }

        public override IDataEnumerator GetEnumerator(Query query)
        {
            
            return new MyIDataEnumerator();
        }

        public override IDataEnumerator InitialLoad()
        {
            /*
            List<IDataEnumerator> enumerators = new List<IDataEnumerator>();
            enumerators.Add(GetEnumerator(null));

            return new ConcurrentMultiDataEnumerator(enumerators, 1, 1);
            */
            return GetEnumerator(null);
        }

    }
}
