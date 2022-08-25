using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

namespace final_year
{
    public partial class WebForm2 : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            
            div1.Attributes["onClick"] = ClientScript.GetPostBackEventReference(this, "ClickDiv");
            div2.Attributes["onClick"] = ClientScript.GetPostBackEventReference(this, "ClickDiv2");

            if (IsPostBack)
            {
                if (Request["__EVENTTARGET"] == "__Page" &&
                    Request["__EVENTARGUMENT"] == "ClickDiv")
                {

                    div_Click();

                }

                else if (Request["__EVENTTARGET"] == "__Page" &&
                    Request["__EVENTARGUMENT"] == "ClickDiv2")
                {
                    div2_Click();

                }


            }
            else {
                paypal1.Visible = true;
                paypal2.Visible = true;
                paypal3.Visible = true;
                paypal4.Visible = true;

            }
            //--------------------------------------------------------------------------------
            SqlConnection con;
            string strCon = ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString;

            con = new SqlConnection(strCon);
            con.Open();

            string retrieveName = "Select customerName From CustomerTable Where customerId = @customerId";
            SqlCommand retrieveNameCmd = new SqlCommand(retrieveName, con);
            retrieveNameCmd.Parameters.AddWithValue("@customerId", 1);
            Name.Text = (retrieveNameCmd.ExecuteScalar()).ToString();
            con.Close();

            con.Open();

            string retrieveContact = "Select contactNumber From CustomerTable Where customerId = @customerId";
            SqlCommand retrieveContactCmd = new SqlCommand(retrieveContact, con);
            retrieveContactCmd.Parameters.AddWithValue("@customerId", 1);
            Contact.Text = (retrieveContactCmd.ExecuteScalar()).ToString();
            con.Close();

            con.Open();

            string retrieveAddress = "Select address From CustomerTable Where customerId = @customerId";
            SqlCommand retrieveAddressCmd = new SqlCommand(retrieveAddress, con);
            retrieveAddressCmd.Parameters.AddWithValue("@customerId", 1);
            Address.Text = (retrieveAddressCmd.ExecuteScalar()).ToString();
            con.Close();




            //---------------------------------------------------------------------------------
            showPoint();
            double subtotalPrice = Convert.ToDouble(Session["totalPrice"]);
            subtotal.Text = "RM " + subtotalPrice.ToString("0.00");
            double shippingPrice = Convert.ToInt32(Session["delivery"]);
            shipping.Text = "RM " + shippingPrice.ToString("0.00");
            double voucherApplied = 0;
            try
            {
                voucherApplied = double.Parse(TextBoxVoucher.Text);
            }
            catch (Exception ex) { 

            }

            double totalPrice = Convert.ToDouble(Session["totalPrice"]);
            double taxPrice = calculateTax(totalPrice);
            tax.Text = "RM " + taxPrice.ToString("0.00");

            double finalPrice = calculateFinalPrice(subtotalPrice, shippingPrice, voucherApplied, taxPrice);
            finalTotal.Text = "RM " + finalPrice.ToString("0.00");

            if (String.IsNullOrEmpty(TextBoxVoucher.Text) || TextBoxVoucher.Text == "0")
            {
                int tokenObtained = tokenEarned(finalPrice);
                pointsEarned.Text = tokenObtained.ToString();
            }
            else { 
                pointsEarned.Text = "0";    
            }

            Session["paidPrice"] = finalPrice;


            //con.Open();

            //string DeleteProduct = "Delete From ShippingOrder Where customerId = @customerId";
            //SqlCommand deleteCommand = new SqlCommand(DeleteProduct, con);

            //deleteCommand.Parameters.AddWithValue("@customerId", 1);
            //deleteCommand.ExecuteNonQuery();

            //con.Close();

        }

        protected void div_Click() {
            shippingSelected.Text = "*Standard Delivery";
            shipping.Text = "3.00";
            Session["delivery"] = 3.00;
            Session["deliveryType"] = "Standard";
        }

        protected void div2_Click() {
            shippingSelected.Text = "*Express Delivery";
            shipping.Text = "10.00";
            Session["delivery"] = 10.00;
            Session["deliveryType"] = "Express";
        }

        protected void DropDownList1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (DropDownList1.SelectedValue == "1")
            {
                paypal1.Visible = true;
                paypal2.Visible = true;
                paypal3.Visible = true;
                paypal4.Visible = true;
                touchAndGo1.Visible = false;
                touchAndGo2.Visible = false;
                visa1.Visible = false;
                visa2.Visible = false;
                visa3.Visible = false;
                visa4.Visible = false;
                visa5.Visible = false;
                visa6.Visible = false;
                visa7.Visible = false;
                visaMonth.Visible = false;
                visaYear.Visible = false;
                crypto1.Visible = false;
                crypto2.Visible = false;
                crypto3.Visible = false;
                crypto4.Visible = false;

            }
            else if (DropDownList1.SelectedValue == "2")
            {
                paypal1.Visible = false;
                paypal2.Visible = false;
                paypal3.Visible = false;
                paypal4.Visible = false;
                touchAndGo1.Visible = true;
                touchAndGo2.Visible = true;
                visa1.Visible = false;
                visa2.Visible = false;
                visa3.Visible = false;
                visa4.Visible = false;
                visa5.Visible = false;
                visa6.Visible = false;
                visa7.Visible = false;
                visaMonth.Visible = false;
                visaYear.Visible = false;
                crypto1.Visible = false;
                crypto2.Visible = false;
                crypto3.Visible = false;
                crypto4.Visible = false;

            }
            else if (DropDownList1.SelectedValue == "3")
            {
                paypal1.Visible = false;
                paypal2.Visible = false;
                paypal3.Visible = false;
                paypal4.Visible = false;
                touchAndGo1.Visible = false;
                touchAndGo2.Visible = false;
                visa1.Visible = true;
                visa2.Visible = true;
                visa3.Visible = true;
                visa4.Visible = true;
                visa5.Visible = true;
                visa6.Visible = true;
                visa7.Visible = true;
                visaMonth.Visible = true;
                visaYear.Visible = true;
                crypto1.Visible = false;
                crypto2.Visible = false;
                crypto3.Visible = false;
                crypto4.Visible = false;

            }
            else if (DropDownList1.SelectedValue == "4") 
            {
                paypal1.Visible = false;
                paypal2.Visible = false;
                paypal3.Visible = false;
                paypal4.Visible = false;
                touchAndGo1.Visible = false;
                touchAndGo2.Visible = false;
                visa1.Visible = false;
                visa2.Visible = false;
                visa3.Visible = false;
                visa4.Visible = false;
                visa5.Visible = false;
                visa6.Visible = false;
                visa7.Visible = false;
                visaMonth.Visible = false;
                visaYear.Visible = false;
                crypto1.Visible = true;
                crypto2.Visible = true;
                crypto3.Visible = true;
                crypto4.Visible = true;

            }
        }

        protected void Proceed_Click(object sender, EventArgs e)
        {
            //if (DropDownList1.SelectedValue == "1")
            //{
            //    Response.Write("<form action='https://www.sandbox.paypal.com/cgi-bin/webscr' method ='post' target ='_top'>");
            //    Response.Write("<input type='hidden' name='cmd' value='_s - xclick'>");
            //    Response.Write("<input type='hidden' name='hosted_button_id' value='RZSC45TKZXBAA'>");
            //    Response.Write("<input type='hidden' name='amount' value='20'>");
            //    Response.Write("<input type='hidden' name='item_name' value='Total Price'>");
            //    Response.Write("<input type='hidden' name='quantity' value='1'>");
            //    Response.Write("<input type='image' src = 'https://www.sandbox.paypal.com/en_US/i/btn/btn_buynowCC_LG.gif' border = '0' name = 'submit' alt = 'PayPal - The safer, easier way to pay online!'>");
            //    Response.Write("<input type='hidden' name='quantity' value='1'>");

            //}
            //else if (DropDownList1.SelectedValue == "2") 
            //{
            //    Response.Write("<form action='https://www.google.com'>");
            //}

            SqlConnection con;
            string strCon = ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString;

            con = new SqlConnection(strCon);

            con.Open();//retieve points to compare
            string retrievePoint = "Select point from Voucher Where customerId = @customerId";
            SqlCommand cmd1 = new SqlCommand(retrievePoint, con);
            cmd1.Parameters.AddWithValue("@customerId", 1);
            int tokenCompare = Convert.ToInt32(cmd1.ExecuteScalar().ToString());
            con.Close();

            if (Regex.IsMatch(TextBoxVoucher.Text, @"\d") && ((paypal3.Text.ToString()).Length > 1 || (touchAndGo2.Text.ToString()).Length > 1 || (crypto3.Text.ToString()).Length > 1 || (visa5.Text.ToString()).Length > 1))
            {
                if (Convert.ToInt32(TextBoxVoucher.Text) < tokenCompare)
                {


                    Response.Write("<script>alert('Payment Successfully')</script>");
                    con.Open();

                    int voucherLeft = Convert.ToInt32(TextBoxVoucher.Text);
                    string updateVoucher = "Update Voucher Set Point = Point - @used";
                    SqlCommand updateVoucherCmd = new SqlCommand(updateVoucher, con);
                    updateVoucherCmd.Parameters.AddWithValue("@used", voucherLeft);
                    updateVoucherCmd.ExecuteNonQuery();
                    con.Close();

                    TextBoxVoucher.Text = voucherLeft.ToString();
                    showPoint();

                    con.Open();
                    string selectProduct = "Select CartTable.productId from CartTable Inner Join TryProduct On CartTable.productId = TryProduct.productId";
                    List<String> product = new List<string>();
                    SqlCommand selectProductCmd = new SqlCommand(selectProduct, con);
                    SqlDataReader reader = selectProductCmd.ExecuteReader();
                    while (reader.Read())
                    {
                        product.Add(reader["productId"].ToString());
                    }
                    con.Close();

                    con.Open();
                    string selectQuantity = "Select CartTable.quantity from CartTable Inner Join TryProduct On CartTable.productId = TryProduct.productId";
                    List<int> quantity = new List<int>();
                    SqlCommand selectQuantityCmd = new SqlCommand(selectQuantity, con);
                    SqlDataReader reader2 = selectQuantityCmd.ExecuteReader();
                    while (reader2.Read())
                    {
                        quantity.Add(Convert.ToInt32(reader2["quantity"]));
                    }
                    con.Close();

                    con.Open();
                    string countProduct = "Select Count(cartId) From CartTable Where customerId = @customerId";
                    SqlCommand countProductCmd = new SqlCommand(countProduct, con);
                    countProductCmd.Parameters.AddWithValue("@customerId", 1);
                    int productCount = Convert.ToInt32((countProductCmd.ExecuteScalar()).ToString());

                    con.Close();

                    for (int i = 0; i < productCount; i++)
                    {
                        con.Open();
                        string updateStock = "Update TryProduct Set stock = stock - @stock Where productId = @productId";
                        SqlCommand updateStockCmd = new SqlCommand(updateStock, con);
                        updateStockCmd.Parameters.AddWithValue("@productId", product[i]);
                        updateStockCmd.Parameters.AddWithValue("@stock", quantity[i]);
                        updateStockCmd.ExecuteNonQuery();
                        con.Close();
                    }

                    con.Open();
                    string updateShipping = "Insert into ShippingOrder(amount, shippingDesc, customerId, shippingInfo) VALUES(@amount, @shippingDesc, @customerId, @shippingInfo)";
                    SqlCommand updateShippingCmd = new SqlCommand(updateShipping, con);
                    updateShippingCmd.Parameters.AddWithValue("@amount", Session["paidPrice"]);
                    updateShippingCmd.Parameters.AddWithValue("@shippingDesc", Convert.ToInt32(productCount));
                    updateShippingCmd.Parameters.AddWithValue("@customerId", 1);
                    updateShippingCmd.Parameters.AddWithValue("@shippingInfo", Session["deliveryType"]);
                    updateShippingCmd.ExecuteNonQuery();
                    con.Close();

                    con.Open();
                    string deleteCart = "Delete From CartTable Where customerId = @customerId";
                    SqlCommand deleteCartCmd = new SqlCommand(deleteCart, con);
                    deleteCartCmd.Parameters.AddWithValue("@customerId", 1);
                    deleteCartCmd.ExecuteNonQuery();
                    con.Close();
                }
                else
                {
                    Response.Write("<script>alert('Insufficient of token')</script>");
                }
            }
            else {
                Response.Write("<script>alert('Please fill in the required field')</script>");
            }

        }

        public double calculateTax(double totalPrice) {

            double finalTax = totalPrice * 0.06;
            return finalTax;
        }

        public double calculateFinalPrice(double first, double second, double third, double forth) {

            double finalPrice = first + second - (third/100) + forth;
            return finalPrice;

        }

        public int tokenEarned(double finalPrice) {

            double tokenEarned = finalPrice * 0.05;
            int adjustedToken = Convert.ToInt32(tokenEarned);
            return adjustedToken;
        }

        protected void showPoint() {

            SqlConnection con;
            string strCon = ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString;

            con = new SqlConnection(strCon);
            con.Open();

            string showPoints = "Select Voucher.point From Voucher Inner Join CartTable On CartTable.customerId = Voucher.customerId Where CartTable.customerId = @customerId";
            SqlCommand showPointsCmd = new SqlCommand(showPoints, con);
            showPointsCmd.Parameters.AddWithValue("@customerId", 1);
            LabelPoint.Text = (showPointsCmd.ExecuteScalar()).ToString();
            con.Close();

        }

        protected void btnVoucher_Click(object sender, EventArgs e)
        {
            Response.Redirect("voucherStakingPage.aspx");
        }
    }
}