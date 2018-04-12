using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MessagePublishingTest.Events;
using MessageBus;

namespace MessagePublishingTest.Controllers
{
    public class PublisherController : Controller
    {
        private string Index_VIEW = "Index";
        // GET: Publisher
        public ActionResult Index()
        {
            return View(Index_VIEW,new FundsTransfer());
        }

        [HttpPost]
        public ActionResult PublishMessage(FundsTransfer fundsTransfer)
        {
            var factory = ObjectFactory.GetBusManager<FundsTransfer>();
            factory.SendMessage(fundsTransfer, new ApplicationInfo{ApplicationName="WebApp" });
            return View(Index_VIEW, fundsTransfer);
        }                
    }
}
