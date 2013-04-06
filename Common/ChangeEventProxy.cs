using System;

public delegate void StateChangeEvent(OrderState state, Order order);

public class ChangeEventProxy : MarshalByRefObject
{
    public event StateChangeEvent StateChangeNotifier;

    public void BindEventNotifier(OrderState state, Order order)
    {
        if (StateChangeNotifier != null)
            StateChangeNotifier(state, order);
    }

    public override object InitializeLifetimeService()
    {
        return null;
    }
}