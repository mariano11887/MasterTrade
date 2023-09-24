using _2.Service.Indicator.Interface;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Web.Mvc;

namespace MasterTrade.Models
{
    public class NewStrategyStep2Model
    {
        public int StrategyId { get; set; }

        [DisplayName("Indicador")]
        public int IndicatorId { get; set; }

        public IIndicator IndicatorStructure { get; set; }

        public int PreviousIndicatorId { get; set; }

        public List<SelectListItem> AllIndicators { get; set; }

        public List<Tuple<int, string>> AddedIndicators { get; set; }
    }
}