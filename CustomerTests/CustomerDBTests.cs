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

using DBCommand = System.Data.SqlClient.SqlCommand;

namespace CustomerTests
{
    [TestFixture]

   public class CustomerDBTests
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
        public void TestUpdate()
        {
            // extract all column data from (1)
            CustomerProps props = (CustomerProps)db.Retrieve(1);
            // change columns data
            props.name = "Linda Fillery";
            props.address = "345 South Fairy LN";
            props.city = "Casino Bay";
            props.state = "FL";
            props.zipcode = "77897";
            // update columns data
            db.Update(props);

            //check all new column data
            CustomerProps prop = (CustomerProps)db.Retrieve(1);
            Assert.AreNotEqual(prop.name, "Molunguri, A");
            Assert.AreNotEqual(prop.address, "1108 Johanna Bay Drive");
            Assert.AreNotEqual(prop.city, "Birmingham");
            Assert.AreNotEqual(prop.state, "AL");
            Assert.AreNotEqual(prop.zipcode, "35216-6909");
            


            
        }

        [Test]
        public void TestDelete()
        {
            CustomerProps props = (CustomerProps)db.Retrieve(1);
            db.Delete(props);
            Assert.Throws<Exception>(()=>db.Retrieve(1));

        }


        [Test]
        public void TestRetrive()
        {
            CustomerProps props = (CustomerProps)db.Retrieve(1);
            Assert.AreEqual(1, props.ID);
            Assert.AreEqual("Molunguri, A", props.name);
            Assert.AreEqual("1108 Johanna Bay Drive", props.address);
            Assert.AreEqual("Birmingham", props.city);
            Assert.AreEqual("AL", props.state);
            Assert.AreEqual("35216-6909", props.zipcode);
            
        }
        [Test]
        public void TestRetrieveAll()
        {
           
        }
        [Test]
       public void TestCreate()
        {
            CustomerProps props = new CustomerProps();
            

            
            props.name = "Linda Seafoam";
            props.address = "145 South Lindholm Ave";
            props.city = "Yukata";
            props.state = "FL";
            props.zipcode = "88765";
            
           props= (CustomerProps)db.Create(props);
            CustomerProps prop2 = (CustomerProps)db.Retrieve(props.ID);
            Assert.AreEqual(props.name, prop2.name);
            Assert.AreEqual(props.address, prop2.address);
            Assert.AreEqual(props.city, prop2.city);
            Assert.AreEqual(props.state, prop2.state);
            Assert.AreEqual(props.zipcode, prop2.zipcode);
            
            //CustomerProps props2 = (CustomerProps)db.Retrieve(780);
            //Assert.AreEqual(props2.ID, 780);
        }
            

    }
}
