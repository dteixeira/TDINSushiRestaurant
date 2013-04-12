using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class html_form_action : System.Web.UI.Page
{
    private double _totalPrice = 0.0;
    private double[] partialPrices;
    private Order order = new Order();
    private List<Sushi> orderItemsList = new List<Sushi>();
    private static readonly string _configFile = @"config/Menu.xml";
    private List<Sushi> itemList = new List<Sushi>();
    System.Text.StringBuilder quantityValues = new System.Text.StringBuilder();

    protected void Page_Load(object sender, EventArgs e)
    {
        String cc_number = "";
        String name = "";
        String address = "";
        System.Collections.Specialized.NameValueCollection postedValues = Request.Form;
        String nextKey;
        for (int i = 0; i < postedValues.AllKeys.Length; i++)
        {
            nextKey = postedValues.AllKeys[i];
            if (nextKey.Substring(0, 2) != "__")
            {
                if (nextKey.Equals("cc"))
                {
                    cc_number = postedValues[i];
                }
                else if (nextKey.Equals("name"))
                {
                    name = postedValues[i];
                }
                else if (nextKey.Equals("address"))
                {
                    address = postedValues[i];
                } 
                else
                {
                    quantityValues.Append(nextKey);
                    quantityValues.Append('-');
                    quantityValues.Append(postedValues[i]);
                    quantityValues.Append(';');
                }
                
                //displayValues.Append("<br>");
                //displayValues.Append(nextKey);
                //displayValues.Append(" = ");
                //displayValues.Append(postedValues[i]);
            }
        }


        itemList = Global.LoadMenu(Server.MapPath(_configFile));
        String[] orderItems = quantityValues.ToString().Split(';');
        partialPrices = new double[orderItems.Length];

        var sushis =
            from s in orderItems
            let spl = s.Split('-')
            from sushi in itemList
            where sushi.Type.Equals(spl[0])
            select new { Type = sushi.Type, Price = sushi.Price, Quantity = int.Parse(spl[1]) };

        int j = 0;
        foreach (var sushi in sushis)
        {
            partialPrices[j] = sushi.Quantity * sushi.Price;
            j++;
        }

        foreach (double d in partialPrices)
        {
            _totalPrice += d;
        }

        Label1.Text = name;
        Label2.Text = address;
        Label3.Text = cc_number;
        Label4.Text = String.Format("{0:0.00}", _totalPrice.ToString());

        for(int k = 0; k < partialPrices.Length - 1; k++){
            PlaceHolder2.Controls.Add(new LiteralControl(@"<tr>"));
            PlaceHolder2.Controls.Add(new LiteralControl("<td>" + sushis.ElementAt(k).Type + "</td>"));
            PlaceHolder2.Controls.Add(new LiteralControl("<td>" + String.Format("{0:0.00}", sushis.ElementAt(k).Price) + "</td>"));
            PlaceHolder2.Controls.Add(new LiteralControl("<td>" + String.Format("{0:0.00}", sushis.ElementAt(k).Quantity) + "</td>"));
            PlaceHolder2.Controls.Add(new LiteralControl("<td>" + String.Format("{0:0.00}", partialPrices[k]) + "</td>"));
            PlaceHolder2.Controls.Add(new LiteralControl(@"</tr>"));    
        }

        
        for (int z = 0; z < sushis.Count(); z++)
        {
            Sushi s = new Sushi(sushis.ElementAt(z).Type, sushis.ElementAt(z).Price, sushis.ElementAt(z).Quantity);
            orderItemsList.Add(s);
        }

        order.SushiList = orderItemsList;
        order.ClientName = name;
        order.ClientAddress = address;
        order.ClientCC = cc_number;
       
    }

    protected void Button1_Click(object sender, EventArgs e)
    {

    }
}