using System;
using System.Collections.Generic;
using System.Web;
using System.Web.Mvc;
using System.Data.SqlClient;
using System.Configuration;
using Acme.Models;
using System.IO;

namespace Acme.Controllers
{
    public class ProductController : Controller
    {
        SqlConnection dbcon = new SqlConnection(ConfigurationManager.ConnectionStrings["Acmedb"].ConnectionString.ToString());

        // GET: Product
        public ActionResult Index()
        {
            List<Product> productList;
            try
            {
                dbcon.Open();
                productList = Product.GetProductList(dbcon, "");
                dbcon.Close();
            }
            catch (Exception ex) { throw new Exception(ex.Message); }
            return View(productList);
        }

        public ActionResult Create()
        {
            Product prod = new Product();
            return View(prod);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Product prod, HttpPostedFileBase uploadfile)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    if (uploadfile != null && uploadfile.ContentLength > 0)
                    {
                        var fileName = Path.GetFileName(uploadfile.FileName);
                        var path = Path.Combine(
                        Server.MapPath("~/Content/Images/products"), fileName);
                        uploadfile.SaveAs(path);
                        prod.ImageFile = fileName;
                    }
                    if (ModelState.IsValid)
                    {
                        try
                        {
                            dbcon.Open();
                            int intresult = Product.CUDProduct(dbcon, "create", prod);
                            dbcon.Close();
                            return RedirectToAction("Index");
                        }
                        catch (Exception ex) { throw new Exception(ex.Message); }
                    }
                }
                catch (Exception ex) { throw new Exception(ex.Message); }
            }
            ViewBag.errmsg = "Data validation error in Edit method";
            return View("Error");
        }

        public ActionResult Edit(String id = "")
        {
            try
            {
                dbcon.Open();
                Product prod = Product.GetProductSingle(dbcon, id);
                dbcon.Close();
                if (prod.ProductID != null)
                    return View(prod);
            }
            catch (Exception ex) { throw new Exception(ex.Message); }

            ViewBag.errormsg = "Invalid data in the Edit Page";
            return View("Error");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Product prod, HttpPostedFileBase uploadfile)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    if (uploadfile != null && uploadfile.ContentLength > 0)
                    {
                        var fileName = Path.GetFileName(uploadfile.FileName);
                        var path = Path.Combine(
                        Server.MapPath("~/Content/Images/products"), fileName);
                        uploadfile.SaveAs(path);
                        prod.ImageFile = fileName;
                    }
                    if (ModelState.IsValid)
                    {
                        try
                        {
                            dbcon.Open();
                            int intresult = Product.CUDProduct(dbcon, "update", prod);
                            dbcon.Close();
                            return RedirectToAction("Index");
                        }
                        catch (Exception ex) { throw new Exception(ex.Message); }
                    }
                }
                catch (Exception ex) { throw new Exception(ex.Message); }
            }
            ViewBag.errmsg = "Data validation error in Edit method";
            return View("Error");
        }

        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public ActionResult Edit(Product prod)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        try
        //        {
        //            dbcon.Open();
        //            int intresult = Product.CUDProduct(dbcon, "update", prod);
        //            dbcon.Close();
        //            return RedirectToAction("Index");
        //        }
        //        catch (Exception ex) { throw new Exception(ex.Message); }
        //    }
        //    ViewBag.errormsg = "Invalid data in Edit Post action method";
        //    return View("Error");
        //}

        public ActionResult Delete(String id)
        {
            try
            {
                dbcon.Open();
                Product prod = Product.GetProductSingle(dbcon, id);
                dbcon.Close();
                if (prod.ProductID != null)
                    return View(prod);
            }
            catch (Exception ex) { throw new Exception(ex.Message); }
            ViewBag.errormsg = "Invalid data in the Delete Page";
            return View("Error");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(string id, FormCollection fc)
        {
            Product prod = new Models.Product();
            prod.ProductID = id;
            try
            {
                dbcon.Open();
                int intresult = Product.CUDProduct(dbcon, "delete", prod);
                dbcon.Close();
                return RedirectToAction("Index");
            }
            catch (Exception ex) { throw new Exception(ex.Message); }
        }
    }
}