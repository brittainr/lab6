using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CustomerProductPropsClasses;
using ToolsCSharp;

using System.Data;
using System.Data.SqlClient;

// *** I use an "alias" for the ado.net classes throughout my code
// When I switch to an oracle database, I ONLY have to change the actual classes here
using DBBase = ToolsCSharp.BaseSQLDB;
using DBConnection = System.Data.SqlClient.SqlConnection;
using DBCommand = System.Data.SqlClient.SqlCommand;
using DBParameter = System.Data.SqlClient.SqlParameter;
using DBDataReader = System.Data.SqlClient.SqlDataReader;
using DBDataAdapter = System.Data.SqlClient.SqlDataAdapter;

namespace CustomerProductDBClasses
{
    public class CustomerDB : DBBase, IReadDB, IWriteDB
    {
        public CustomerDB() : base() { }
        public CustomerDB(string cnString) : base(cnString) { }
        public CustomerDB(DBConnection cn) : base(cn) { }

        public IBaseProps Retrieve(Object key)
        {
            DBDataReader data = null;
            CustomerProps props = new CustomerProps();
            DBCommand command = new DBCommand();

            command.CommandText = "usp_CustomerSelect";
            command.CommandType = CommandType.StoredProcedure;
            command.Parameters.Add("@CustomerID", SqlDbType.Int);
            command.Parameters["@CustomerID"].Value = (Int32)key;

            try
            {
                data = RunProcedure(command);
                if (!data.IsClosed)
                {
                    if (data.Read())
                    {
                        props.SetState(data);
                    }
                    else
                        throw new Exception("Record does not exist in the database.");
                }
                return props;
            }
            catch (Exception e)
            {
                // log this exception
                throw;
            }
            finally
            {
                if (data != null)
                {
                    if (!data.IsClosed)
                        data.Close();
                }
            }
        } //end of Retrieve()
        public object RetrieveAll(Type type)
        {
            List<CustomerProps> list = new List<CustomerProps>();
            DBDataReader reader = null;
            CustomerProps props;

            try
            {
                reader = RunProcedure("usp_CustomerSelectAll");
                if (!reader.IsClosed)
                {
                    while (reader.Read())
                    {
                        props = new CustomerProps();
                        props.SetState(reader);
                        list.Add(props);
                    }
                }
                return list;
            }
            catch (Exception e)
            {
                // log this exception
                throw;
            }
            finally
            {
                if (!reader.IsClosed)
                {
                    reader.Close();
                }
            }
        }
        public IBaseProps Create(IBaseProps p) // possibly might need to change to props to match origional parameter.
        {
//            @CustomerID int output,
//@Name varchar(100)

//    @Address varchar(50)
//	@City varchar(20)
//        @State char(2)

//    @ZipCode char(15)
            int rowsAffected = 0;
            CustomerProps props = (CustomerProps)p;

            DBCommand command = new DBCommand();
            command.CommandText = "usp_CustomerCreate";
            command.CommandType = CommandType.StoredProcedure;
            command.Parameters.Add("@CustomerID", SqlDbType.Int);
            command.Parameters.Add("@Name", SqlDbType.VarChar);// these match the parameters int he stored procedure. name adreess etc
            command.Parameters.Add("@Address", SqlDbType.VarChar);
            command.Parameters.Add("@City", SqlDbType.VarChar);
            command.Parameters.Add("@State", SqlDbType.Char);
            command.Parameters.Add("@ZipCode", SqlDbType.Char);
            command.Parameters[0].Direction = ParameterDirection.Output;
            // there were three statements here before, it did not include customerID ask about if it needs to be 
            //added
            // assuming it would be redundant if it wasnt there before....

            command.Parameters["@Name"].Value = props.name;
            command.Parameters["@Address"].Value = props.address;
            command.Parameters["@City"].Value = props.city;
            command.Parameters["@State"].Value = props.state;
            command.Parameters["@ZipCode"].Value = props.zipcode;

            try
            {
                rowsAffected = RunNonQueryProcedure(command);
                if (rowsAffected == 1)
                {
                    props.ID = (int)command.Parameters[0].Value;
                    props.ConcurrencyID = 1;
                    return props;
                }
                else
                    throw new Exception("Unable to insert record. " + props.ToString());
            }
            catch (Exception e)
            {
                // log this error
                throw;
            }
            finally
            {
                if (mConnection.State == ConnectionState.Open)
                    mConnection.Close();
            }
        }

        public bool Delete(IBaseProps p)// this is the one we want not the other one!!!!
        {
            CustomerProps props = (CustomerProps)p;
            int rowsAffected = 0;

            DBCommand command = new DBCommand();
            command.CommandText = "usp_CustomerDelete";
            command.CommandType = CommandType.StoredProcedure;
            command.Parameters.Add("@CustomerID", SqlDbType.Int);
            command.Parameters.Add("@ConcurrencyID", SqlDbType.Int);
            command.Parameters["@CustomerID"].Value = props.ID;
            command.Parameters["@ConcurrencyID"].Value = props.ConcurrencyID;

            try
            {
                rowsAffected = RunNonQueryProcedure(command);
                if (rowsAffected == 1)
                {
                    return true;
                }
                else
                {
                    string message = "Record cannot be deleted. It has been edited by another user.";
                    throw new Exception(message);
                }

            }
            catch (Exception e)
            {
                // log this exception
                throw;
            }
            finally
            {
                if (mConnection.State == ConnectionState.Open)
                    mConnection.Close();
            }
        } // end of Delete()


        public bool Update(IBaseProps p)
        {
            int rowsAffected = 0;
            CustomerProps props = (CustomerProps)p;

            DBCommand command = new DBCommand();
            command.CommandText = "usp_CustomerUpdate";
            command.CommandType = CommandType.StoredProcedure;
            command.Parameters.Add("@CustomerID", SqlDbType.Int);
            command.Parameters.Add("@Name", SqlDbType.VarChar);
            command.Parameters.Add("@Address", SqlDbType.VarChar);
            command.Parameters.Add("@City", SqlDbType.VarChar);
            command.Parameters.Add("@State", SqlDbType.Char);
            command.Parameters.Add("@ZipCode", SqlDbType.Char);
            command.Parameters.Add("@ConcurrencyID", SqlDbType.Int);
            command.Parameters["@CustomerID"].Value = props.ID;
            command.Parameters["@Name"].Value = props.name;
            command.Parameters["@Address"].Value = props.address;
            command.Parameters["@City"].Value = props.city;
            command.Parameters["@State"].Value = props.state;
            command.Parameters["@ZipCode"].Value = props.zipcode;
            command.Parameters["@ConcurrencyID"].Value = props.ConcurrencyID;

            try
            {
                rowsAffected = RunNonQueryProcedure(command);
                if (rowsAffected == 1)
                {
                    props.ConcurrencyID++;
                    return true;
                }
                else
                {
                    string message = "Record cannot be updated. It has been edited by another user.";
                    throw new Exception(message);
                }
            }
            catch (Exception e)
            {
                // log this exception
                throw;
            }
            finally
            {
                if (mConnection.State == ConnectionState.Open)
                    mConnection.Close();
            }
        } // end of Update()
    }
}
