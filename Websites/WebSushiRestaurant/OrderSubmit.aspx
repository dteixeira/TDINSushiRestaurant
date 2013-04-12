<%@ Page Language="C#" AutoEventWireup="true" CodeFile="OrderSubmit.aspx.cs" Inherits="OrderSubmit" Debug="true" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <link href="style1.css" rel="stylesheet" type="text/css" />
</head>
<body>
    <h1>Order Confirmation</h1>
    <form id="form1" runat="server">
        <div id="content">
            <asp:PlaceHolder ID="GoodOrder" runat="server">
                <p><span>Full Name </span>
                    <asp:Label ID="Label1" runat="server" Text="Label" CssClass="info"></asp:Label></p>
                <p><span>Address </span>
                    <asp:Label ID="Label2" runat="server" Text="Label" CssClass="info"></asp:Label></p>
                <p><span>Credit Card Number </span>
                    <asp:Label ID="Label3" runat="server" Text="Label" CssClass="info"></asp:Label></p>
                <table>
                    <tr>
                        <th>Dish Name</th>
                        <th>Price</th>
                        <th>Quantity</th>
                        <th>Dish Total</th>
                    </tr>
                    <asp:PlaceHolder ID="Table" runat="server"></asp:PlaceHolder>
                </table>
                <p><span>Total Price</span><asp:Label ID="Label4" runat="server" Text="Label" CssClass="info"></asp:Label></p>
                <asp:Button ID="Confirm" runat="server" OnClick="Confirm_Click" Text="Confirm Order" CssClass="button first" />
                <asp:Button ID="Cancel" runat="server" OnClick="Cancel_Click" Text="Cancel Order" CssClass="button" />
            </asp:PlaceHolder>
            <asp:PlaceHolder ID="BadOrder" runat="server">
                <p><span id="bad">Your order was not correct. Please make another one.</span></p>
                <asp:Button ID="Invalid" runat="server" OnClick="Cancel_Click" Text="Return to the order page" CssClass="button first" />
            </asp:PlaceHolder>
        </div>
    </form>
</body>
</html>