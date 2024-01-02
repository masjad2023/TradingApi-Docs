using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TradingPOC.EmailService.Model.Settings
{
    public class BusSettings
    {
        public string ConnectionString { get; set; }
        public string QueueName { get; set; }
        public ushort PrefetchCount { get; set; }

        public string UserName { get; set; }
        public string Password { get; set; }
        public string Host { get; set; }
        public int Port { get; set; }
        public string VirtualHost { get; set; }
    }
}
