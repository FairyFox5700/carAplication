using System;
using System.Collections.Generic;
using System.Text;
using Json2KeyValue;

namespace CarApplication.Entities
{
    class TechData
    {
        public Guid ModelId { get; set; }
        public Guid TechDataId { get; set; } = Guid.NewGuid();
        public string TechInfo { get; set; }
        public virtual Model Model { get; set; }
       /* public virtual Dictionary<string, string> ModelProperties
        {
            get
            { return JsonConvert.DeserializeObject<Dictionary<string, string>>(ModelProperties.ToString()); }
            set { TechInfo = JsonConvert.SerializeObject(ModelProperties); }

        }
        */
        //is it object?

    }
}
