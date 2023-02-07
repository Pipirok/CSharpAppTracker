using ProjectBack.Models.EntityFramework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Helpers;
using System.Web.Mvc;
using Newtonsoft.Json;

namespace ProjectBack.Controllers
{
    public class APIController : Controller
    {
        // GET: API
        ProjectDBEntities db = new ProjectDBEntities();
        public ActionResult Index()
        {
            if (Session["username"] == null) return Redirect("/Home/Index");

            return View();
        }

        public ActionResult AllUsers()
        {
            if (Session["username"] == null) return Redirect("/Home/Index");

            return View(db.UserApplications.Select(_ => _.UserIP).ToList().Distinct().ToList());
        }

        public ActionResult UserApps()
        {
            if (Session["username"] == null) return Redirect("/Home/Index");

            string UserIP = Request.Params["UserIP"].ToString();
            List<UsersAndApplication> data = db.UsersAndApplications.Where(_ => _.UserIP == UserIP).ToList();
            return View(data);
        }

        public ActionResult AllApps()
        {
            if (Session["username"] == null) return Redirect("/Home/Index");

            return View(db.Applications.ToList());
        }

        [Route("/API/ToggleAllowed/{ID}")]
        public ActionResult ToggleAllowed(int ID)
        {
            if (Session["username"] == null) return Redirect("/Home/Index");

            Application app = db.Applications.Find(ID);
            app.Allowed = app.Allowed == 1 ? 0 : 1;
            db.SaveChanges();

            return Redirect("/API/AllApps");
        }

        [HttpPost]
        public ActionResult SubmitApps()
        {
            if (Session["username"] == null) return Redirect("/Home/Index");

            try
            {
                List<string> apps = JsonConvert.DeserializeObject<List<string>>(Request.Params["apps"]);
                string IP = Request.UserHostAddress;
                foreach (string appName in apps)
                {
                    int appID;
                    if (db.Applications.Where(app => app.Name == appName).ToList().Count == 0)
                    {
                        appID = db.Applications.Add(new Application { Allowed = 0, Name = appName }).ID;
                        db.SaveChanges();
                    }
                    else
                    {
                        appID = db.Applications.Where(app => app.Name == appName).First().ID;
                    }
                    if (db.UserApplications.Where(_ => _.UserIP == IP && _.AppID == appID).ToList().Count != 0) continue;
                    UserApplication userAppToAdd = new UserApplication()
                    {
                        UserIP = IP,
                        AppID = appID,
                    };
                    db.UserApplications.Add(userAppToAdd);
                }
                db.SaveChanges();
                return Json(new { error = false });
            }
            catch (Exception e)
            {
                return Json(new { message = e.Message, error = true });
            }
        }

        [HttpPost]
        public ActionResult Login(string login, string password)
        {
            AdminUser admin = db.AdminUsers.Where(d => d.Login == login).First();
            if(admin == null) return Redirect("/Home/Index");
            if (!(admin.Password == password)) return Redirect("/Home/Index");

            Session["username"] = admin.Login;

            return Redirect("/API/Index");
        }
    }
}