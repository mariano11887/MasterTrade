using System;
using System.Collections.Generic;

namespace _4.DTO
{
    public class DTOBacktestingResult
    {
        public List<DTOBacktestingOperation> Operations { get; set; } = new List<DTOBacktestingOperation>();
        public decimal InitialCapital { get; set; }
        public decimal FinalCapital { get; set; }
        public decimal ProfitPercentage {
            get
            {
                return (FinalCapital / InitialCapital - 1) * 100;
            }
        }
        public decimal MaxDrawdown { get; set; }
        public decimal WinRate { get; set; }
        public string StrategyName { get; set; }
        public string CryptoPair { get; set; }
        public DateTime DateFrom { get; set; }
        public DateTime DateTo { get; set; }
        public string Temporality { get; set; }

        public List<DTOBacktestingResultIndicatorConfig> IndicatorsConfig { get; set; }
    }

    public class DTOBacktestingResultIndicatorConfig
    {
        public string IndicatorName { get; set; }
        public string ConfigurationName { get; set; }
        public decimal ConfigurationValue { get; set; }
    }
}
