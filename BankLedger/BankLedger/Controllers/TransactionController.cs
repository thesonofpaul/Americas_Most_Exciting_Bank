using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using BankLedger.Models;

namespace BankLedger.Controllers
{
    public class TransactionController : ApiController
    {
        private static List<Transaction> transactionList = new List<Transaction>();

        public IEnumerable<Transaction> Get()
        {
            return transactionList;
        }


        public Transaction Get(int id)
        {
            var transaction = transactionList.FirstOrDefault(c => c.Id == id);
            if (transaction == null)
            {
                throw new HttpResponseException(
                    System.Net.HttpStatusCode.NotFound);
            }
            return transaction;
        }


        public IHttpActionResult Post(Transaction transaction)
        {
            if (transaction == null)
            {
                return BadRequest("Argument Null");
            }
            var transactionExists = transactionList.Any(c => c.Id == transaction.Id);

            if (transactionExists)
            {
                return BadRequest("Exists");
            }

            transactionList.Add(transaction);
            return Ok();
        }

        public IHttpActionResult Put(Transaction transaction)
        {
            if (transaction == null)
            {
                return BadRequest("Argument Null");
            }
            var existing = transactionList.FirstOrDefault(c => c.Id == transaction.Id);

            if (existing == null)
            {
                return NotFound();
            }

            existing.Id = transaction.Id;
            return Ok();
        }


        public IHttpActionResult Delete(int id)
        {
            var transaction = transactionList.FirstOrDefault(c => c.Id == id);
            if (transaction == null)
            {
                return NotFound();
            }
            transactionList.Remove(transaction);
            return Ok();
        }
    }
}
