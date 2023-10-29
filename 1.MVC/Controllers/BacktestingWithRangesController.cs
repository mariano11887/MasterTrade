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
                }).ToList()
            };

            return View(model);
        }

        public ActionResult LoadCryptoPairInfo(int? cryptoPairId)
        {
            BacktestingWithRangesStep1Model model = new BacktestingWithRangesStep1Model();

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

            return PartialView("Step1CryptoPairInfo", model);
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
                        IndicatorConfigurationId = configuration.Id,
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
            Session["BacktestingWithRangesStep2"] = model;
            return RedirectToAction("Confirmation", "BacktestingWithRanges");
        }

        public ActionResult Confirmation()
        {
            BacktestingWithRangesStep1Model modelStep1 = (BacktestingWithRangesStep1Model)Session["BacktestingWithRangesStep1"];
            BacktestingWithRangesStep2Model modelStep2 = (BacktestingWithRangesStep2Model)Session["BacktestingWithRangesStep2"];

            BacktestingWithRangesConfirmationModel model = new BacktestingWithRangesConfirmationModel
            {
                StrategyName = new ServiceStrategy().GetById(modelStep1.StrategyId, GetUserId()).Name,
                CryptoPairName = new ServiceCryptoPair().GetById(modelStep1.CryptoPairId).Name,
                DateRange = $"{modelStep1.DateFrom:dd/MM/yyyy HH:mm} - {modelStep1.DateTo:dd/MM/yyyy HH:mm}",
                Temporality = new ServiceTemporality().GetById(modelStep1.TemporalityId).Description,
                Indicators = modelStep2.Configurations.Select(c => new BacktestingWithRangesConfirmationIndicator
                {
                    IndicatorName = c.IndicatorName,
                    ConfigurationName = c.ConfigurationName,
                    Range = $"{c.MinValue} - {c.MaxValue}",
                    Increment = c.Increment.ToString()
                }).ToList()
            };

            return View(model);
        }

        [HttpPost]
        public ActionResult Confirmation(BacktestingWithRangesConfirmationModel model)
        {
            BacktestingWithRangesStep1Model modelStep1 = (BacktestingWithRangesStep1Model)Session["BacktestingWithRangesStep1"];
            BacktestingWithRangesStep2Model modelStep2 = (BacktestingWithRangesStep2Model)Session["BacktestingWithRangesStep2"];

            DTOBacktestingWithRangesParameters parameters = new DTOBacktestingWithRangesParameters
            {
                Strategy = new ServiceStrategy().GetById(modelStep1.StrategyId, GetUserId()),
                CryptoPairId = modelStep1.CryptoPairId,
                DateFrom = modelStep1.DateFrom,
                DateTo = modelStep1.DateTo,
                TemporalityId = modelStep1.TemporalityId,
                IndicatorConfigurations = modelStep2.Configurations.Select(c => new DTOBacktestingWithRangesIndicatorConfiguration
                {
                    IndicatorConfigurationId = c.IndicatorConfigurationId,
                    MinValue = c.MinValue,
                    MaxValue = c.MaxValue,
                    Increment = c.Increment
                }).ToList()
            };

            DTOBacktestingWithRangesResult result = new ServiceBacktesting().ExecuteWithRanges(parameters);
            Session["BacktestingWithRangesResult"] = result;

            return RedirectToAction("Results", "BacktestingWithRanges");
        }

        public ActionResult Results()
        {
            DTOBacktestingWithRangesResult result = (DTOBacktestingWithRangesResult)Session["BacktestingWithRangesResult"];
            BacktestingWithRangesResultModel model = new BacktestingWithRangesResultModel
            {
                OptimalIndicators = result.OptimalIndicators.Select(oi => new BacktestingWithRangesResultIndicatorConfig
                {
                    IndicatorName = oi.IndicatorName,
                    ConfigurationName = oi.ConfigurationName,
                    ConfigurationValue = oi.ConfigurationValue
                }).ToList(),
                Backtestings = result.Backtestings.Select(b => new BacktestingWithRangesResultBacktesting
                {
                    BacktestingId = b.BacktestingId,
                    InitialCapital= b.InitialCapital,
                    FinalCapital = b.FinalCapital,
                    Revenue = b.Revenue
                }).ToList()
            };

            return View(model);
        }

        public ActionResult LoadBacktestingDetails(int backtestingId)
        {
            DTOBacktestingResult result = new ServiceBacktesting().GetById(backtestingId, GetUserId());

            int operationNumber = 0;
            BacktestingWithRangesResultModel model = new BacktestingWithRangesResultModel
            {
                BacktestingDetail = new BacktestingWithRangesResultDetail
                {
                    IndicatorsConfig = result.IndicatorsConfig.Select(ic => new BacktestingWithRangesResultIndicatorConfig
                    {
                        ConfigurationName = ic.ConfigurationName,
                        ConfigurationValue = ic.ConfigurationValue,
                        IndicatorName = ic.IndicatorName
                    }).ToList(),
                    InitialCapital = result.InitialCapital,
                    FinalCapital = result.FinalCapital,
                    RevenuePercentage = result.ProfitPercentage,
                    MaxDrawdownPercentage = result.MaxDrawdown * 100,
                    WinRatePercentage = result.WinRate * 100,
                    Operations = result.Operations.Select(o => new BacktestingWithRangesResultDetailOperation
                    {
                        OperationNumber = ++operationNumber,
                        StartDate = o.OpenDate,
                        EndDate = o.CloseDate,
                        InitialCapital = o.InitialCapital,
                        FinalCapital = o.FinalCapital,
                        Revenue = o.Profit
                    }).ToList()
                }
            };

            return PartialView("ResultsBacktest", model);
        }
    }
}