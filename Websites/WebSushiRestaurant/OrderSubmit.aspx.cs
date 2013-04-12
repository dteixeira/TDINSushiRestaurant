using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class OrderSubmit : System.Web.UI.Page
{
    private static readonly string _configFile = @"config/Menu.xml";
    private List<Sushi> itemList = new List<Sushi>();

    protected void Cancel_Click(object sender, EventArgs e)
    {
        Server.Transfer("Default.aspx");
    }

    protected void Confirm_Click(object sender, EventArgs e)
    {
        Order order = (Order)Session["Order"];
        Global.OrderList.AddOrder(order);
        Server.Transfer("OrderConsult.aspx?name=" + order.ClientName);
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        // Parse form values
        itemList = Global.LoadMenu(Server.MapPath(_configFile));
        string cc_number = Request.Form["cc"];
        string name = Request.Form["name"];
        string address = Request.Form["address"];
        var values =
            from key in Request.Form.AllKeys
            where !key.Equals("name") && !key.Equals("address") && !key.Equals("cc")
            from sushi in itemList
            where sushi.Type.Equals(key) && int.Parse(Request.Form[key]) != 0
            select new
            {
                Type = sushi.Type,
                Price = sushi.Price,
                Quantity = int.Parse(Request.Form[key]),
                Total = int.Parse(Request.Form[key]) * sushi.Price
            };
        double total = values.Sum(s => s.Total);

        if (total == 0)
        {
            GoodOrder.Visible = false;
            return;
        }
        else
        {
            BadOrder.Visible = false;
        }

        // Add client information
        Label1.Text = name;
        Label2.Text = address;
        Label3.Text = cc_number;
        Label4.Text = String.Format("{0:0.00}", total.ToString());

        // Create and add order information
        Order order = new Order();
        order.ClientAddress = address;
        order.ClientCC = cc_number;
        order.ClientName = name;
        foreach (var sushi in values)
        {
            Table.Controls.Add(new LiteralControl(@"<tr>"));
            Table.Controls.Add(new LiteralControl("<td>" + sushi.Type + "</td>"));
            Table.Controls.Add(new LiteralControl("<td>" + String.Format("{0:0.00}", sushi.Price) + "</td>"));
            Table.Controls.Add(new LiteralControl("<td>" + String.Format("{0:0.00}", sushi.Quantity) + "</td>"));
            Table.Controls.Add(new LiteralControl("<td>" + String.Format("{0:0.00}", sushi.Total) + "</td>"));
            Table.Controls.Add(new LiteralControl(@"</tr>"));
            Sushi s = new Sushi(sushi.Type, sushi.Price);
            s.Quantity = sushi.Quantity;
            order.SushiList.Add(s);
        }
        Session["Order"] = order;
    }
}