using _2.Service.Indicator.Interface;
using System.Collections.Generic;
using System.ComponentModel;
using System.Web.Mvc;

namespace MasterTrade.Models
{
    public class NewStrategyModel
    {
        [DisplayName("Modalidad")]
        public int InvestOptionId { get; set; }

        [DisplayName("Monto (USD)")]
        public double InvestAmount { get; set; }

        public List<SelectListItem> AllInvestOptions { get; set; }
    }
}