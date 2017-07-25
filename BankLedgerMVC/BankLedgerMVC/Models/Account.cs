using System.Collections.Generic;

namespace BankLedgerMVC.Models
{
    public class Account
    {
        public Account(Account account) {
            this.AccountNumber = account.AccountNumber;
            this.Name = account.Name;
            this.UserName = account.UserName;
            this.Password = account.Password;
            this.Balance = account.Balance;
        }
        public Account(string name, string username, string password, string accountNumber) {
            this.AccountNumber = accountNumber;
            this.Name = name;
            this.UserName = username;
            this.Password = password;
            this.Balance = 0;
        }
        public Account() {}
        public string AccountNumber { get; set; }
        public string Name { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public decimal Balance { get; set; }
        public bool LoggedIn { get; set; }
    }
}