using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using QlikAuthentication.Models;
using QlikAuthentication.QlikAuthentication;
using System.Web.SessionState;

namespace QlikAuthentication.Controllers
{
    public class QlikSessionController : Controller
    {
        public ActionResult Index()
        {
            QlikSessionViewModel model = new QlikSessionViewModel
            {
                Server = "https://172.19.13.100",
                Port = "4243",
                Certificate = "QlikClient",
                VirtualProxy = "detectr",
                UserDirectory = "MITRAIS",
                User = "agungb_d"
            };
            return View(model);
        }

        [HttpPost]
        public ActionResult RequestSession(QlikSessionViewModel model)
        {
            Authentication auth = new Authentication(model.Server, model.VirtualProxy, model.Port, model.Certificate, model.UserDirectory, model.User, model.SessionId);

            try
            {
                SessionResponse response = auth.GetSession();

                DateTime now = DateTime.Now;
                HttpCookie cookie = new HttpCookie(string.Format("X-Qlik-Session{0}", string.IsNullOrEmpty(model.VirtualProxy) ? string.Empty : "-" + model.VirtualProxy));
                cookie.Value = response.SessionId;
                cookie.Expires = DateTime.MinValue;
                cookie.HttpOnly = true;

                cookie.Domain = model.Server; // only works if qliksense url domain is same with this web app

                Request.Cookies.Add(cookie);
                Response.Cookies.Add(cookie);

                return Json(string.Format("{0}{1}/qmc", model.Server, string.IsNullOrEmpty(model.VirtualProxy) ? string.Empty : "/" + model.VirtualProxy));
            }
            catch (Exception ex)
            {
                return Json(ex.Message);
            }
        }

        [HttpPost]
        public ActionResult GetSession()
        {
            SessionIDManager manager = new SessionIDManager();
            string sessionId = manager.CreateSessionID(System.Web.HttpContext.Current);
            bool redirected = false;
            bool isAdded = false;
            manager.SaveSessionID(System.Web.HttpContext.Current, sessionId, out redirected, out isAdded);
            return Json(sessionId);
        }
    }
}