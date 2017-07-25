using System.Collections.Generic;

namespace BankLedger.Models
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
    }
}