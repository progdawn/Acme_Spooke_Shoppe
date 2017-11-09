using Acme.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

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
            //int custid = 136;
            int custid = Convert.ToInt32(Session["custid"].ToString());
            ViewBag.Statelist = Utility.GetStatesDropDown(dbcon);
            Customer cust = Customer.GetCustomerSingle(dbcon, custid, "");
            dbcon.Close();
            return View(cust);
        }
    }
}