using System;
using System.Collections.Generic;
using System.Linq;
using System.Diagnostics;
using System.Text;
using System.Threading.Tasks;
using System.Net.Http;
using BankLedgerClient.Clients;
using BankLedgerClient.Models;


namespace BankLedgerClient
{
    class Program
    {
        static void Main(string[] args)
        {
            //Console.WriteLine("Starting WebAPI Server...");
            //Process webAPIProcess = Process.Start(@"..\..\BankLedger\BankLedger\bin\Debug\BankLedger.exe");
            //Console.WriteLine("WebAPI Server Successfully Started!");
            var accountClient = new AccountClient("http://localhost:8080");
            var transactionClient = new TransactionClient("http://localhost:8080");

            while (true)
            {
                Console.WriteLine("Enter command:\n");
                string input = Console.ReadLine();
                string[] inputList = input.Split(' ');
                int result;
                switch (inputList[0])
                {
                    case "create":
                        if (inputList.Count() > 4)
                        {
                            result = CreateAccount(inputList[1], inputList[2], inputList[3], inputList[4]);
                        }
                        else
                        {
                            result = 1;
                        }
                        break;
                    case "login":
                        if (inputList.Count() > 2)
                        {
                            result = Login(inputList[1], inputList[2]);
                        }
                        else
                        {
                            result = 1;
                        }
                        break;
                    case "logout":
                        result = Logout();
                        break;
                    case "deposit":
                        if (inputList.Count() > 1)
                        {
                            result = Deposit(Decimal.Parse(inputList[1]));
                        }
                        else
                        {
                            result = 1;
                        }
                        break;
                    case "withdraw":
                        if (inputList.Count() > 1)
                        {
                            result = Withdraw(Decimal.Parse(inputList[1]));
                        }
                        else
                        {
                            result = 1;
                        }
                        break;
                    case "transactions":
                        result = PrintTransactions();
                        break;
                    case "balance":
                        result = CheckBalance();
                        break;
                    case "help":
                        Console.WriteLine("please enter one of the following commands:\n" +
                            "'create $ACCOUNT#$ $NAME$ $USERNAME$ $PASSWORD$' - Create Account\n" +
                            "'login $USERNAME$ $PASSWORD$' - Log in to account\n" +
                            "'logout' - Log out of account\n" +
                            "'deposit $AMNT$' - Deposit amount into account\n" +
                            "'withdraw $AMNT$' - Withdraw amount from account\n" +
                            "'transactions' - Show transaction history\n" +
                            "'balance' - Show current balance\n");
                        result = 0;
                        break;
                    default:
                        result = 0;
                        break;
                }
                string resultString;
                switch(result)
                {
                    case 0:
                        resultString = "Success";
                        break;
                    case 1:
                        resultString = "Invalid input. Please try again.";
                        break;
                    case 2:
                        resultString = "Account Number or Username already exists";
                        break;
                    case 3:
                        resultString = "Invalid login credentials. Please try again.";
                        break;
                    case 4:
                        resultString = "No account is logged in. Please log in then try again.";
                        break;
                    case 5:
                        resultString = "Account is already logged in. Please log out before logging into another account.";
                        break;
                    case 6:
                        resultString = "Amount would overdraw account.Please try again.";
                        break;
                    default:
                        resultString = "";
                        break;
                }
                Console.WriteLine(resultString + "\n");
            }

            int CreateAccount(string accountNumber, string name, string username, string password)
            {
                var accountList = accountClient.GetAccounts();

                var x = (from account in accountList
                         where account.UserName.Equals(username) || account.AccountNumber.Equals(accountNumber)
                         select account.AccountNumber);

                if (x.Any())
                {
                    return 2;
                }
                accountClient.AddAccount(new Account
                                        {
                                            AccountNumber = accountNumber,
                                            Name = name,
                                            UserName = username,
                                            Password = password,
                                            Balance = 0
                                        });
                return 0;
            }

            int Login(string username, string password)
            {
                var accountList = accountClient.GetAccounts();

                if (IsLoggedIn())
                {
                    return 5;
                }

                var validLogIn = (from account in accountList
                                  where account.UserName.Equals(username) && account.Password.Equals(password)
                                  select account);
                if (!validLogIn.Any())
                {
                    return 3;
                }
                else
                {
                    Account newLogin = validLogIn.First();
                    newLogin.LoggedIn = true;
                    Console.WriteLine(newLogin.ToString());
                    accountClient.UpdateAccount(newLogin);
                    return 0;
                }
            }

            int Logout()
            {
                var accountList = accountClient.GetAccounts();

                var isLoggedIn = (from account in accountList
                                  where account.LoggedIn
                                  select account);
                foreach(Account account in isLoggedIn)
                {
                    account.LoggedIn = false;
                    accountClient.UpdateAccount(account);
                }
                return 0;
            }

            int Deposit(decimal amount)
            {
                var accountList = accountClient.GetAccounts();

                if (!IsLoggedIn())
                {
                    return 4;
                }

                var currLogIn = (from account in accountList where account.LoggedIn select account).First();
                currLogIn.Balance += amount;
                accountClient.UpdateAccount(currLogIn);

                var transactionList = transactionClient.GetTransactions();
                transactionClient.AddTransaction(new Transaction
                {
                    Id = transactionList.Count(),
                    Date = DateTime.Now,
                    AccountNumber = currLogIn.AccountNumber,
                    TransactionType = "Deposit",
                    Amount = amount
                });
                return 0;
            }

            int Withdraw(decimal amount)
            {
                var accountList = accountClient.GetAccounts();

                if (!IsLoggedIn())
                {
                    return 4;
                }

                var currLogIn = (from account in accountList where account.LoggedIn select account).First();
                if (currLogIn.Balance - amount < 0)
                {
                    return 6;
                }
                currLogIn.Balance -= amount;
                accountClient.UpdateAccount(currLogIn);

                var transactionList = transactionClient.GetTransactions();
                transactionClient.AddTransaction(new Transaction
                {
                    Id = transactionList.Count(),
                    Date = DateTime.Now,
                    AccountNumber = currLogIn.AccountNumber,
                    TransactionType = "Withdraw",
                    Amount = amount
                });
                return 0;
            }

            int CheckBalance()
            {
                var accountList = accountClient.GetAccounts();

                if (!IsLoggedIn())
                {
                    return 4;
                }

                var currLogIn = (from account in accountList where account.LoggedIn select account).First();
                Console.WriteLine("$"+currLogIn.Balance.ToString());
                return 0;
            }

            int PrintTransactions()
            {
                var accountList = accountClient.GetAccounts();

                if (!IsLoggedIn())
                {
                    return 4;
                }

                var currLogIn = (from account in accountList where account.LoggedIn select account).First();
                var transactionList = transactionClient.GetTransactions();

                var transactions = (from transaction in transactionList
                                    where transaction.AccountNumber.Equals(currLogIn.AccountNumber)
                                    select transaction);
                string resultString = String.Format("Transaction history for Account #{0}\n\n", currLogIn.AccountNumber);
                foreach (var transaction in transactions)
                {
                    resultString += String.Format("{0}\nTransaction type: {1}\nAmount: {2}\n", 
                                                transaction.Date, 
                                                transaction.TransactionType, 
                                                transaction.Amount);
                }
                Console.WriteLine(resultString);
                return 0;
            }

            bool IsLoggedIn()
            {
                var accountList = accountClient.GetAccounts();
                var isLoggedIn = (from account in accountList
                                  where account.LoggedIn
                                  select account.AccountNumber);
                if (isLoggedIn.Any())
                {
                    return true;
                } else
                {
                    return false;
                }
            }
        }
    }
}