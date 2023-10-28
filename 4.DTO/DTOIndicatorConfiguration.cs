using _4.DTO.Enums;

namespace _4.DTO
{
    public class DTOIndicatorConfiguration
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Value { get; set; }
        public IndicatorMetaDataType Type { get; set; }
    }
}
