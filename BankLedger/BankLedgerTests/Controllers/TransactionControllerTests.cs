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
    public class TransactionControllerTests
    {
        private TransactionController transactionController;

        [TestMethod()]
        public void GetTest()
        {
            SetUp();
            var result = transactionController.Get();
            Assert.IsInstanceOfType(result, typeof(IEnumerable<Transaction>));
            Assert.AreEqual(result.First().Id, 0);
        }

        [TestMethod()]
        public void GetTest1()
        {
            SetUp();
            var result = transactionController.Get(0);
            Assert.IsInstanceOfType(result, typeof(Transaction));
            Assert.AreEqual(result.Id, 0);
            result = transactionController.Get(2);
            Assert.IsInstanceOfType(result, typeof(Transaction));
            Assert.AreEqual(result.Id, 2);
        }

        [TestMethod()]
        public void PostTest()
        {
            SetUp();
            Transaction testTransaction = new Transaction
            {
                Id = 10,
                Date = DateTime.Now,
                AccountNumber = "testAccountNumber10",
                TransactionType = "testTransactionType",
                Amount = 10
            };
            var result = transactionController.Post(testTransaction);
            Assert.IsInstanceOfType(result, typeof(IHttpActionResult));
        }

        [TestMethod()]
        public void PutTest()
        {
            SetUp();
            var testTransaction = transactionController.Get(1);
            Assert.AreEqual(testTransaction.Amount, 1);
            testTransaction.Amount = 10;
            testTransaction.AccountNumber = "update";
            var result = transactionController.Put(testTransaction);
            Assert.IsInstanceOfType(result, typeof(IHttpActionResult));
            var result2 = transactionController.Get(1);
            Assert.AreEqual(result2.Amount, 10);
            Assert.AreEqual(result2.AccountNumber, "update");
        }

        [TestMethod()]
        public void DeleteTest()
        {
            SetUp();
            var testTransactions = transactionController.Get();
            var transactionCount = testTransactions.Count();
            var result = transactionController.Delete(1);
            Assert.IsInstanceOfType(result, typeof(IHttpActionResult));
            testTransactions = transactionController.Get();
            Assert.AreEqual(testTransactions.Count(), transactionCount-1);
        }

        public void SetUp()
        {
            transactionController = new TransactionController();
            for (int i=0; i<4; i++)
            {
                Transaction testTransaction = new Transaction
                {
                    Id = i,
                    Date = DateTime.Now,
                    AccountNumber = String.Format("testAccountNumber{0}", i),
                    TransactionType = "testTransactionType",
                    Amount = i
                };
                transactionController.Post(testTransaction);
            }
            
        }
    }
}