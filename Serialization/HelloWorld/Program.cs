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
        static string serializeObjectType = "json"; // json/bytearray/CustomBulkItem
        static string typePrefix = "GigaSpaces.Examples.HelloWorld.common.";
        static string sampleStr = "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";
        static string sampleOrderJsonstr = "{\"op\":\"Write\",\"type\":\"GigaSpaces.Examples.HelloWorld.common.GS_Order\",\"spaceId\":\"GigaSpaces.Examples.HelloWorld.common.GS_Order_ID\",\"payload\":[{\"columnName\":\"OrderID\",\"value\":0},{\"columnName\":\"TraderID\",\"value\":-1},{\"columnName\":\"Symbol\",\"value\":\"IBM\"},{\"columnName\":\"Quantity\",\"value\":1},{\"columnName\":\"CumQty\",\"value\":0},{\"columnName\":\"Price\",\"value\":10.0},{\"columnName\":\"CalCumQty\",\"value\":0},{\"columnName\":\"CalExecValue\",\"value\":0.0},{\"columnName\":\"FldInt_1\",\"value\":103},{\"columnName\":\"FldInt_2\",\"value\":100},{\"columnName\":\"FldInt_3\",\"value\":12},{\"columnName\":\"FldInt_4\",\"value\":118},{\"columnName\":\"FldInt_5\",\"value\":58},{\"columnName\":\"FldInt_6\",\"value\":97},{\"columnName\":\"FldInt_7\",\"value\":21},{\"columnName\":\"FldInt_8\",\"value\":104},{\"columnName\":\"FldInt_9\",\"value\":107},{\"columnName\":\"FldInt_10\",\"value\":64},{\"columnName\":\"FldInt_11\",\"value\":91},{\"columnName\":\"FldInt_12\",\"value\":19},{\"columnName\":\"FldInt_13\",\"value\":15},{\"columnName\":\"FldInt_14\",\"value\":39},{\"columnName\":\"FldInt_15\",\"value\":119},{\"columnName\":\"FldInt_16\",\"value\":101},{\"columnName\":\"FldInt_17\",\"value\":78},{\"columnName\":\"FldInt_18\",\"value\":111},{\"columnName\":\"FldInt_19\",\"value\":58},{\"columnName\":\"FldInt_20\",\"value\":50},{\"columnName\":\"FldInt_21\",\"value\":104},{\"columnName\":\"FldInt_22\",\"value\":75},{\"columnName\":\"FldInt_23\",\"value\":114},{\"columnName\":\"FldInt_24\",\"value\":104},{\"columnName\":\"FldInt_25\",\"value\":83},{\"columnName\":\"FldInt_26\",\"value\":48},{\"columnName\":\"FldInt_27\",\"value\":58},{\"columnName\":\"FldInt_28\",\"value\":82},{\"columnName\":\"FldInt_29\",\"value\":93},{\"columnName\":\"FldInt_30\",\"value\":61},{\"columnName\":\"FldInt_31\",\"value\":89},{\"columnName\":\"FldInt_32\",\"value\":72},{\"columnName\":\"FldInt_33\",\"value\":56},{\"columnName\":\"FldInt_34\",\"value\":116},{\"columnName\":\"FldInt_35\",\"value\":91},{\"columnName\":\"FldInt_36\",\"value\":62},{\"columnName\":\"FldInt_37\",\"value\":70},{\"columnName\":\"FldInt_38\",\"value\":56},{\"columnName\":\"FldInt_39\",\"value\":54},{\"columnName\":\"FldInt_40\",\"value\":119},{\"columnName\":\"FldTime_1\",\"value\":\"2023-08-14T10:44:06.3836062+00:00\"},{\"columnName\":\"FldTime_2\",\"value\":\"2023-08-14T10:44:06.3846268+00:00\"},{\"columnName\":\"FldTime_3\",\"value\":\"2023-08-14T10:44:06.3846268+00:00\"},{\"columnName\":\"FldTime_4\",\"value\":\"2023-08-14T10:44:06.3846268+00:00\"},{\"columnName\":\"FldTime_5\",\"value\":\"2023-08-14T10:44:06.3846268+00:00\"},{\"columnName\":\"FldTime_6\",\"value\":\"2023-08-14T10:44:06.3846268+00:00\"},{\"columnName\":\"FldTime_7\",\"value\":\"2023-08-14T10:44:06.3846268+00:00\"},{\"columnName\":\"FldTime_8\",\"value\":\"2023-08-14T10:44:06.3846268+00:00\"},{\"columnName\":\"FldStr_1\",\"value\":\"__F3F2\"},{\"columnName\":\"FldStr_2\",\"value\":\"1I\"},{\"columnName\":\"FldStr_3\",\"value\":\"2\"},{\"columnName\":\"FldStr_4\",\"value\":\"3LI3ILFL\"},{\"columnName\":\"FldStr_5\",\"value\":\"FL2I\"},{\"columnName\":\"FldStr_6\",\"value\":\"F__II_\"},{\"columnName\":\"FldStr_7\",\"value\":\"2LF33\"},{\"columnName\":\"FldStr_8\",\"value\":\"1LF_L\"},{\"columnName\":\"FldStr_9\",\"value\":\"2_1_1LFL1\"},{\"columnName\":\"FldStr_10\",\"value\":\"_I\"},{\"columnName\":\"FldStr_11\",\"value\":\"2_1\"},{\"columnName\":\"FldStr_12\",\"value\":\"F32LLI\"},{\"columnName\":\"FldStr_13\",\"value\":\"I\"},{\"columnName\":\"FldStr_14\",\"value\":\"I2_FL133L\"},{\"columnName\":\"FldStr_15\",\"value\":\"_F\"},{\"columnName\":\"FldStr_16\",\"value\":\"3FI2_231I\"},{\"columnName\":\"FldStr_17\",\"value\":\"3\"},{\"columnName\":\"FldStr_18\",\"value\":\"3__1_I1\"},{\"columnName\":\"FldStr_19\",\"value\":\"1IL\"},{\"columnName\":\"FldStr_20\",\"value\":\"122ILLII\"},{\"columnName\":\"FldStr_21\",\"value\":\"IF2F_\"},{\"columnName\":\"FldStr_22\",\"value\":\"ILL31_I\"},{\"columnName\":\"FldStr_23\",\"value\":\"FLI3\"},{\"columnName\":\"FldStr_24\",\"value\":\"22I12LI2F\"},{\"columnName\":\"FldStr_25\",\"value\":\"I322I2L\"},{\"columnName\":\"FldStr_26\",\"value\":null},{\"columnName\":\"FldStr_27\",\"value\":null},{\"columnName\":\"FldStr_28\",\"value\":null},{\"columnName\":\"FldStr_29\",\"value\":null},{\"columnName\":\"FldStr_30\",\"value\":null},{\"columnName\":\"FldStr_31\",\"value\":null},{\"columnName\":\"FldStr_32\",\"value\":null},{\"columnName\":\"FldStr_33\",\"value\":null},{\"columnName\":\"FldStr_34\",\"value\":null},{\"columnName\":\"FldStr_35\",\"value\":null},{\"columnName\":\"FldStr_36\",\"value\":null},{\"columnName\":\"FldStr_37\",\"value\":null},{\"columnName\":\"FldStr_38\",\"value\":null},{\"columnName\":\"FldStr_39\",\"value\":null},{\"columnName\":\"FldStr_40\",\"value\":null},{\"columnName\":\"FldDbl_1\",\"value\":306.0},{\"columnName\":\"FldDbl_2\",\"value\":209.0},{\"columnName\":\"FldDbl_3\",\"value\":343.0},{\"columnName\":\"FldDbl_4\",\"value\":215.0},{\"columnName\":\"FldDbl_5\",\"value\":193.0},{\"columnName\":\"FldDbl_6\",\"value\":343.0},{\"columnName\":\"FldDbl_7\",\"value\":150.0},{\"columnName\":\"FldDbl_8\",\"value\":103.0},{\"columnName\":\"FldDbl_9\",\"value\":250.0},{\"columnName\":\"FldDbl_10\",\"value\":140.0},{\"columnName\":\"FldDbl_11\",\"value\":146.0},{\"columnName\":\"FldDbl_12\",\"value\":156.0},{\"columnName\":\"FldDbl_13\",\"value\":281.0},{\"columnName\":\"FldDbl_14\",\"value\":284.0},{\"columnName\":\"FldDbl_15\",\"value\":75.0},{\"columnName\":\"FldDbl_16\",\"value\":168.0},{\"columnName\":\"FldDbl_17\",\"value\":134.0},{\"columnName\":\"FldDbl_18\",\"value\":178.0},{\"columnName\":\"FldDbl_19\",\"value\":296.0},{\"columnName\":\"FldDbl_20\",\"value\":343.0},{\"columnName\":\"FldDbl_21\",\"value\":156.0},{\"columnName\":\"FldDbl_22\",\"value\":40.0},{\"columnName\":\"FldDbl_23\",\"value\":271.0},{\"columnName\":\"FldDbl_24\",\"value\":143.0},{\"columnName\":\"FldDbl_25\",\"value\":365.0},{\"columnName\":\"FldDbl_26\",\"value\":81.0},{\"columnName\":\"FldDbl_27\",\"value\":250.0},{\"columnName\":\"FldDbl_28\",\"value\":40.0},{\"columnName\":\"FldDbl_29\",\"value\":62.0},{\"columnName\":\"FldDbl_30\",\"value\":337.0},{\"columnName\":\"FldDbl_31\",\"value\":290.0},{\"columnName\":\"FldDbl_32\",\"value\":56.0},{\"columnName\":\"FldDbl_33\",\"value\":262.0},{\"columnName\":\"FldDbl_34\",\"value\":262.0},{\"columnName\":\"FldDbl_35\",\"value\":243.0},{\"columnName\":\"FldDbl_36\",\"value\":281.0},{\"columnName\":\"FldDbl_37\",\"value\":37.0},{\"columnName\":\"FldDbl_38\",\"value\":171.0},{\"columnName\":\"FldDbl_39\",\"value\":137.0},{\"columnName\":\"FldDbl_40\",\"value\":325.0}]}";
        static string sampleFillJsonstr = "{\"op\":\"Write\",\"type\":\"GigaSpaces.Examples.HelloWorld.common.GS_Fill\",\"spaceId\":\"GigaSpaces.Examples.HelloWorld.common.GS_Fill_ID\",\"payload\":[{\"columnName\":\"FillID\",\"value\":0},{\"columnName\":\"OrderID\",\"value\":0},{\"columnName\":\"LastShares\",\"value\":0},{\"columnName\":\"LastPrice\",\"value\":10.0},{\"columnName\":\"FldInt_1\",\"value\":0},{\"columnName\":\"FldInt_2\",\"value\":0},{\"columnName\":\"FldInt_3\",\"value\":0},{\"columnName\":\"FldInt_4\",\"value\":0},{\"columnName\":\"FldInt_5\",\"value\":0},{\"columnName\":\"FldInt_6\",\"value\":0},{\"columnName\":\"FldInt_7\",\"value\":0},{\"columnName\":\"FldInt_8\",\"value\":0},{\"columnName\":\"FldInt_9\",\"value\":0},{\"columnName\":\"FldInt_10\",\"value\":0},{\"columnName\":\"FldInt_11\",\"value\":0},{\"columnName\":\"FldTime_1\",\"value\":\"2023-08-14T10:46:24.9692548+00:00\"},{\"columnName\":\"FldTime_2\",\"value\":\"2023-08-14T10:46:24.9672508+00:00\"},{\"columnName\":\"FldTime_3\",\"value\":\"2023-08-14T10:46:24.9672508+00:00\"},{\"columnName\":\"FldTime_4\",\"value\":\"2023-08-14T10:46:24.9672508+00:00\"},{\"columnName\":\"FldDbl_1\",\"value\":0.0},{\"columnName\":\"FldDbl_2\",\"value\":0.0},{\"columnName\":\"FldDbl_3\",\"value\":0.0},{\"columnName\":\"FldStr_1\",\"value\":\"3F\"},{\"columnName\":\"FldStr_2\",\"value\":\"1L1L3\"},{\"columnName\":\"FldStr_3\",\"value\":\"FI32IIL1\"},{\"columnName\":\"FldStr_4\",\"value\":\"I2L\"},{\"columnName\":\"FldStr_5\",\"value\":\"F3_3212F3\"},{\"columnName\":\"FldStr_6\",\"value\":\"3ILF2I_2\"},{\"columnName\":\"FldStr_7\",\"value\":\"FFL\"},{\"columnName\":\"FldStr_8\",\"value\":\"11_F_L_\"},{\"columnName\":\"FldStr_9\",\"value\":\"I2F\"},{\"columnName\":\"FldStr_10\",\"value\":\"1I_21F1IL\"},{\"columnName\":\"FldStr_11\",\"value\":\"1\"},{\"columnName\":\"FldStr_12\",\"value\":\"F__2L_L22\"},{\"columnName\":\"FldStr_13\",\"value\":\"3L\"},{\"columnName\":\"FldStr_14\",\"value\":\"_L2_\"},{\"columnName\":\"FldStr_15\",\"value\":\"2_IIIF\"},{\"columnName\":\"FldStr_16\",\"value\":\"3\"},{\"columnName\":\"FldStr_17\",\"value\":\"3LFL13_\"},{\"columnName\":\"FldStr_18\",\"value\":\"32121LIIL\"},{\"columnName\":\"FldStr_19\",\"value\":\"F_F\"},{\"columnName\":\"FldStr_20\",\"value\":\"322II13\"},{\"columnName\":\"FldStr_21\",\"value\":null},{\"columnName\":\"FldStr_22\",\"value\":null},{\"columnName\":\"FldStr_23\",\"value\":null},{\"columnName\":\"FldStr_24\",\"value\":null},{\"columnName\":\"FldStr_25\",\"value\":null},{\"columnName\":\"FldStr_26\",\"value\":null},{\"columnName\":\"FldStr_27\",\"value\":null},{\"columnName\":\"FldStr_28\",\"value\":null},{\"columnName\":\"FldStr_29\",\"value\":null},{\"columnName\":\"FldStr_30\",\"value\":null},{\"columnName\":\"FldStr_31\",\"value\":null},{\"columnName\":\"FldStr_32\",\"value\":null},{\"columnName\":\"FldStr_33\",\"value\":null},{\"columnName\":\"FldStr_34\",\"value\":null},{\"columnName\":\"FldStr_35\",\"value\":null},{\"columnName\":\"FldStr_36\",\"value\":null},{\"columnName\":\"FldStr_37\",\"value\":null},{\"columnName\":\"FldStr_38\",\"value\":null},{\"columnName\":\"FldStr_39\",\"value\":null},{\"columnName\":\"FldStr_40\",\"value\":null},{\"columnName\":\"FldStr_41\",\"value\":null},{\"columnName\":\"FldStr_42\",\"value\":null},{\"columnName\":\"FldStr_43\",\"value\":null},{\"columnName\":\"FldStr_44\",\"value\":null},{\"columnName\":\"FldStr_45\",\"value\":null},{\"columnName\":\"FldStr_46\",\"value\":null},{\"columnName\":\"FldStr_47\",\"value\":null},{\"columnName\":\"FldStr_48\",\"value\":null},{\"columnName\":\"FldStr_49\",\"value\":null},{\"columnName\":\"FldStr_50\",\"value\":null}]}";

        static bool withRefection = false;
        public static void Main(string[] args)
        {
            int totalOrders = 5000;
            int totalFills = 50000;
            // pre populate order & fills
            IList<GS_Order> orderList =  createOrders(totalOrders);
            IList<GS_Fill> fillList = createFills(totalFills,totalOrders);
            if (serializeObjectType == "json")
            {
                Console.WriteLine("Reflection : "+ withRefection);
                var watch = new System.Diagnostics.Stopwatch();
                watch.Start();
                JArray jarrayOrder = createJsonArrayResponse(typePrefix + "GS_Order", orderList);
                watch.Stop();
                Console.WriteLine($"Json  array order creation Time: {watch.ElapsedMilliseconds} ms");
                orderList.Clear();
                Console.WriteLine("created order array "+ jarrayOrder.Count());
                watch.Restart();
                JArray jarrayFill = createJsonArrayResponse(typePrefix + "GS_Fill", fillList);
                watch.Stop();
                Console.WriteLine($"Json  array fill creation Time: {watch.ElapsedMilliseconds} ms");

                fillList.Clear();
                Console.WriteLine("created fill array " + jarrayFill.Count());

                watch.Restart();
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
              //  Console.WriteLine(JsonConvert.SerializeObject(jsonObj));
                Encoding.ASCII.GetBytes(
                    JsonConvert.SerializeObject(jsonObj));
                // serialize same length string as json
                /* Encoding.ASCII.GetBytes(
                    sampleStr);*/
            }
        }

       /* static void JsonSerialize2(int size, string data)
        {
            for(int i=0;i< size;i++)
            {
                //  Console.WriteLine(JsonConvert.SerializeObject(jsonObj));
                Encoding.ASCII.GetBytes(
                    data);
            }
        }*/

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
                    JObject jsonobj;
                    if (withRefection)
                    {
                        jsonobj = createJsonResponse(typeName, order);

                    } else
                    {
                        jsonobj = createJsonResponse2(typeName, order);
                    }
                    jarr.Add(jsonobj);
                }
            }

            if (typeName == typePrefix + "GS_Fill")
            {
                IList<GS_Fill> fillList = (IList<GS_Fill>)list;
                foreach (GS_Fill fill in fillList)
                {
                    JObject jsonobj;
                    if (withRefection)
                    {
                        jsonobj = createJsonResponse(typeName, fill);
                    } else
                    {
                        jsonobj = createJsonResponse2(typeName, fill);
                    }
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

        static JObject createJsonResponse2(string typeName, object itemValue)
        {
            // object itemValue = bulkItem.;

           // Type type = assembly.GetType(typeName);
           // PropertyInfo[] propertyInfo = type.GetProperties();

           /* string[] propertyNames = new string[propertyInfo.Length];
            string[] propertyType = new string[propertyInfo.Length];
            object[] propertyValue = new object[propertyInfo.Length];*/
            JObject mainObject = new JObject();
            mainObject["op"] = "Write";
            mainObject["type"] = typeName;
            mainObject["spaceId"] = typeName + "_ID";

            JArray payload = new JArray();
            GS_Order order = null;
            GS_Fill fill =null;
            if (typeName == typePrefix + "GS_Order")
            {
                order = (GS_Order)itemValue;
            }
            if (typeName == typePrefix + "GS_Fill")
            {
                fill = (GS_Fill)itemValue;
            }

                if (fill != null)
                {

                    JObject fieldDetails = new JObject();
                    fieldDetails["columnName"] = "FillID";
                    fieldDetails["value"] = fill.FillID;
                    payload.Add(fieldDetails);
                    fieldDetails = new JObject();
                    fieldDetails["columnName"] = "OrderID";
                    fieldDetails["value"] = fill.OrderID;
                    payload.Add(fieldDetails);
                    fieldDetails = new JObject();
                    fieldDetails["columnName"] = "LastShares";
                    fieldDetails["value"] = fill.LastShares;
                    payload.Add(fieldDetails);
                    fieldDetails = new JObject();
                    fieldDetails["columnName"] = "LastPrice";
                    fieldDetails["value"] = fill.LastPrice;
                    payload.Add(fieldDetails);
                    fieldDetails = new JObject();
                    fieldDetails["columnName"] = "FldInt_1";
                    fieldDetails["value"] = fill.FldInt_1;
                    payload.Add(fieldDetails);
                    fieldDetails = new JObject();
                    fieldDetails["columnName"] = "FldInt_2";
                    fieldDetails["value"] = fill.FldInt_2;
                    payload.Add(fieldDetails);
                    fieldDetails = new JObject();
                    fieldDetails["columnName"] = "FldInt_3";
                    fieldDetails["value"] = fill.FldInt_3;
                    payload.Add(fieldDetails);
                    fieldDetails = new JObject();
                    fieldDetails["columnName"] = "FldInt_4";
                    fieldDetails["value"] = fill.FldInt_4;
                    payload.Add(fieldDetails);
                    fieldDetails = new JObject();
                    fieldDetails["columnName"] = "FldInt_5";
                    fieldDetails["value"] = fill.FldInt_5;
                    payload.Add(fieldDetails);
                    fieldDetails = new JObject();
                    fieldDetails["columnName"] = "FldInt_6";
                    fieldDetails["value"] = fill.FldInt_6;
                    payload.Add(fieldDetails);
                    fieldDetails = new JObject();
                    fieldDetails["columnName"] = "FldInt_7";
                    fieldDetails["value"] = fill.FldInt_7;
                    payload.Add(fieldDetails);
                    fieldDetails = new JObject();
                    fieldDetails["columnName"] = "FldInt_8";
                    fieldDetails["value"] = fill.FldInt_8;
                    payload.Add(fieldDetails);
                    fieldDetails = new JObject();
                    fieldDetails["columnName"] = "FldInt_9";
                    fieldDetails["value"] = fill.FldInt_9;
                    payload.Add(fieldDetails);
                    fieldDetails = new JObject();
                    fieldDetails["columnName"] = "FldInt_10";
                    fieldDetails["value"] = fill.FldInt_10;
                    payload.Add(fieldDetails);
                    fieldDetails = new JObject();
                    fieldDetails["columnName"] = "FldInt_11";
                    fieldDetails["value"] = fill.FldInt_11;
                    payload.Add(fieldDetails);
                    fieldDetails = new JObject();
                    fieldDetails["columnName"] = "FldTime_1";
                    fieldDetails["value"] = fill.FldTime_1;
                    payload.Add(fieldDetails);
                    fieldDetails = new JObject();
                    fieldDetails["columnName"] = "FldTime_2";
                    fieldDetails["value"] = fill.FldTime_2;
                    payload.Add(fieldDetails);
                    fieldDetails = new JObject();
                    fieldDetails["columnName"] = "FldTime_3";
                    fieldDetails["value"] = fill.FldTime_3;
                    payload.Add(fieldDetails);
                    fieldDetails = new JObject();
                    fieldDetails["columnName"] = "FldTime_4";
                    fieldDetails["value"] = fill.FldTime_4;
                    payload.Add(fieldDetails);
                    fieldDetails = new JObject();
                    fieldDetails["columnName"] = "FldDbl_1";
                    fieldDetails["value"] = fill.FldDbl_1;
                    payload.Add(fieldDetails);
                    fieldDetails = new JObject();
                    fieldDetails["columnName"] = "FldDbl_2";
                    fieldDetails["value"] = fill.FldDbl_2;
                    payload.Add(fieldDetails);
                    fieldDetails = new JObject();
                    fieldDetails["columnName"] = "FldDbl_3";
                    fieldDetails["value"] = fill.FldDbl_3;
                    payload.Add(fieldDetails);
                    fieldDetails = new JObject();
                    fieldDetails["columnName"] = "FldStr_1";
                    fieldDetails["value"] = fill.FldStr_1;
                    payload.Add(fieldDetails);
                    fieldDetails = new JObject();
                    fieldDetails["columnName"] = "FldStr_2";
                    fieldDetails["value"] = fill.FldStr_2;
                    payload.Add(fieldDetails);
                    fieldDetails = new JObject();
                    fieldDetails["columnName"] = "FldStr_3";
                    fieldDetails["value"] = fill.FldStr_3;
                    payload.Add(fieldDetails);
                    fieldDetails = new JObject();
                    fieldDetails["columnName"] = "FldStr_4";
                    fieldDetails["value"] = fill.FldStr_4;
                    payload.Add(fieldDetails);
                    fieldDetails = new JObject();
                    fieldDetails["columnName"] = "FldStr_5";
                    fieldDetails["value"] = fill.FldStr_5;
                    payload.Add(fieldDetails);
                    fieldDetails = new JObject();
                    fieldDetails["columnName"] = "FldStr_6";
                    fieldDetails["value"] = fill.FldStr_6;
                    payload.Add(fieldDetails);
                    fieldDetails = new JObject();
                    fieldDetails["columnName"] = "FldStr_7";
                    fieldDetails["value"] = fill.FldStr_7;
                    payload.Add(fieldDetails);
                    fieldDetails = new JObject();
                    fieldDetails["columnName"] = "FldStr_8";
                    fieldDetails["value"] = fill.FldStr_8;
                    payload.Add(fieldDetails);
                    fieldDetails = new JObject();
                    fieldDetails["columnName"] = "FldStr_9";
                    fieldDetails["value"] = fill.FldStr_9;
                    payload.Add(fieldDetails);
                    fieldDetails = new JObject();
                    fieldDetails["columnName"] = "FldStr_10";
                    fieldDetails["value"] = fill.FldStr_10;
                    payload.Add(fieldDetails);
                    fieldDetails = new JObject();
                    fieldDetails["columnName"] = "FldStr_11";
                    fieldDetails["value"] = fill.FldStr_11;
                    payload.Add(fieldDetails);
                    fieldDetails = new JObject();
                    fieldDetails["columnName"] = "FldStr_12";
                    fieldDetails["value"] = fill.FldStr_12;
                    payload.Add(fieldDetails);
                    fieldDetails = new JObject();
                    fieldDetails["columnName"] = "FldStr_13";
                    fieldDetails["value"] = fill.FldStr_13;
                    payload.Add(fieldDetails);
                    fieldDetails = new JObject();
                    fieldDetails["columnName"] = "FldStr_14";
                    fieldDetails["value"] = fill.FldStr_14;
                    payload.Add(fieldDetails);
                    fieldDetails = new JObject();
                    fieldDetails["columnName"] = "FldStr_15";
                    fieldDetails["value"] = fill.FldStr_15;
                    payload.Add(fieldDetails);
                    fieldDetails = new JObject();
                    fieldDetails["columnName"] = "FldStr_16";
                    fieldDetails["value"] = fill.FldStr_16;
                    payload.Add(fieldDetails);
                    fieldDetails = new JObject();
                    fieldDetails["columnName"] = "FldStr_17";
                    fieldDetails["value"] = fill.FldStr_17;
                    payload.Add(fieldDetails);
                    fieldDetails = new JObject();
                    fieldDetails["columnName"] = "FldStr_18";
                    fieldDetails["value"] = fill.FldStr_18;
                    payload.Add(fieldDetails);
                    fieldDetails = new JObject();
                    fieldDetails["columnName"] = "FldStr_19";
                    fieldDetails["value"] = fill.FldStr_19;
                    payload.Add(fieldDetails);
                    fieldDetails = new JObject();
                    fieldDetails["columnName"] = "FldStr_20";
                    fieldDetails["value"] = fill.FldStr_20;
                    payload.Add(fieldDetails);
                    fieldDetails = new JObject();
                    fieldDetails["columnName"] = "FldStr_21";
                    fieldDetails["value"] = fill.FldStr_21;
                    payload.Add(fieldDetails);
                    fieldDetails = new JObject();
                    fieldDetails["columnName"] = "FldStr_22";
                    fieldDetails["value"] = fill.FldStr_22;
                    payload.Add(fieldDetails);
                    fieldDetails = new JObject();
                    fieldDetails["columnName"] = "FldStr_23";
                    fieldDetails["value"] = fill.FldStr_23;
                    payload.Add(fieldDetails);
                    fieldDetails = new JObject();
                    fieldDetails["columnName"] = "FldStr_24";
                    fieldDetails["value"] = fill.FldStr_24;
                    payload.Add(fieldDetails);
                    fieldDetails = new JObject();
                    fieldDetails["columnName"] = "FldStr_25";
                    fieldDetails["value"] = fill.FldStr_25;
                    payload.Add(fieldDetails);
                    fieldDetails = new JObject();
                    fieldDetails["columnName"] = "FldStr_26";
                    fieldDetails["value"] = fill.FldStr_26;
                    payload.Add(fieldDetails);
                    fieldDetails = new JObject();
                    fieldDetails["columnName"] = "FldStr_27";
                    fieldDetails["value"] = fill.FldStr_27;
                    payload.Add(fieldDetails);
                    fieldDetails = new JObject();
                    fieldDetails["columnName"] = "FldStr_28";
                    fieldDetails["value"] = fill.FldStr_28;
                    payload.Add(fieldDetails);
                    fieldDetails = new JObject();
                    fieldDetails["columnName"] = "FldStr_29";
                    fieldDetails["value"] = fill.FldStr_29;
                    payload.Add(fieldDetails);
                    fieldDetails = new JObject();
                    fieldDetails["columnName"] = "FldStr_30";
                    fieldDetails["value"] = fill.FldStr_30;
                    payload.Add(fieldDetails);
                    fieldDetails = new JObject();
                    fieldDetails["columnName"] = "FldStr_31";
                    fieldDetails["value"] = fill.FldStr_31;
                    payload.Add(fieldDetails);
                    fieldDetails = new JObject();
                    fieldDetails["columnName"] = "FldStr_32";
                    fieldDetails["value"] = fill.FldStr_32;
                    payload.Add(fieldDetails);
                    fieldDetails = new JObject();
                    fieldDetails["columnName"] = "FldStr_33";
                    fieldDetails["value"] = fill.FldStr_33;
                    payload.Add(fieldDetails);
                    fieldDetails = new JObject();
                    fieldDetails["columnName"] = "FldStr_34";
                    fieldDetails["value"] = fill.FldStr_34;
                    payload.Add(fieldDetails);
                    fieldDetails = new JObject();
                    fieldDetails["columnName"] = "FldStr_35";
                    fieldDetails["value"] = fill.FldStr_35;
                    payload.Add(fieldDetails);
                    fieldDetails = new JObject();
                    fieldDetails["columnName"] = "FldStr_36";
                    fieldDetails["value"] = fill.FldStr_36;
                    payload.Add(fieldDetails);
                    fieldDetails = new JObject();
                    fieldDetails["columnName"] = "FldStr_37";
                    fieldDetails["value"] = fill.FldStr_37;
                    payload.Add(fieldDetails);
                    fieldDetails = new JObject();
                    fieldDetails["columnName"] = "FldStr_38";
                    fieldDetails["value"] = fill.FldStr_38;
                    payload.Add(fieldDetails);
                    fieldDetails = new JObject();
                    fieldDetails["columnName"] = "FldStr_39";
                    fieldDetails["value"] = fill.FldStr_39;
                    payload.Add(fieldDetails);
                    fieldDetails = new JObject();
                    fieldDetails["columnName"] = "FldStr_40";
                    fieldDetails["value"] = fill.FldStr_40;
                    payload.Add(fieldDetails);
                    fieldDetails = new JObject();
                    fieldDetails["columnName"] = "FldStr_41";
                    fieldDetails["value"] = fill.FldStr_41;
                    payload.Add(fieldDetails);
                    fieldDetails = new JObject();
                    fieldDetails["columnName"] = "FldStr_42";
                    fieldDetails["value"] = fill.FldStr_42;
                    payload.Add(fieldDetails);
                    fieldDetails = new JObject();
                    fieldDetails["columnName"] = "FldStr_43";
                    fieldDetails["value"] = fill.FldStr_43;
                    payload.Add(fieldDetails);
                    fieldDetails = new JObject();
                    fieldDetails["columnName"] = "FldStr_44";
                    fieldDetails["value"] = fill.FldStr_44;
                    payload.Add(fieldDetails);
                    fieldDetails = new JObject();
                    fieldDetails["columnName"] = "FldStr_45";
                    fieldDetails["value"] = fill.FldStr_45;
                    payload.Add(fieldDetails);
                    fieldDetails = new JObject();
                    fieldDetails["columnName"] = "FldStr_46";
                    fieldDetails["value"] = fill.FldStr_46;
                    payload.Add(fieldDetails);
                    fieldDetails = new JObject();
                    fieldDetails["columnName"] = "FldStr_47";
                    fieldDetails["value"] = fill.FldStr_47;
                    payload.Add(fieldDetails);
                    fieldDetails = new JObject();
                    fieldDetails["columnName"] = "FldStr_48";
                    fieldDetails["value"] = fill.FldStr_48;
                    payload.Add(fieldDetails);
                    fieldDetails = new JObject();
                    fieldDetails["columnName"] = "FldStr_49";
                    fieldDetails["value"] = fill.FldStr_49;
                    payload.Add(fieldDetails);
                    fieldDetails = new JObject();
                    fieldDetails["columnName"] = "FldStr_50";
                    fieldDetails["value"] = fill.FldStr_50;
                    payload.Add(fieldDetails);
                }
                if (order != null) {
                    JObject fieldDetails = new JObject();
                    fieldDetails["columnName"] = "OrderID";
                    fieldDetails["value"] = order.OrderID;
                    payload.Add(fieldDetails);
                    fieldDetails = new JObject();
                    fieldDetails["columnName"] = "TraderID";
                    fieldDetails["value"] = order.TraderID;
                    payload.Add(fieldDetails);
                    fieldDetails = new JObject();
                    fieldDetails["columnName"] = "Symbol";
                    fieldDetails["value"] = order.Symbol;
                    payload.Add(fieldDetails);
                    fieldDetails = new JObject();
                    fieldDetails["columnName"] = "Quantity";
                    fieldDetails["value"] = order.Quantity;
                    payload.Add(fieldDetails);
                    fieldDetails = new JObject();
                    fieldDetails["columnName"] = "CumQty";
                    fieldDetails["value"] = order.CumQty;
                    payload.Add(fieldDetails);
                    fieldDetails = new JObject();
                    fieldDetails["columnName"] = "Price";
                    fieldDetails["value"] = order.Price;
                    payload.Add(fieldDetails);
                    fieldDetails = new JObject();
                    fieldDetails["columnName"] = "CalCumQty";
                    fieldDetails["value"] = order.CalCumQty;
                    payload.Add(fieldDetails);
                    fieldDetails = new JObject();
                    fieldDetails["columnName"] = "CalExecValue";
                    fieldDetails["value"] = order.CalExecValue;
                    payload.Add(fieldDetails);
                    fieldDetails = new JObject();
                    fieldDetails["columnName"] = "FldInt_1";
                    fieldDetails["value"] = order.FldInt_1;
                    payload.Add(fieldDetails);
                    fieldDetails = new JObject();
                    fieldDetails["columnName"] = "FldInt_2";
                    fieldDetails["value"] = order.FldInt_2;
                    payload.Add(fieldDetails);
                    fieldDetails = new JObject();
                    fieldDetails["columnName"] = "FldInt_3";
                    fieldDetails["value"] = order.FldInt_3;
                    payload.Add(fieldDetails);
                    fieldDetails = new JObject();
                    fieldDetails["columnName"] = "FldInt_4";
                    fieldDetails["value"] = order.FldInt_4;
                    payload.Add(fieldDetails);
                    fieldDetails = new JObject();
                    fieldDetails["columnName"] = "FldInt_5";
                    fieldDetails["value"] = order.FldInt_5;
                    payload.Add(fieldDetails);
                    fieldDetails = new JObject();
                    fieldDetails["columnName"] = "FldInt_6";
                    fieldDetails["value"] = order.FldInt_6;
                    payload.Add(fieldDetails);
                    fieldDetails = new JObject();
                    fieldDetails["columnName"] = "FldInt_7";
                    fieldDetails["value"] = order.FldInt_7;
                    payload.Add(fieldDetails);
                    fieldDetails = new JObject();
                    fieldDetails["columnName"] = "FldInt_8";
                    fieldDetails["value"] = order.FldInt_8;
                    payload.Add(fieldDetails);
                    fieldDetails = new JObject();
                    fieldDetails["columnName"] = "FldInt_9";
                    fieldDetails["value"] = order.FldInt_9;
                    payload.Add(fieldDetails);
                    fieldDetails = new JObject();
                    fieldDetails["columnName"] = "FldInt_10";
                    fieldDetails["value"] = order.FldInt_10;
                    payload.Add(fieldDetails);
                    fieldDetails = new JObject();
                    fieldDetails["columnName"] = "FldInt_11";
                    fieldDetails["value"] = order.FldInt_11;
                    payload.Add(fieldDetails);
                    fieldDetails = new JObject();
                    fieldDetails["columnName"] = "FldInt_12";
                    fieldDetails["value"] = order.FldInt_12;
                    payload.Add(fieldDetails);
                    fieldDetails = new JObject();
                    fieldDetails["columnName"] = "FldInt_13";
                    fieldDetails["value"] = order.FldInt_13;
                    payload.Add(fieldDetails);
                    fieldDetails = new JObject();
                    fieldDetails["columnName"] = "FldInt_14";
                    fieldDetails["value"] = order.FldInt_14;
                    payload.Add(fieldDetails);
                    fieldDetails = new JObject();
                    fieldDetails["columnName"] = "FldInt_15";
                    fieldDetails["value"] = order.FldInt_15;
                    payload.Add(fieldDetails);
                    fieldDetails = new JObject();
                    fieldDetails["columnName"] = "FldInt_16";
                    fieldDetails["value"] = order.FldInt_16;
                    payload.Add(fieldDetails);
                    fieldDetails = new JObject();
                    fieldDetails["columnName"] = "FldInt_17";
                    fieldDetails["value"] = order.FldInt_17;
                    payload.Add(fieldDetails);
                    fieldDetails = new JObject();
                    fieldDetails["columnName"] = "FldInt_18";
                    fieldDetails["value"] = order.FldInt_18;
                    payload.Add(fieldDetails);
                    fieldDetails = new JObject();
                    fieldDetails["columnName"] = "FldInt_19";
                    fieldDetails["value"] = order.FldInt_19;
                    payload.Add(fieldDetails);
                    fieldDetails = new JObject();
                    fieldDetails["columnName"] = "FldInt_20";
                    fieldDetails["value"] = order.FldInt_20;
                    payload.Add(fieldDetails);
                    fieldDetails = new JObject();
                    fieldDetails["columnName"] = "FldInt_21";
                    fieldDetails["value"] = order.FldInt_21;
                    payload.Add(fieldDetails);
                    fieldDetails = new JObject();
                    fieldDetails["columnName"] = "FldInt_22";
                    fieldDetails["value"] = order.FldInt_22;
                    payload.Add(fieldDetails);
                    fieldDetails = new JObject();
                    fieldDetails["columnName"] = "FldInt_23";
                    fieldDetails["value"] = order.FldInt_23;
                    payload.Add(fieldDetails);
                    fieldDetails = new JObject();
                    fieldDetails["columnName"] = "FldInt_24";
                    fieldDetails["value"] = order.FldInt_24;
                    payload.Add(fieldDetails);
                    fieldDetails = new JObject();
                    fieldDetails["columnName"] = "FldInt_25";
                    fieldDetails["value"] = order.FldInt_25;
                    payload.Add(fieldDetails);
                    fieldDetails = new JObject();
                    fieldDetails["columnName"] = "FldInt_26";
                    fieldDetails["value"] = order.FldInt_26;
                    payload.Add(fieldDetails);
                    fieldDetails = new JObject();
                    fieldDetails["columnName"] = "FldInt_27";
                    fieldDetails["value"] = order.FldInt_27;
                    payload.Add(fieldDetails);
                    fieldDetails = new JObject();
                    fieldDetails["columnName"] = "FldInt_28";
                    fieldDetails["value"] = order.FldInt_28;
                    payload.Add(fieldDetails);
                    fieldDetails = new JObject();
                    fieldDetails["columnName"] = "FldInt_29";
                    fieldDetails["value"] = order.FldInt_29;
                    payload.Add(fieldDetails);
                    fieldDetails = new JObject();
                    fieldDetails["columnName"] = "FldInt_30";
                    fieldDetails["value"] = order.FldInt_30;
                    payload.Add(fieldDetails);
                    fieldDetails = new JObject();
                    fieldDetails["columnName"] = "FldInt_31";
                    fieldDetails["value"] = order.FldInt_31;
                    payload.Add(fieldDetails);
                    fieldDetails = new JObject();
                    fieldDetails["columnName"] = "FldInt_32";
                    fieldDetails["value"] = order.FldInt_32;
                    payload.Add(fieldDetails);
                    fieldDetails = new JObject();
                    fieldDetails["columnName"] = "FldInt_33";
                    fieldDetails["value"] = order.FldInt_33;
                    payload.Add(fieldDetails);
                    fieldDetails = new JObject();
                    fieldDetails["columnName"] = "FldInt_34";
                    fieldDetails["value"] = order.FldInt_34;
                    payload.Add(fieldDetails);
                    fieldDetails = new JObject();
                    fieldDetails["columnName"] = "FldInt_35";
                    fieldDetails["value"] = order.FldInt_35;
                    payload.Add(fieldDetails);
                    fieldDetails = new JObject();
                    fieldDetails["columnName"] = "FldInt_36";
                    fieldDetails["value"] = order.FldInt_36;
                    payload.Add(fieldDetails);
                    fieldDetails = new JObject();
                    fieldDetails["columnName"] = "FldInt_37";
                    fieldDetails["value"] = order.FldInt_37;
                    payload.Add(fieldDetails);
                    fieldDetails = new JObject();
                    fieldDetails["columnName"] = "FldInt_38";
                    fieldDetails["value"] = order.FldInt_38;
                    payload.Add(fieldDetails);
                    fieldDetails = new JObject();
                    fieldDetails["columnName"] = "FldInt_39";
                    fieldDetails["value"] = order.FldInt_39;
                    payload.Add(fieldDetails);
                    fieldDetails = new JObject();
                    fieldDetails["columnName"] = "FldInt_40";
                    fieldDetails["value"] = order.FldInt_40;
                    payload.Add(fieldDetails);
                    fieldDetails = new JObject();
                    fieldDetails["columnName"] = "FldTime_1";
                    fieldDetails["value"] = order.FldTime_1;
                    payload.Add(fieldDetails);
                    fieldDetails = new JObject();
                    fieldDetails["columnName"] = "FldTime_2";
                    fieldDetails["value"] = order.FldTime_2;
                    payload.Add(fieldDetails);
                    fieldDetails = new JObject();
                    fieldDetails["columnName"] = "FldTime_3";
                    fieldDetails["value"] = order.FldTime_3;
                    payload.Add(fieldDetails);
                    fieldDetails = new JObject();
                    fieldDetails["columnName"] = "FldTime_4";
                    fieldDetails["value"] = order.FldTime_4;
                    payload.Add(fieldDetails);
                    fieldDetails = new JObject();
                    fieldDetails["columnName"] = "FldTime_5";
                    fieldDetails["value"] = order.FldTime_5;
                    payload.Add(fieldDetails);
                    fieldDetails = new JObject();
                    fieldDetails["columnName"] = "FldTime_6";
                    fieldDetails["value"] = order.FldTime_6;
                    payload.Add(fieldDetails);
                    fieldDetails = new JObject();
                    fieldDetails["columnName"] = "FldTime_7";
                    fieldDetails["value"] = order.FldTime_7;
                    payload.Add(fieldDetails);
                    fieldDetails = new JObject();
                    fieldDetails["columnName"] = "FldTime_8";
                    fieldDetails["value"] = order.FldTime_8;
                    payload.Add(fieldDetails);
                    fieldDetails = new JObject();
                    fieldDetails["columnName"] = "FldStr_1";
                    fieldDetails["value"] = order.FldStr_1;
                    payload.Add(fieldDetails);
                    fieldDetails = new JObject();
                    fieldDetails["columnName"] = "FldStr_2";
                    fieldDetails["value"] = order.FldStr_2;
                    payload.Add(fieldDetails);
                    fieldDetails = new JObject();
                    fieldDetails["columnName"] = "FldStr_3";
                    fieldDetails["value"] = order.FldStr_3;
                    payload.Add(fieldDetails);
                    fieldDetails = new JObject();
                    fieldDetails["columnName"] = "FldStr_4";
                    fieldDetails["value"] = order.FldStr_4;
                    payload.Add(fieldDetails);
                    fieldDetails = new JObject();
                    fieldDetails["columnName"] = "FldStr_5";
                    fieldDetails["value"] = order.FldStr_5;
                    payload.Add(fieldDetails);
                    fieldDetails = new JObject();
                    fieldDetails["columnName"] = "FldStr_6";
                    fieldDetails["value"] = order.FldStr_6;
                    payload.Add(fieldDetails);
                    fieldDetails = new JObject();
                    fieldDetails["columnName"] = "FldStr_7";
                    fieldDetails["value"] = order.FldStr_7;
                    payload.Add(fieldDetails);
                    fieldDetails = new JObject();
                    fieldDetails["columnName"] = "FldStr_8";
                    fieldDetails["value"] = order.FldStr_8;
                    payload.Add(fieldDetails);
                    fieldDetails = new JObject();
                    fieldDetails["columnName"] = "FldStr_9";
                    fieldDetails["value"] = order.FldStr_9;
                    payload.Add(fieldDetails);
                    fieldDetails = new JObject();
                    fieldDetails["columnName"] = "FldStr_10";
                    fieldDetails["value"] = order.FldStr_10;
                    payload.Add(fieldDetails);
                    fieldDetails = new JObject();
                    fieldDetails["columnName"] = "FldStr_11";
                    fieldDetails["value"] = order.FldStr_11;
                    payload.Add(fieldDetails);
                    fieldDetails = new JObject();
                    fieldDetails["columnName"] = "FldStr_12";
                    fieldDetails["value"] = order.FldStr_12;
                    payload.Add(fieldDetails);
                    fieldDetails = new JObject();
                    fieldDetails["columnName"] = "FldStr_13";
                    fieldDetails["value"] = order.FldStr_13;
                    payload.Add(fieldDetails);
                    fieldDetails = new JObject();
                    fieldDetails["columnName"] = "FldStr_14";
                    fieldDetails["value"] = order.FldStr_14;
                    payload.Add(fieldDetails);
                    fieldDetails = new JObject();
                    fieldDetails["columnName"] = "FldStr_15";
                    fieldDetails["value"] = order.FldStr_15;
                    payload.Add(fieldDetails);
                    fieldDetails = new JObject();
                    fieldDetails["columnName"] = "FldStr_16";
                    fieldDetails["value"] = order.FldStr_16;
                    payload.Add(fieldDetails);
                    fieldDetails = new JObject();
                    fieldDetails["columnName"] = "FldStr_17";
                    fieldDetails["value"] = order.FldStr_17;
                    payload.Add(fieldDetails);
                    fieldDetails = new JObject();
                    fieldDetails["columnName"] = "FldStr_18";
                    fieldDetails["value"] = order.FldStr_18;
                    payload.Add(fieldDetails);
                    fieldDetails = new JObject();
                    fieldDetails["columnName"] = "FldStr_19";
                    fieldDetails["value"] = order.FldStr_19;
                    payload.Add(fieldDetails);
                    fieldDetails = new JObject();
                    fieldDetails["columnName"] = "FldStr_20";
                    fieldDetails["value"] = order.FldStr_20;
                    payload.Add(fieldDetails);
                    fieldDetails = new JObject();
                    fieldDetails["columnName"] = "FldStr_21";
                    fieldDetails["value"] = order.FldStr_21;
                    payload.Add(fieldDetails);
                    fieldDetails = new JObject();
                    fieldDetails["columnName"] = "FldStr_22";
                    fieldDetails["value"] = order.FldStr_22;
                    payload.Add(fieldDetails);
                    fieldDetails = new JObject();
                    fieldDetails["columnName"] = "FldStr_23";
                    fieldDetails["value"] = order.FldStr_23;
                    payload.Add(fieldDetails);
                    fieldDetails = new JObject();
                    fieldDetails["columnName"] = "FldStr_24";
                    fieldDetails["value"] = order.FldStr_24;
                    payload.Add(fieldDetails);
                    fieldDetails = new JObject();
                    fieldDetails["columnName"] = "FldStr_25";
                    fieldDetails["value"] = order.FldStr_25;
                    payload.Add(fieldDetails);
                    fieldDetails = new JObject();
                    fieldDetails["columnName"] = "FldStr_26";
                    fieldDetails["value"] = order.FldStr_26;
                    payload.Add(fieldDetails);
                    fieldDetails = new JObject();
                    fieldDetails["columnName"] = "FldStr_27";
                    fieldDetails["value"] = order.FldStr_27;
                    payload.Add(fieldDetails);
                    fieldDetails = new JObject();
                    fieldDetails["columnName"] = "FldStr_28";
                    fieldDetails["value"] = order.FldStr_28;
                    payload.Add(fieldDetails);
                    fieldDetails = new JObject();
                    fieldDetails["columnName"] = "FldStr_29";
                    fieldDetails["value"] = order.FldStr_29;
                    payload.Add(fieldDetails);
                    fieldDetails = new JObject();
                    fieldDetails["columnName"] = "FldStr_30";
                    fieldDetails["value"] = order.FldStr_30;
                    payload.Add(fieldDetails);
                    fieldDetails = new JObject();
                    fieldDetails["columnName"] = "FldStr_31";
                    fieldDetails["value"] = order.FldStr_31;
                    payload.Add(fieldDetails);
                    fieldDetails = new JObject();
                    fieldDetails["columnName"] = "FldStr_32";
                    fieldDetails["value"] = order.FldStr_32;
                    payload.Add(fieldDetails);
                    fieldDetails = new JObject();
                    fieldDetails["columnName"] = "FldStr_33";
                    fieldDetails["value"] = order.FldStr_33;
                    payload.Add(fieldDetails);
                    fieldDetails = new JObject();
                    fieldDetails["columnName"] = "FldStr_34";
                    fieldDetails["value"] = order.FldStr_34;
                    payload.Add(fieldDetails);
                    fieldDetails = new JObject();
                    fieldDetails["columnName"] = "FldStr_35";
                    fieldDetails["value"] = order.FldStr_35;
                    payload.Add(fieldDetails);
                    fieldDetails = new JObject();
                    fieldDetails["columnName"] = "FldStr_36";
                    fieldDetails["value"] = order.FldStr_36;
                    payload.Add(fieldDetails);
                    fieldDetails = new JObject();
                    fieldDetails["columnName"] = "FldStr_37";
                    fieldDetails["value"] = order.FldStr_37;
                    payload.Add(fieldDetails);
                    fieldDetails = new JObject();
                    fieldDetails["columnName"] = "FldStr_38";
                    fieldDetails["value"] = order.FldStr_38;
                    payload.Add(fieldDetails);
                    fieldDetails = new JObject();
                    fieldDetails["columnName"] = "FldStr_39";
                    fieldDetails["value"] = order.FldStr_39;
                    payload.Add(fieldDetails);
                    fieldDetails = new JObject();
                    fieldDetails["columnName"] = "FldStr_40";
                    fieldDetails["value"] = order.FldStr_40;
                    payload.Add(fieldDetails);
                    fieldDetails = new JObject();
                    fieldDetails["columnName"] = "FldDbl_1";
                    fieldDetails["value"] = order.FldDbl_1;
                    payload.Add(fieldDetails);
                    fieldDetails = new JObject();
                    fieldDetails["columnName"] = "FldDbl_2";
                    fieldDetails["value"] = order.FldDbl_2;
                    payload.Add(fieldDetails);
                    fieldDetails = new JObject();
                    fieldDetails["columnName"] = "FldDbl_3";
                    fieldDetails["value"] = order.FldDbl_3;
                    payload.Add(fieldDetails);
                    fieldDetails = new JObject();
                    fieldDetails["columnName"] = "FldDbl_4";
                    fieldDetails["value"] = order.FldDbl_4;
                    payload.Add(fieldDetails);
                    fieldDetails = new JObject();
                    fieldDetails["columnName"] = "FldDbl_5";
                    fieldDetails["value"] = order.FldDbl_5;
                    payload.Add(fieldDetails);
                    fieldDetails = new JObject();
                    fieldDetails["columnName"] = "FldDbl_6";
                    fieldDetails["value"] = order.FldDbl_6;
                    payload.Add(fieldDetails);
                    fieldDetails = new JObject();
                    fieldDetails["columnName"] = "FldDbl_7";
                    fieldDetails["value"] = order.FldDbl_7;
                    payload.Add(fieldDetails);
                    fieldDetails = new JObject();
                    fieldDetails["columnName"] = "FldDbl_8";
                    fieldDetails["value"] = order.FldDbl_8;
                    payload.Add(fieldDetails);
                    fieldDetails = new JObject();
                    fieldDetails["columnName"] = "FldDbl_9";
                    fieldDetails["value"] = order.FldDbl_9;
                    payload.Add(fieldDetails);
                    fieldDetails = new JObject();
                    fieldDetails["columnName"] = "FldDbl_10";
                    fieldDetails["value"] = order.FldDbl_10;
                    payload.Add(fieldDetails);
                    fieldDetails = new JObject();
                    fieldDetails["columnName"] = "FldDbl_11";
                    fieldDetails["value"] = order.FldDbl_11;
                    payload.Add(fieldDetails);
                    fieldDetails = new JObject();
                    fieldDetails["columnName"] = "FldDbl_12";
                    fieldDetails["value"] = order.FldDbl_12;
                    payload.Add(fieldDetails);
                    fieldDetails = new JObject();
                    fieldDetails["columnName"] = "FldDbl_13";
                    fieldDetails["value"] = order.FldDbl_13;
                    payload.Add(fieldDetails);
                    fieldDetails = new JObject();
                    fieldDetails["columnName"] = "FldDbl_14";
                    fieldDetails["value"] = order.FldDbl_14;
                    payload.Add(fieldDetails);
                    fieldDetails = new JObject();
                    fieldDetails["columnName"] = "FldDbl_15";
                    fieldDetails["value"] = order.FldDbl_15;
                    payload.Add(fieldDetails);
                    fieldDetails = new JObject();
                    fieldDetails["columnName"] = "FldDbl_16";
                    fieldDetails["value"] = order.FldDbl_16;
                    payload.Add(fieldDetails);
                    fieldDetails = new JObject();
                    fieldDetails["columnName"] = "FldDbl_17";
                    fieldDetails["value"] = order.FldDbl_17;
                    payload.Add(fieldDetails);
                    fieldDetails = new JObject();
                    fieldDetails["columnName"] = "FldDbl_18";
                    fieldDetails["value"] = order.FldDbl_18;
                    payload.Add(fieldDetails);
                    fieldDetails = new JObject();
                    fieldDetails["columnName"] = "FldDbl_19";
                    fieldDetails["value"] = order.FldDbl_19;
                    payload.Add(fieldDetails);
                    fieldDetails = new JObject();
                    fieldDetails["columnName"] = "FldDbl_20";
                    fieldDetails["value"] = order.FldDbl_20;
                    payload.Add(fieldDetails);
                    fieldDetails = new JObject();
                    fieldDetails["columnName"] = "FldDbl_21";
                    fieldDetails["value"] = order.FldDbl_21;
                    payload.Add(fieldDetails);
                    fieldDetails = new JObject();
                    fieldDetails["columnName"] = "FldDbl_22";
                    fieldDetails["value"] = order.FldDbl_22;
                    payload.Add(fieldDetails);
                    fieldDetails = new JObject();
                    fieldDetails["columnName"] = "FldDbl_23";
                    fieldDetails["value"] = order.FldDbl_23;
                    payload.Add(fieldDetails);
                    fieldDetails = new JObject();
                    fieldDetails["columnName"] = "FldDbl_24";
                    fieldDetails["value"] = order.FldDbl_24;
                    payload.Add(fieldDetails);
                    fieldDetails = new JObject();
                    fieldDetails["columnName"] = "FldDbl_25";
                    fieldDetails["value"] = order.FldDbl_25;
                    payload.Add(fieldDetails);
                    fieldDetails = new JObject();
                    fieldDetails["columnName"] = "FldDbl_26";
                    fieldDetails["value"] = order.FldDbl_26;
                    payload.Add(fieldDetails);
                    fieldDetails = new JObject();
                    fieldDetails["columnName"] = "FldDbl_27";
                    fieldDetails["value"] = order.FldDbl_27;
                    payload.Add(fieldDetails);
                    fieldDetails = new JObject();
                    fieldDetails["columnName"] = "FldDbl_28";
                    fieldDetails["value"] = order.FldDbl_28;
                    payload.Add(fieldDetails);
                    fieldDetails = new JObject();
                    fieldDetails["columnName"] = "FldDbl_29";
                    fieldDetails["value"] = order.FldDbl_29;
                    payload.Add(fieldDetails);
                    fieldDetails = new JObject();
                    fieldDetails["columnName"] = "FldDbl_30";
                    fieldDetails["value"] = order.FldDbl_30;
                    payload.Add(fieldDetails);
                    fieldDetails = new JObject();
                    fieldDetails["columnName"] = "FldDbl_31";
                    fieldDetails["value"] = order.FldDbl_31;
                    payload.Add(fieldDetails);
                    fieldDetails = new JObject();
                    fieldDetails["columnName"] = "FldDbl_32";
                    fieldDetails["value"] = order.FldDbl_32;
                    payload.Add(fieldDetails);
                    fieldDetails = new JObject();
                    fieldDetails["columnName"] = "FldDbl_33";
                    fieldDetails["value"] = order.FldDbl_33;
                    payload.Add(fieldDetails);
                    fieldDetails = new JObject();
                    fieldDetails["columnName"] = "FldDbl_34";
                    fieldDetails["value"] = order.FldDbl_34;
                    payload.Add(fieldDetails);
                    fieldDetails = new JObject();
                    fieldDetails["columnName"] = "FldDbl_35";
                    fieldDetails["value"] = order.FldDbl_35;
                    payload.Add(fieldDetails);
                    fieldDetails = new JObject();
                    fieldDetails["columnName"] = "FldDbl_36";
                    fieldDetails["value"] = order.FldDbl_36;
                    payload.Add(fieldDetails);
                    fieldDetails = new JObject();
                    fieldDetails["columnName"] = "FldDbl_37";
                    fieldDetails["value"] = order.FldDbl_37;
                    payload.Add(fieldDetails);
                    fieldDetails = new JObject();
                    fieldDetails["columnName"] = "FldDbl_38";
                    fieldDetails["value"] = order.FldDbl_38;
                    payload.Add(fieldDetails);
                    fieldDetails = new JObject();
                    fieldDetails["columnName"] = "FldDbl_39";
                    fieldDetails["value"] = order.FldDbl_39;
                    payload.Add(fieldDetails);
                    fieldDetails = new JObject();
                    fieldDetails["columnName"] = "FldDbl_40";
                    fieldDetails["value"] = order.FldDbl_40;
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
