using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

// Add Usings:
using System.Net.Http;
using BankLedgerClient.Models;

namespace BankLedgerClient.Clients
{
    public class TransactionClient
    {
        string _hostUri;
        public TransactionClient(string hostUri)
        {
            _hostUri = hostUri;
        }


        public HttpClient CreateAccount()
        {
            var client = new HttpClient();
            client.BaseAddress = new Uri(new Uri(_hostUri), "api/transaction/");
            return client;
        }


        public IEnumerable<Transaction> GetTransactions()
        {
            HttpResponseMessage response;
            using (var client = CreateAccount())
            {
                response = client.GetAsync(client.BaseAddress).Result;
            }
            var result = response.Content.ReadAsAsync<IEnumerable<Transaction>>().Result;
            return result;
        }


        public Transaction GetTransaction(int id)
        {
            HttpResponseMessage response;
            using (var client = CreateAccount())
            {
                response = client.GetAsync(
                    new Uri(client.BaseAddress, id.ToString())).Result;
            }
            var result = response.Content.ReadAsAsync<Transaction>().Result;
            return result;
        }


        public System.Net.HttpStatusCode AddTransaction(Transaction transaction)
        {
            HttpResponseMessage response;
            using (var client = CreateAccount())
            {
                response = client.PostAsJsonAsync(client.BaseAddress, transaction).Result;
            }
            return response.StatusCode;
        }
    }
}