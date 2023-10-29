using System;
using System.Collections.Generic;

namespace MasterTrade.Models
{
    public class BacktestingWithRangesResultModel
    {
        public List<BacktestingWithRangesResultIndicatorConfig> OptimalIndicators { get; set; }
        public List<BacktestingWithRangesResultBacktesting> Backtestings { get; set; }
        public BacktestingWithRangesResultDetail BacktestingDetail { get; set; }
    }

    public class BacktestingWithRangesResultIndicatorConfig
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

    public class BacktestingWithRangesResultDetail
    {
        public List<BacktestingWithRangesResultIndicatorConfig> IndicatorsConfig { get; set; }
        public decimal InitialCapital { get; set; }
        public decimal FinalCapital { get; set; }
        public decimal RevenuePercentage { get; set; }
        public decimal MaxDrawdownPercentage { get; set; }
        public decimal WinRatePercentage { get; set; }
        public List<BacktestingWithRangesResultDetailOperation> Operations { get; set; }
    }

    public class BacktestingWithRangesResultDetailOperation
    {
        public int OperationNumber { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public decimal InitialCapital { get; set; }
        public decimal FinalCapital { get; set; }
        public decimal Revenue { get; set; }
    }
}