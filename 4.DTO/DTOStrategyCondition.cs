using _4.DTO.Enums;

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
    }
}
