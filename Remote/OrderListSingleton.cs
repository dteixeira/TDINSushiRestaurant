using System;
using System.Collections.Generic;

public class OrderListSingleton : MarshalByRefObject, IOrderList
{
    private long _deliveryTeamId;
    private long _orderId;
    private List<Order> _orderList;
    private StateKeeper _stateKeeper;

    public OrderListSingleton()
    {
        StateKeeper.ConfigureStateKeeper();
        _stateKeeper = new StateKeeper();
        _orderId = 1;
        _orderList = new List<Order>();
        _deliveryTeamId = 1;
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
    }

    public void SaveState()
    {
        _stateKeeper.SavedId = _orderId;
        _stateKeeper.SavedList = _orderList;
        _stateKeeper.SaveState();
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