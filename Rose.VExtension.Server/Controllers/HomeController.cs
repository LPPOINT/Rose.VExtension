using System.Web.Mvc;

namespace Rose.VExtension.Server.Controllers
{
    public class HomeController : PluginsController
    {
        public ActionResult Index()
        {
            AddAreaDataToken();
            return View();
        }
	}
}