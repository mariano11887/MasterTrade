using MasterTrade.Controllers.Base;
using MasterTrade.Models;
using System.Collections.Generic;
using System.Web.Mvc;

namespace MasterTrade.Controllers
{
    public class BacktestingWithRangesController : BaseController
    {
        public ActionResult Step1()
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
        public ActionResult Step1(BacktestingWithRangesModel model)
        {
            return RedirectToAction("Step2", "BacktestingWithRanges");
        }

        public ActionResult Step2()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Step2(BacktestingWithRangesModel model)
        {
            return RedirectToAction("Confirmation", "BacktestingWithRanges");
        }

        public ActionResult Confirmation()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Confirmation(BacktestingWithRangesModel model)
        {
            return RedirectToAction("Results", "BacktestingWithRanges");
        }

        public ActionResult Results()
        {
            return View();
        }
    }
}