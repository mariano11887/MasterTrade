using _2.Service.Indicator.Interface;
using _2.Service.Service;
using _4.DTO;
using _4.DTO.Enums;
using MasterTrade.Controllers.Base;
using MasterTrade.Models;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace MasterTrade.Controllers
{
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

        public ActionResult NewStep2(int id)
        {
            DTOStrategy strategy = new ServiceStrategy().GetById(id, GetUserId());
            NewStrategyStep2Model model = new NewStrategyStep2Model
            {
                StrategyId = strategy.Id
            };

            FillStep2Selects(model);

            return View(model);
        }

        [HttpPost]
        public ActionResult NewStep2(NewStrategyStep2Model model, bool indicatorAdded)
        {
            if (model.IndicatorId > 0 && model.IndicatorId != model.PreviousIndicatorId)
            {
                model.IndicatorStructure = new ServiceIndicator().GetIndicator(model.IndicatorId);
                ModelState.Remove("PreviousIndicatorId");
                model.PreviousIndicatorId = model.IndicatorId;
                FillStep2Selects(model);
                return View(model);
            }

            if (indicatorAdded)
            {
                ServiceStrategy serviceStrategy = new ServiceStrategy();
                DTOStrategy strategy = serviceStrategy.GetById(model.StrategyId, GetUserId());
                DTOIndicator indicator = new DTOIndicator
                {
                    TypeId = model.IndicatorId
                };
                
                model.IndicatorStructure = new ServiceIndicator().GetIndicator(model.IndicatorId);
                foreach (var meta in model.IndicatorStructure.Meta)
                {
                    indicator.Metas.Add(new DTOIndicatorMeta
                    {
                        Name = meta.Name,
                        Value = Request[meta.HtmlName],
                        Type = meta.Type
                    });
                }

                strategy.Indicators.Add(indicator);
                model.StrategyId = serviceStrategy.Save(strategy);

                FillStep2Selects(model);
                return View(model);
            }

            return RedirectToAction("NewStep3", "MyStrategies");
        }

        private void FillStep2Selects(NewStrategyStep2Model model)
        {
            List<DTOIndicatorType> indicatorTypes = new ServiceIndicator().GetIndicatorTypes();
            model.AllIndicators = indicatorTypes.Select(it => new SelectListItem
            {
                Value = it.Id.ToString(),
                Text = it.Description
            }).ToList();
        }

        public ActionResult NewStep3()
        {
            NewStrategyModel model = new NewStrategyModel()
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
                MovingAverageValues = new List<SelectListItem>
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
        public ActionResult NewStep3(NewStrategyModel model)
        {
            return RedirectToAction("NewStep4", "MyStrategies");
        }

        public ActionResult NewStep4()
        {
            NewStrategyModel model = new NewStrategyModel()
            {
                AllInvestOptions = new List<SelectListItem>
                {
                    new SelectListItem { Value = "1", Text = "Cantidad fija de dinero" },
                    new SelectListItem { Value = "2", Text = "Porcentaje del portafolio" }
                }
            };
            return View(model);
        }

        [HttpPost]
        public ActionResult NewStep4(NewStrategyModel model)
        {
            return RedirectToAction("NewStep5", "MyStrategies");
        }

        public ActionResult NewStep5()
        {
            NewStrategyModel model = new NewStrategyModel()
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
                MovingAverageValues = new List<SelectListItem>
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

        public ActionResult NewConfirmation()
        {
            return View();
        }

        [HttpPost]
        public ActionResult NewConfirmation(NewStrategyModel model)
        {
            return RedirectToAction("Index", "MyStrategies");
        }
    }
}