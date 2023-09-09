using _2.Service.Service;
using MasterTrade.Controllers.Base;
using MasterTrade.Models;
using System.Collections.Generic;
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
        public ActionResult NewStep1(NewStrategyModel model)
        {
            bool isNameValid = new ServiceStrategy().CheckStrategyName(GetUserId(), model.Name);
            if (!isNameValid)
            {
                ViewBag.ErrorMsg = "El nombre elegido ya existe.";
                return View(model);
            }

            return RedirectToAction("NewStep2", "MyStrategies");
        }

        public ActionResult NewStep2()
        {
            NewStrategyModel model = new NewStrategyModel()
            {
                AllIndicators = new List<SelectListItem>
                {
                    new SelectListItem { Value = "1", Text = "Media móvil" }
                }
            };
            return View(model);
        }

        [HttpPost]
        public ActionResult NewStep2(NewStrategyModel model)
        {
            return RedirectToAction("NewStep3", "MyStrategies");
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