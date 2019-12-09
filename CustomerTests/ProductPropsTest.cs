using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;
using CustomerProductPropsClasses;
using System.Data;
using System.Data.SqlClient;

using DBCommand = System.Data.SqlClient.SqlCommand;


namespace CustomerTests
{
    [TestFixture]
    public class ProductPropsTest
    {
        ProductProps props;
        [SetUp]
        public void Setup()
        {
            props = new ProductProps();
            props.ID = 1;
            props.ProductCode = "1234567891";
            props.Description = "Ramen Noodles";
            props.UnitPrice = 14.55m;
            props.OnHandQuantity = 19;
            props.ConcurrencyID = 4;

        }
        [Test]
        public void GetStateTest()
        {
            string output = props.GetState();
            Console.WriteLine(output);

        }
        [Test]
        public void SetStateTest()
        {
            ProductProps props2 = new ProductProps();
            props2.SetState(props.GetState());
            Assert.AreEqual(props.ID, props2.ID);
            Assert.AreEqual(props.ProductCode, props2.ProductCode);
            Assert.AreEqual(props.Description, props2.Description);
            Assert.AreEqual(props.UnitPrice, props2.UnitPrice);
            Assert.AreEqual(props.OnHandQuantity, props2.OnHandQuantity);
            Assert.AreEqual(props.ConcurrencyID, props2.ConcurrencyID);

            //finish properties
            //test clone
            // do productprops in the same way. 
            // 

        }
        
    }
}

