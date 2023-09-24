using System.Collections.Generic;

namespace MasterTrade.Models
{
    public class NewStrategyModel
    {
        public int StrategyId { get; set; }
        public string Name { get; set; }
        public List<string> Indicators { get; set; }
        public List<string> OpenConditions { get; set; }
        public string Investment { get; set; }
        public List<string> CloseConditions { get; set; }
    }
}