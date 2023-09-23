using System.Collections.Generic;

namespace _4.DTO
{
    public class DTOStrategy
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int UserId { get; set; }
        public bool IsComplete { get; set; }
        public decimal? InvestmentAmount { get; set; }
        public decimal? InvestmentPercentage { get; set; }

        public List<DTOIndicator> Indicators { get; set; } = new List<DTOIndicator>();
        public List<DTOStrategyCondition> Conditions { get; set; } = new List<DTOStrategyCondition>();
    }
}
