using _2.Service.Indicator.Interface;
using System.Collections.Generic;

namespace _2.Service.Indicator
{
    public class CurrentCandle : IIndicator
    {
        public string ConfigurationTitle
        {
            get
            {
                return "";
            }
        }

        public List<IndicatorMeta> Meta
        {
            get
            {
                return new List<IndicatorMeta>();
            }
        }
    }
}
