using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Text.RegularExpressions;
using System.Data.SqlClient;
using System.Configuration;
using Acme.Models;
using Acme.Models.ViewModels;

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

        [HttpPost]
        public ActionResult Order(Cart_Lineitem cart)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    dbcon.Open();
                    //cart.CartNumber = 100;
                    if (Session["cartnumber"] == null)
                        Session["cartnumber"] = Utility.GetIdNumber(dbcon, "CartNumber");
                    int cartnumber = Convert.ToInt32(Session["cartnumber"].ToString());
                    int intresult = Cart_Lineitem.CartUpSert(dbcon, cart);
                    dbcon.Close();
                    return RedirectToAction("Cart");
                }
                catch (Exception ex) { throw new Exception(ex.Message); }
            }
            ViewBag.errormsg = "Invalid data found in Order Page";
            return View("error");
        }

        public ActionResult Cart()
        {
            //List<Cartvm1> cartvm1List;
            List<Cartvm1> cartlist = new List<Cartvm1>();
            try
            {
                if(Session["cartnumber"] != null)
                {
                    dbcon.Open();
                    int cartnumber = Convert.ToInt32(Session["cartnumber"].ToString());
                    //cartvm1List = Cartvm1.GetCartList(dbcon, 100);
                    cartlist = Cartvm1.GetCartList(dbcon, cartnumber);
                    dbcon.Close();
                }
                return View(cartlist);
            }
            catch (Exception ex) { throw new Exception(ex.Message); }
            //return View(cartvm1List);
        }

        [HttpPost]
        public ActionResult Cart(Cart_Lineitem cart, string udaction="")
        {
            udaction = udaction.ToLower();
            if(ModelState.IsValid && (udaction == "update" || udaction == "delete"))
            {
                try
                {
                    dbcon.Open();
                    cart.CartNumber = 100;
                    int intresult = Cart_Lineitem.CUDCart_Lineitem(dbcon, udaction, cart);
                    dbcon.Close();
                    return RedirectToAction("Cart");
                }
                catch(Exception ex) { throw new Exception(ex.Message); }
            }
            ViewBag.errormsg = "Invalid data found in Cart Page";
            return View("error");            
        }
    }
}