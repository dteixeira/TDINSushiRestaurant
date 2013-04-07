using System;
using System.IO;
using System.Linq;
using System.Runtime.Remoting;
using System.Xml.Linq;

public class OrderListServer
{
    private static readonly string _configFile = @"config/Config.xml";
    private static ChangeEventProxy _eventProxy = null;
    private static OrderListSingleton _list = null;
    private static TextWriter _logfile = null;
    private static string _logfileName;
    private static string _remoteUrl;

    private static void ConfigureDefaults()
    {
        XDocument doc = XDocument.Load(_configFile);
        XElement elem = doc.Descendants("orderlist").First();
        _logfileName = elem.Attribute("logfile").Value;
        _remoteUrl = elem.Attribute("url").Value;
    }

    private static void ConfigureLogfile()
    {
        if (!File.Exists(_logfileName))
        {
            File.Create(_logfileName).Close();
        }
        _logfile = File.AppendText(_logfileName);
    }

    private static void ConfigureRemoteObject()
    {
        _list = (OrderListSingleton)Activator.GetObject(typeof(OrderListSingleton), _remoteUrl);
        _eventProxy = new ChangeEventProxy();
        _eventProxy.StateChangeNotifier += new StateChangeEvent(RemoteHandle);
        _list.StateChangeNotifier += new StateChangeEvent(_eventProxy.BindEventNotifier);
    }

    private static void ConfigureServer()
    {
        RemotingConfiguration.Configure("OrderListServer.exe.config", false);
    }

    private static void FinalizeServer()
    {
        _logfile.Close();
        _list.SaveState();
    }

    private static void Main(string[] args)
    {
        // Configure server
        Console.Write(String.Format("{0,-30}", "Configuring server..."));
        ConfigureServer();
        Console.WriteLine(String.Format("{0,10}", "DONE"));

        // Configure defaults
        Console.Write(String.Format("{0,-30}", "Configuring defaults..."));
        ConfigureDefaults();
        Console.WriteLine(String.Format("{0,10}", "DONE"));

        // Configure remote object
        Console.Write(String.Format("{0,-30}", "Connecting to hosted object..."));
        ConfigureRemoteObject();
        if (!(args.Length == 1) || !args[0].Equals("--reset"))
        {
            _list.LoadState();
        }
        Console.WriteLine(String.Format("{0,10}\n", "DONE"));

        // Creating logfile
        ConfigureLogfile();

        // Loop until enter is pressed
        ConsoleKey key = ConsoleKey.A;
        Console.WriteLine("Server is running. Press <RETURN> to terminate and <L> to open log.");
        while ((key = Console.ReadKey().Key) != ConsoleKey.Enter)
        {
            Console.Clear();
            Console.WriteLine("Server is running. Press <RETURN> to terminate and <L> to open log.");
            _list.SaveState();
            if (key == ConsoleKey.L && File.Exists(_logfileName))
            {
                System.Diagnostics.Process.Start(_logfileName);
            }
        }

        // Finalizing server and saving current state
        FinalizeServer();
    }

    private static void RemoteHandle(OrderState state, Order order)
    {
        // Log payment
        if (state == OrderState.PREPARING)
        {
            string log = String.Format("{0,10:D10} {1,25} {2,30} {3,15}", order.OrderID, DateTime.Now, order.ClientName, order.ClientCC);
            _logfile.WriteLine(log);
            _logfile.Flush();
        }
    }
}