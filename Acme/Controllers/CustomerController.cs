﻿using Acme.Models;
using System;
using System.Configuration;
using System.Data.SqlClient;
using System.Web.Mvc;

namespace Acme.Controllers
{
    public class CustomerController : Controller
    {
        SqlConnection dbcon = new SqlConnection(ConfigurationManager.ConnectionStrings["Acmedb"].ConnectionString.ToString());

        // GET: Customer
        [Authorize]
        public ActionResult Update()
        {
            dbcon.Open();
            if (Session["custid"] == null) return RedirectToAction("Index", "home");
            int custid = Convert.ToInt32(Session["custid"].ToString());
            ViewBag.Statelist = Utility.GetStatesDropDown(dbcon);
            Customer cust = Customer.GetCustomerSingle(dbcon, custid, "");
            dbcon.Close();
            return View(cust);
        }
    }
}