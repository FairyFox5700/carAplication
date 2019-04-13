using System;
using System.Collections.Generic;
using System.Text;

namespace CarApplication.Entities
{
    class TradeMark
    {
        public Guid TradeMarkId { get; set; } = Guid.NewGuid();
        public string TradeMarkUrl { get; set; }
        public string TradeMarkName { get; set; }
        public virtual ICollection<Family> CarFamilies { get; set; }
    }
}
