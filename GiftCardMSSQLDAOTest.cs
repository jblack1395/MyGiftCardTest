using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Net;
using Newtonsoft.Json;
using MyGiftCard;

namespace GiftCertProTest
{
    [TestClass]
    public class GiftCardMSSQLDAOTest
    {
        GiftCardMSSQLDAO test = null;

        [TestInitialize]
        public void TestInit()
        {
            test = new GiftCardMSSQLDAO();
            test = test.Init();
        }

        [TestMethod]
        public void TestRetrieveClients()
        {
            var results = test.retrieveClients();
            if (results.Count == 0)
                new AssertFailedException("Nothing was returned from the test");
        }

        [TestMethod]
        public void TestRetrievePendingOrders()
        {
            var results = test.retrievePendingOrders("testcompany", DateTime.Now.AddDays(-30), DateTime.Now);
            if (results.Count == 0)
                new AssertFailedException("Nothing was returned from the test");
        }
        [TestMethod]
        public void TestRetrievePendingOrdersWithFilteredName()
        {
            var results = test.retrievePendingOrders("testcompany", DateTime.Now.AddDays(-30), DateTime.Now, "testrecipient");
            if (results.Count == 0)
                new AssertFailedException("Nothing was returned from the test");
        }
    }
}
