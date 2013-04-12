using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Runtime.Remoting;
using System.Xml.Linq;

public class Global
{
    private static IOrderList _list = null;

    public static IOrderList OrderList
    {
        get { return _list; }
    }

    public static void ConfigureRemote()
    {
        if (_list == null)
        {
            WellKnownClientTypeEntry[] types = RemotingConfiguration.GetRegisteredWellKnownClientTypes();
            WellKnownClientTypeEntry remote = null;
            foreach (WellKnownClientTypeEntry entry in types)
            {
                if (entry.ObjectType == typeof(IOrderList))
                {
                    remote = entry;
                    break;
                }
            }

            // If the configuration was not correct throw an exception
            if (remote == null)
                throw new RemotingException("Type <IOrderList> not found.");

            // Connect to remote list object
            _list = (IOrderList)RemotingServices.Connect(typeof(IOrderList), remote.ObjectUrl);
        }
    }

    public static List<Sushi> LoadMenu(string path)
    {
        XDocument menu = XDocument.Load(path);
        List<Sushi> itemList = new List<Sushi>();
        foreach (XElement elem in menu.Descendants("item"))
        {
            string itemName = elem.Descendants("name").First().Value;
            double price = double.Parse(elem.Descendants("price").First().Value);
            Sushi newItem = new Sushi(itemName, price);
            itemList.Add(newItem);
        }
        return itemList;
    }
}