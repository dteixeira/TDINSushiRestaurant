using System;
using System.Linq;
using System.Web.UI.WebControls;

public partial class OrderConsult : System.Web.UI.Page
{
    protected void New_Order(object sender, EventArgs e)
    {
        Server.Transfer("Default.aspx");
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        string name = Request.QueryString["name"];
        Table.Visible = false;
        if (name != null)
        {
            var orders =
                from order in Global.OrderList.GetOrderList()
                where order.ClientName.Equals(name)
                let price = order.SushiList.Sum(s => s.Price * s.Quantity)
                select new { Id = order.OrderID, State = order.State, Price = price };

            if (orders.Count() == 0)
            {
                Information.Text = "This client has no orders registered";
                Table.Visible = false;
            }
            else
            {
                Information.Text = "Orders of " + name;
                Table.Visible = true;
                foreach (var order in orders)
                {
                    TableRow row = new TableRow();
                    TableCell id = new TableCell();
                    id.Text = order.Id.ToString();
                    TableCell state = new TableCell();
                    state.Text = order.State.ToString();
                    TableCell price = new TableCell();
                    price.Text = String.Format("{0:0.00}", order.Price);
                    row.Controls.Add(id);
                    row.Controls.Add(price);
                    row.Controls.Add(state);
                    Table.Controls.Add(row);
                }
            }
        }
    }

    protected void Search_Orders(object sender, EventArgs e)
    {
    }
}