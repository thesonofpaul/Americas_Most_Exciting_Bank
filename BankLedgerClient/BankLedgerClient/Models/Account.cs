using System.Collections.Generic;

namespace BankLedgerClient.Models
{
    public class Account
    {
        public Account() {}

        public string AccountNumber { get; set; }
        public string Name { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public decimal Balance { get; set; }
        public bool LoggedIn { get; set; }

        
        public override string ToString()
        {
            return string.Format("Account Number: {0}\n" +
                "Name: {1}\nUsername: {2}\nPassword: {3}\n" +
                "Balance: {4}\nLogged in status: {5}",
                AccountNumber, Name, UserName, Password, Balance, LoggedIn);
        }
    }
}