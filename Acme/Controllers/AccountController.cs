using Acme.Models;
using Acme.Models.ViewModels;
using System;
using System.Configuration;
using System.Data.SqlClient;
using System.Web.Mvc;
using System.Web.Security;

namespace Acme.Controllers
{
    public class AccountController : Controller
    {
        SqlConnection dbcon = new SqlConnection(ConfigurationManager.ConnectionStrings["Acmedb"].ConnectionString.ToString());

        // GET: Account
        [HttpGet]
        [AllowAnonymous]
        public ActionResult Login()
        {
            if (Request.QueryString["returnurl"] == null)
                return RedirectToAction("Index", "Home");
            Loginvm loginvm = new Loginvm();
            ViewBag.message = "";
            return View(loginvm);
        }

        [HttpPost]
        public ActionResult Login(Loginvm login)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    dbcon.Open();
                    Customer cust = Customer.GetCustomerSingle(dbcon, 0, login.Username);
                    dbcon.Close();
                    if (cust.CustNumber > 0 && cust.PWD == login.Password)
                    {
                        string ReturnUrl = Request.QueryString["returnurl"].ToString();
                        if (ReturnUrl.Length > 1 && Url.IsLocalUrl(ReturnUrl))
                        {
                            Session["custid"] = cust.CustNumber;
                            FormsAuthentication.SetAuthCookie(login.Username, false);
                            return Redirect(ReturnUrl);
                        }
                        else
                            return RedirectToAction("Index", "Home");
                    }
                }
                catch (Exception ex) { throw new Exception(ex.Message); }
            }
            ViewBag.message = "Credentials are not valid";
            return View(login);
        }
    }
}