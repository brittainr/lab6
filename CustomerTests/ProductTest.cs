using System;

using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;
using CustomerProductPropsClasses;
using System.Data;
using System.Data.SqlClient;
using CustomerProductDBClasses;
using ToolsCSharp;
using CustomerProductClasses;

using DBCommand = System.Data.SqlClient.SqlCommand;

namespace CustomerTests
{
    [TestFixture]
    public class ProductTest
    {

        ProductDB db;
        string datasource = "Data Source=DESKTOP-BB3I58F\\SQLEXPRESS;Initial Catalog=MMABooksUpdated;Integrated Security=True";

        [SetUp]
        public void TestResetDatabase()
        {
            db = new ProductDB(datasource);
            DBCommand command = new DBCommand();
            command.CommandText = "usp_testingResetData";
            command.CommandType = CommandType.StoredProcedure;
            db.RunNonQueryProcedure(command);
        }

        [Test]
        public void TestNewCustomerConstructor()
        {
            // not in Data Store - no id
             Product p = new Product(datasource);
            Console.WriteLine(p.ToString());
            Assert.Greater(p.ToString().Length, 1);
        }


        [Test]
        public void TestRetrieveFromDataStoreContructor()
        {
            // retrieves from Data Store
            Product p = new Product(1, datasource);
            Assert.AreEqual(p.ID, 1);
            Assert.AreEqual(p.ProductCode, "A4CS");
            Console.WriteLine(p.ToString());
        }

        [Test]
        public void TestSaveToDataStore()
        {
           Product p = new Product(datasource);
            p = new Product(1, datasource);
            p.ProductCode = "B5N6";
            p.Description = "Garage";
            p.UnitPrice = 14000.56m;
            p.OnhandQuantity = 1;
            p.Save();
            Assert.AreEqual("B5N6", p.ProductCode);
        }

        [Test]
        public void TestUpdate()
        {
            Product p = new Product(1, datasource);
            p.ProductCode = "5K6N7";
            p.Description = "polka dot halter";
            p.UnitPrice = 20.55m;
            p.OnhandQuantity = 160;
            p.Save();

            p = new Product(1, datasource);
            Assert.AreEqual(p.ProductCode, "5K6N7");
            Assert.AreEqual(p.Description, "polka dot halter");
            Assert.AreEqual(p.UnitPrice, 20.55);
            Assert.AreEqual(p.OnhandQuantity, 160);
            
        }
        [Test]
        public void TestDelete()
        {
            Product p = new Product(2, datasource);
            p.Delete();
            p.Save();
            Assert.Throws<Exception>(() => new Product(2, datasource));
        }
        [Test]
        public void TestGetList()
        {
            Product p = new Product(datasource);
            List<Product> products = (List<Product>)p.GetList();
            Assert.AreEqual(16, products.Count);
            Assert.AreEqual(1, products[0].ID);
            Assert.AreEqual("A4CS", products[0].ProductCode);
        }

        [Test]
        public void TestNoRequiredPropertiesNotSet()
        {
            // not in Data Store - userid, title and description must be provided
            Product p = new Product(datasource);
            Assert.Throws<Exception>(() => p.Save());
        }

        [Test]
        public void TestSomeRequiredPropertiesNotSet() // this one is needed.
        {
            // not in Data Store - userid, title and description must be provided
            Product p = new Product(datasource);

            p.ProductCode = "6K7IU";
            Assert.Throws<Exception>(() => p.Save());
            p.Description = "Stuffed animal";
            Assert.Throws<Exception>(() => p.Save());
            p.UnitPrice = 5.55m;
            Assert.Throws<Exception>(() => p.Save());
        }

        [Test]
        public void TestInvalidPropertyUserIDSet() // this one is needed.
        {
            Product p = new Product(datasource);
            Assert.Throws<ArgumentOutOfRangeException>(() => p.ProductCode = "This is too long");


        }
        [Test]
        public void TestConcurrencyIssue() // concurrency test , to show how concurrency works. maybe try to do it yourself !
        {
            Product p1 = new Product(1, datasource);
            Product p2 = new Product(1, datasource);

            p1.ProductCode = "First";
            p1.Save();

            p2.ProductCode = "Second";
            Assert.Throws<Exception>(() => p2.Save());
        }



    }
}
