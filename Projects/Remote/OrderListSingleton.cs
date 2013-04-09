using System;
using System.Collections.Generic;

public class OrderListSingleton : MarshalByRefObject, IOrderList
{
    private int _deliveryTeamId;
    private long _orderId;
    private List<Order> _orderList;
    private StateKeeper _stateKeeper;
    private List<string> _teamList;

    public OrderListSingleton()
    {
        StateKeeper.ConfigureStateKeeper();
        _stateKeeper = new StateKeeper();
        _orderId = 1;
        _orderList = new List<Order>();
        _deliveryTeamId = 1;
        _teamList = new List<string>();

        // TODO REMOVE
        Order order = new Order();
        order.ClientAddress = "Something Street, 333, Somewhere";
        order.ClientCC = "123456123456";
        order.ClientName = "John Doe";
        order.State = OrderState.WAITING;
        order.OrderID = 1;
        Sushi sushi = new Sushi("Sashimi", 5.0);
        sushi.Quantity = 5;
        order.SushiList.Add(sushi);
        sushi = new Sushi("Fugu", 25.0);
        sushi.Quantity = 1;
        order.SushiList.Add(sushi);
        _orderList.Add(order);
    }

    public event StateChangeEvent StateChangeNotifier;

    public void ChangeOrderState(long id, OrderState state)
    {
        foreach (Order order in _orderList)
        {
            if (order.OrderID == id)
            {
                order.State = state;
                NotifyClients(state, order);
                break;
            }
        }
    }

    public void ChangeOrderState(long id, OrderState state, string team)
    {
        foreach (Order order in _orderList)
        {
            if (order.OrderID == id)
            {
                order.State = state;
                order.DeliveryTeam = team;
                NotifyClients(state, order);
                break;
            }
        }
    }

    public List<Order> GetOrderList()
    {
        return _orderList;
    }

    public override object InitializeLifetimeService()
    {
        return null;
    }

    public void LoadState()
    {
        _stateKeeper.LoadState();
        _orderId = _stateKeeper.SavedId;
        _orderList = _stateKeeper.SavedList;
        _deliveryTeamId = _stateKeeper.SavedDeliveryTeamId;
    }

    public string RegisterDeliveryTeam()
    {
        foreach (Order order in _orderList)
        {
            if (order.State == OrderState.DELIVERING && _teamList.IndexOf(order.DeliveryTeam) == -1)
            {
                _teamList.Add(order.DeliveryTeam);
                return order.DeliveryTeam;
            }
        }
        string team = "#Team" + _deliveryTeamId;
        ++_deliveryTeamId;
        return team;
    }

    public void SaveState()
    {
        _stateKeeper.SavedId = _orderId;
        _stateKeeper.SavedList = _orderList;
        _stateKeeper.SavedDeliveryTeamId = _deliveryTeamId;
        _stateKeeper.SaveState();
    }

    public void UnregisterDeliveryTeam(string team)
    {
        _teamList.Remove(team);
    }

    private void NotifyClients(OrderState state, Order order)
    {
        if (StateChangeNotifier != null)
        {
            Delegate[] invkList = StateChangeNotifier.GetInvocationList();
            foreach (StateChangeEvent handler in invkList)
            {
                try
                {
                    IAsyncResult ar = handler.BeginInvoke(state, order, null, null);
                }
                catch (Exception)
                {
                    StateChangeNotifier -= handler;
                }
            }
        }
    }
}