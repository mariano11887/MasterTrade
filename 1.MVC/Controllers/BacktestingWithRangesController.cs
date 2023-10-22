using _2.Service.Service;
using _4.DTO;
using _4.DTO.Enums;
using MasterTrade.Controllers.Base;
using MasterTrade.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace MasterTrade.Controllers
{
    [Authorize]
    public class BacktestingWithRangesController : BaseController
    {
        public ActionResult Step1()
        {
            List<DTOStrategy> strategies = new ServiceStrategy().GetUserStrategies(GetUserId());
            List<DTOCryptoPair> cryptoPairs = new ServiceCryptoPair().GetCryptoPairs();

            BacktestingWithRangesStep1Model model = new BacktestingWithRangesStep1Model
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
            BacktestingWithRangesStep1Model model = new BacktestingWithRangesStep1Model();

            if (cryptoPairId.HasValue)
            {
                (DateTime, DateTime) dateRange = new ServiceCryptoPair().GetDateRange(cryptoPairId.Value);

                model.DateFrom = dateRange.Item1;
                model.DateTo = dateRange.Item2;
            }

            return PartialView("Step1Dates", model);
        }

        [HttpPost]
        public ActionResult Step1(BacktestingWithRangesStep1Model model)
        {
            Session["BacktestingWithRangesStep1"] = model;
            return RedirectToAction("Step2", "BacktestingWithRanges");
        }

        public ActionResult Step2()
        {
            BacktestingWithRangesStep1Model previousModel = (BacktestingWithRangesStep1Model)Session["BacktestingWithRangesStep1"];
            DTOStrategy strategy = new ServiceStrategy().GetById(previousModel.StrategyId, GetUserId());

            BacktestingWithRangesStep2Model model = new BacktestingWithRangesStep2Model()
            {
                Configurations = new List<BacktestingWithRangesIndicatorConfiguration>()
            };

            foreach (DTOIndicator indicator in strategy.Indicators)
            {
                foreach (DTOIndicatorConfiguration configuration in indicator.Configurations)
                {
                    decimal value = decimal.Parse(configuration.Value);
                    decimal minValue = value * (decimal)0.9;
                    decimal maxValue = value * (decimal)1.1;
                    decimal increment = value * (decimal)0.1;

                    if (configuration.Type == IndicatorMetaDataType.Integer)
                    {
                        minValue = Math.Floor(minValue);
                        maxValue = Math.Ceiling(maxValue);
                        increment = 1;
                    }

                    model.Configurations.Add(new BacktestingWithRangesIndicatorConfiguration
                    {
                        IndicatorId = indicator.Id,
                        IndicatorName = indicator.ToString(),
                        ConfigurationName = configuration.Name,
                        MinValue = minValue,
                        MaxValue = maxValue,
                        Increment = increment
                    });
                }
            }

            return View(model);
        }

        [HttpPost]
        public ActionResult Step2(BacktestingWithRangesStep2Model model)
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