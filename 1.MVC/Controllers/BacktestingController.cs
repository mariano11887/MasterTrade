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
                }).ToList()
            };

            return View(model);
        }

        public ActionResult LoadCryptoPairInfo(int? cryptoPairId)
        {
            BacktestingModel model = new BacktestingModel();

            if (cryptoPairId.HasValue)
            {
                (DateTime, DateTime) dateRange = new ServiceCryptoPair().GetDateRange(cryptoPairId.Value);
                model.DateFrom = dateRange.Item1;
                model.DateTo = dateRange.Item2;

                List<DTOTemporality> temporalities = new ServiceTemporality().GetAll(cryptoPairId.Value);
                model.AllTemporalities = temporalities.Select(t => new SelectListItem()
                {
                    Text = t.Description,
                    Value = t.Id.ToString()
                }).ToList();
            }

            return PartialView("IndexCryptoPairInfo", model);
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
                TemporalityId = model.TemporalityId
            };

            DTOBacktestingResult backtestingResult = new ServiceBacktesting().Execute(parameters);
            backtestingResult.DateFrom = model.DateFrom;
            backtestingResult.DateTo = model.DateTo;

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
    }
}