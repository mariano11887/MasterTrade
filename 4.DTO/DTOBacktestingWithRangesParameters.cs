using System;
using System.Collections.Generic;

namespace _4.DTO
{
    public class DTOBacktestingWithRangesParameters : DTOBacktestingParameters
    {
        public List<DTOBacktestingWithRangesIndicatorConfiguration> IndicatorConfigurations { get; set; }
    }

    public class DTOBacktestingWithRangesIndicatorConfiguration
    {
        public int IndicatorConfigurationId { get; set; }
        public decimal MinValue { get; set; }
        public decimal MaxValue { get; set; }
        public decimal Increment { get; set; }
    }
}
