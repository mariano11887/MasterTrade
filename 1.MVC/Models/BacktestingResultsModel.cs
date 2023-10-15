using _4.DTO;
using System;
using System.Collections.Generic;

namespace MasterTrade.Models
{
    public class BacktestingResultsModel
    {
        public List<DTOBacktestingOperation> Operations { get; set; }
        public decimal InitialCapital { get; set; }
        public decimal FinalCapital { get; set; }
        public decimal ProfitPercentage { get; set; }
        public decimal MaxDrawdown { get; set; }
        public decimal WinRate { get; set; }
        public string StrategyName { get; set; }
        public string CryptoPair { get; set; }
        public DateTime DateFrom { get; set; }
        public DateTime DateTo { get; set; }
        public string Temporality { get; set; }
    }
}