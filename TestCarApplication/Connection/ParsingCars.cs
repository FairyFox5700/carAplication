﻿using System;
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
        private static List<Task> tasks;
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

        protected internal static async Task ProcessUrlAsync(string url, WebClient webClient, HtmlDocument doc)
        {
            string page = await webClient.DownloadStringTaskAsync(url);
            doc.LoadHtml(page);

        }

        protected internal static async Task GetAutomobileMarks()
        {
            WebClient WebClient = new WebClient();
            HtmlDocument Doc = new HtmlDocument();
            string page = WebClient.DownloadString(Url);
            Doc.LoadHtml(page);
            var table = Doc.DocumentNode.SelectSingleNode("//table[@class='tck']");
            var rows = table.Descendants("tr");
            var tasks = new List<Task>();
            int count = 1;
            while (count-- > 0)
            {
                var taskToGetMarks = Task.Run(async () =>
                {
                    try
                    {
                       
                        
                        //Parallel.ForEach(rows, node =>
                        foreach (var node in rows)
                        {   //pages
                            var hrefs = node.SelectNodes(".//a");
                            string[] array = node.InnerText.Trim().Split(new[] { "\r\n", "\r", "\n" }, StringSplitOptions.RemoveEmptyEntries);
                            for (int i = 0; i < array.Length - 1; i++)
                            {
                                TradeMark tradeMark = new TradeMark
                                {
                                    TradeMarkName = array[i],
                                    TradeMarkUrl = "http://www.autosvit.com.ua/" + hrefs[i].GetAttributeValue("href", "/tech")
                                };
                                tradeMarks.Add(tradeMark);
                                Console.WriteLine(tradeMark.TradeMarkName);
                                listOfMarksHrefs.Add(tradeMark.TradeMarkUrl);
                            }
                        }
                        //is it better to write each item or list of items?
                        using (var context = new CarEntitiesContext())
                        {
                            tradeMarks.ForEach(s => context.TradeMarks.Add(s));
                            await context.SaveChangesAsync();
                        }
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }

                });
                
                tasks.Add(taskToGetMarks);
            }
           Task t = Task.WhenAll(tasks);
            try { 
            t.Wait();
        }
      catch (AggregateException)
      {}

    await GetAutomobileFamily();
        }
        protected internal static async Task GetAutomobileFamily()
        {
            WebClient WebClient = new WebClient();
            HtmlDocument Doc = new HtmlDocument();
            foreach (string href in listOfMarksHrefs)
            {
                var tasks = new List<Task>();
                int count = 10;
                while (count-- > 0)
                {
                    var taskToGetFamilies = Task.Run(async () =>
                    {
                        try
                        {
                            string page = await WebClient.DownloadStringTaskAsync(href);
                            Doc.LoadHtml(page);
                            var row = Doc.DocumentNode.SelectNodes("//table[@class='tck']");
                            foreach (var node in row)
                            {
                                var hrefs = node.SelectNodes(".//a");
                                if (hrefs != null)
                                {
                                    foreach (var link in hrefs)
                                    {
                                        Family carFamily = new Family
                                        {
                                            FamilyName = node.InnerText.Trim(),
                                            FamilyUrl = "http://www.autosvit.com.ua/" + link.GetAttributeValue("href", "/tech")
                                            //how i link mark with families?
                                            //TradeMarkId = id
                                        };
                                        families.Add(carFamily);
                                        listOfFamiliesHrefs.Add(carFamily.FamilyUrl);
                                    }
                                }
                            }
                            using (var context = new CarEntitiesContext())
                            {
                                families.ForEach(s => context.Families.Add(s));
                                await context.SaveChangesAsync();
                            }
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine(ex.Message);
                        }

                    });
                    tasks.Add(taskToGetFamilies);
                }
               Task t= Task.WhenAll(tasks);
                try
                {
                    await t.ContinueWith(task => { GetAutomobileSubFamily(); });
                }
                catch (AggregateException)
                { }

              
            }
        }

        protected internal static async Task GetAutomobileSubFamily()
        {
            WebClient WebClient = new WebClient();
            HtmlDocument Doc = new HtmlDocument();
            foreach (string href in listOfFamiliesHrefs)
            {
                var tasks = new List<Task>();
                int count = 10;
                while (count-- > 0)
                {
                    var taskToGetSubFamily = Task.Run(async () =>
                    {
                        try
                        {
                            string page = await WebClient.DownloadStringTaskAsync(href);
                            Doc.LoadHtml(page);
                            var div = Doc.DocumentNode.SelectNodes("//div[@class='text']").Descendants("center");
                            foreach (var elem in div)
                            {
                                var hrefs = elem.SelectNodes(".//a");
                                if (hrefs != null)
                                {
                                    foreach (var link in hrefs)
                                    {
                                        SubFamily subFamily = new SubFamily
                                        {
                                            SubFamilyName = elem.InnerText.Trim(),
                                            SubFamilyUrl = "http://www.autosvit.com.ua/" + link.GetAttributeValue("href", "/tech"),
                                            //FamilyId = id
                                        };
                                        subFamilies.Add(subFamily);
                                        listOfSubFamiliesHrefs.Add(subFamily.SubFamilyUrl);
                                    }
                                }
                            }
                            using (var context = new CarEntitiesContext())
                            {
                                subFamilies.ForEach(s => context.SubFamilies.Add(s));
                                await context.SaveChangesAsync();
                            }
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine(ex.Message);
                        }

                    });
                    tasks.Add(taskToGetSubFamily);
                }
                await Task.WhenAll(tasks);
                await GetAutomobileModel();
            }
        }

        protected internal static async Task GetAutomobileModel()
        {

            WebClient WebClient = new WebClient();
            HtmlDocument Doc = new HtmlDocument();
            foreach (string href in listOfSubFamiliesHrefs)
            {
                var tasks = new List<Task>();
                int count = 10;
                while (count-- > 0)
                {
                    var taskToGetModels = Task.Run(async () =>
                    {
                        try
                        {
                            string page = await WebClient.DownloadStringTaskAsync(href);
                            Doc.LoadHtml(page);
                            var table = Doc.DocumentNode.SelectNodes("//table[@class='text']");
                            if (table != null)
                            {
                                var rows = table.Descendants("tr");
                                foreach (var elem in rows)
                                {
                                    if (elem != null)
                                    {
                                        var hrefs = elem.SelectNodes(".//a");
                                        if (hrefs != null)
                                        {
                                            foreach (var link in hrefs)
                                            {
                                                Model carModel = new Model
                                                {
                                                    ModelName = elem.InnerText.Trim(),
                                                    ModelUrl = "http://www.autosvit.com.ua/" + link.GetAttributeValue("href", "/tech"),
                                                    // SubFamilyId = id
                                                };
                                                models.Add(carModel);
                                                listOfModelsHrefs.Add(carModel.ModelUrl);
                                            }
                                        }
                                    }
                                }
                                using (var context = new CarEntitiesContext())
                                {
                                    models.ForEach(s => context.Models.Add(s));
                                    await context.SaveChangesAsync();
                                }
                            }
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine(ex.Message);
                        }
                    });
                    tasks.Add(taskToGetModels);
                }
                await Task.WhenAll(tasks);
                await GetAutomobileTechData();
            }
        }
        protected internal static async Task GetAutomobileTechData()
        {
            WebClient WebClient = new WebClient();
            HtmlDocument Doc = new HtmlDocument();
            Dictionary<string, string> itemProperties = new Dictionary<string, string>();
            foreach (string href in listOfModelsHrefs)
            {
                var tasks = new List<Task>();
                int count = 10;
                while (count-- > 0)
                {
                    var taskToGetTechData = Task.Run(async () =>
                    {
                        try
                        {
                            string page = await WebClient.DownloadStringTaskAsync(href);
                            Doc.LoadHtml(page);
                            var rows = Doc.DocumentNode.SelectNodes("//table[@class='text']").Descendants("tr");
                            foreach (var elem in rows)
                            {
                                var countNode = elem.SelectNodes("./td").Count;
                                if (countNode > 1)
                                {
                                    string propKey = elem.SelectSingleNode("./td").InnerText.Trim();
                                    string propValue = elem.SelectSingleNode("./td[2]").InnerText.Trim();
                                    itemProperties[propKey] = propValue;
                                }
                                TechData techInfor = new TechData
                                {
                                    //ModelId = modelId,
                                    TechInfo = SerealizeDictionary(itemProperties)
                                };
                                techData.Add(techInfor);
                            }
                            using (var context = new CarEntitiesContext())
                            {
                                techData.ForEach(s => context.TechData.Add(s));
                                await context.SaveChangesAsync();
                            }
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine(ex.Message);
                        }
                    });
                    tasks.Add(taskToGetTechData);
                }
                await Task.WhenAll(tasks);

            }
        }
    }
}