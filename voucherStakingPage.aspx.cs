using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data.SqlClient;
using System.Configuration;
using System.Text.RegularExpressions;

namespace final_year
{
    public partial class WebForm1 : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

            showPoint();
            showStakePoint();
            showTotalPoint();

            //calculate percentage
            //100percent = 10000token
            //addwith previous value
            SqlConnection con;
            string strCon = ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString;

            con = new SqlConnection(strCon);
            con.Open();

            string retrieveStakePoint = "Select pointStake From Voucher Where customerId = @customerId";
            SqlCommand retrieveStakePointCmd = new SqlCommand(retrieveStakePoint, con);
            retrieveStakePointCmd.Parameters.AddWithValue("@customerId", 1);
            int stakePoint = Convert.ToInt32(retrieveStakePointCmd.ExecuteScalar().ToString());

            con.Close();

            con.Open();

            string retrieveTotalPoint = "Select point From TokenPool";
            SqlCommand retrieveTotalPointCmd = new SqlCommand(retrieveTotalPoint, con);
            int totalPoint = Convert.ToInt32(retrieveTotalPointCmd.ExecuteScalar().ToString());

            con.Close();

            int reward = calculateReward(stakePoint, totalPoint);
            pointReward.Text = reward.ToString();

            con.Open();

            string retrieveRewardPoint = "Select reward from TokenPool";
            SqlCommand retrieveRewardPointCmd = new SqlCommand(retrieveRewardPoint, con);
            int rewardPoint = Convert.ToInt32(retrieveRewardPointCmd.ExecuteScalar().ToString());
            label1.Text = rewardPoint.ToString();

            con.Close();

            int stakePercentage = calculateStakeRate(totalPoint, rewardPoint);
            rate.Text = stakePercentage.ToString() + "%";

            //String foo = DateTime.Now.ToString("HH:mm");
            //label2.Text = foo.ToString();

            TimeSpan hourMinute;
            hourMinute = DateTime.Now.TimeOfDay;
            Boolean status = true;
            if (hourMinute >= new TimeSpan(12, 15, 0))
            {
                con.Open();
                string insertReward = "Update Voucher Set pointReward = pointReward + @pointReward Where customerId = @customerId";
                SqlCommand insertRewardCmd = new SqlCommand(insertReward, con);
                insertRewardCmd.Parameters.AddWithValue("@pointReward", reward);
                insertRewardCmd.Parameters.AddWithValue("@customerId", 1);
                insertRewardCmd.ExecuteNonQuery();
                con.Close();

                con.Open();
                string retrieveReward = "Select pointReward From Voucher Where customerId = @customerId";
                SqlCommand retrieveRewardCmd = new SqlCommand(retrieveReward, con);
                retrieveRewardCmd.Parameters.AddWithValue("@customerId", 1);
                label2.Text = retrieveRewardCmd.ExecuteScalar().ToString();
                con.Close();
            }

        }

        protected void stakeBtn_Click(object sender, EventArgs e)
        {
            SqlConnection con;
            string strCon = ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString;

            con = new SqlConnection(strCon);

            con.Open();//retieve points to compare
            string retrievePoint = "Select point from Voucher Where customerId = @customerId";
            SqlCommand cmd1 = new SqlCommand(retrievePoint, con);
            cmd1.Parameters.AddWithValue("@customerId", 1);
            int tokenCompare = Convert.ToInt32(cmd1.ExecuteScalar().ToString());
            con.Close();

            if (Regex.IsMatch(tokenStake.Text, @"\d"))
            {

                if (Convert.ToInt32(tokenStake.Text) <= tokenCompare)
                {

                    con.Open();
                    int storeToken = Convert.ToInt32(tokenStake.Text);
                    string updatePoint = "Update Voucher set point = point - @point";
                    SqlCommand cmd2 = new SqlCommand(updatePoint, con);
                    cmd2.Parameters.AddWithValue("@point", storeToken);
                    cmd2.ExecuteNonQuery();
                    con.Close();

                    con.Open();
                    string pointStake = "Update Voucher set pointStake = pointStake + @pointStake where customerId = @customerId";
                    SqlCommand pointStakeCommand = new SqlCommand(pointStake, con);
                    pointStakeCommand.Parameters.AddWithValue("@pointStake", storeToken);
                    pointStakeCommand.Parameters.AddWithValue("@customerId", 1);
                    pointStakeCommand.ExecuteNonQuery();
                    con.Close();

                    con.Open();
                    string tokenPool = "Update TokenPool Set point = point + @point";
                    SqlCommand cmd4 = new SqlCommand(tokenPool, con);
                    cmd4.Parameters.AddWithValue("@point", storeToken);
                    cmd4.ExecuteNonQuery();
                    con.Close();


                    showPoint();
                    showStakePoint();
                    showTotalPoint();
                    Response.Redirect("voucherStakingPage.aspx");
                }
                else {
                    Response.Write("<script>alert('Insufficient of token')</script>");
                }
            }
            else {
                Response.Write("<script>alert('Invalid input, Please enter number only')</script>");
            }
        }

        protected void showPoint() {
            SqlConnection con;
            string strCon = ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString;

            con = new SqlConnection(strCon);
            con.Open();

            string selectPoint = "Select point from Voucher Where customerId = @customerId";
            SqlCommand cmd1 = new SqlCommand(selectPoint, con);
            cmd1.Parameters.AddWithValue("@customerId", 1);
            string tokenDisplay = cmd1.ExecuteScalar().ToString();

            token.Text = tokenDisplay;
            con.Close();

        }

        protected void showStakePoint() {
            SqlConnection con;
            string strCon = ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString;

            con = new SqlConnection(strCon);
            con.Open();
            string displayStakeToken = "Select pointStake from Voucher Where customerId = @customerId";
            SqlCommand displayStakeTokenCmd = new SqlCommand(displayStakeToken, con);
            displayStakeTokenCmd.Parameters.AddWithValue("@customerId", 1);
            stakeValue.Text = (displayStakeTokenCmd.ExecuteScalar()).ToString();
            con.Close();

        }

        protected void showTotalPoint() {
            SqlConnection con;
            string strCon = ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString;

            con = new SqlConnection(strCon);
            con.Open();
            string displayTotalToken = "Select point from TokenPool";
            SqlCommand cmd5 = new SqlCommand(displayTotalToken, con);

            string totalTokenDisplay = cmd5.ExecuteScalar().ToString();
            tokenCirculate.Text = totalTokenDisplay;

            con.Close();
        }

        protected int calculateReward(int one, int two) {
            double parameterOne = (double)one;
            double parameterTwo = (double)two;
            double totalPercentage = (parameterOne * 100) / parameterTwo;

            SqlConnection con;
            string strCon = ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString;

            con = new SqlConnection(strCon);
            con.Open();

            string retrieveRewardPoint = "Select reward from TokenPool";
            SqlCommand retrieveRewardPointCmd = new SqlCommand(retrieveRewardPoint, con);
            int reward = Convert.ToInt32(retrieveRewardPointCmd.ExecuteScalar().ToString());
            int totalReward = Convert.ToInt32((totalPercentage * reward) / 100);

            return totalReward;
        }

        protected int calculateStakeRate(int one, int two) {
            int stakePercentage = (one * 100) / two;
            return stakePercentage;
        }

        protected void Button1_Click(object sender, EventArgs e)//claim
        {

            SqlConnection con;
            string strCon = ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString;

            con = new SqlConnection(strCon);
            con.Open();
            string retrieveReward = "Select pointReward From Voucher Where customerId = @customerId";
            SqlCommand retrieveRewardCmd = new SqlCommand(retrieveReward, con);
            retrieveRewardCmd.Parameters.AddWithValue("@customerId", 1);
            int pointReward = Convert.ToInt32(retrieveRewardCmd.ExecuteScalar());
            con.Close();

            con.Open();
            string insertReward = "Update Voucher Set pointReward = @pointReward Where customerId = @customerId";
            SqlCommand insertRewardCmd = new SqlCommand(insertReward, con);
            insertRewardCmd.Parameters.AddWithValue("@pointReward", 0);
            insertRewardCmd.Parameters.AddWithValue("@customerId", 1);
            insertRewardCmd.ExecuteNonQuery();
            con.Close();



            con.Open();
            string updatePoint = "Update point Set point = point + @point Where customerId = @customerId";
            SqlCommand updatePointCmd = new SqlCommand(updatePoint, con);
            updatePointCmd.Parameters.AddWithValue("@point", pointReward);
            updatePointCmd.Parameters.AddWithValue("@customerId", 1);
            updatePointCmd.ExecuteNonQuery();
            con.Close();

            Response.Redirect("voucherStakingPage.aspx");

        }

        protected void Button2_Click(object sender, EventArgs e)//restake
        {
            SqlConnection con;
            string strCon = ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString;

            con = new SqlConnection(strCon);
            con.Open();
            string retrieveReward = "Select pointReward From Voucher Where customerId = @customerId";
            SqlCommand retrieveRewardCmd = new SqlCommand(retrieveReward, con);
            retrieveRewardCmd.Parameters.AddWithValue("@customerId", 1);
            int pointReward = Convert.ToInt32(retrieveRewardCmd.ExecuteScalar());
            con.Close();

            con.Open();
            string insertReward = "Update Voucher Set pointReward = @pointReward Where customerId = @customerId";
            SqlCommand insertRewardCmd = new SqlCommand(insertReward, con);
            insertRewardCmd.Parameters.AddWithValue("@pointReward", 0);
            insertRewardCmd.Parameters.AddWithValue("@customerId", 1);
            insertRewardCmd.ExecuteNonQuery();
            con.Close();

            con.Open();
            string updatePointStake = "Update pointStake Set pointStake = pointStake + @pointStake Where customerId = @customerId";
            SqlCommand updatePointStakeCmd = new SqlCommand(updatePointStake, con);
            updatePointStakeCmd.Parameters.AddWithValue("@pointStake", pointReward);
            updatePointStakeCmd.Parameters.AddWithValue("@customerId", 1);
            updatePointStakeCmd.ExecuteNonQuery();
            con.Close();

            showStakePoint();

        }


    }
}