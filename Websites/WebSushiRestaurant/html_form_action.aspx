<%@ Page Language="C#" AutoEventWireup="true" CodeFile="html_form_action.aspx.cs" Inherits="html_form_action" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
    <div id="container">
        <h2>Order Confirmation</h2>
        <strong>Full Name: </strong><asp:Label ID="Label1" runat="server" Text="Label"></asp:Label><br />
        <strong>Address: </strong><asp:Label ID="Label2" runat="server" Text="Label"></asp:Label><br />
        <strong>Credit Card Number: </strong><asp:Label ID="Label3" runat="server" Text="Label"></asp:Label>
        <br />
        <br />
        <table>
            <tr>
                <th>Dish Name</th>
                <th>Price</th>
                <th>Quantity</th>
                <th>Dish Total</th>
            </tr>
        <asp:Placeholder ID="PlaceHolder2" runat="server"></asp:Placeholder>
        </table>
        <br />
        <strong>Total Price</strong><br /><asp:Label ID="Label4" runat="server" Text="Label"></asp:Label>
        <br />
        <br />
        <br />
        <asp:Button ID="Button1" runat="server" OnClick="Button1_Click" Text="Confirm Order" />
        
    </div>
    </form>
</body>
</html>
