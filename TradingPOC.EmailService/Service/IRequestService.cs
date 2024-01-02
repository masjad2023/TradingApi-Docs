using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TradingPOC.EmailService.Model;

namespace TradingPOC.EmailService.Service
{
    public interface IRequestService
    {
        bool HasError { get; }
        string ErrorDescription { get; }

        /// <summary>
        /// Sends order notification to the customer email
        /// </summary>
        /// <param name="order"></param>
        void ProcessNotificationEmailRequest(RequestModel? request);
    }
}
