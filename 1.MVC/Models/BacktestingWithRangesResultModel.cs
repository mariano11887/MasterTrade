using System.Collections.Generic;

namespace MasterTrade.Models
{
    public class BacktestingWithRangesResultModel
    {
        public List<BacktestingWithRangesResultOptimalIndicator> OptimalIndicators { get; set; }
        public List<BacktestingWithRangesResultBacktesting> Backtestings { get; set; }
    }

    public class BacktestingWithRangesResultOptimalIndicator
    {
        public string IndicatorName { get; set; }
        public string ConfigurationName { get; set; }
        public decimal ConfigurationValue { get; set; }
    }

    public class BacktestingWithRangesResultBacktesting
    {
        public int BacktestingId { get; set; }
        public decimal InitialCapital { get; set; }
        public decimal FinalCapital { get; set; }
        public decimal Revenue { get; set; }
    }
}