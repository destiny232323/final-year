<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="shippingPage.aspx.cs" Inherits="final_year.shippingPage" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
     <style type="text/css">

        .box {
            margin: 70px;
            display:grid;
            grid-template-areas: 
                "header header header"
                "foot1 foot2 foot3"   ;
            grid-template-rows: 300px 200px;

        }

        .title {
            font-size:30px;
        }

         .main {
             text-align: center;             
             grid-area: header;
         }

         .sub {
            margin-top: 50px;
            border: 1px solid black;
            text-align: center;
            border-radius: 10px;
            grid-area: foot1;
            margin-right: 20px;
            padding-top: 60px;
            font-size: 20px;

         }

         .sub2 {
            margin-top: 50px;
            border: 1px solid black;
            text-align: center;
            border-radius: 10px;
            grid-area: foot2;
            margin-right: 20px;
            padding-top: 60px;
            font-size: 20px;
         }

         .sub3 {
            margin-top: 50px;
            border: 1px solid black;
            text-align: center;
            border-radius:10px;
            grid-area: foot3;
            margin-right: 20px;
            padding-top: 60px;
            font-size: 20px;
         }

        .sub:hover {
            color:blue;
            cursor: pointer;
        }

        .sub2:hover {
            color:blue;
            cursor: pointer;
        }

        .sub3:hover {
            color:blue;
            cursor: pointer;
        }

        .auto-style1 {
             width: 60%;
        }

         .table2{
             width: 100%;
             
         }

         .table1, tr, td{
             width: 100%;
         }

    </style>

</head>
<body>
    <form id="form1" runat="server">
        <div class="title">Shipping Detail</div>
            
        <div class="box">
        <div class="main">
            <table class="table1">
            <tr>
                <td style="width: 25%">Shipping ID</td>
                <td style="width: 25%">Product</td>
                <td style="width: 25%">Shipping Type</td>
                <td style="width: 25%">TotalAmount</td>

            </tr>
            </table>
             <hr>   
                <asp:DataList ID="DataList1" runat="server" DataSourceID="SqlDataSource1" Width="100%" >

                    <ItemTemplate>
                        <table class="table2">
                            <tr>
                                <td style="width: 25%"><asp:Label ID="Label1" runat="server" Text='<%# Eval("shippingId") %>'></asp:Label></td>
                                <td style="width: 25%"><asp:Label ID="Label2" runat="server" Text='<%# Eval("shippingDesc") %>'></asp:Label></td>
                                <td style="width: 25%"><asp:Label ID="Label4" runat="server" Text='<%# Eval("shippingInfo") %>'></asp:Label></td>
                                <td style="width: 25%"><asp:Label ID="Label3" runat="server" Text='<%# Eval("amount") %>'></asp:Label></td>
                                
                            </tr>
                            
                        </table>
                        <hr>
                    </ItemTemplate>
                    </asp:DataList>

                    <asp:SqlDataSource ID="SqlDataSource1" runat="server" ConnectionString="<%$ ConnectionStrings:ConnectionString %>" SelectCommand="SELECT [shippingId], [shippingDesc], [shippingInfo], [amount] FROM [ShippingOrder]"></asp:SqlDataSource>

                 
        </div>
        
        <div class="sub">Contact Seller </div>
        <div class="sub2">Request Refund </div>
        <div class="sub3">Order Received</div>


        </div>
    </form>
</body>
</html>
