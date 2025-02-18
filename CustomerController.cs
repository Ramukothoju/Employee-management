using Microsoft.AspNetCore.Mvc;
using MVCDHProject.Models;
using Microsoft.AspNetCore.Authorization;
namespace MVCDHProject.Controllers
{
    [Authorize]
    public class CustomerController : Controller
    {
        // replace the code with Dal code to InterFace code 

        ICustomerDAL cx;  // creatting the instance of the service class instace
        public CustomerController(ICustomerDAL cx)  // creating the construncter of the customer controller and initializes the parent class instace
        {
            this.cx = cx;
        }

        [AllowAnonymous]
        public ViewResult DisplayCustomers()
        {
            return View(cx.Customers_Select());
        }
        [AllowAnonymous]
        public ViewResult DisplayCustomer(int Custid)
        {
            return View(cx.Customer_Select(Custid));
        }

        public ViewResult AddCustomer()
        {
            return View();
        }
        [HttpPost]
        public RedirectToActionResult AddCustomer(Customer customer)
        {
            cx.Customer_Insert(customer);
            return RedirectToAction("DisplayCustomers");
        }

        public ViewResult EditCustomer(int Custid)
        {
           
            return View(cx.Customer_Select(Custid));
        }
        
        public RedirectToActionResult UpdateCustomer(Customer customer)
        {
            cx.Customer_Update(customer);
            return RedirectToAction("DisplayCustomers");
        }

        public RedirectToActionResult DeleteCustomer(int Custid)
        {
            cx.Customer_Delete(Custid);
            return RedirectToAction("DisplayCustomers");
        }
    }
}
