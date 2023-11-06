using _2.Service.Service;
using MasterTrade.Controllers.Base;
using MasterTrade.Models;
using System.Linq;
using System.Web.Mvc;

namespace MasterTrade.Controllers
{
    public class HomeController : BaseController
    {
        public ActionResult Index()
        {
            HomeModel model = new HomeModel
            {
                UserHasStrategies = new ServiceStrategy().GetUserStrategies(GetUserId()).Any()
            };

            return View(model);
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}