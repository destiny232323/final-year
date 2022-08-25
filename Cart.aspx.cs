using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Windows;

namespace final_year
{
    public partial class Cart : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

            Label1.Text = getTotalPrice().ToString("0.00");
            Session["totalPrice"] = getTotalPrice();


        }

        protected void BtnAdd_Click(object sender, EventArgs e)
        {


            DataListItem item = ((Button)sender).NamingContainer as DataListItem;
            Label labelQuantity = (Label)item.FindControl("quantity");
            double value = double.Parse(labelQuantity.Text);
            value = value + 1;
            labelQuantity.Text = value.ToString();
            double newValue = double.Parse(labelQuantity.Text);

            DataListItem item2 = ((Button)sender).NamingContainer as DataListItem;
            Label labelProductName = (Label)item.FindControl("productName");
            String product = labelProductName.Text;

            calculateUnitTotalPrice(newValue, product);

            //------------------------------UpdateCartDatabase--------------------------------
            updateCartDatabase(value, product);
            

        }

        protected void delete_Click(object sender, EventArgs e)
        {
            SqlConnection con;
            string strCon = ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString;

            con = new SqlConnection(strCon);
            con.Open();

            DataListItem item = ((Button)sender).NamingContainer as DataListItem;
            Label labelProductName = (Label)item.FindControl("productName");
            String productName = labelProductName.Text;

            string DeleteProduct = "Delete From CartTable From CartTable Inner Join TryProduct On CartTable.productId = TryProduct.productId Where TryProduct.productName = @productName";
            SqlCommand deleteCommand = new SqlCommand(DeleteProduct,con);

            deleteCommand.Parameters.AddWithValue("@productName", productName);
            deleteCommand.ExecuteNonQuery();

            con.Close();

            Response.Redirect("Cart.aspx");
        }

        protected void BtnMinus_Click(object sender, EventArgs e)
        {
            DataListItem item = ((Button)sender).NamingContainer as DataListItem;
            Label labelQuantity = (Label)item.FindControl("quantity");
            double value = double.Parse(labelQuantity.Text);
            if (value > 0)
            {
                value = value - 1;
            }
            labelQuantity.Text = value.ToString();
            double newValue = double.Parse(labelQuantity.Text);

            DataListItem item2 = ((Button)sender).NamingContainer as DataListItem;
            Label labelProductName = (Label)item.FindControl("productName");
            String product = labelProductName.Text;

            calculateUnitTotalPrice(newValue, product);

            //------------------------------UpdateCartDatabase--------------------------------
            updateCartDatabase(value, product);

        }

        public void updateCartDatabase(double quantity, string name) {

            SqlConnection con;
            string strCon = ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString;

            con = new SqlConnection(strCon);
            con.Open();

            string retrieveStock = "Select stock From TryProduct Where productName = @productName";
            SqlCommand retrieveStockCmd = new SqlCommand(retrieveStock, con);
            retrieveStockCmd.Parameters.AddWithValue("@productName", name);
            string stock = retrieveStockCmd.ExecuteScalar().ToString();
            con.Close();

            if (quantity <= double.Parse(stock))
            {

                con.Open();

                string updateQuantity = "UPDATE CartTable SET quantity = @quantity FROM CartTable INNER JOIN TryProduct ON CartTable.productId = TryProduct.productId WHERE (TryProduct.productName = @productName)";
                SqlCommand updateQuantityCmd = new SqlCommand(updateQuantity, con);
                updateQuantityCmd.Parameters.AddWithValue("@productName", name);
                updateQuantityCmd.Parameters.AddWithValue("@quantity", quantity);
                updateQuantityCmd.ExecuteNonQuery();

                con.Close();
                

            }
            else {
                
                quantity -= 1;
                con.Open();

                string updateQuantity = "UPDATE CartTable SET quantity = @quantity FROM CartTable INNER JOIN TryProduct ON CartTable.productId = TryProduct.productId WHERE (TryProduct.productName = @productName)";
                SqlCommand updateQuantityCmd = new SqlCommand(updateQuantity, con);
                updateQuantityCmd.Parameters.AddWithValue("@productName", name);
                updateQuantityCmd.Parameters.AddWithValue("@quantity", quantity);
                updateQuantityCmd.ExecuteNonQuery();

                con.Close();
                MessageBox.Show("Sorry we run out of stock","Alert");

            }
            Response.Redirect("Cart.aspx");

        }

        double calculateUnitTotalPrice(double quantity, string name) {
            double totalPrice = 0;
            String productName = name;

            SqlConnection con;
            string strCon = ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString;

            con = new SqlConnection(strCon);
            con.Open();

            string getUnitTotal = "Select price From TryProduct where productName = @productName";
            SqlCommand getUnitTotalCommand = new SqlCommand(getUnitTotal,con);
            getUnitTotalCommand.Parameters.AddWithValue("@productName", productName);
            var unitPrice = getUnitTotalCommand.ExecuteScalar();
            totalPrice = quantity * double.Parse(unitPrice.ToString());

            con.Close();
            return totalPrice;
        }
        double getTotalPrice() {
            SqlConnection con;
            string strCon = ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString;

            con = new SqlConnection(strCon);
            con.Open();

            String getTotal = "Select CartTable.quantity, TryProduct.price From CartTable Inner Join TryProduct On CartTable.productId = TryProduct.productId";
            SqlCommand getTotalCmd = new SqlCommand(getTotal, con);
            SqlDataReader priceAndQuantity = getTotalCmd.ExecuteReader();
            double totalPrice = 0;  

            while (priceAndQuantity.Read()) { 
               
                totalPrice = totalPrice + (double.Parse(priceAndQuantity["price"].ToString()) * (double.Parse(priceAndQuantity["quantity"].ToString())));
            }
            con.Close();
            return totalPrice;



        }

        protected void Button1_Click(object sender, EventArgs e)
        {
            if (Label1.Text == "0")
            {
                Response.Write("<script>alert('Please select some item')</script>");
            }

            else
            {
                Response.Redirect("paymentPage.aspx");
            }
        }
    }
}