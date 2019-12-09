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

    public class ProductDBTests
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
        public void TestUpdate()
        {
            // extract all column data from (1)
            ProductProps props = (ProductProps)db.Retrieve(1);
            // change columns data
            props.ProductCode = "45681";
            props.Description = "This is my description";
            props.UnitPrice = 12.55m;
            props.OnHandQuantity = 16;

            // update columns data
            db.Update(props);

            //check all new column data
            ProductProps prop = (ProductProps)db.Retrieve(1);
            Assert.AreNotEqual(prop.ProductCode, "A4CS");
            Assert.AreNotEqual(prop.Description, "Murach's ASP.NET 4 Web Programming with C# 2010");
            Assert.AreNotEqual(prop.UnitPrice, 56.50);
            Assert.AreNotEqual(prop.OnHandQuantity, 4637);





        }
    


        [Test]
        public void TestDelete()
            {
                ProductProps props = (ProductProps)db.Retrieve(1);
                db.Delete(props);
                Assert.Throws<Exception>(() => db.Retrieve(1));

            }



        [Test]
        public void TestRetrive()
        {
            ProductProps props = (ProductProps)db.Retrieve(1);
            Assert.AreEqual(1, props.ID);
            Assert.AreEqual("A4CS", props.ProductCode);
            Assert.AreEqual("Murach's ASP.NET 4 Web Programming with C# 2010", props.Description);
            Assert.AreEqual(56.50m, props.UnitPrice);
            Assert.AreEqual(4637, props.OnHandQuantity);
          

        }
        [Test]
        public void TestRetrieveAll()
        {

        }
        [Test]
        public void TestCreate()
        {
            ProductProps props = new ProductProps();



            props.ProductCode = "t5k6uf";
            props.Description = "thanksgiving feast.";
            props.UnitPrice = 200.00m;
            props.OnHandQuantity = 156;
            

            props = (ProductProps)db.Create(props);
            ProductProps prop2 = (ProductProps)db.Retrieve(props.ID);
            Assert.AreEqual(props.ProductCode, prop2.ProductCode);
            Assert.AreEqual(props.Description, prop2.Description);
            Assert.AreEqual(props.UnitPrice, prop2.UnitPrice);
            Assert.AreEqual(props.OnHandQuantity, prop2.OnHandQuantity);
           

            //CustomerProps props2 = (CustomerProps)db.Retrieve(780);
            //Assert.AreEqual(props2.ID, 780);
        }


    }
}



