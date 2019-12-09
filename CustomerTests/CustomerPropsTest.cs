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
    public class CustomerPropsTest
    {
        CustomerProps props;
        [SetUp]
        public void Setup()
        {
            props = new CustomerProps();
            props.ID = 1;
                props.name = "Mickey Mouse";
            props.address = "Main Street";
            props.city = "Orlando";
            props.state = "FL";
            props.zipcode = "10001";
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
            CustomerProps props2 = new  CustomerProps();
            props2.SetState(props.GetState());
            Assert.AreEqual(props.ID, props2.ID);
            Assert.AreEqual(props.name, props2.name);
            Assert.AreEqual(props.address, props2.address);
            Assert.AreEqual(props.state, props2.state);
            Assert.AreEqual(props.city, props2.city);
            Assert.AreEqual(props.zipcode, props2.zipcode);
            Assert.AreEqual(props.ConcurrencyID, props2.ConcurrencyID);

            
            
            // do productprops in the same way. 
            // 

        }
        [Test]
        public void CloneTest()
        {
            CustomerProps props2 = new CustomerProps();
            
          


        }

       
       
    }
}
