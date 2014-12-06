using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Net;
using Newtonsoft.Json;
using MyGiftCard;
using MyGIftCard;
using System.Text;
using System.Security.Cryptography;
using System.IO;

namespace GiftCertProTest
{
    [TestClass]
    public class GiftCardMSSQLDAOTest
    {
        GiftCardMSSQLDAO test = null;
        private CompanyModel createTestCompanyModel()
        {
            var CompanyName = "TestSalon" + DateTime.Now.Millisecond + DateTime.Now.Second;
            var address = new Address()
            {
                AddressOne = DateTime.Now.Millisecond.ToString() + " Salon Str",
                AddressTwo = DateTime.Now.Second % 2 == 0 ? null : DateTime.Now.Millisecond + " Box number",
                City = "Some city",
                State = "TN",
                Zip = DateTime.Now.Millisecond.ToString()
            };
            return new CompanyModel()
            {
                CompanyName = CompanyName,
                CompanyAddress = address,
                AmericanExpressAccepted = DateTime.Now.Millisecond * DateTime.Now.Second % 2 == 0,
                DiscoverAccepted = DateTime.Now.Millisecond * DateTime.Now.Second % 2 == 1,
                MasterCardAccepted = DateTime.Now.Millisecond / (DateTime.Now.Second == 0 ? 3 : DateTime.Now.Second) % 2 == 0,
                VisaAccepted = DateTime.Now.Millisecond + DateTime.Now.Second % 2 == 0,
                AllowGratuity = false,
                FinePrint = "Some test fineprint",
                AllowMailOption = true,
                ExpireAfterDays = 20,
                ShippingCost = 2.5,
                PayPalID = "eee",
                CompanyContactInfo = new ContactInfo()
                {
                    Email = "test@nyc.com",
                    FirstName = "salon first name",
                    LastName = "salon last name",
                    Phone = "8883334444",
                    Fax = "8889992222"
                }
            };
        }
        private PendingOrders createTestPendingOrder(CompanyModel salon)
        {
            var input = new PendingOrders();
            input.CompanyName = salon.CompanyName;
            input.Purchaser = new Purchaser() {
                Name = "test user " + DateTime.Now.Second
            };
            var address = new Address()
            {
                AddressOne = DateTime.Now.Millisecond.ToString() + " Salon Str",
                AddressTwo = DateTime.Now.Second % 2 == 0 ? null : DateTime.Now.Millisecond + " Box number",
                City = "Some city",
                State = "TN",
                Zip = DateTime.Now.Millisecond.ToString()
            };
            input.DeliveryInfo = new DeliveryMethod()
            {
                DeliveryAddress = address,
                Email = "somebogusemail@email.com"
            };
            input.CardInfo = new CreditCardInfo()
            {
                BillingInfo = address,
                CardCode = "3344",
                ExpirationDate = "10/23",
                CardNumber = "344444334r43",
                CardType = "Visa"
            };
            input.Recipient = new Recipient() {
                Name = "some fake person" + DateTime.Now.Second
            };
            return input;
        }

        [TestMethod]
        public void TestInputPendingOrder()
        {
            test = new GiftCardMSSQLDAO().Init();
            var salon = createTestCompanyModel();
            var input = createTestPendingOrder(salon);
            Assert.IsTrue(test.UpdateSalon(salon));
            input.CompanyId = salon.Id.Value;
            var s = test.InsertPendingOrder(input);
            Assert.IsTrue(s.HasValue);
            var results = test.retrievePendingOrders(salon.Id.Value, DateTime.Now.AddMinutes(-3), DateTime.Now.AddMinutes(2));
            Assert.IsNotNull(results);
            Console.WriteLine("Number returned is " + results.Count);
            Assert.IsTrue(results.Count > 0);
        }
        [TestMethod]
        public void TestCreateCategoryForSalon()
        {
            test = new GiftCardMSSQLDAO();
            var salon = createTestCompanyModel();
            Assert.IsTrue(test.UpdateSalon(salon));
            var t = "test" + (DateTime.Now.Millisecond % 100);
            var ret = test.CreateCategoryForCompany(salon.Id.Value, t);
            Assert.IsTrue(ret.HasValue);
            var results = test.RetrieveCategoriesByCompany(salon.Id.Value);
            Assert.IsNotNull(results);
            Assert.IsNotNull(results.Find(item => item.Trim() == t));
        }
        [TestMethod]
        public void TestCreateProductForCategoryBySalon()
        {
            test = new GiftCardMSSQLDAO();
            var salon = createTestCompanyModel();
            Assert.IsTrue(test.UpdateSalon(salon));
            var category = "test" + (DateTime.Now.Millisecond % 100);
            var ret = test.CreateCategoryForCompany(salon.Id.Value, category);
            Assert.IsTrue(ret.HasValue);
            String p = "Product_" + (DateTime.Now.Millisecond % 90) + "_";
            var r = new Random();
            for (int t = 0; t < 5; t++)
            {
                var ret2 = test.CreateProductForCategoryByCompany(ret.Value, p + t, ((((int)(r.NextDouble() * 10000)) / 100)));
                Assert.IsTrue(ret2.HasValue);
            }
            var results = test.RetrieveProductsBySalonCategory(ret.Value);
            Assert.IsNotNull(results);
            Assert.AreEqual(5, results.Count);
            Assert.AreEqual(p + 3, results[3].Name);
            Assert.IsNotNull(results[4].ProductId);
        }

        [TestMethod]
        public void TestSaveAuthInfo()
        {
            test = new GiftCardMSSQLDAO();
            var salon = createTestCompanyModel();
            Assert.IsTrue(test.UpdateSalon(salon));
            var testsalon = test.retrieveClients();
            Assert.IsTrue(testsalon.Count > 0);
            var foundsalon = testsalon.Find(i => i.CompanyName.Equals(salon.CompanyName));
            string username = "testuser" + DateTime.Now.Second;
            string password = "testpass" + DateTime.Now.Millisecond;
            var enc = Encoding.Default;
            SHA256 mySHA256 = SHA256Managed.Create();
            Assert.IsTrue(test.SaveAuthInfo(foundsalon.Id.Value, username, mySHA256.ComputeHash(enc.GetBytes(password))));
            var hash = mySHA256.ComputeHash(enc.GetBytes(password));
            var d = test.CheckPassword(foundsalon.Id.Value, username, hash);
            Assert.IsNotNull(d);
            Assert.IsTrue(d, "SELECT password_len FROM AuthInfo WHERE salon_id=" + foundsalon.Id.Value + " AND username='" + username + "'\n AND password=" + Convert.ToBase64String(hash));
        }

        [TestMethod]
        public void TestUpdateSalon()
        {
            test = new GiftCardMSSQLDAO();
            var salon = createTestCompanyModel();
            Assert.IsTrue(test.UpdateSalon(salon));
            var testsalon = test.retrieveClients();
            Assert.IsTrue(testsalon.Count > 0);
            var foundsalon = testsalon.Find(i => i.CompanyName.Equals(salon.CompanyName));
            Assert.IsNotNull(foundsalon);
            Assert.AreEqual<String>(foundsalon.CompanyAddress.AddressOne, salon.CompanyAddress.AddressOne);
            Assert.IsTrue(salon.Id.HasValue);
        }

        private byte[] loadImage()
        {
            var filename = "C:\\Users\\James\\Pictures\\debate_logo.jpg";
            var img = System.Drawing.Image.FromFile(filename);
            MemoryStream ms = new MemoryStream();
            img.Save(ms, System.Drawing.Imaging.ImageFormat.Jpeg);
            return ms.ToArray();
        }

        [TestMethod]
        public void TestSaveUploadedFile()
        {
            test = new GiftCardMSSQLDAO();
            var salon = createTestCompanyModel();
            Assert.IsTrue(test.UpdateSalon(salon));
            var img = loadImage();
            var input = new UploadedFile()
            {
                FileLength = img.Length,
                FileName = "mylogo.jpg"
            };
            var logo_id = test.SaveUploadedFile(input);
            Assert.IsNotNull(logo_id);
            Assert.IsTrue(logo_id.HasValue);
            var result = test.RetrieveLogoFileInfoForClient(salon.Id.Value);
            Assert.IsNotNull(result);
            Assert.AreEqual(input.FileLength, result.FileLength);
            Assert.AreEqual(input.FileName, result.FileName);
        }

        [TestMethod]
        public void TestUpdateCurrentSalonSettings()
        {
            test = new GiftCardMSSQLDAO();
            var salon = createTestCompanyModel();
            Assert.IsTrue(test.UpdateSalon(salon));
            var img = loadImage();
            var logo_id = test.SaveUploadedFile(new UploadedFile()
            {
                CompanyId = salon.Id.Value,
                FileLength = img.Length,
                FileName = "mylogo.jpg"
            });
            var input = new CurrentCompanyDisplaySettings()
            {
                CompanyId = salon.Id,
                CurrentThemeName = "Test theme"
            };
            var ret = test.UpdateCompanySettings(input, logo_id.Value);
            Assert.IsNotNull(ret);

            input.CurrentThemeName = "Test theme 2";
            ret = test.UpdateCompanySettings(input, logo_id.Value);
            Assert.IsNotNull(ret);
            var result = test.RetrieveDisplaySettings(salon.Id.Value);
        }
    }
}
