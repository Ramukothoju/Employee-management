
namespace MVCDHProject.Models
{
    public class CustomerSQLDal : ICustomerDAL
    {
        public MVCCoreDbContext context;   //In this case the context class is provideing the dependency services
        public CustomerSQLDal(MVCCoreDbContext contex)
        {
            this.context = contex;
        }
        public List<Customer> Customers_Select()
        {
            var customer = context.customers.Where(S => S.Status == true).ToList();
            return customer;
        }

        public Customer Customer_Select(int Custid)
        {
            return context.customers.Find(Custid);
        }


        public void Customer_Insert(Customer customer)
        {
            context.customers.Add(customer);
            context.SaveChanges();
        }

        public void Customer_Update(Customer customer)
        {
            customer.Status = true;
            context.customers.Update(customer);
            context.SaveChanges();
        }
        public void Customer_Delete(int Custid)
        {
            Customer c = context.customers.Find(Custid);
            c.Status = false;
            context.SaveChanges();
        }

     
    }
}
