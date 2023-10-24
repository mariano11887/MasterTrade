using System.Collections.Generic;

namespace MasterTrade.Models
{
    public class BacktestingWithRangesConfirmationModel
    {
        public string StrategyName { get; set; }
        public string CryptoPairName { get; set; }
        public string DateRange { get; set; }
        public string Temporality { get; set; }
        public List<BacktestingWithRangesConfirmationIndicator> Indicators { get; set; } = new List<BacktestingWithRangesConfirmationIndicator>();
    }

    public class BacktestingWithRangesConfirmationIndicator
    {
        public string IndicatorName { get; set; }
        public string ConfigurationName { get; set; }
        public string Range { get; set; }
        public string Increment { get; set; }
    }
}