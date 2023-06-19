using MasterTrade.Models;
using Microsoft.Ajax.Utilities;
using System.Collections.Generic;
using System.Web.Mvc;

namespace MasterTrade.Controllers
{
    public class MyStrategiesController : Controller
    {
        // GET: MyStrategies
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult NewStep1()
        {
            return View();
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
    }
}