using _2.Service.Service;
using MasterTrade.Controllers.Base;
using MasterTrade.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MasterTrade.Controllers
{
    public class CryptoPairController : BaseController
    {
        // GET: CryptoPair
        public ActionResult Index()
        {
            //new ServiceCryptoPair().ImportCandles();

            return View(new List<CryptoPairModel>
            {
                new CryptoPairModel { Name = "BTC/USDT" }
            });
        }

        // GET: CryptoPair/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: CryptoPair/Create
        public ActionResult Create()
        {
            CryptoPairModel model = new CryptoPairModel
            {
                AllSuppliers = new List<SelectListItem>
                {
                    new SelectListItem { Text = "TradingView", Value = "1" }
                },
                AllCryptoPairs = new List<SelectListItem>
                {
                    new SelectListItem { Text = "BTC/USDT", Value = "1" }
                }
            };

            return View(model);
        }

        // POST: CryptoPair/Create
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

        // GET: CryptoPair/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: CryptoPair/Edit/5
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

        // GET: CryptoPair/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: CryptoPair/Delete/5
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
