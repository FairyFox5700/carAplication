using System;
using System.Collections.Generic;
using System.Text;
using CarApplication.Entities;
using CarApplication.Connection;

namespace CarApplication.Connection
{
    public class CarInitializer
    {
        public static void OnUpdate()
        {
            using (var context = new CarEntitiesContext())
            {
                context.Database.EnsureCreated();

                ParsingCars.GetAutomobileMarks();
               var marks = ParsingCars.GetListOfTradeMarks();
                var families = ParsingCars.GetListOfFamilies();
                var subFimilies = ParsingCars.GetListOfSubFamilies();
                var models = ParsingCars.GetListOfModels();
               var techData = ParsingCars.GetListOfTEchInfo();
                marks.ForEach(s => context.TradeMarks.Add(s));
                families.ForEach(s => context.Families.Add(s));
                subFimilies.ForEach(s => context.SubFamilies.Add(s));
                models.ForEach(s => context.Models.Add(s));
                techData.ForEach(s => context.TechData.Add(s));
                


            }
        }
        public static void PrintData()
        {
            using (var context = new CarEntitiesContext())
            {
                var car = context.TradeMarks;
                foreach (var data in car)
                {
                    var d = new StringBuilder();
                    d.AppendLine($"ISBN: {data.TradeMarkId}");
                }

            }
        }
    }
}
