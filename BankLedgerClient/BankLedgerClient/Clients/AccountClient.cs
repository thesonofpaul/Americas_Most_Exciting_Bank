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
    public class AccountClient
    {
        string _hostUri;
        public AccountClient(string hostUri)
        {
            _hostUri = hostUri;
        }


        public HttpClient CreateAccount()
        {
            var client = new HttpClient();
            client.BaseAddress = new Uri(new Uri(_hostUri), "api/account/");
            return client;
        }


        public IEnumerable<Account> GetAccounts()
        {
            HttpResponseMessage response;
            using (var client = CreateAccount())
            {
                response = client.GetAsync(client.BaseAddress).Result;
            }
            var result = response.Content.ReadAsAsync<IEnumerable<Account>>().Result;
            return result;
        }


        public Account GetAccount(int id)
        {
            HttpResponseMessage response;
            using (var client = CreateAccount())
            {
                response = client.GetAsync(
                    new Uri(client.BaseAddress, id.ToString())).Result;
            }
            var result = response.Content.ReadAsAsync<Account>().Result;
            return result;
        }


        public System.Net.HttpStatusCode AddAccount(Account account)
        {
            HttpResponseMessage response;
            using (var client = CreateAccount())
            {
                response = client.PostAsJsonAsync(client.BaseAddress, account).Result;
            }
            return response.StatusCode;
        }


        public System.Net.HttpStatusCode UpdateAccount(Account account)
        {
            HttpResponseMessage response;
            using (var client = CreateAccount())
            {
                response = client.PutAsJsonAsync(client.BaseAddress, account).Result;
            }
            return response.StatusCode;
        }


        public System.Net.HttpStatusCode DeleteAccount(int id)
        {
            HttpResponseMessage response;
            using (var client = CreateAccount())
            {
                response = client.DeleteAsync(
                    new Uri(client.BaseAddress, id.ToString())).Result;
            }
            return response.StatusCode;
        }
    }
}