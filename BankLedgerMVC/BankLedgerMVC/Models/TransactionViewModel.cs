using System;
using System.Collections.Generic;

namespace BankLedgerMVC.Models
{
    public class TransactionViewModel
    {
        public Account TransactionAccount { get; set; }
        public List<Transaction> Transactions { get; set; }
        public TransactionViewModel(Account account, List<Transaction> transactions) {
            this.TransactionAccount = account;
            this.Transactions = transactions;
        }
    }
}