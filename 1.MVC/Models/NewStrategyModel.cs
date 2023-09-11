using _2.Service.Indicator.Interface;
using System.Collections.Generic;
using System.ComponentModel;
using System.Web.Mvc;

namespace MasterTrade.Models
{
    public class NewStrategyModel
    {
        [DisplayName("Momento de ejecución")]
        public int ExecutionMomentId { get; set; }

        [DisplayName("Indicador 1")]
        public int IndicatorId1 { get; set; }

        [DisplayName("Elemento de la media móvil")]
        public int MovingAverageValue { get; set; }

        [DisplayName("Condición")]
        public int ConditionId { get; set; }

        [DisplayName("Indicador 2")]
        public int IndicatorId2 { get; set; }

        [DisplayName("Modalidad")]
        public int InvestOptionId { get; set; }

        [DisplayName("Monto (USD)")]
        public double InvestAmount { get; set; }

        
        public List<SelectListItem> AllExecutionMoments { get; set; }
        public List<SelectListItem> StrategyIndicators { get; set; }
        public List<SelectListItem> MovingAverageValues { get; set; }
        public List<SelectListItem> AllConditions { get; set; }
        public List<SelectListItem> AllInvestOptions { get; set; }
    }
}