using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Data.SqlClient;
using System.Data;
using Acme.Models.ViewModels;


namespace Acme.Controllers
{
    public class Cart_Lineitem
    {
        [Required, Key, Column(Order = 1)]
        public int CartNumber { get; set; }
        [Required, Key, Column(Order = 2), MaxLength(10)]
        public string ProductId { get; set; }
        [Required, Range(minimum: 1, maximum: 99, ErrorMessage = "Qty sb 1-99")]
        public int Quantity { get; set; }

        public static Int32 CartUpSert(SqlConnection dbcon, Cart_Lineitem cart)
        {
            SqlCommand cmd = new SqlCommand("sp_cart_upsert", dbcon);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.Add("@cartid", SqlDbType.Int).Value = cart.CartNumber;
            cmd.Parameters.Add("@prodid", SqlDbType.VarChar).Value = cart.ProductId;
            cmd.Parameters.Add("@qty", SqlDbType.Int).Value = cart.Quantity;
            int intCnt = cmd.ExecuteNonQuery();
            return intCnt;
        }

        public static List<Cart_Lineitem> GetCart_LineitemList(SqlConnection dbcon, string SqlClause)
        {
            List<Cart_Lineitem> itemlist = new List<Cart_Lineitem>();
            
            string strsql = "select * from Cart_LineItems " + SqlClause;
            SqlCommand cmd = new SqlCommand(strsql, dbcon);
            SqlDataReader myReader;
            myReader = cmd.ExecuteReader();
            while(myReader.Read())
            {
                Cart_Lineitem obj = new Cart_Lineitem();
                obj.CartNumber = Convert.ToInt32(myReader["CartNumber"].ToString());
                obj.ProductId = myReader["ProductId"].ToString();
                obj.Quantity = Convert.ToInt32(myReader["Quantity"].ToString());
                itemlist.Add(obj);
            }
            myReader.Close();
            cmd.Dispose();
            return itemlist;
        }

        public static Int32 CUDCart_Lineitem(SqlConnection dbcon, string CUDAction, Cart_Lineitem cart)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;

            if(CUDAction == "update")
            {
                cmd.CommandText = "update Cart_LineItems set CartNumber = @CartNumber" +
                    "ProductID = @ProductID" +
                    "Quantity = @Quantity";
                cmd.Parameters.AddWithValue("@cartid", SqlDbType.Int).Value = cart.CartNumber;
                cmd.Parameters.AddWithValue("@prodid", SqlDbType.VarChar).Value = cart.ProductId;
                cmd.Parameters.AddWithValue("@qty", SqlDbType.Int).Value = cart.Quantity;
            }
            else if (CUDAction == "delete")
            {
                cmd.CommandText = "delete Cart_LineItems where CartNumber = @CartNumber";
                cmd.Parameters.AddWithValue("@cartid", SqlDbType.Int).Value = cart.CartNumber;
            }
            cmd.Connection = dbcon;
            int intCnt = cmd.ExecuteNonQuery();
            cmd.Dispose();
            return intCnt;
        }
    }
}