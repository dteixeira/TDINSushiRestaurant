using System;
using System.Web.UI;

public partial class _Default : System.Web.UI.Page
{
    private static readonly string _configFile = @"config/Menu.xml";

    protected void ListView1_SelectedIndexChanged(object sender, EventArgs e)
    {
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        form1.Action = "OrderSubmit.aspx";

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
    }
}