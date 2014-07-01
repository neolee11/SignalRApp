﻿using System;
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
    [HubName("AccessManager")]
    public class AccessManagerHub : Hub
    {
        public void BroadCastMessage(string message)
        {
            Clients.Caller.getMessage(message); //same as below
            Clients.Client(Context.ConnectionId).getMessage(message); //Context.ConnectionId is the caller client's connection id

            //Clients.Others.getMessage(message) //all other clients besides me...same as below
            //Clients.AllExcept(Context.ConnectionId).getMessage(message)

            Clients.All.getMessage(message);
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

    }

    public class CustomData
    {
        public int Id { get; set; }
        public string Message { get; set; }

    }
}