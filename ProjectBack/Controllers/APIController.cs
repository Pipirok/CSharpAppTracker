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
            return View();
        }

        public ActionResult AllUsers()
        {
            return View(db.UserApplications.Select(_ => _.UserIP).ToList().Distinct().ToList());
        }

        public ActionResult UserApps()
        {
            string UserIP = Request.Params["UserIP"].ToString();
            List<UsersAndApplication> data = db.UsersAndApplications.Where(_ => _.UserIP == UserIP).ToList();
            return View(data);
        }

        public ActionResult AllApps()
        {
            return View(db.Applications.ToList());
        }

        [HttpPost]
        public ActionResult SubmitApps()
        {
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
    }
}