using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Host;
using Microsoft.Extensions.Logging;
using TradingPOC.ContractNote.Entities;

namespace TradingPOC.ContractNote
{
    public class Function1
    {
        private readonly TradingdatabaseContext _dbContext;

        public Function1()
        {
            _dbContext = new TradingdatabaseContext();
        }
        [FunctionName("Function1")]
        public void Run([TimerTrigger("0 */1 * * * *")]TimerInfo myTimer, ILogger log)
        {
            List<int> users = _dbContext.Users.Select(x => x.Id).ToList();

            foreach (var userId in users)
            {
                _dbContext.ContractNoteLogs.Add(new ContractNoteLog()
                {
                    UserId = userId,
                    LogDatetime = System.DateTime.Now
                });
                _dbContext.SaveChanges();
            }
            log.LogInformation($"C# Timer trigger function executed at: {DateTime.Now}");
        }
    }
}
