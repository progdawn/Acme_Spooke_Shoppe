using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Text.RegularExpressions;
using System.Data.SqlClient;
using System.Configuration;
using Acme.Models;

namespace Acme.Controllers
{
    public class ShoppingController : Controller
    {
        SqlConnection dbcon = new SqlConnection(ConfigurationManager.ConnectionStrings["Acmedb"].ConnectionString.ToString());

        // GET: Shopping
        public ActionResult Index(string id = "")
        {
            List<Product> productList;
            string query = "WHERE CategoryID = '" + id + "'";

            if (Regex.IsMatch(id, @"^[A-Za-z0-9]{2,10}$"))
            {
                try
                {
                    dbcon.Open();
                    productList = Product.GetProductList(dbcon, query);
                    dbcon.Close();
                }
                catch (Exception ex) { throw new Exception(ex.Message); }
                return View(productList);
            }
            ViewBag.errormsg = "Invalid data found in Shopping Page";
            return View("error");
        }

        // GET: Shopping
        public ActionResult Order(string id = "")
        {
            Product prod;

            if (Regex.IsMatch(id, @"^[A-Za-z0-9]{2,10}$"))
            {
                try
                {
                    dbcon.Open();
                    prod = Product.GetProductSingle(dbcon, id);
                    dbcon.Close();
                }
                catch (Exception ex) { throw new Exception(ex.Message); }
                return View(prod);
            }
            ViewBag.errormsg = "Invalid data found in Shopping Page";
            return View("error");
        }

        public ActionResult Cart()
        {
            return View();
        }
    }
}