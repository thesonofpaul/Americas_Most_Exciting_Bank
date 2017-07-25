using System;

namespace BankLedgerClient.Models
{
    public class Transaction
    {
        public Transaction() {}
        public int Id { get; set; }
        public DateTime Date { get; set; }
        public string AccountNumber { get; set; }
        public string TransactionType { get; set; }
        public decimal Amount { get; set; }
    }
}