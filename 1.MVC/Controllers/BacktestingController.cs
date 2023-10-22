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
                    new SelectListItem() { Text = GetTemporalityText(1), Value = "1" },
                    new SelectListItem() { Text = GetTemporalityText(2), Value = "2" },
                    new SelectListItem() { Text = GetTemporalityText(4), Value = "4" },
                    new SelectListItem() { Text = GetTemporalityText(8), Value = "8" },
                    new SelectListItem() { Text = GetTemporalityText(24), Value = "24" },
                    new SelectListItem() { Text = GetTemporalityText(48), Value = "48" }
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
            DTOBacktestingParameters parameters = new DTOBacktestingParameters
            {
                Strategy = new ServiceStrategy().GetById(model.StrategyId, GetUserId()),
                CryptoPairId = model.CryptoPairId,
                DateFrom = model.DateFrom,
                DateTo = model.DateTo,
                CandlesGroupingAmount = model.TemporalityId
            };

            DTOBacktestingResult backtestingResult = new ServiceBacktesting().Execute(parameters);
            backtestingResult.DateFrom = model.DateFrom;
            backtestingResult.DateTo = model.DateTo;
            backtestingResult.Temporality = GetTemporalityText(model.TemporalityId);

            Session["BacktestingResult"] = backtestingResult;

            return RedirectToAction("Results", "Backtesting");
        }

        public ActionResult Results()
        {
            DTOBacktestingResult backtestingResult = (DTOBacktestingResult)Session["BacktestingResult"];

            BacktestingResultsModel model = new BacktestingResultsModel()
            {
                Operations = backtestingResult.Operations,
                InitialCapital = backtestingResult.InitialCapital,
                FinalCapital = backtestingResult.FinalCapital,
                MaxDrawdown = backtestingResult.MaxDrawdown,
                ProfitPercentage = backtestingResult.ProfitPercentage,
                WinRate = backtestingResult.WinRate,
                StrategyName = backtestingResult.StrategyName,
                CryptoPair = backtestingResult.CryptoPair,
                DateFrom = backtestingResult.DateFrom,
                DateTo = backtestingResult.DateTo,
                Temporality = backtestingResult.Temporality
            };

            return View(model);
        }

        private string GetTemporalityText(int temporalityId)
        {
            switch (temporalityId)
            {
                case 1:
                    return "30 min";
                case 2:
                    return "1 hr";
                case 4:
                    return "2 hs";
                case 8:
                    return "4 hs";
                case 24:
                    return "12 hs";
                case 48:
                    return "1 día";
                default:
                    return "";
            }
        }
    }
}