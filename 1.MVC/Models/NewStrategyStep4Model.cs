using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace MasterTrade.Models
{
    public class NewStrategyStep4Model
    {
        public int StrategyId { get; set; }

        [DisplayName("Modalidad")]
        public int InvestOptionId { get; set; }

        [DisplayFormat(DataFormatString = "{0:n2}", ApplyFormatInEditMode = true)]
        [DisplayName("Monto (USD)")]
        public decimal InvestAmount { get; set; }

        [DisplayName("Porcentaje del portafolio")]
        public decimal InvestPercentage { get; set; }

        public List<SelectListItem> AllInvestOptions { get; set; }
    }
}