using System;
using System.Collections.Generic;
using System.Linq;
using BankLedgerMVC.Clients;
using System.Web.Mvc;
using BankLedgerMVC.Models;

namespace BankLedgerMVC.Controllers
{
    public class BankController : Controller
    {
        private static AccountClient accountClient = new AccountClient("http://localhost:8080");
        private static TransactionClient transactionClient = new TransactionClient("http://localhost:8080");

        public ActionResult Index()
        {
            if (IsLoggedIn()) {
                var account = getLoggedIn();
                return View(account);
            }
            return View();
        }

        public ActionResult VerifyLogin(string username, string password) {
            var accountList = accountClient.GetAccounts();

            if (IsLoggedIn())
            {
                @ViewBag.Message = 5;
            }

            var validLogIn = (from account in accountList
                              where account.UserName.Equals(username) && account.Password.Equals(password)
                              select account);
            if (!validLogIn.Any())
            {
                @ViewBag.Message = username+password;
                return View("Index");
            }
            else
            {
                Account newLogin = validLogIn.First();
                newLogin.LoggedIn = true;
                Console.WriteLine(newLogin.ToString());
                accountClient.UpdateAccount(newLogin);
                return View("Index", newLogin);
            }            
        }

        public ActionResult CreateAccount()
        {
            return View();
        }

        public ActionResult VerifyCreate(string accountNumber, string name, string username, string password) {
            var accountList = accountClient.GetAccounts();

            var x = (from account in accountList
                     where account.UserName.Equals(username) || account.AccountNumber.Equals(accountNumber)
                     select account.AccountNumber);

            if (x.Any())
            {
                @ViewBag.Message = 2;
                return View("CreateAccount");
            }

            Account newAccount = new Account
            {
                AccountNumber = accountNumber,
                Name = name,
                UserName = username,
                Password = password,
                Balance = 0
            };
            accountClient.AddAccount(newAccount);
            return View("Index", newAccount);
        }

        public ActionResult Logout() {
            var accountList = accountClient.GetAccounts();

            var isLoggedIn = (from account in accountList
                              where account.LoggedIn
                              select account);
            foreach (Account account in isLoggedIn)
            {
                account.LoggedIn = false;
                accountClient.UpdateAccount(account);
            }
            return View("Index");
        }

        public ActionResult Transaction()
        {
            if (!IsLoggedIn()) {
                return View("Index");
            } else {
                var account = getLoggedIn();
                return View(account);
            }
            
        }
        
        public ActionResult VerifyTransaction(string transactionAmount, string transactionType) {
            var accountList = accountClient.GetAccounts();

            if (!IsLoggedIn())
            {
                return View("Index");
            }

            var currLogIn = (from account in accountList where account.LoggedIn select account).First();

            try
            {
                decimal amount = Decimal.Parse(transactionAmount);
                
                if (transactionType.Equals("Withdraw"))
                {
                    if (currLogIn.Balance - amount < 0)
                    {
                        @ViewBag.Message = 6;
                        return View("Transaction", currLogIn);
                    }
                    currLogIn.Balance -= amount;
                }
                else if (transactionType.Equals("Deposit"))
                {
                    currLogIn.Balance += amount;
                }
                else
                {
                    @ViewBag.Message = "Error: Invalid transaction type. Please try again";
                    return View("Transaction", currLogIn);
                }
                accountClient.UpdateAccount(currLogIn);

                var transactionList = transactionClient.GetTransactions();
                transactionClient.AddTransaction(new Transaction
                {
                    Id = transactionList.Count(),
                    Date = DateTime.Now,
                    AccountNumber = currLogIn.AccountNumber,
                    TransactionType = transactionType,
                    Amount = amount
                });
                @ViewBag.Success = true;
                @ViewBag.Message = String.Format("Transaction successful!\n{0} amount of {1} from account {2}.\nNew balance is {3}",
                                                transactionType,
                                                amount,
                                                currLogIn.AccountNumber,
                                                currLogIn.Balance);
                return View("Transaction", currLogIn);
            } catch (FormatException) {
                @ViewBag.Message = "Error: Invalid amount. Please try again";
                return View("Transaction", currLogIn);
            }
        }

        public ActionResult CheckBalance()
        {
            var accountList = accountClient.GetAccounts();

            if (!IsLoggedIn())
            {
                return View("Index");
            }

            var currLogIn = (from account in accountList where account.LoggedIn select account).First();
            return View("Index", currLogIn);
        }

        public ActionResult TransactionHistory()
        {
            var accountList = accountClient.GetAccounts();

            if (!IsLoggedIn())
            {
                return View("Index");
            }

            var currLogIn = (from account in accountList where account.LoggedIn select account).First();
            var transactionList = transactionClient.GetTransactions();

            var transactions = (from transaction in transactionList
                                where transaction.AccountNumber.Equals(currLogIn.AccountNumber)
                                select transaction);
            if (!transactions.Any())
            {
                return View();
            }
            else
            {
                TransactionViewModel transactionViewModel =
                                new TransactionViewModel(currLogIn, transactions.ToList());
                return View(transactionViewModel);
            }
        }

        private bool IsLoggedIn()
        {
            var accountList = accountClient.GetAccounts();
            var isLoggedIn = (from account in accountList
                              where account.LoggedIn
                              select account.AccountNumber);
            if (isLoggedIn.Any())
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        private Models.Account getLoggedIn()
        {
            var accountList = accountClient.GetAccounts();

            if (!IsLoggedIn())
            {
                return null;
            }
            else
            {
                return (from account in accountList where account.LoggedIn select account).First();
            }
        }
    }
}
