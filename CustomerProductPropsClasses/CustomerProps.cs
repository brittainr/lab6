using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using System.IO;
using ToolsCSharp;

using DBDataReader = System.Data.SqlClient.SqlDataReader; // this creates an alias so that if 
//i can swap it out for an oracle database, if i wanted to change it 
//i would change it to oracledatareader,
//and since it is referred to as its alias, it is easily changeable later!

namespace CustomerProductPropsClasses
{
    [Serializable()]
    public class CustomerProps : IBaseProps
    {

        #region instance variables
        /// <summary>
        /// 
        /// </summary>
        public int ID = Int32.MinValue;

        /// <summary>
        /// 
        /// </summary>
        public string name = "";


        /// <summary>
        /// 
        /// </summary>
        public string address = "";

        /// <summary>
        /// 
        /// </summary>
        public string city = "";

        /// <summary>
        /// 
        /// </summary>
        public string state = "";

        public string zipcode = "";

        /// <summary>
        /// ConcurrencyID. See main docs, don't manipulate directly
        /// </summary>
        public int ConcurrencyID = 0;
        #endregion

        #region constructor
        /// <summary>
        /// Constructor. This object should only be instantiated by Customer, not used directly.
        /// </summary>
        public CustomerProps()
        {
        }

        #endregion

        #region BaseProps Members
        /// <summary>
        /// Serializes this props object to XML, and writes the key-value pairs to a string.
        /// </summary>
        /// <returns>String containing key-value pairs</returns>	
        public string GetState()
        {
            XmlSerializer serializer = new XmlSerializer(this.GetType());
            StringWriter writer = new StringWriter();
            serializer.Serialize(writer, this);
            return writer.GetStringBuilder().ToString();
        }

        // I don't always want to generate xml in the db class so the 
        // props class can read in from xml
        public void SetState(DBDataReader dr)
        {
            this.ID = (Int32)dr["CustomerID"];
            this.name = dr["Name"].ToString();
            this.address = (string)dr["Address"];
            this.city = (string)dr["City"];
            this.state = ((string)dr["State"]).Trim();
            this.zipcode = ((string)dr["ZipCode"]).Trim();
            this.ConcurrencyID = (Int32)dr["ConcurrencyID"];
            // do this for char to string conversion from database.
        }

        /// <summary>
        /// 
        /// </summary>
        public void SetState(string xml)
        {
            XmlSerializer serializer = new XmlSerializer(this.GetType());
            StringReader reader = new StringReader(xml);
            CustomerProps p = (CustomerProps)serializer.Deserialize(reader);
            this.ID = p.ID;
            this.name = p.name;
            this.address = p.address;
            this.city = p.city;
            this.state = p.state;
            this.zipcode = p.zipcode;
            this.ConcurrencyID = p.ConcurrencyID;
        }
        #endregion

        #region ICloneable Members
        /// <summary>
        /// Clones this object.
        /// </summary>
        /// <returns>A clone of this object.</returns>
        public Object Clone()
        {
            CustomerProps p = new CustomerProps();
            p.ID = this.ID;
            p.name = this.name;
            p.address = this.address;
            p.city = this.city;
            p.state = this.state;
            p.zipcode = this.zipcode;
            p.ConcurrencyID = this.ConcurrencyID;
            return p;
        }
        #endregion

    }
}
