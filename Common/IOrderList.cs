using System.Collections.Generic;

public interface IOrderList
{
    event StateChangeEvent StateChangeNotifier;

    void ChangeOrderState(long id, OrderState state);
    List<Order> GetOrderList();
}