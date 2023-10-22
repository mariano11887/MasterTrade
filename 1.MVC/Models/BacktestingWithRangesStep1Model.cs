using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Web.Mvc;

namespace MasterTrade.Models
{
    public class BacktestingWithRangesStep1Model
    {
        [DisplayName("Estrategia")]
        public int StrategyId { get; set; }

        [DisplayName("Par de criptomonedas")]
        public int CryptoPairId { get; set; }

        [DisplayName("Fecha desde")]
        public DateTime DateFrom { get; set; } = DateTime.MinValue;

        [DisplayName("Fecha hasta")]
        public DateTime DateTo { get; set; } = DateTime.MinValue;

        [DisplayName("Temporalidad")]
        public int TemporalityId { get; set; }

        public List<SelectListItem> AllStrategies { get; set; }
        public List<SelectListItem> AllCryptoPairs { get; set; }
        public List<SelectListItem> AllTemporalities { get; set; }
    }
}