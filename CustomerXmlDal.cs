using System.Data;
using System.Reflection.Metadata.Ecma335;
using System.Security.Cryptography.Xml;
namespace MVCDHProject.Models
{

    public class CustomerXmlDal:ICustomerDAL  // make it as ICustomerDAl calass parent for this class and impliment the methods and write database coding here
    {
        DataSet ds;
        public CustomerXmlDal()
        {
            ds = new DataSet();
            ds.ReadXml("Customers.Xml");

            ds.Tables[0].PrimaryKey = new DataColumn[] { ds.Tables[0].Columns["Custid"] };
        }
        public List<Customer> Customers_Select()
        {
            List<Customer> customers = new List<Customer>();
            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                Customer odj = new Customer
                {
                    Custid = Convert.ToInt32(dr["Custid"]),
                    Name = (string)dr["Name"],
                    Balance = Convert.ToDecimal(dr["Balance"]),
                    City = (string)dr["City"],
                    Status = Convert.ToBoolean(dr["Status"])
                };
                customers.Add(odj);
            }
            return customers;


        }

        public Customer Customer_Select(int Custid)
        {
            // find record bade on id
            DataRow dr = ds.Tables[0].Rows.Find(Custid);

            // convertin data in to a Customer type.
            Customer customer = new Customer
            {
                Custid = Convert.ToInt32(dr["Custid"]),
                Name = Convert.ToString(dr["Name"]),
                Balance = Convert.ToDecimal(dr["Balance"]),
                City = Convert.ToString(dr["City"]),
                Status = Convert.ToBoolean(dr["Status"])
            };
            return customer;
        }


        public void Customer_Insert(Customer customer)
        {
            // add a row into a file based on the existing signature.
            DataRow dr = ds.Tables[0].NewRow();

            // add old value with new values
            dr["Custid"] = customer.Custid;
            dr["Name"] = customer.Name;
            dr["City"] = customer.City;
            dr["Balance"] = customer.Balance;
            dr["Status"] = customer.Status;

            //modified row add with tabele or file.
            ds.Tables[0].Rows.Add(dr);
            ds.WriteXml("Customers.Xml");
        }

        public void Customer_Update(Customer customer)
        {
            DataRow dr = ds.Tables[0].Rows.Find(customer.Custid);

            int index = ds.Tables[0].Rows.IndexOf(dr);

            ds.Tables[0].Rows[index]["Name"]=customer.Name;
            ds.Tables[0].Rows[index]["City"]=customer.City;
            ds.Tables[0].Rows[index]["Balance"] =customer.Balance;

            ds.WriteXml("Customers.Xml");

        }

        public void Customer_Delete(int Custid)
        {
            DataRow dr = ds.Tables[0].Rows.Find(Custid);

            ds.Tables[0].Rows.Remove(dr);

            ds.WriteXml("Customers.Xml");
        }


    }
}

