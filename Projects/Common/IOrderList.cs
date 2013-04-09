using System.Collections.Generic;

public interface IOrderList
{
    event StateChangeEvent StateChangeNotifier;

    void ChangeOrderState(long id, OrderState state);

    void ChangeOrderState(long id, OrderState state, string team);

    List<Order> GetOrderList();

    string RegisterDeliveryTeam();

    void UnregisterDeliveryTeam(string team);
}