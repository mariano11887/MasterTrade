using _2.Service.Service;
using _4.DTO;
using MasterTrade.Controllers.Base;
using MasterTrade.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace MasterTrade.Controllers
{
    [Authorize]
    public class BacktestingController : BaseController
    {
        // GET: Backtesting
        public ActionResult Index()
        {
            List<DTOStrategy> strategies = new ServiceStrategy().GetUserStrategies(GetUserId());
            List<DTOCryptoPair> cryptoPairs = new ServiceCryptoPair().GetCryptoPairs();

            BacktestingModel model = new BacktestingModel
            {
                AllStrategies = strategies.Select(s => new SelectListItem() 
                {
                    Text = s.Name,
                    Value = s.Id.ToString()
                }).ToList(),
                AllCryptoPairs = cryptoPairs.Select(cp => new SelectListItem() 
                {
                    Text = cp.Name, 
                    Value = cp.Id.ToString() 
                }).ToList(),
                AllTemporalities = new List<SelectListItem>
                {
                    new SelectListItem() { Text = "30 min", Value = "1" },
                    new SelectListItem() { Text = "1 hr", Value = "2" },
                    new SelectListItem() { Text = "2 hs", Value = "4" },
                    new SelectListItem() { Text = "4 hs", Value = "8" },
                    new SelectListItem() { Text = "12 hs", Value = "24" },
                    new SelectListItem() { Text = "1 día", Value = "48" }
                }
            };

            return View(model);
        }

        public ActionResult LoadCryptoPairDates(int? cryptoPairId)
        {
            BacktestingModel model = new BacktestingModel();

            if (cryptoPairId.HasValue)
            {
                (DateTime, DateTime) dateRange = new ServiceCryptoPair().GetDateRange(cryptoPairId.Value);

                model.DateFrom = dateRange.Item1;
                model.DateTo = dateRange.Item2;
            }

            return PartialView("IndexDates", model);
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