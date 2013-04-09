using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Runtime.Remoting;

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
}