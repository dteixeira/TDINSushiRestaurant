<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Default.aspx.cs" Inherits="_Default" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <div style="height: 385px">
    
        <asp:Panel ID="Panel1" runat="server">
            <div id="container">
                <form id="form1" runat="server">
                    <table>
                        <tr>
                            <th>Dish name</th>
                            <th>Price</th>
                        </tr>
                        <asp:PlaceHolder ID="PlaceHolder1" runat="server">
                        </asp:PlaceHolder>
                    </table><br/><br/>

                    Full Name: <input type="text" name="name" required="required" /><br/><br/>
                    Address: <input type="text" name="address" required="required" /><br/><br/>
                    Credit Card Number: <input type="text" name="cc" required="required" /><br/><br/>
                    <input type="submit" value="Submit Order (You can review it before it's final!)"/>


                </form>
            </div>
        </asp:Panel>
        <br />
        <br />
        <br />
    
        &nbsp;&nbsp;
        <br />
        <br />
        &nbsp;&nbsp;
            
        <br />
        <br />
&nbsp;&nbsp;
            
    </div>
    
</body>
</html>
