using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TradingPOC.EmailService.Model.Settings
{
    public class AppSettings
    {
        public RabbitMQSettings RabbitMQSettings { get; set; }
        //public DatabaseSettings DatabaseSettings { get; set; }
    }
}
