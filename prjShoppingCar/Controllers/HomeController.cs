using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using prjShoppingCar.Models;

namespace prjShoppingCar.Controllers
{
    public class HomeController : Controller
    {
        dbShoppingCarEntities db = new dbShoppingCarEntities();
        // GET: Home
        public ActionResult Index()
        {
            List<tProduct> p= db.tProduct.ToList();

            if(Session["Member"]==null)
                return View(p);
            return View("Index","_LayoutMember",p);
        }
        public ActionResult MyCart()
        {

            return View();
        }
        public ActionResult Login() {
            return View();
        }
        [HttpPost]
        public ActionResult Login(string fUserId,string fPwd)
        {
            var member = db.tMember.Where(m => m.fUserId == fUserId && m.fPwd == fPwd).FirstOrDefault();
            if (member == null) {
                ViewBag.Message = "帳號或密碼錯誤";
                return View();
            }


            Session["Member"] = member;
            Session["Welcome"] = member.fName + "真正高興地見到你";

            return RedirectToAction("Index");
        }

        public ActionResult Logout()
        {
            Session.Clear();
            return RedirectToAction("Index");
        }
        public ActionResult Order()
        {
            if (Session["Member"] == null)
                return RedirectToAction("Login");
            return View("Order", "_LayoutMember");
        }
        [HttpPost]
        public ActionResult Order(string fAddress,string data)
        {
            var member = (tMember)Session["Member"];
            db.Add_Order(member.fUserId, fAddress, data);
            db.SaveChanges();

            TempData["Order"] = "Y";

            return RedirectToAction("MyCart");
        }
    }
}