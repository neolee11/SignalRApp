using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebFormHost.Hubs
{
    public static class ConnectionStatus
    {
        //public static ConcurrentDictionary<string, int> 
        public static HashSet<string>  ConnectedIds = new HashSet<string>();
    }
}