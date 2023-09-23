using _2.Service.Indicator.Interface;
using _2.Service.Service;
using _4.DTO;
using _4.DTO.Enums;
using _4.DTO.Helpers;
using MasterTrade.Controllers.Base;
using MasterTrade.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace MasterTrade.Controllers
{
    [Authorize]
    public class MyStrategiesController : BaseController
    {
        // GET: MyStrategies
        public ActionResult Index()
        {
            MyStrategiesModel model = new MyStrategiesModel()
            {
                UserStrategies = new ServiceStrategy().GetUserStrategies(GetUserId())
            };
            return View(model);
        }

        #region Step 1

        public ActionResult NewStep1()
        {
            return View();
        }

        [HttpPost]
        public ActionResult NewStep1(NewStrategyStep1Model model)
        {
            ServiceStrategy service = new ServiceStrategy();
            bool isNameValid = service.CheckStrategyName(GetUserId(), model.Name);
            if (!isNameValid)
            {
                ViewBag.ErrorMsg = "El nombre elegido ya existe.";
                return View(model);
            }

            DTOStrategy dto = new DTOStrategy()
            {
                Name = model.Name,
                UserId = GetUserId()
            };
            int strategyId = service.Save(dto);

            return RedirectToAction("NewStep2", "MyStrategies", new { id = strategyId });
        }

        #endregion

        #region Step 2

        public ActionResult NewStep2(int id)
        {
            DTOStrategy strategy = new ServiceStrategy().GetById(id, GetUserId());
            List<DTOIndicatorType> indicatorTypes = new ServiceIndicator().GetIndicatorTypes();

            NewStrategyStep2Model model = new NewStrategyStep2Model
            {
                StrategyId = strategy.Id,
                AllIndicators = indicatorTypes.Select(it => new SelectListItem
                {
                    Value = it.Id.ToString(),
                    Text = it.Description
                }).ToList(),
                AddedIndicators = strategy.Indicators.Select(i => new Tuple<int, string>(i.Id, i.ToString())).ToList()
            };

            return View(model);
        }

        public ActionResult LoadIndicatorMeta(int? indicatorId)
        {
            NewStrategyStep2Model model = new NewStrategyStep2Model()
            {
                IndicatorStructure = indicatorId.HasValue ? new ServiceIndicator().GetIndicator(indicatorId.Value) : null
            };

            return PartialView("NewStep2IndicatorMeta", model);
        }

        [HttpPost]
        public ActionResult NewStep2(NewStrategyStep2Model model)
        {
            ServiceStrategy serviceStrategy = new ServiceStrategy();
            DTOStrategy strategy = serviceStrategy.GetById(model.StrategyId, GetUserId());

            DTOIndicator indicator = new DTOIndicator
            {
                TypeId = model.IndicatorId
            };

            model.IndicatorStructure = new ServiceIndicator().GetIndicator(model.IndicatorId);
            foreach (var meta in model.IndicatorStructure.Configuration)
            {
                indicator.Configurations.Add(new DTOIndicatorConfiguration
                {
                    Name = meta.Name,
                    Value = Request[meta.HtmlName],
                    Type = meta.Type
                });
            }

            strategy.Indicators.Add(indicator);
            model.StrategyId = serviceStrategy.Save(strategy);

            return RedirectToAction("NewStep2", "MyStrategies", new { id = model.StrategyId });
        }

        [HttpPost]
        public ActionResult RemoveIndicator(int strategyId, int indicatorId)
        {
            ServiceStrategy serviceStrategy = new ServiceStrategy();
            DTOStrategy strategy = serviceStrategy.GetById(strategyId, GetUserId());

            strategy.Indicators.FirstOrDefault(i => i.Id == indicatorId).Removed = true;
            serviceStrategy.Save(strategy);

            strategy = serviceStrategy.GetById(strategyId, GetUserId());
            NewStrategyStep2Model model = new NewStrategyStep2Model()
            {
                AddedIndicators = strategy.Indicators.Select(i => new Tuple<int, string>(i.Id, i.ToString())).ToList()
            };

            return PartialView("NewStep2AddedIndicators", model);
        }

        #endregion

        #region Step 3

        public ActionResult NewStep3(int id)
        {
            ServiceStrategy serviceStrategy = new ServiceStrategy();
            DTOStrategy strategy = serviceStrategy.GetById(id, GetUserId());

            NewStrategyStep3Model model = new NewStrategyStep3Model()
            {
                StrategyId = id,
                AllExecutionMoments = new List<SelectListItem>
                {
                    new SelectListItem { Value = ((int)ExecutionMoment.CandleClose).ToString(), Text = "Cierre de la vela" },
                    new SelectListItem { Value = ((int)ExecutionMoment.CandleOpen).ToString(), Text = "Apertura de la vela" }
                },
                StrategyIndicators = strategy.Indicators.Select(i => new SelectListItem
                {
                    Value = i.Id.ToString(),
                    Text = i.ToString()
                }).ToList(),
                AddedConditions = strategy.Conditions.Select(c => new Tuple<int, string>(c.Id, c.ToString())).ToList()
            };
            return View(model);
        }

        public ActionResult LoadIndicator1MetaStep3(int? indicatorId, int strategyId)
        {
            DTOStrategy strategy = new ServiceStrategy().GetById(strategyId, GetUserId());

            NewStrategyStep3Model model = new NewStrategyStep3Model()
            {
                AllConditions = new List<SelectListItem>
                {
                    new SelectListItem { Value = ((int)Comparer.Equal).ToString(), Text = EnumsHelper.GetDescription(Comparer.Equal) },
                    new SelectListItem { Value = ((int)Comparer.Lower).ToString(), Text = EnumsHelper.GetDescription(Comparer.Lower) },
                    new SelectListItem { Value = ((int)Comparer.LowerOrEqual).ToString(), Text = EnumsHelper.GetDescription(Comparer.LowerOrEqual) },
                    new SelectListItem { Value = ((int)Comparer.Greater).ToString(), Text = EnumsHelper.GetDescription(Comparer.Greater) },
                    new SelectListItem { Value = ((int)Comparer.GreaterOrEqual).ToString(), Text = EnumsHelper.GetDescription(Comparer.GreaterOrEqual) }
                },
                StrategyIndicators = strategy.Indicators.Select(i => new SelectListItem
                {
                    Value = i.Id.ToString(),
                    Text = i.ToString()
                }).ToList()
            };

            if (indicatorId.HasValue)
            {
                IIndicator indicator = new ServiceIndicator().GetIndicatorById(indicatorId.Value);

                model.Indicator1Elements = indicator.Meta.Select(im => new SelectListItem
                {
                    Value = im.Name,
                    Text = im.Name
                }).ToList();
            }

            return PartialView("NewStep3Conditions", model);
        }

        public ActionResult LoadIndicator2MetaStep3(int? indicatorId)
        {
            NewStrategyStep3Model model = new NewStrategyStep3Model();

            if (indicatorId.HasValue)
            {
                IIndicator indicator = new ServiceIndicator().GetIndicatorById(indicatorId.Value);

                model.Indicator2Elements = indicator.Meta.Select(im => new SelectListItem
                {
                    Value = im.Name,
                    Text = im.Name
                }).ToList();
            }

            return PartialView("NewStep3Conditions2", model);
        }

        [HttpPost]
        public ActionResult NewStep3(NewStrategyStep3Model model)
        {
            ServiceStrategy serviceStrategy = new ServiceStrategy();
            DTOStrategy strategy = serviceStrategy.GetById(model.StrategyId, GetUserId());

            DTOStrategyCondition strategyCondition = new DTOStrategyCondition
            {
                ExecutionMoment = (ExecutionMoment)model.ExecutionMomentId,
                FirstIndicatorMeta = new DTOIndicatorMeta
                {
                    Indicator = new DTOIndicator { Id = model.IndicatorId1 },
                    Name = model.Indicator1Element
                },
                SecondIndicatorMeta = new DTOIndicatorMeta
                {
                    Indicator = new DTOIndicator { Id = model.IndicatorId2 },
                    Name = model.Indicator2Element
                },
                Comparer = (Comparer)model.ConditionId,
                IsOpenCondition = true
            };

            strategy.Conditions.Add(strategyCondition);
            model.StrategyId = serviceStrategy.Save(strategy);

            return RedirectToAction("NewStep3", "MyStrategies", new { id = model.StrategyId });
        }

        [HttpPost]
        public ActionResult RemoveOpenCondition(int strategyId, int conditionId)
        {
            ServiceStrategy serviceStrategy = new ServiceStrategy();
            DTOStrategy strategy = serviceStrategy.GetById(strategyId, GetUserId());

            strategy.Conditions.FirstOrDefault(c => c.Id == conditionId).Removed = true;
            serviceStrategy.Save(strategy);

            strategy = serviceStrategy.GetById(strategyId, GetUserId());
            NewStrategyStep3Model model = new NewStrategyStep3Model()
            {
                AddedConditions = strategy.Conditions.Select(c => new Tuple<int, string>(c.Id, c.ToString())).ToList()
            };

            return PartialView("NewStep3AddedConditions", model);
        }

        #endregion

        #region Step 4

        public ActionResult NewStep4(int id)
        {
            NewStrategyStep4Model model = new NewStrategyStep4Model()
            {
                StrategyId = id,
                AllInvestOptions = new List<SelectListItem>
                {
                    new SelectListItem { Value = ((int)InvestmentOption.FixedAmount).ToString(), Text = "Cantidad fija de dinero" },
                    new SelectListItem { Value = ((int)InvestmentOption.PortfolioPercentage).ToString(), Text = "Porcentaje del portafolio" }
                }
            };
            return View(model);
        }

        [HttpPost]
        public ActionResult NewStep4(NewStrategyStep4Model model)
        {
            ServiceStrategy serviceStrategy = new ServiceStrategy();
            DTOStrategy strategy = serviceStrategy.GetById(model.StrategyId, GetUserId());

            if (model.InvestOptionId == (int)InvestmentOption.FixedAmount)
            {
                strategy.InvestmentAmount = model.InvestAmount;
            }
            else if (model.InvestOptionId == (int)InvestmentOption.PortfolioPercentage)
            {
                strategy.InvestmentPercentage = model.InvestPercentage;
            }

            model.StrategyId = serviceStrategy.Save(strategy);

            return RedirectToAction("NewStep5", "MyStrategies", new { id = model.StrategyId });
        }

        #endregion

        #region Step 5

        public ActionResult NewStep5()
        {
            NewStrategyStep3Model model = new NewStrategyStep3Model()
            {
                AllExecutionMoments = new List<SelectListItem>
                {
                    new SelectListItem { Value = "1", Text = "Cierre de la vela" },
                    new SelectListItem { Value = "2", Text = "Apertura de la vela" }
                },
                StrategyIndicators = new List<SelectListItem>
                {
                    new SelectListItem { Value = "1", Text = "Media móvil (5)" },
                    new SelectListItem { Value = "2", Text = "Media móvil (10)" },
                    new SelectListItem { Value = "3", Text = "Media móvil (20)" }
                },
                Indicator1Elements = new List<SelectListItem>
                {
                    new SelectListItem { Value = "1", Text = "Valor" }
                },
                AllConditions = new List<SelectListItem>
                {
                    new SelectListItem { Value = "1", Text = "mayor que" },
                    new SelectListItem { Value = "2", Text = "igual a" },
                    new SelectListItem { Value = "3", Text = "menor que" }
                }
            };
            return View(model);
        }

        [HttpPost]
        public ActionResult NewStep5(NewStrategyModel model)
        {
            return RedirectToAction("NewConfirmation", "MyStrategies");
        }

        #endregion

        #region Confirmation

        public ActionResult NewConfirmation()
        {
            return View();
        }

        [HttpPost]
        public ActionResult NewConfirmation(NewStrategyModel model)
        {
            return RedirectToAction("Index", "MyStrategies");
        }

        #endregion
    }
}