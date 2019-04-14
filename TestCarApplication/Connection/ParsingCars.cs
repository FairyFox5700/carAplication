using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using HtmlAgilityPack;
using System.Net;
using CarApplication.Entities;
using Json2KeyValue;

namespace CarApplication.Connection
{
    class ParsingCars
    {
        static readonly string Url = "http://www.autosvit.com.ua/tech.php";
        private static WebClient WebClient = new WebClient();
        private static HtmlDocument Doc = new HtmlDocument();
        private static List<string> listOfMarksHrefs = new List<string>();
        private static List<string> listOfFamiliesHrefs = new List<string>();
        private static List<string> listOfSubFamiliesHrefs = new List<string>();
        private static List<string> listOfModelsHrefs = new List<string>();
        private static List<TradeMark> tradeMarks = new List<TradeMark>();
        private static List<Family> families = new List<Family>();
        private static List<SubFamily> subFamilies = new List<SubFamily>();
        private static List<Model> models = new List<Model>();
        private static List<TechData> techData = new List<TechData>();
        private static List<Task> tasksMark = new List<Task>();
        private static List<Task> tasksFamily = new List<Task>();
        private static List<Task> tasksModel = new List<Task>();
        private static List<Task> tasksTechData = new List<Task>();
        private static List<Task> tasksSubFamily = new List<Task>();

        public static List<TradeMark> GetListOfTradeMarks()
        {
            //Task.WaitAll(tasks.ToArray());
            return tradeMarks;
        }

        public static List<Family> GetListOfFamilies()
        {
            
            return families;
        }

        public static List<SubFamily> GetListOfSubFamilies()
        {
            
            return subFamilies;
        }
        public static List<Model> GetListOfModels()
        {
            
            return models;
        }
        public static List<TechData> GetListOfTEchInfo()
        {
           
            return techData;
        }

        public static string SerealizeDictionary(Dictionary<string, string> keyValuePairs)
        {
            return JsonConvert.SerializeObject(keyValuePairs);
        }
        public static object DeserealizeDictionary(string dictInJson)
        {
            return JsonConvert.DeserializeObject<Dictionary<string, string>>(dictInJson.ToString());
        }

        protected internal static Task taskUpdate;
        protected internal static Task taskFamily;
        protected internal static Task taskFFamily;
        protected internal static Task taskSubFamily;
        protected internal static Task taskModel;
      
    

        protected internal static void GetAutomobileMarks()
        {
            try
            {

                WebClient WebClient = new WebClient();
                string page = WebClient.DownloadString(Url);
                HtmlDocument Doc = new HtmlDocument();
                Doc.LoadHtml(page);

                var table = Doc.DocumentNode.SelectSingleNode("//table[@class='tck']");

                var rows = table.Descendants("tr");

                //Parallel.ForEach(rows, node =>
                foreach (var node in rows)
                {
                    var hrefs = node.SelectNodes(".//a");

                    var ghre = node.GetAttributeValue("href", "/tech");
                    string[] array = node.InnerText.Trim().Split(new[] { "\r\n", "\r", "\n" }, StringSplitOptions.RemoveEmptyEntries);
                    for (int i = 0; i < array.Length - 1; i++)
                    {


                        TradeMark tradeMark = new TradeMark
                        {
                            TradeMarkName = array[i],
                            TradeMarkUrl = "http://www.autosvit.com.ua/" + hrefs[i].GetAttributeValue("href", "/tech")
                        };
                        tradeMarks.Add(tradeMark);
                        // db.TradeMarks.Add(tradeMark);
                        // db.SaveChanges();
                        listOfMarksHrefs.Add(tradeMark.TradeMarkUrl);



                    }
                }
            }

            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
           
        }
        protected internal static void GetAutomobileFamily(string href, Guid id)
        {
            try
            {
               
                //Parallel.ForEach(listOfMarksHrefs, href =>
               
                WebClient WebClient = new WebClient();
                string page = WebClient.DownloadString(href);
                HtmlDocument Doc = new HtmlDocument();
                Doc.LoadHtml(page);

                var row = Doc.DocumentNode.SelectNodes("//table[@class='tck']");

                foreach (var node in row)
                //Parallel.ForEach(row, node =>
                {

                    var hrefs = node.SelectNodes(".//a");
                    if (hrefs != null)
                    {
                        //Parallel.ForEach(hrefs, link =>
                        foreach (var link in hrefs)
                        {
                            // using (CarDbModel db = new CarDbModel())
                            //{
                            Family carFamily = new Family
                            {
                                FamilyName = node.InnerText.Trim(),
                                FamilyUrl = "http://www.autosvit.com.ua/" + link.GetAttributeValue("href", "/tech"),
                                TradeMarkId = id
                            };
                            families.Add(carFamily);
                            //db.Families.Add(carFamily);
                            //db.SaveChanges();
                            listOfFamiliesHrefs.Add(carFamily.FamilyUrl);
                            

                        }
                     
                    }
                   
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        
        }


        protected internal static void GetAutomobileSubFamily(string href, Guid id)
        {
            try
            {
                //Parallel.ForEach()
                //Parallel.ForEach(listOfFamiliesHrefs, href =>

                WebClient WebClient = new WebClient();
                string page = WebClient.DownloadString(href);
                HtmlDocument Doc = new HtmlDocument();
                Doc.LoadHtml(page);
                var div = Doc.DocumentNode.SelectNodes("//div[@class='text']").Descendants("center");
                //Parallel.ForEach(div, elem =>
                foreach (var elem in div)
                {
                    var hrefs = elem.SelectNodes(".//a");
                    if (hrefs != null)
                    {
                        //Parallel.ForEach(hrefs, link =>
                        foreach (var link in hrefs)
                        {
                            // using (CarDbModel db = new CarDbModel())
                            // {
                            SubFamily subFamily = new SubFamily
                            {
                                SubFamilyName = elem.InnerText.Trim(),
                                SubFamilyUrl = "http://www.autosvit.com.ua/" + link.GetAttributeValue("href", "/tech"),
                                FamilyId = id
                            };
                            subFamilies.Add(subFamily);
                            // db.SubFamilies.Add(subFamily);
                            // db.SaveChanges();
                            listOfSubFamiliesHrefs.Add(subFamily.SubFamilyUrl);

                        }


                    }
                }
            }


            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            
        }
        protected internal static void GetAutomobileModel(string href, Guid id)
        {
            try
            {
                //Parallel.ForEach(listOfSubFamiliesHrefs, href =>

                WebClient WebClient = new WebClient();
                string page = WebClient.DownloadString(href);
                HtmlDocument Doc = new HtmlDocument();
                Doc.LoadHtml(page);
                var table = Doc.DocumentNode.SelectNodes("//table[@class='text']");
                if (table != null)
                {
                    var rows = table.Descendants("tr");
                    // Parallel.ForEach(rows, elem =>
                    foreach (var elem in rows)
                    {
                        if (elem != null)
                        {
                            var hrefs = elem.SelectNodes(".//a");
                            if (hrefs != null)
                            {
                                // using (CarDbModel db = new CarDbModel())
                                ///{
                                // Parallel.ForEach(hrefs, link =>
                                foreach (var link in hrefs)
                                {

                                    Model carModel = new Model
                                    {
                                        ModelName = elem.InnerText.Trim(),
                                        ModelUrl = "http://www.autosvit.com.ua/" + link.GetAttributeValue("href", "/tech"),
                                        SubFamilyId = id

                                    };
                                    // carModel.ModelProperties = GetAutomobileTechData(carModel.ModelUrl, carModel);
                                    models.Add(carModel);


                                    // db.Models.Add(carModel);
                                    // db.SaveChanges();
                                    listOfModelsHrefs.Add(carModel.ModelUrl);

                                }

                            }
                        }


                    }
                }
            }
            
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            
        }
        protected internal static void GetAutomobileTechData(string href, Guid modelId)
        {
            try
            {
                
                Dictionary<string, string> itemProperties = new Dictionary<string, string>();
                //foreach (string href in listOfModelsHrefs)
                WebClient WebClient = new WebClient();
                string page = WebClient.DownloadString(href);
                HtmlDocument Doc = new HtmlDocument();
                Doc.LoadHtml(page);
                var rows = Doc.DocumentNode.SelectNodes("//table[@class='text']").Descendants("tr");
                //Parallel.ForEach(rows, elem =>
                foreach (var elem in rows)
                {
                    var count = elem.SelectNodes("./td").Count;
                    if (count > 1)
                    {
                        string propKey = elem.SelectSingleNode("./td").InnerText.Trim();
                        string propValue = elem.SelectSingleNode("./td[2]").InnerText.Trim();
                       
                        itemProperties[propKey] = propValue;
                     
                    }
                 
                }
                TechData techInfor = new TechData
                {
                    ModelId = modelId,
                    TechInfo = SerealizeDictionary(itemProperties)
                };
                techData.Add(techInfor);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
    }
}
