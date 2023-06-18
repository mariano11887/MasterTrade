using MasterTrade.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
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
                },
                AddedStrategies = new List<string>
                {
                    "Media móvil (5)",
                    "Media móvil (10)"
                }
            };
            return View(model);
        }
    }
}