namespace MVCDHProject.Models
{
    public interface ICustomerDAL
    { // class defin for Consuming of the services class data
        // this class body will develop in XML and SQL and ORACle classes
        List<Customer> Customers_Select();
        Customer Customer_Select(int Custid);
        void Customer_Insert(Customer customer);
        void Customer_Update(Customer customer);
        void Customer_Delete(int Custid);
    }
}
