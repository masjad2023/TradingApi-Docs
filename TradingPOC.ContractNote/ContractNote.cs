using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using MailKit.Net.Smtp;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Host;
using Microsoft.Extensions.Logging;
using MimeKit;
using TradingPOC.ContractNote.Entities;
using TradingPOC.ContractNote.Models;

namespace TradingPOC.ContractNote
{
    public class ContractNote
    {
        [FunctionName("Function1")]
        public void Run([TimerTrigger("0 0 17 * */5 *")]TimerInfo myTimer, ILogger log)
        {
            log.LogInformation($"C# Timer trigger function executed at: {DateTime.Now}");

            TradingdatabaseContext context = new TradingdatabaseContext();
            
            var today = DateTime.Today;
            var tradeData = context.Trades.Where(t => t.TradeTimeStamp >= today).ToList();
            var userIds = tradeData.GroupBy(x => x.UserId).Select(t => t.Key).ToList();

            foreach (var userId in userIds)
            {
                var userTrades = tradeData.Where(x => x.UserId == userId).ToList();
                var user = context.Users.Where(x => x.Id == userId).FirstOrDefault();
                decimal totalBuyAmount = 0, totalSellAmount = 0;
                TradeInfo tradeInfo = GetTradeInformation(userTrades, user, ref totalBuyAmount, ref totalSellAmount);

                string htmlBody = GetEmailTemplate(totalBuyAmount, totalSellAmount, tradeInfo);
                SendEmail(tradeInfo, htmlBody, log);

            }
        }

        private static void SendEmail(TradeInfo tradeInfo, string htmlBody, ILogger log)
        {
            var email = new MimeMessage();
            email.From.Add(new MailboxAddress("Vlink Email", "kartikeyakhosla07@gmail.com"));


            email.To.Add(new MailboxAddress("Vlink Receiver", "kartikey.khosla@vlinkinfo.com"));
            email.To.Add(new MailboxAddress("Vlink Receiver", "sanjay.vaghela@vlinkinfo.com"));
            email.To.Add(new MailboxAddress("Vlink Receiver", tradeInfo.UserEmail));

            email.Subject = string.Format("VLink Trading - Contract Note {0}", tradeInfo.Date);
            email.Body = new TextPart(MimeKit.Text.TextFormat.Html)
            {
                Text = htmlBody
            };

            try
            {
                using (var smtp = new SmtpClient())
                {
                    smtp.Connect("smtp.gmail.com", 587, false);

                    // Note: only needed if the SMTP server requires authentication
                    smtp.Authenticate("kartikeyakhosla07@gmail.com", "ondx bmly zyuc nhqa");

                    smtp.Send(email);
                    smtp.Disconnect(true);
                    log.LogInformation(string.Format("Email has been sent to user {0}", tradeInfo.UserEmail));
                }
            }
            catch (Exception ex)
            {
                log.LogError(ex.Message.ToString());
            }
        }

        private static string GetEmailTemplate(decimal totalBuyAmount, decimal totalSellAmount, TradeInfo tradeInfo)
        {
            string emailTemplate = Path.Combine(Directory.GetParent(Directory.GetCurrentDirectory()).Parent.Parent.FullName, @"EmailTemplate\ContractNote.html");

            string htmlBody = string.Empty;
            using (StreamReader reader = new StreamReader(emailTemplate))
            {
                htmlBody = reader.ReadToEnd();
            }

            htmlBody = htmlBody.Replace("##UserName##", tradeInfo.UserName);
            htmlBody = htmlBody.Replace("##TradeDate##", tradeInfo.Date);
            StringBuilder tradeBody = new StringBuilder();
            foreach (var item in tradeInfo.Details)
            {
                tradeBody.Append(@"<tr>
                        <td>##OrderId##</td>
                        <td>##TradeTimeStamp##</td>
                        <td class='script'>##Script##</td>
                        <td>##TradeType##</td>
                        <td>##Quantity##</td>
                        <td>##Price##</td>
                        <td class='loss'>##BuyAmount##</td>
                        <td class='gain'>##SellAmount##</td>
                      </tr>");

                tradeBody.Replace("##OrderId##", item.OrderId.ToString());
                tradeBody.Replace("##TradeTimeStamp##", item.TradeTimeStamp.ToString());
                tradeBody.Replace("##Script##", item.Script.ToString());
                tradeBody.Replace("##TradeType##", item.TradeType.ToString() == "B" ? "Buy" : "Sell");
                tradeBody.Replace("##Quantity##", item.Quantity.ToString());
                tradeBody.Replace("##Price##", string.Format("{0}{1}", "&#8377;", item.Price.ToString()));
                tradeBody.Replace("##BuyAmount##", item.TradeType.ToString() == "B" ? string.Format("{0}{1}", "&#8377;", item.BuyAmount.ToString()) : string.Empty);
                tradeBody.Replace("##SellAmount##", item.TradeType.ToString() == "S" ? string.Format("{0}{1}", "&#8377;", item.SellAmount.ToString()) : string.Empty);
            }
            htmlBody = htmlBody.Replace("##TradeBody##", tradeBody.ToString());

            htmlBody = htmlBody.Replace("##PayableText##", tradeInfo.isPayable ? "Net Payable" : "Net Receivable");
            htmlBody = htmlBody.Replace("##Amount##", tradeInfo.Amount.ToString());
            htmlBody = htmlBody.Replace("##TotalBuyAmount##", totalBuyAmount > 0 ? string.Format("{0}{1}", "&#8377;", totalBuyAmount.ToString()) : string.Empty);
            htmlBody = htmlBody.Replace("##TotalSellAmount##", totalSellAmount > 0 ? string.Format("{0}{1}", "&#8377;", totalSellAmount.ToString()) : string.Empty);
            htmlBody = htmlBody.Replace("##PayableClassName##", tradeInfo.isPayable ? "loss" : "gain");
            return htmlBody;
        }

        private static TradeInfo GetTradeInformation(List<Trade> userTrades, User user, ref decimal totalBuyAmount, ref decimal totalSellAmount)
        {
            TradeInfo tradeInfo = new TradeInfo();
            tradeInfo.UserEmail = user?.EmailId;
            tradeInfo.UserName = string.Format("{0} {1}", user?.FirstName, user?.LastName);
            tradeInfo.Date = DateTime.Today.ToShortDateString();

            tradeInfo.Details = new List<TradeDetail> { };

            foreach (var userTrade in userTrades)
            {
                tradeInfo.Details.Add(new TradeDetail
                {
                    OrderId = userTrade.OrderId,
                    TradeTimeStamp = userTrade.TradeTimeStamp,
                    Script = userTrade.Symbol,
                    TradeType = userTrade.TradeType,
                    Quantity = userTrade.TradeQuantity,
                    Price = userTrade.TradePrice,
                    BuyAmount = userTrade.TradeType == "B" ? (userTrade.TradeQuantity * userTrade.TradePrice) : 0,
                    SellAmount = userTrade.TradeType == "S" ? (userTrade.TradeQuantity * userTrade.TradePrice) : 0,
                });
                if (userTrade.TradeType == "B")
                {
                    totalBuyAmount += userTrade.TradeQuantity * userTrade.TradePrice;
                }
                if (userTrade.TradeType == "S")
                {
                    totalSellAmount += userTrade.TradeQuantity * userTrade.TradePrice;
                }
            }
            if (totalSellAmount > totalBuyAmount)
            {
                tradeInfo.Amount = totalSellAmount - totalBuyAmount;
                tradeInfo.isPayable = false;
            }
            else if (totalBuyAmount > totalSellAmount)
            {
                tradeInfo.Amount = totalBuyAmount - totalSellAmount;
                tradeInfo.isPayable = true;
            }

            return tradeInfo;
        }
    }
}
