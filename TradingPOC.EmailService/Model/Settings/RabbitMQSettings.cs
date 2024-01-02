using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TradingPOC.EmailService.Model.Settings
{
    public class RabbitMQSettings
    {
        public string ConnectionString { get; set; }
        public BusSettings BusSettings { get; set; }
    }
}
