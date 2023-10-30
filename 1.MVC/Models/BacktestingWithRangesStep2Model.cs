using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace MasterTrade.Models
{
    public class BacktestingWithRangesStep2Model
    {
        public List<BacktestingWithRangesIndicatorConfiguration> Configurations { get; set; }
    }

    public class BacktestingWithRangesIndicatorConfiguration
    {
        public int IndicatorConfigurationId { get; set; }
        public string IndicatorName { get; set; }
        public string ConfigurationName { get; set; }

        [DisplayName("Mínimo")]
        [Range(1, 1000, ErrorMessage = "Ingrese un valor entre 1 y 1000")]
        public decimal MinValue { get; set; }

        [DisplayName("Máximo")]
        [Range(1, 1000, ErrorMessage = "Ingrese un valor entre 1 y 1000")]
        public decimal MaxValue { get; set; }

        [DisplayName("Incremento")]
        public decimal Increment { get; set; }

        public decimal CurrentValue { get; set; }
        public int TypeId { get; set; }
    }
}