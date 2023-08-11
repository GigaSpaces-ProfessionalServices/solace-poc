using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using GigaSpaces.Examples.HelloWorld.common;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using ProtoBuf;

namespace GigaSpaces.Examples.HelloWorld
{
    class Program
    {
        private static Random random = new Random();
       // static string assemblyFilePath = "";
        static Assembly assembly = Assembly.GetExecutingAssembly();
        static string serializeObjectType = "CustomBulkItem"; // json/bytearray/CustomBulkItem
        static string typePrefix = "GigaSpaces.Examples.HelloWorld.common.";
        static string sampleStr = "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";
        public static void Main(string[] args)
        {
            int totalOrders = 5000;
            int totalFills = 50000;
            // pre populate order & fills
            IList<GS_Order> orderList =  createOrders(totalOrders);
            IList<GS_Fill> fillList = createFills(totalFills,totalOrders);
            if (serializeObjectType == "json")
            {
                JArray jarrayOrder = createJsonArrayResponse(typePrefix + "GS_Order", orderList);
                orderList.Clear();
                Console.WriteLine("created order array "+ jarrayOrder.Count());
                JArray jarrayFill = createJsonArrayResponse(typePrefix + "GS_Fill", fillList);
                fillList.Clear();
                Console.WriteLine("created fill array " + jarrayFill.Count());
                
                var watch = new System.Diagnostics.Stopwatch();
                watch.Start();
                JsonSerialize(jarrayOrder);
                JsonSerialize(jarrayFill);
                watch.Stop();
                Console.WriteLine($"Json Serialization Time: {watch.ElapsedMilliseconds} ms");
            } else if(serializeObjectType == "CustomBulkItem")
            {
                IList<CustomBulkItem> orderBulkItem = createCustomBulkItemListResponse(typePrefix + "GS_Order", orderList);
                IList<CustomBulkItem> fillBulkItem = createCustomBulkItemListResponse(typePrefix + "GS_Fill", fillList);
                Console.WriteLine("Starting serialization");

                var watch = new System.Diagnostics.Stopwatch();
                watch.Start();
                Console.WriteLine("created order array " + orderBulkItem.Count());
                CustomBulkItemSerialize(orderBulkItem);
                Console.WriteLine("created fill array " + fillBulkItem.Count());
                CustomBulkItemSerialize(fillBulkItem);
                watch.Stop();
                Console.WriteLine($"CustomBulkItem Serialization Time: {watch.ElapsedMilliseconds} ms");
            }
            Console.WriteLine("Press ENTER to exit.");
            Console.ReadLine();
        }
        static void JsonSerialize(JArray jArray)
        {
            foreach(JObject jsonObj in jArray)
            {
                Encoding.ASCII.GetBytes(
                    JsonConvert.SerializeObject(jsonObj));
                // serialize same length string as json
                /* Encoding.ASCII.GetBytes(
                    sampleStr);*/
            }
        }

        static void CustomBulkItemSerialize(IList<CustomBulkItem> customBulkItemList)
        {
            foreach (CustomBulkItem customBulkItem in customBulkItemList)
            {
                try
                {
                    using (var stream = new MemoryStream())
                    {
                        Serializer.Serialize(stream, customBulkItem);
                        stream.ToArray();
                    }
                }
                catch(Exception e)
                {
                    Console.WriteLine(e);
                    Console.ReadLine();
                }
            }
        }

        static IList<GS_Order> createOrders(int totalOrders)
        {
            IList<GS_Order> orderList = new List<GS_Order>();
            for (int k = 0; k < totalOrders; k++)
            {
                GS_Order order = new GS_Order();
                order.OrderID = k;
                order.Symbol = "IBM";
                order.Quantity = (k % 100) + 1;
                order.Price = 10;
                order.CumQty = 0;
                order.CalCumQty = 0;
                order.CalExecValue = 0;
                int i = 100;
                order.FldInt_1 = random.Next(10, 20 + i);
                order.FldInt_2 = random.Next(10, 20 + i);
                order.FldInt_3 = random.Next(10, 20 + i);
                order.FldInt_4 = random.Next(10, 20 + i);
                order.FldInt_5 = random.Next(10, 20 + i);
                order.FldInt_6 = random.Next(10, 20 + i);
                order.FldInt_7 = random.Next(10, 20 + i);
                order.FldInt_8 = random.Next(10, 20 + i);
                order.FldInt_9 = random.Next(10, 20 + i);
                order.FldInt_10 = random.Next(10, 20 + i);
                order.FldInt_11 = random.Next(10, 20 + i);
                order.FldInt_12 = random.Next(10, 20 + i);
                order.FldInt_13 = random.Next(10, 20 + i);
                order.FldInt_14 = random.Next(10, 20 + i);
                order.FldInt_15 = random.Next(10, 20 + i);
                order.FldInt_16 = random.Next(10, 20 + i);
                order.FldInt_17 = random.Next(10, 20 + i);
                order.FldInt_18 = random.Next(10, 20 + i);
                order.FldInt_19 = random.Next(10, 20 + i);
                order.FldInt_20 = random.Next(10, 20 + i);
                order.FldInt_21 = random.Next(10, 20 + i);
                order.FldInt_22 = random.Next(10, 20 + i);
                order.FldInt_23 = random.Next(10, 20 + i);
                order.FldInt_24 = random.Next(10, 20 + i);
                order.FldInt_25 = random.Next(10, 20 + i);
                order.FldInt_26 = random.Next(10, 20 + i);
                order.FldInt_27 = random.Next(10, 20 + i);
                order.FldInt_28 = random.Next(10, 20 + i);
                order.FldInt_29 = random.Next(10, 20 + i);
                order.FldInt_30 = random.Next(10, 20 + i);
                order.FldInt_31 = random.Next(10, 20 + i);
                order.FldInt_32 = random.Next(10, 20 + i);
                order.FldInt_33 = random.Next(10, 20 + i);
                order.FldInt_34 = random.Next(10, 20 + i);
                order.FldInt_35 = random.Next(10, 20 + i);
                order.FldInt_36 = random.Next(10, 20 + i);
                order.FldInt_37 = random.Next(10, 20 + i);
                order.FldInt_38 = random.Next(10, 20 + i);
                order.FldInt_39 = random.Next(10, 20 + i);
                order.FldInt_40 = random.Next(10, 20 + i);
                order.FldTime_1 = DateTime.Now;
                order.FldTime_2 = DateTime.Now;
                order.FldTime_3 = DateTime.Now;
                order.FldTime_4 = DateTime.Now;
                order.FldTime_5 = DateTime.Now;
                order.FldTime_6 = DateTime.Now;
                order.FldTime_7 = DateTime.Now;
                order.FldTime_8 = DateTime.Now;
                order.FldStr_1 = getRandomString(random.Next(1, 10));
                order.FldStr_2 = getRandomString(random.Next(1, 10));
                order.FldStr_3 = getRandomString(random.Next(1, 10));
                order.FldStr_4 = getRandomString(random.Next(1, 10));
                order.FldStr_5 = getRandomString(random.Next(1, 10));
                order.FldStr_6 = getRandomString(random.Next(1, 10));
                order.FldStr_7 = getRandomString(random.Next(1, 10));
                order.FldStr_8 = getRandomString(random.Next(1, 10));
                order.FldStr_9 = getRandomString(random.Next(1, 10));
                order.FldStr_10 = getRandomString(random.Next(1, 10));
                order.FldStr_11 = getRandomString(random.Next(1, 10));
                order.FldStr_12 = getRandomString(random.Next(1, 10));
                order.FldStr_13 = getRandomString(random.Next(1, 10));
                order.FldStr_14 = getRandomString(random.Next(1, 10));
                order.FldStr_15 = getRandomString(random.Next(1, 10));
                order.FldStr_16 = getRandomString(random.Next(1, 10));
                order.FldStr_17 = getRandomString(random.Next(1, 10));
                order.FldStr_18 = getRandomString(random.Next(1, 10));
                order.FldStr_19 = getRandomString(random.Next(1, 10));
                order.FldStr_20 = getRandomString(random.Next(1, 10));
                order.FldStr_21 = getRandomString(random.Next(1, 10));
                order.FldStr_22 = getRandomString(random.Next(1, 10));
                order.FldStr_23 = getRandomString(random.Next(1, 10));
                order.FldStr_24 = getRandomString(random.Next(1, 10));
                order.FldStr_25 = getRandomString(random.Next(1, 10));
                order.FldDbl_1 = random.Next(10, 20 + i) * 100 / 32;
                order.FldDbl_2 = random.Next(10, 20 + i) * 100 / 32;
                order.FldDbl_3 = random.Next(10, 20 + i) * 100 / 32;
                order.FldDbl_4 = random.Next(10, 20 + i) * 100 / 32;
                order.FldDbl_5 = random.Next(10, 20 + i) * 100 / 32;
                order.FldDbl_6 = random.Next(10, 20 + i) * 100 / 32;
                order.FldDbl_7 = random.Next(10, 20 + i) * 100 / 32;
                order.FldDbl_8 = random.Next(10, 20 + i) * 100 / 32;
                order.FldDbl_9 = random.Next(10, 20 + i) * 100 / 32;
                order.FldDbl_10 = random.Next(10, 20 + i) * 100 / 32;
                order.FldDbl_11 = random.Next(10, 20 + i) * 100 / 32;
                order.FldDbl_12 = random.Next(10, 20 + i) * 100 / 32;
                order.FldDbl_13 = random.Next(10, 20 + i) * 100 / 32;
                order.FldDbl_14 = random.Next(10, 20 + i) * 100 / 32;
                order.FldDbl_15 = random.Next(10, 20 + i) * 100 / 32;
                order.FldDbl_16 = random.Next(10, 20 + i) * 100 / 32;
                order.FldDbl_17 = random.Next(10, 20 + i) * 100 / 32;
                order.FldDbl_18 = random.Next(10, 20 + i) * 100 / 32;
                order.FldDbl_19 = random.Next(10, 20 + i) * 100 / 32;
                order.FldDbl_20 = random.Next(10, 20 + i) * 100 / 32;
                order.FldDbl_21 = random.Next(10, 20 + i) * 100 / 32;
                order.FldDbl_22 = random.Next(10, 20 + i) * 100 / 32;
                order.FldDbl_23 = random.Next(10, 20 + i) * 100 / 32;
                order.FldDbl_24 = random.Next(10, 20 + i) * 100 / 32;
                order.FldDbl_25 = random.Next(10, 20 + i) * 100 / 32;
                order.FldDbl_26 = random.Next(10, 20 + i) * 100 / 32;
                order.FldDbl_27 = random.Next(10, 20 + i) * 100 / 32;
                order.FldDbl_28 = random.Next(10, 20 + i) * 100 / 32;
                order.FldDbl_29 = random.Next(10, 20 + i) * 100 / 32;
                order.FldDbl_30 = random.Next(10, 20 + i) * 100 / 32;
                order.FldDbl_31 = random.Next(10, 20 + i) * 100 / 32;
                order.FldDbl_32 = random.Next(10, 20 + i) * 100 / 32;
                order.FldDbl_33 = random.Next(10, 20 + i) * 100 / 32;
                order.FldDbl_34 = random.Next(10, 20 + i) * 100 / 32;
                order.FldDbl_35 = random.Next(10, 20 + i) * 100 / 32;
                order.FldDbl_36 = random.Next(10, 20 + i) * 100 / 32;
                order.FldDbl_37 = random.Next(10, 20 + i) * 100 / 32;
                order.FldDbl_38 = random.Next(10, 20 + i) * 100 / 32;
                order.FldDbl_39 = random.Next(10, 20 + i) * 100 / 32;
                order.FldDbl_40 = random.Next(10, 20 + i) * 100 / 32;
                orderList.Add(order);
            }
            return orderList;

        }
        static IList<GS_Fill> createFills(int totalFills, int totalOrders)
        {
            IList<GS_Fill> fillList = new List<GS_Fill>();
            for (int k = 0; k < totalFills; k++)
            {
                GS_Fill fill = new GS_Fill();
                fill.OrderID = k % totalOrders;
                fill.FldInt_1 = k;
                fill.FldInt_2 = k;
                fill.FldInt_3 = k;
                fill.FldInt_4 = k;
                fill.FldInt_5 = k;
                fill.FldInt_6 = k;
                fill.FldInt_7 = k;
                fill.FldInt_8 = k;
                fill.FldInt_9 = k;
                fill.FldInt_10 = k;
                fill.FldInt_11 = k;
                fill.FldTime_1 = DateTime.Now;
                fill.FldTime_2 = DateTime.Now;
                fill.FldTime_3 = DateTime.Now;
                fill.FldTime_4 = DateTime.Now;
                fill.FldDbl_1 = random.Next(10, 20 + k) % 10000 / 32;
                fill.FldDbl_2 = random.Next(10, 20 + k) % 10000 / 32;
                fill.FldDbl_3 = random.Next(10, 20 + k) % 10000 / 32;
                fill.FldStr_1 = getRandomString(random.Next(1, 10));
                fill.FldStr_2 = getRandomString(random.Next(1, 10));
                fill.FldStr_3 = getRandomString(random.Next(1, 10));
                fill.FldStr_4 = getRandomString(random.Next(1, 10));
                fill.FldStr_5 = getRandomString(random.Next(1, 10));
                fill.FldStr_6 = getRandomString(random.Next(1, 10));
                fill.FldStr_7 = getRandomString(random.Next(1, 10));
                fill.FldStr_8 = getRandomString(random.Next(1, 10));
                fill.FldStr_9 = getRandomString(random.Next(1, 10));
                fill.FldStr_10 = getRandomString(random.Next(1, 10));
                fill.FldStr_11 = getRandomString(random.Next(1, 10));
                fill.FldStr_12 = getRandomString(random.Next(1, 10));
                fill.FldStr_13 = getRandomString(random.Next(1, 10));
                fill.FldStr_14 = getRandomString(random.Next(1, 10));
                fill.FldStr_15 = getRandomString(random.Next(1, 10));
                fill.FldStr_16 = getRandomString(random.Next(1, 10));
                fill.FldStr_17 = getRandomString(random.Next(1, 10));
                fill.FldStr_18 = getRandomString(random.Next(1, 10));
                fill.FldStr_19 = getRandomString(random.Next(1, 10));
                fill.FldStr_20 = getRandomString(random.Next(1, 10));
                fill.FldTime_1 = DateTime.Now;
                fill.LastShares = k;
                fill.LastPrice = 10;

                fillList.Add(fill);
            }
            return fillList;
        }

       static JArray createJsonArrayResponse(string typeName, object list)
        {
            JArray jarr = new JArray();

            if (typeName == typePrefix+"GS_Order")
            {
                IList<GS_Order> orderList = (IList<GS_Order>)list;
                foreach (GS_Order order in orderList)
                {
                    JObject jsonobj = createJsonResponse(typeName, order);
                    jarr.Add(jsonobj);
                }
            }

            if (typeName == typePrefix + "GS_Fill")
            {
                IList<GS_Fill> fillList = (IList<GS_Fill>)list;
                foreach (GS_Fill fill in fillList)
                {
                    JObject jsonobj = createJsonResponse(typeName, fill);
                    jarr.Add(jsonobj);
                }
            }

            return jarr;
        }

       static JObject createJsonResponse(string typeName, object itemValue)
        {
           // object itemValue = bulkItem.;

            Type type = assembly.GetType(typeName);
            PropertyInfo[] propertyInfo = type.GetProperties();

            string[] propertyNames = new string[propertyInfo.Length];
            string[] propertyType = new string[propertyInfo.Length];
            object[] propertyValue = new object[propertyInfo.Length];
            JObject mainObject = new JObject();
            mainObject["op"] = "Write";
            mainObject["type"] = typeName;
            mainObject["spaceId"] = typeName+"_ID";

            JArray payload = new JArray();

            for (int i = 0; i < propertyInfo.Length; i++)
            {
                object itemValTmp = itemValue.GetType().GetProperty(propertyInfo[i].Name).GetValue(itemValue);
                propertyNames[i] = propertyInfo[i].Name;
                propertyValue[i] = itemValue.GetType().GetProperty(propertyInfo[i].Name).GetValue(itemValue, null);
                propertyType[i] = propertyInfo[i].PropertyType.Name;

                JObject fieldDetails = new JObject();
                fieldDetails["columnName"] = propertyInfo[i].Name;
                fieldDetails["value"] = new JValue(itemValTmp);
                payload.Add(fieldDetails);
            }
            mainObject.Add("payload", payload);

            return mainObject;
        }

        static IList<CustomBulkItem> createCustomBulkItemListResponse(string typeName, object list)
        {
            IList<CustomBulkItem> bulkItems = new List<CustomBulkItem>();

            if (typeName == typePrefix + "GS_Order")
            {
                IList<GS_Order> orderList = (IList<GS_Order>)list;
                foreach (GS_Order order in orderList)
                {
                    CustomBulkItem bulkItem= createCustomBulkItemResponse(typeName, order);
                    bulkItems.Add(bulkItem);
                }
            }

            if (typeName == typePrefix + "GS_Fill")
            {
                IList<GS_Fill> fillList = (IList<GS_Fill>)list;
                foreach (GS_Fill fill in fillList)
                {
                    CustomBulkItem bulkItem = createCustomBulkItemResponse(typeName, fill);
                    bulkItems.Add(bulkItem);
                }
            }

            return bulkItems;
        }

        static CustomBulkItem createCustomBulkItemResponse(string typeName, object itemValue)
        {
            CustomBulkItem customBulkItem = new CustomBulkItem();
            customBulkItem.spaceId = typeName + "_ID";
            customBulkItem.typeName = typeName;
            customBulkItem.operation = "Write";
            customBulkItem.items = itemValue;
            return customBulkItem;
        }
        public static string getRandomString(int length)
        {
            const string chars = "FIL_123";
            return new string(Enumerable.Repeat(chars, length)
                .Select(s => s[random.Next(s.Length)]).ToArray());
        }
    }
}
