using _4.DTO.Enums;

namespace _2.Service.Indicator.Interface
{
    public class IndicatorConfiguration
    {
        public string Name { get; set; }
        public string HtmlName { get; set; }
        public string Value { get; set; }
        public IndicatorMetaDataType Type { get; set; }
    }
}
