using GigaSpaces.Core.Persistency;
using System;
using System.Collections;
using ReadRedoLogContentsTest.Common;

namespace CustomExternalDataSource.ExternalDataSource
{
    internal class MyIDataEnumerator : IDataEnumerator
    {
        List<Data> _list = new List<Data>();
        int position;

        /********** set up sample test objects **********/
        private static Data createTestData1()
        {
            Data data = new Data();
            data.Id = 101;
            data.Info = "testData101";
            data.Type = 101;
            data.Date = DateTime.Now;
            data.Processed = false;
            return data;
        }
        // for take test
        private static Data createTestData2()
        {
            Data data = new Data();
            data.Id = 102;
            data.Info = "testData102";
            data.Type = 102;
            data.Date = DateTime.Now;
            data.Processed = false;
            return data;
        }
        /********** end set up sample test objects **********/

        

        public MyIDataEnumerator()
        {
            position = -1;
            _list.Add(createTestData1());
            _list.Add(createTestData2());

        }

        //public object Current => throw new NotImplementedException();

        object IEnumerator.Current
        {
            get { return Current; }
        }

        public object Current
        {
            get
            {
                try
                {
                    return _list[position];
                }
                catch (IndexOutOfRangeException)
                {
                    throw new InvalidOperationException();
                }
            }
        }
        public void Dispose()
        {
            return;
        }

        public bool MoveNext()
        {
            position++;
            return (position < _list.Count);            
        }

        public void Reset()
        {
            position = -1;
            return;
        }
    }
    /*
    class App
    {
        static void Main()
        {
            MyIDataEnumerator enumerator = new MyIDataEnumerator();
           
            while (enumerator.MoveNext())
            {
                Console.WriteLine(enumerator.Current);
            }

        }
    } */
}
