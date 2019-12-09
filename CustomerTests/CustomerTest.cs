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
    public class CustomerTest
    {
        
        CustomerDB db;
        string datasource = "Data Source=DESKTOP-BB3I58F\\SQLEXPRESS;Initial Catalog=MMABooksUpdated;Integrated Security=True";

        [SetUp]
        public void TestResetDatabase()
        {
            db = new CustomerDB(datasource);
            DBCommand command = new DBCommand();
            command.CommandText = "usp_testingResetData";
            command.CommandType = CommandType.StoredProcedure;
            db.RunNonQueryProcedure(command);
        }

        [Test]
        public void TestNewCustomerConstructor()
        {
            // not in Data Store - no id
            Customer c = new Customer(datasource);
            Console.WriteLine(c.ToString());
            Assert.Greater(c.ToString().Length, 1);
        }


        [Test]
        public void TestRetrieveFromDataStoreContructor()
        {
            // retrieves from Data Store
            Customer c = new Customer(1, datasource);
            Assert.AreEqual(c.ID, 1);
            Assert.AreEqual(c.Name, "Molunguri, A");
            Console.WriteLine(c.ToString());
        }
          
        [Test]
        public void TestSaveToDataStore()
        {
            Customer c = new Customer(datasource);
            c = new Customer(1, datasource);
            c.Name = "Tara Everglade";
            c.Address = "255 south candilice ave";
            c.City = "fullflower";
            c.State = "FL";
            c.ZipCode = "763876";
            c.Save();
            Assert.AreEqual("Tara Everglade", c.Name);
        }

        [Test]
        public void TestUpdate()
        {
           Customer c = new Customer(1, datasource);
            c.Name= "Erin Seafoam";
            c.Address = "5593 south waveward st";
            c.City = "Reefwood";
            c.State = "CA";
            c.ZipCode = "664752";
            c.Save();

            c = new Customer(1, datasource);
            Assert.AreEqual(c.Name,"Erin Seafoam");
            Assert.AreEqual(c.Address, "5593 south waveward st");
            Assert.AreEqual(c.City, "Reefwood");
            Assert.AreEqual(c.State, "CA");
            Assert.AreEqual(c.ZipCode, "664752");
        }
        [Test]
        public void TestDelete()
        {
            Customer c = new Customer(2, datasource);
            c.Delete();
            c.Save();
            Assert.Throws<Exception>(() => new Customer(2, datasource));
        }
        [Test]
        public void TestGetList()
        {
            Customer c = new Customer(datasource);
            List<Customer> customers = (List<Customer>)c.GetList();
            Assert.AreEqual(696, customers.Count);
            Assert.AreEqual(1, customers[0].ID);
            Assert.AreEqual("Molunguri, A", customers[0].Name);
        }

        [Test]
        public void TestNoRequiredPropertiesNotSet()
        {
            // not in Data Store - userid, title and description must be provided
            Customer c = new Customer(datasource);
            Assert.Throws<Exception>(() => c.Save());
        }

        [Test]
        public void TestSomeRequiredPropertiesNotSet() // this one is needed.
        {
            // not in Data Store - userid, title and description must be provided
            Customer c = new Customer(datasource);

            c.Name = "Titania Silvermeadow";
            Assert.Throws<Exception>(() => c.Save());
            c.State = "NY";
            Assert.Throws<Exception>(() => c.Save());
            c.City = "Fluffwood";
            Assert.Throws<Exception>(() => c.Save());
        }

        [Test]
        public void TestInvalidPropertyUserIDSet() // this one is needed.
        {
            Customer c = new Customer(datasource);
            Assert.Throws<ArgumentException>(() => c.State = "This is too long");
           
            
        }
        [Test]
        public void TestConcurrencyIssue() // concurrency test , to show how concurrency works. maybe try to do it yourself !
        {
            Customer c1 = new Customer(1, datasource);
            Customer c2 = new Customer(1, datasource);

            c1.Name = "First";
            c1.Save();

            c2.Name = "Second";
            Assert.Throws<Exception>(() => c2.Save());
        }




        // 16 products and 696 customers
    }
}
