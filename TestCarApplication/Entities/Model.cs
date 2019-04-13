using System;
using System.Collections.Generic;
using System.Text;

namespace CarApplication.Entities
{
    class Model
    {
        public Guid SubFamilyId { get; set; }
        public Guid ModelId { get; set; } = Guid.NewGuid();
        public string ModelName { get; set; }
        public string ModelUrl { get; set; }
        //public string TechData { get; set; }
        public virtual SubFamily SubFamily { get; set; }
        public virtual ICollection<TechData> TechData { get; set; }


    }
}
