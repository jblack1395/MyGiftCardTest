using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Net;
using Newtonsoft.Json;

namespace GiftCertProTest
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void TestGetSalonList()
        {
            /*
            WebClient proxy = new WebClient();
            string serviceURL = 
                    string.Format("http://localhost:61090/OrderService.svc
                                    /GetOrderDetails/{0}", orderID); 
            byte[] data = proxy.DownloadData(serviceURL);
            Stream stream = new MemoryStream(data);
            DataContractJsonSerializer obj = 
                        new DataContractJsonSerializer(typeof(OrderContract));
            OrderContract order = obj.ReadObject(stream) as OrderContract;
            Console.WriteLine("Order ID : " + order.OrderID);
            Console.WriteLine("Order Date : " + order.OrderDate);
            Console.WriteLine("Order Shipped Date : " + order.ShippedDate);
            Console.WriteLine("Order Ship Country : " + order.ShipCountry);
            Console.WriteLine("Order Total : " + order.OrderTotal);
             */
        }
    }
}
