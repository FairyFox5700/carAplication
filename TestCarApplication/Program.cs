using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CarApplication.Entities;
using CarApplication.Connection;

namespace TestCarApplication
{
    class Program
    {
        static void Main(string[] args)
        {
          

            ParsingCars.GetAutomobileMarks();
            var marks = ParsingCars.GetListOfTradeMarks();
            using (var context = new CarEntitiesContext())
            {
                marks.ForEach(s => context.TradeMarks.Add(s));
                context.SaveChanges();
            }

        }
    }
}
