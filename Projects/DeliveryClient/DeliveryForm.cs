using System;
using System.Collections.Generic;
using System.Runtime.Remoting;
using System.Windows.Forms;

namespace DeliveryClient
{
    public partial class DeliveryForm : Form
    {
        private Order _changedOrder = null;
        private OrderState _changedState;
        private StateChangeEvent _event = null;
        private ChangeEventProxy _eventProxy = null;
        private IOrderList _list = null;
        private string _teamName;

        public DeliveryForm()
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
                    _list.ChangeOrderState(order.OrderID, OrderState.DELIVERING, _teamName);
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
                    _list.ChangeOrderState(order.OrderID, OrderState.CONCLUDED);
                }
            }
        }

        private void ConnectRemote()
        {
            // Get client configuration
            RemotingConfiguration.Configure("DeliveryClient.exe.config", false);
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
            _event = new StateChangeEvent(_eventProxy.BindEventNotifier);
            _list.StateChangeNotifier += _event;

            // Register delivery team and change window name
            _teamName = _list.RegisterDeliveryTeam();
            this.Text = "Delivery Client " + _teamName;
        }

        private void DeliveryForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            _list.StateChangeNotifier -= _event;
            _list.UnregisterDeliveryTeam(_teamName);
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

        private void HandleDelivering()
        {
            if (InvokeRequired)
            {
                Invoke(new MethodInvoker(HandleDelivering));
            }
            else
            {
                foreach (ListViewItem it in listView1.Items)
                {
                    if (Convert.ToInt64(it.SubItems[0].Text) == _changedOrder.OrderID)
                    {
                        listView1.Items.Remove(it);
                        break;
                    }
                }
                if (_changedOrder.DeliveryTeam.Equals(_teamName))
                {
                    ListViewItem item = new ListViewItem(new string[] { _changedOrder.OrderID.ToString(), _changedOrder.ClientName, _changedOrder.ClientAddress });
                    listView2.Items.Add(item);
                }
            }
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
                        break;
                    }
                }
                foreach (ListViewItem it in listView2.Items)
                {
                    if (Convert.ToInt64(it.SubItems[0].Text) == _changedOrder.OrderID)
                    {
                        listView2.Items.Remove(it);
                        break;
                    }
                }
            }
        }

        private void HandleWaitingDelivery()
        {
            if (InvokeRequired)
            {
                Invoke(new MethodInvoker(HandleWaitingDelivery));
            }
            else
            {
                ListViewItem item = new ListViewItem(new string[] { _changedOrder.OrderID.ToString(), _changedOrder.ClientName, _changedOrder.ClientAddress });
                listView1.Items.Add(item);
            }
        }

        private void PopulateLists()
        {
            List<Order> list = _list.GetOrderList();
            foreach (Order order in list)
            {
                if (order.State == OrderState.WAITING_DELIVERY)
                {
                    ListViewItem item = new ListViewItem(new string[] { order.OrderID.ToString(), order.ClientName, order.ClientAddress });
                    listView1.Items.Add(item);
                }
                else if (order.State == OrderState.DELIVERING)
                {
                    if (order.DeliveryTeam.Equals(_teamName))
                    {
                        ListViewItem item = new ListViewItem(new string[] { order.OrderID.ToString(), order.ClientName, order.ClientAddress });
                        listView2.Items.Add(item);
                    }
                }
            }
        }

        private void RemoteHandle(OrderState state, Order order)
        {
            _changedState = state;
            _changedOrder = order;
            switch (state)
            {
                case OrderState.WAITING_DELIVERY:
                    HandleWaitingDelivery();
                    break;

                case OrderState.DELIVERING:
                    HandleDelivering();
                    break;

                default:
                    HandleOthers();
                    break;
            }
        }
    }
}