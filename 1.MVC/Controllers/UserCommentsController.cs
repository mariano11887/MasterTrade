using _2.Service.Service;
using MasterTrade.Controllers.Base;
using MasterTrade.Models;
using System.Web.Mvc;

namespace MasterTrade.Controllers
{
    [Authorize]
    public class UserCommentsController : BaseController
    {
        // GET: UserComments
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Index(UserCommentsModel model)
        {
            try
            {
                new ServiceUserComments().Save(GetUserId(), model.Comments);

                return RedirectToAction("Index", "Home");
            }
            catch
            {
                return View(model);
            }
        }
    }
}
