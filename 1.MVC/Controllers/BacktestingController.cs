using MasterTrade.Controllers.Base;
using MasterTrade.Models;
using System.Collections.Generic;
using System.Web.Mvc;

namespace MasterTrade.Controllers
{
    public class BacktestingController : BaseController
    {
        // GET: Backtesting
        public ActionResult Index()
        {
            BacktestingModel model = new BacktestingModel
            {
                AllStrategies = new List<SelectListItem>
                {
                    new SelectListItem() { Text = "Prueba", Value = "1" }
                },
                AllCryptoPairs = new List<SelectListItem>
                {
                    new SelectListItem() { Text = "BTC/USDT", Value = "1" },
                    new SelectListItem() { Text = "ETH/USDT", Value = "2" }
                },
                AllTemporalities = new List<SelectListItem>
                {
                    new SelectListItem() { Text = "5 min", Value = "1" },
                    new SelectListItem() { Text = "1 hora", Value = "2" }
                }
            };

            return View(model);
        }

        [HttpPost]
        public ActionResult Index(BacktestingModel model)
        {
            return RedirectToAction("Results", "Backtesting");
        }

        public ActionResult Results()
        {
            return View();
        }
    }
}