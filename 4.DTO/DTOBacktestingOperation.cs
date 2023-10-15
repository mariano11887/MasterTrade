using System;

namespace _4.DTO
{
    public class DTOBacktestingOperation
    {
        public DateTime OpenDate { get; set; }
        public DateTime CloseDate { get; set; }
        public decimal InitialCapital { get; set; }
        public decimal FinalCapital { get; set; }
        public decimal ProfitPercentage { 
            get
            {
                return (FinalCapital / InitialCapital - 1) * 100;
            }
        }
        public decimal Profit
        {
            get
            {
                return FinalCapital - InitialCapital;
            }
        }
    }
}
