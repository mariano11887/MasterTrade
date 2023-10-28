using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _4.DTO
{
    public class DTOBacktestingWithRangesResult
    {
        public List<DTOBacktestingWithRangesResultOptimalIndicator> OptimalIndicators { get; set; }
        public List<DTOBacktestingWithRangesResultBacktesting> Backtestings { get; set; }
    }

    public class DTOBacktestingWithRangesResultOptimalIndicator
    {
        public string IndicatorName { get; set; }
        public string ConfigurationName { get; set; }
        public decimal ConfigurationValue { get; set; }
    }

    public class DTOBacktestingWithRangesResultBacktesting
    {
        public int BacktestingId { get; set; }
        public decimal InitialCapital { get; set; }
        public decimal FinalCapital { get; set; }
        public decimal Revenue { get; set; }
    }
}
