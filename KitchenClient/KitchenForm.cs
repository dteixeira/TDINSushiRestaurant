using System;
using System.Collections.Generic;
using System.Runtime.Remoting;
using System.Windows.Forms;

namespace DeliveryClient
{
    public partial class KitchenForm : Form
    {
        private Order _changedOrder = null;
        private OrderState _changedState;
        private ChangeEventProxy _eventProxy = null;
        private IOrderList _list = null;

        public KitchenForm()
        {
            InitializeComponent();
            ConnectRemote();
            PopulateLists();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (listView1.SelectedItems.Count > 0)
            {
                ListViewItem item = listView1.SelectedItems[0];
                Order order = FindById(Convert.ToInt64(item.SubItems[0].Text));
                if (order != null)
                {
                    _list.ChangeOrderState(order.OrderID, OrderState.PREPARING);
                }
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (listView2.SelectedItems.Count > 0)
            {
                ListViewItem item = listView2.SelectedItems[0];
                Order order = FindById(Convert.ToInt64(item.SubItems[0].Text));
                if (order != null)
                {
                    _list.ChangeOrderState(order.OrderID, OrderState.WAITING_DELIVERY);
                }
            }
        }

        private void ConnectRemote()
        {
            // Get client configuration
            RemotingConfiguration.Configure("KitchenClient.exe.config", false);
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
            _eventProxy = new ChangeEventProxy();
            _eventProxy.StateChangeNotifier += new StateChangeEvent(RemoteHandle);
            _list.StateChangeNotifier += new StateChangeEvent(_eventProxy.BindEventNotifier);
        }

        private Order FindById(long id)
        {
            Order order = null;
            foreach (Order o in _list.GetOrderList())
            {
                if (o.OrderID == id)
                {
                    order = o;
                    break;
                }
            }
            return order;
        }

        private void HandleOthers()
        {
            if (InvokeRequired)
            {
                Invoke(new MethodInvoker(HandleOthers));
            }
            else
            {
                foreach (ListViewItem it in listView1.Items)
                {
                    if (Convert.ToInt64(it.SubItems[0].Text) == _changedOrder.OrderID)
                    {
                        listView1.Items.Remove(it);
                        listView3.Items.Clear();
                        break;
                    }
                }
                foreach (ListViewItem it in listView2.Items)
                {
                    if (Convert.ToInt64(it.SubItems[0].Text) == _changedOrder.OrderID)
                    {
                        listView2.Items.Remove(it);
                        listView4.Items.Clear();
                        break;
                    }
                }
            }
        }

        private void HandlePreparing()
        {
            if (InvokeRequired)
            {
                Invoke(new MethodInvoker(HandlePreparing));
            }
            else
            {
                foreach (ListViewItem it in listView1.Items)
                {
                    if (Convert.ToInt64(it.SubItems[0].Text) == _changedOrder.OrderID)
                    {
                        listView1.Items.Remove(it);
                        listView3.Items.Clear();
                        break;
                    }
                }
                if (_changedState == OrderState.PREPARING)
                {
                    ListViewItem item = new ListViewItem(new string[] { _changedOrder.OrderID.ToString(), _changedOrder.ClientName, _changedOrder.SushiList.Count.ToString() });
                    listView2.Items.Add(item);
                }
            }
        }

        private void HandleWaiting()
        {
            if (InvokeRequired)
            {
                Invoke(new MethodInvoker(HandleWaiting));
            }
            else
            {
                ListViewItem item = new ListViewItem(new string[] { _changedOrder.OrderID.ToString(), _changedOrder.ClientName, _changedOrder.SushiList.Count.ToString() });
                listView1.Items.Add(item);
            }
        }

        private void listView1_SelectedIndexChanged(object sender, EventArgs e)
        {
            ListView list = sender as ListView;
            if (list.SelectedItems.Count > 0)
            {
                ListViewItem item = list.SelectedItems[0];
                Order order = FindById(Convert.ToInt64(item.SubItems[0].Text));
                if (order != null)
                {
                    listView3.Items.Clear();
                    foreach (Sushi sushi in order.SushiList)
                    {
                        listView3.Items.Add(new ListViewItem(new string[] { sushi.Type, sushi.Quantity.ToString() }));
                    }
                }
            }
        }

        private void listView2_SelectedIndexChanged(object sender, EventArgs e)
        {
            ListView list = sender as ListView;
            if (list.SelectedItems.Count > 0)
            {
                ListViewItem item = list.SelectedItems[0];
                Order order = FindById(Convert.ToInt64(item.SubItems[0].Text));
                if (order != null)
                {
                    listView4.Items.Clear();
                    foreach (Sushi sushi in order.SushiList)
                    {
                        listView4.Items.Add(new ListViewItem(new string[] { sushi.Type, sushi.Quantity.ToString() }));
                    }
                }
            }
        }

        private void PopulateLists()
        {
            List<Order> list = _list.GetOrderList();
            foreach (Order order in list)
            {
                if (order.State == OrderState.WAITING)
                {
                    ListViewItem item = new ListViewItem(new string[] { order.OrderID.ToString(), order.ClientName, order.SushiList.Count.ToString() });
                    listView1.Items.Add(item);
                }
                else if (order.State == OrderState.PREPARING)
                {
                    ListViewItem item = new ListViewItem(new string[] { order.OrderID.ToString(), order.ClientName, order.SushiList.Count.ToString() });
                    listView2.Items.Add(item);
                }
            }
        }

        private void RemoteHandle(OrderState state, Order order)
        {
            _changedState = state;
            _changedOrder = order;
            switch (state)
            {
                case OrderState.WAITING:
                    HandleWaiting();
                    break;

                case OrderState.PREPARING:
                    HandlePreparing();
                    break;

                default:
                    HandleOthers();
                    break;
            }
        }
    }
}