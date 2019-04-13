using System;
using System.Collections.Generic;
using System.Text;

namespace CarApplication.Entities
{
    class Family
    {
        public Guid TradeMarkId { get; set; }
        public string FamilyUrl { get; set; }
        public Guid FamilyId { get; set; } = Guid.NewGuid();
        public string FamilyName { get; set; }
        public virtual ICollection<SubFamily> CarSubFamilies { get; set; }
        public virtual TradeMark TradeMark { get; set; }
    }
}
