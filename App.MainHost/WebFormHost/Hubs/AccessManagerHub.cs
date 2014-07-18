using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Web;
using Microsoft.AspNet.SignalR;
using Microsoft.AspNet.SignalR.Hubs;
using WebGrease;

namespace WebFormHost.Hubs
{
    [HubName("accessManager")]
    public class AccessManagerHub : Hub
    {
        public void BroadCastMessage(string message)
        {
            //Clients.Caller.getMessage(message); //same as below
            //Clients.Client(Context.ConnectionId).getMessage(message); //Context.ConnectionId is the caller client's connection id

            //Clients.Others.getMessage(message) //all other clients besides me...same as below
            //Clients.AllExcept(Context.ConnectionId).getMessage(message)

            Clients.All.getMessage("ClientID " + Context.ConnectionId.ToString() + ": " + message);
        }

        public void JoinGroup(string groupName, int b)
        {
            //Notes: Groups are not persisted
            Groups.Add(Context.ConnectionId, groupName);
        }

        public void SendMessageToGroup(string groupName, string message)
        {
            var msg = string.Format("{0}: {1}", Context.ConnectionId, message);
            Clients.Group(groupName).getMessage(msg);
        }

        public void SendCustomData(CustomData data)
        {
            Clients.All.processCustomData();
        }

        public Task<int> SendDataAsync()
        {
            //do ... async work
            return null;
        }

        public bool IsCaseCheckedOut(int caseId)
        {
            Clients.All.HaveYouCheckedOutCase(caseId);

            return false;
        }


        public void IsAnyoneAccessingPage()
        {
            bool alreadyAccessed = false; //ConnectionStatus.ConnectedIds.Any();

            foreach (var connectionId in ConnectionStatus.ConnectedIds)
            {
                if (connectionId != Context.ConnectionId)
                {
                    alreadyAccessed = true;
                    break;
                }
            }

            Clients.Caller.updateAccessPageStatus(alreadyAccessed);
        }


        public override Task OnConnected()
        {
            if (ConnectionStatus.ConnectedIds.Any() == false)
                ConnectionStatus.ConnectedIds.Add(Context.ConnectionId);
            return base.OnConnected();
        }

        public override Task OnDisconnected()
        {
            ConnectionStatus.ConnectedIds.Remove(Context.ConnectionId);
            return base.OnDisconnected();
        }

        public override Task OnReconnected()
        {
            return base.OnReconnected();
        }

    }

    public class CustomData
    {
        public int Id { get; set; }
        public string Message { get; set; }

        /// <summary>
        /// This is how you can send data from outside a hub - retrieve hub context via dependency resolver
        /// </summary>
        private void Send()
        {
            var context = GlobalHost.ConnectionManager.GetHubContext<AccessManagerHub>();
            context.Clients.All.getMessage("A message");
        }
    }
}