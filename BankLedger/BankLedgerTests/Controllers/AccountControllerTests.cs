using Microsoft.VisualStudio.TestTools.UnitTesting;
using BankLedger.Controllers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BankLedger.Models;
using System.Web.Http;
using System.Collections;

namespace BankLedger.Controllers.Tests
{
    [TestClass()]
    public class AccountControllerTests
    {
        private AccountController accountController;

        [TestMethod()]
        public void GetTest()
        {
            SetUp();
            var result = accountController.Get();
            Assert.IsInstanceOfType(result, typeof(IEnumerable<Account>));
            Assert.AreEqual(result.First().AccountNumber, "0");
        }

        [TestMethod()]
        public void GetTest1()
        {
            SetUp();
            var result = accountController.Get(0);
            Assert.IsInstanceOfType(result, typeof(Account));
            Assert.AreEqual(result.AccountNumber, "0");
            result = accountController.Get(2);
            Assert.IsInstanceOfType(result, typeof(Account));
            Assert.AreEqual(result.AccountNumber, "2");
        }

        [TestMethod()]
        public void PostTest()
        {
            SetUp();
            Account testAccount = new Account
            {
                AccountNumber = "testAccountNumber10",
                Name = "testName10",
                UserName = "testUserName10",
                Password = "testPassword10",
                Balance = 10
            };
            var result = accountController.Post(testAccount);
            Assert.IsInstanceOfType(result, typeof(IHttpActionResult));
        }

        [TestMethod()]
        public void PutTest()
        {
            SetUp();
            var testAccount = accountController.Get(1);
            Assert.AreEqual(testAccount.Balance, 1);
            testAccount.Balance = 10;
            testAccount.Name = "update";
            var result = accountController.Put(testAccount);
            Assert.IsInstanceOfType(result, typeof(IHttpActionResult));
            var result2 = accountController.Get(1);
            Assert.AreEqual(result2.Balance, 10);
            Assert.AreEqual(result2.Name, "update");
        }

        [TestMethod()]
        public void DeleteTest()
        {
            SetUp();
            var testAccounts = accountController.Get();
            var accountCount = testAccounts.Count();
            var result = accountController.Delete(1);
            Assert.IsInstanceOfType(result, typeof(IHttpActionResult));
            testAccounts = accountController.Get();
            Assert.AreEqual(testAccounts.Count(), accountCount-1);
        }

        public void SetUp()
        {
            accountController = new AccountController();
            for (int i = 0; i < 4; i++)
            {
                Account testAccount = new Account
                {
                    AccountNumber = String.Format("{0}", i),
                    Name = String.Format("testName{0}", i),
                    UserName = String.Format("testUserName{0}", i),
                    Password = String.Format("testPassword{0}", i),
                    Balance = i
                };
                accountController.Post(testAccount);
            }
        }
    }
}