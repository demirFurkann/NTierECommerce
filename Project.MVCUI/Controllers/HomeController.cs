using Project.BLL.Repositories.ConcRep;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Project.MVCUI.Controllers
{
    public class HomeController : Controller
    {

        // GET: Home
        public HomeController()
        {

        }
        public ActionResult Login()
        {

			return View();
        }
    }
}