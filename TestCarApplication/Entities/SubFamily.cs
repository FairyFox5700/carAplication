using System;
using System.Collections.Generic;
using System.Text;

namespace CarApplication.Entities
{
    class SubFamily
    {
        public Guid FamilyId { get; set; }
        public Guid SubFamilyId { get; set; } = Guid.NewGuid();
        public string SubFamilyUrl { get; set; }
        public string SubFamilyName { get; set; }
        public virtual ICollection<Model> CarModels { get; set; }
        public virtual Family Family { get; set; }
    }
}
