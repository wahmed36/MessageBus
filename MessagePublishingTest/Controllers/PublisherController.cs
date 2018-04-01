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

        // GET: Publisher/Details/5
        public ActionResult PublishMessage(FundsTransfer fundsTransfer)
        {
            var factory = ObjectFactory.GetBusManager<FundsTransfer>();
            factory.SendMessage(fundsTransfer, new ApplicationInfo{ApplicationName="WebApp" });
            return View(Index_VIEW, fundsTransfer);
        }

        // GET: Publisher/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Publisher/Create
        [HttpPost]
        public ActionResult Create(FormCollection collection)
        {
            try
            {
                // TODO: Add insert logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: Publisher/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: Publisher/Edit/5
        [HttpPost]
        public ActionResult Edit(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add update logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: Publisher/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: Publisher/Delete/5
        [HttpPost]
        public ActionResult Delete(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add delete logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }
    }
}
