using System;
using System.Collections.Generic;

public enum OrderState
{
    WAITING,
    PREPARING,
    WAITING_DELIVERY,
    DELIVERING,
    CONCLUDED
}

[Serializable]
public class Order
{
    private string _clientAddress = null;
    private string _clientCC = null;
    private string _clientName = null;
    private string _deliveryTeam = null;
    private long _orderId = -1;
    private OrderState _state = OrderState.WAITING;
    private List<Sushi> _sushiList = new List<Sushi>();

    public Order()
    {
    }

    public string ClientAddress
    {
        get { return _clientAddress; }
        set { _clientAddress = value; }
    }

    public string ClientCC
    {
        get { return _clientCC; }
        set { _clientCC = value; }
    }

    public string ClientName
    {
        get { return _clientName; }
        set { _clientName = value; }
    }

    public string DeliveryTeam
    {
        get { return _deliveryTeam; }
        set { _deliveryTeam = value; }
    }

    public long OrderID
    {
        get { return _orderId; }
        set { _orderId = value; }
    }

    public OrderState State
    {
        get { return _state; }
        set { _state = value; }
    }

    public List<Sushi> SushiList
    {
        get { return _sushiList; }
        set { _sushiList = value; }
    }
}