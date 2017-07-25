using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;
using System.Net.Http;
using BankLedger.Models;

namespace BankLedger.Controllers
{
    public class AccountController : ApiController
    {
        private static List<Account> accountList = new List<Account>();

        public IEnumerable<Account> Get()
        {
            return accountList;
        }


        public Account Get(int id)
        {
            var account = accountList.FirstOrDefault(c => c.AccountNumber.Equals(id.ToString()));
            if (account == null)
            {
                throw new HttpResponseException(
                    System.Net.HttpStatusCode.NotFound);
            }
            return account;
        }

        public IHttpActionResult Post(Account account)
        {
            if (account == null)
            {
                return BadRequest("Argument Null");
            }
            var accountExists = accountList.Any(c => c.AccountNumber.Equals(account.AccountNumber));

            if (accountExists)
            {
                return BadRequest("Exists");
            }

            accountList.Add(account);
            return Ok();
        }

        public IHttpActionResult Put(Account account)
        {
            if (account == null)
            {
                return BadRequest("Argument Null");
            }
            var existing = accountList.FirstOrDefault(c => c.AccountNumber.Equals(account.AccountNumber));

            if (existing == null)
            {
                return NotFound();
            }

            existing.LoggedIn = account.LoggedIn;
            existing.Balance = account.Balance;
            return Ok();
        }


        public IHttpActionResult Delete(int id)
        {
            var account = accountList.FirstOrDefault(c => c.AccountNumber.Equals(id.ToString()));
            if (account == null)
            {
                return NotFound();
            }
            accountList.Remove(account);
            return Ok();
        }
    }
}
