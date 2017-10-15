using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using QlikAuthentication.Models;
using QlikAuthentication.QlikAuthentication;

namespace QlikAuthentication.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            QlikTicketViewModel model = new QlikTicketViewModel
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
        public ActionResult RequestTicket(QlikTicketViewModel model)
        {
            Authentication auth = new Authentication(model.Server, model.VirtualProxy, model.Port, model.Certificate, model.UserDirectory, model.User);

            try
            {
                TicketResponse response = auth.GetTicket();
                return Json(string.Format("{0}{1}/qmc/?qlikTicket={2}", model.Server, string.IsNullOrEmpty(model.VirtualProxy) ? string.Empty : "/" + model.VirtualProxy, response.Ticket));
            }
            catch (Exception ex)
            {
                return Json(ex.Message);
            }           
        }        
    }
}