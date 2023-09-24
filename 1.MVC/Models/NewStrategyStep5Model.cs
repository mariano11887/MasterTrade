using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Web.Mvc;

namespace MasterTrade.Models
{
    public class NewStrategyStep5Model
    {
        public int StrategyId { get; set; }

        [DisplayName("Momento de ejecución")]
        public int ExecutionMomentId { get; set; }

        [DisplayName("Indicador 1")]
        public int IndicatorId1 { get; set; }

        [DisplayName("Elemento del indicador")]
        public string Indicator1Element { get; set; }

        [DisplayName("Condición")]
        public int ConditionId { get; set; }

        [DisplayName("Indicador 2")]
        public int IndicatorId2 { get; set; }

        [DisplayName("Elemento del indicador")]
        public string Indicator2Element { get; set; }

        public List<Tuple<int, string>> AddedConditions { get; set; }

        public List<SelectListItem> AllExecutionMoments { get; set; }
        public List<SelectListItem> StrategyIndicators { get; set; }
        public List<SelectListItem> Indicator1Elements { get; set; }
        public List<SelectListItem> Indicator2Elements { get; set; }
        public List<SelectListItem> AllConditions { get; set; }
    }
}