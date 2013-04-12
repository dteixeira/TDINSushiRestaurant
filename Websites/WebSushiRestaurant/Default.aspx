<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Default.aspx.cs" Inherits="_Default" Debug="true" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <link href="style.css" rel="stylesheet" type="text/css" />
</head>
<body>
    <h1>New Order</h1>
    <div id="content">

        <asp:Panel ID="Panel1" runat="server">
            <div id="container">
                <form id="form1" runat="server">
                    <table>
                        <tr>
                            <th>Dish name</th>
                            <th>Price</th>
                            <th>Quantity</th>
                        </tr>
                        <asp:PlaceHolder ID="PlaceHolder1" runat="server"></asp:PlaceHolder>
                    </table>
                    <p><span>Full Name </span>
                        <input type="text" name="name" required="required" /></p>
                    <p><span>Address </span>
                        <input type="text" name="address" required="required" /></p>
                    <p><span>Credit Card Number </span>
                        <input type="text" name="cc" required="required" /></p>
                    <input class="button" type="submit" value="Submit Order" />
                    <asp:HyperLink CssClass="button" NavigateUrl="~/OrderConsult.aspx" runat="server" Text="Search Orders"></asp:HyperLink>
                </form>
            </div>
        </asp:Panel>
    </div>
</body>
</html>