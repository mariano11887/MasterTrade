using System.ComponentModel;

namespace MasterTrade.Models
{
    public class BacktestingWithRangesModel
    {
        [DisplayName("Valor mínimo")]
        public double MinValue { get; set; }

        [DisplayName("Valor máximo")]
        public double MaxValue { get; set; }

        [DisplayName("Incremento")]
        public double Increment { get; set; }
    }
}