using System.Collections.Generic;
using System.ComponentModel;
using System.Web.Mvc;

namespace MasterTrade.Models
{
    public class NewStrategyModel
    {
        [DisplayName("Nombre de la estrategia")]
        public string Name { get; set; }

        [DisplayName("Indicador")]
        public int IndicatorId { get; set; }

        [DisplayName("Longitud")]
        public int MovingAverageLength { get; set; }

        public List<SelectListItem> AllIndicators { get; set; }
        public List<string> AddedStrategies { get; set; }
    }
}