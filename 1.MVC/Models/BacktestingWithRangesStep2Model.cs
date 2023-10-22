using _4.DTO;
using System.Collections.Generic;
using System.ComponentModel;

namespace MasterTrade.Models
{
    public class BacktestingWithRangesStep2Model
    {
        public List<BacktestingWithRangesIndicatorConfiguration> Configurations { get; set; }
    }

    public class BacktestingWithRangesIndicatorConfiguration
    {
        public int IndicatorId { get; set; }
        public string IndicatorName { get; set; }
        public string ConfigurationName { get; set; }

        [DisplayName("Mínimo")]
        public decimal MinValue { get; set; }

        [DisplayName("Máximo")]
        public decimal MaxValue { get; set; }

        [DisplayName("Incremento")]
        public decimal Increment { get; set; }
    }
}