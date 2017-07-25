using System;

namespace BankLedgerMVC.Models
{
    public class Transaction
    {
        public Transaction() {}
        public Transaction(string AccountNumber, string TransactionType, decimal Amount) {
            this.Date = DateTime.Now;
            this.AccountNumber = AccountNumber;
            this.TransactionType = TransactionType;
            this.Amount = Amount;
        }
        public int Id { get; set; }
        public DateTime Date { get; set; }
        public string AccountNumber { get; set; }
        public string TransactionType { get; set; }
        public decimal Amount { get; set; }
    }
}