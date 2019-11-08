using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Vienna.Controllers
{
    public class DuenioEventoController : Controller
    {
        // GET: DuenioEvento
        public ActionResult Index()
        {
            return View();
        }

        // GET: DuenioEvento/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: DuenioEvento/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: DuenioEvento/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(IFormCollection collection)
        {
            try
            {
                // TODO: Add insert logic here

                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: DuenioEvento/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: DuenioEvento/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, IFormCollection collection)
        {
            try
            {
                // TODO: Add update logic here

                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: DuenioEvento/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: DuenioEvento/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, IFormCollection collection)
        {
            try
            {
                // TODO: Add delete logic here

                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }
    }
}