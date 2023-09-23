using _4.DTO.Enums;
using _4.DTO.Helpers;

namespace _4.DTO
{
    public class DTOStrategyCondition
    {
        public int Id { get; set; }
        public ExecutionMoment ExecutionMoment { get; set; }
        public DTOIndicatorMeta FirstIndicatorMeta { get; set; }
        public DTOIndicatorMeta SecondIndicatorMeta { get; set; }
        public Comparer Comparer { get; set; }
        public bool IsOpenCondition { get; set; }
        public bool Removed { get; set; }

        public override string ToString()
        {
            return $"{FirstIndicatorMeta.Name} de {FirstIndicatorMeta.Indicator} es {EnumsHelper.GetDescription(Comparer)} {SecondIndicatorMeta.Name} de {SecondIndicatorMeta.Indicator}";
        }
    }
}
