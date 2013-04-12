using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Xml.Linq;

public partial class _Default : System.Web.UI.Page
{
    private static readonly string _configFile = @"config/Menu.xml";

    protected void Page_Load(object sender, EventArgs e)
    {
        form1.Action = "html_form_action.aspx";


       // Panel1.Controls.Add(new LiteralControl(@"<form action=""html_form_action.aspx"" method=""post"">"));
        //Panel1.Controls.Add(new LiteralControl("<table><tr><th>Dish name</th><th>Price</th></tr>"));

        foreach (Sushi item in Global.LoadMenu(Server.MapPath(_configFile)))
        {
            PlaceHolder1.Controls.Add(new LiteralControl("<tr>"));
            string itemName = item.Type;
            double price = item.Price;
            PlaceHolder1.Controls.Add(new LiteralControl("<td>" + itemName + "</td>"));
            PlaceHolder1.Controls.Add(new LiteralControl("<td>" + String.Format("{0:0.00}", price) + "</td>"));
            PlaceHolder1.Controls.Add(new LiteralControl(@"<td><input type=""number"" name="""));
            PlaceHolder1.Controls.Add(new LiteralControl(itemName));
            PlaceHolder1.Controls.Add(new LiteralControl(@""" min=""0"" value=""0"" required=""required""></td>"));
            PlaceHolder1.Controls.Add(new LiteralControl("</tr>"));
        }

        //Panel1.Controls.Add(new LiteralControl("</table>"));

        //Panel1.Controls.Add(new LiteralControl(@"Full Name: <input type=""text"" name=""name""><br><br>"));
        //Panel1.Controls.Add(new LiteralControl(@"Address: <input type=""text"" name=""address""><br><br>"));
        //Panel1.Controls.Add(new LiteralControl(@"Credit Card Number: <input type=""text"" name=""cc""><br><br>"));

        //Panel1.Controls.Add(new LiteralControl(@"<input type=""submit"" value=""Submit Order (You can review it before it's final!)"">"));

        //Panel1.Controls.Add(new LiteralControl("</form>"));
        //Panel1.Controls.Add(new LiteralControl("</div>"));
    }

    protected void ListView1_SelectedIndexChanged(object sender, EventArgs e)
    {

    }
}