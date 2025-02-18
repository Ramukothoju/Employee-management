using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using MVCDHProject.Models;
using System.Text;
using MailKit.Net.Smtp;
using MimeKit;
using System.Security.Claims;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion.Internal;

namespace MVCDHProject.Controllers
{
    public class AccountController : Controller
    {
        public UserManager<IdentityUser> UserManager;  //creating user in persistence store
        public SignInManager<IdentityUser> SignInManager;// works for Login and logout and signin etc works done here 
        public AccountController(UserManager<IdentityUser> userManager, SignInManager<IdentityUser> signInManager)
        {
            UserManager = userManager;
            SignInManager = signInManager;
        }

        public IActionResult Register()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Register(UserModel userModel)
        {
            if (ModelState.IsValid)
            {

                // like Model Class to intaract to the database this will created by Identity faramework.
                IdentityUser identityUser = new IdentityUser
                {

                    UserName = userModel.Name,
                    Email = userModel.Email,
                    PhoneNumber = userModel.Mobile
                };
                // This Method will creates a record in the database 
                var result = await UserManager.CreateAsync(identityUser, userModel.Password);
                if (result.Succeeded)
                {
                    // Send Mail In that mainly having 2 attributes Token and IdUser

                    // Generating the token for Confirm Email 
                    var token = await UserManager.GenerateEmailConfirmationTokenAsync(identityUser);
                    // the Url.Action() will generates the total url for Email confirmation
                    var comfirUrlLink = Url.Action("ConfirmEmail", "Account", new { Userid = identityUser.Id, Token = token }, Request.Scheme);

                    SendMail(identityUser, comfirUrlLink, "Email Confirmation Link");
                    TempData["Title"] = "Email Confirmation Link";
                    TempData["Message"] = "A confirm email link has been sent to your registered mail, click on it to confirm";
                    return View("DisplayMessage");
                }
                else
                {
                    foreach (var Error in result.Errors)
                    {
                        ModelState.AddModelError("", Error.Description);
                    }
                }
            }
            return View(userModel);
        }

        public IActionResult Login()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Login(LoginModel loginModel)
        {
            if (ModelState.IsValid)
            {
                var user = await UserManager.FindByNameAsync(loginModel.Name);// Find a user in the server 
                if (user != null && (await UserManager.CheckPasswordAsync(user, loginModel.Password)) && user.EmailConfirmed == false)
                {
                    // checking the user Email confirmed or not if not show the below message to the user
                    ModelState.AddModelError("", " Your Email is not Confirmed");
                    return View(loginModel);
                }
                var result = await SignInManager.PasswordSignInAsync(loginModel.Name, loginModel.Password, loginModel.RememberMe, false);
                if (result.Succeeded)
                {
                    if (string.IsNullOrEmpty(loginModel.ReturnUrl))
                    {
                        return RedirectToAction("Index", "Home");
                    }
                    else
                    {
                        return LocalRedirect(loginModel.ReturnUrl);
                    }
                }
                else
                {
                    ModelState.AddModelError("", "Invalid Login Creadencials");
                }
            }
            return View(loginModel);
        }


        public async Task<IActionResult> Logout()
        {
            await SignInManager.SignOutAsync();
            return RedirectToAction("Login");
        }

        public void SendMail(IdentityUser identityUser, string requestLink, string subject)
        {
            // string used for Generic Text formate.
            StringBuilder mailbody = new StringBuilder();
            // below code will Disgn for Email page in user tab 
            mailbody.Append("Hellow" + identityUser.UserName + "</br></br>");
            if (subject == "Email Confirm Link")
                mailbody.Append("Click on the Link to Confirm the Email");
            else if (subject == "Change Password Link")
                mailbody.Append("Click on the Link to change the Password");
            mailbody.Append("<br />");
            mailbody.Append(requestLink);
            mailbody.Append("<br /><br /> ");
            mailbody.Append("Regards");
            mailbody.Append("<br /><br />");
            mailbody.Append("Customer Support.");


            // the above html page will ading to the class BodyBuilder. In this class having  Html Body
            BodyBuilder bodyBuilder = new BodyBuilder();
            bodyBuilder.HtmlBody = mailbody.ToString();

            //another class MailboxAdress having Fromaddress and toaddress
            MailboxAddress fromAddress = new MailboxAddress("CustomerSupport", "kothojuramu11@gmail.com");
            MailboxAddress toAddress = new MailboxAddress(identityUser.UserName, identityUser.Email);

            //Minemessage will playsMain role in thsi haiving combining fo all the above 
            MimeMessage mailMessage = new MimeMessage();
            mailMessage.From.Add(fromAddress);
            mailMessage.To.Add(toAddress);
            mailMessage.Subject = subject;
            mailMessage.Body = bodyBuilder.ToMessageBody();


            // we want to send a mile to ant one we have a smtp server by default the Email will provides the Smtp server 
            // for that we have to call the coneect() to coneect the smtp server
            SmtpClient smtpClient = new SmtpClient();
            smtpClient.Connect("smtp.gmail.com", 465, true);
            smtpClient.Authenticate("kothojuramu11@gmail.com", "itfg hxdh itgt gsue");// we need to authenticate our Account in Email settigs and genrates a password, take that pass and pass to this method
            smtpClient.Send(mailMessage);// finally send() will sends mail to aproprieate user thorw readig of MimeMessage class vlaue;
        }


        public async Task<IActionResult> ConfirmEmail(string userid, string token)
        {
            if (userid != null && token != null)
            {
                var user = await UserManager.FindByIdAsync(userid);
                if (userid != null)
                {
                    var result = await UserManager.ConfirmEmailAsync(user, token);
                    if (result.Succeeded)
                    {
                        TempData["Title"] = "Email Confirmation Success.";
                        TempData["Message"] = "Email confirmation is completed. You can now login into the application.";
                        return View("DisplayMessage");
                    }
                    else
                    {
                        StringBuilder Errors = new StringBuilder();
                        foreach (var Error in result.Errors)
                        {
                            Errors.Append(Error.Description + "");
                        }
                        TempData["Title"] = "Confirmation Email Failure";
                        TempData["Message"] = Errors.ToString();
                        return View("DisplayMessage");

                    }
                }
                else
                {
                    TempData["Title"] = "Invalid User Id.";
                    TempData["Message"] = "User Id which is present in confirm email link is in-valid.";
                    return View("DisplayMessage");
                }
            }
            else
            {
                TempData["Title"] = "Invalid Email Confirmation Link.";
                TempData["Message"] = "Email confirmation link is invalid, either missing the User Id or Confirmation Token.";
                return View("DisplayMessage");
            }
        }


        public IActionResult ForgotPassword()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> ForgotPassword(ChangePasswordModel model)
        {
            if (ModelState.IsValid)
            {
                var user = await UserManager.FindByNameAsync(model.Name);
                if (user != null && await UserManager.IsEmailConfirmedAsync(user))
                {
                    var token = await UserManager.GeneratePasswordResetTokenAsync(user);
                    var confirmationUrlLink = Url.Action("ChangePassword", "Account", new { Userid = user.Id, Token = token }, Request.Scheme);
                    SendMail(user, confirmationUrlLink, "Change Password Link");

                    TempData["Title"] = "Change Password Link";
                    TempData["Message"] = "Change password link has been sent to your mail, click on it and change password.";
                    return View("DisplayMessage");
                }
                else
                {
                    TempData["Title"] = "Change Password Mail Generation Failed.";
                    TempData["Message"] = "Either the Username you have entered is in-valid or your email is not confirmed.";
                    return View("DisplayMessage");
                }
            }
            return View(model);
        }

        public IActionResult ChangePassword()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> ChangePassword(ResetPasswordModel model)
        {
            if (ModelState.IsValid)
            {
                var user = await UserManager.FindByIdAsync(model.UserId);
                if (user != null)
                {
                    var result = await UserManager.ResetPasswordAsync(user, model.Token, model.Password);
                    if (result.Succeeded)
                    {
                        TempData["Title"] = "Reset Password Success";
                        TempData["Message"] = "Your password has been reset successfully.";
                        return View("DisplayMessage");
                    }
                    else
                    {
                        foreach (var error in result.Errors)
                        {
                            ModelState.AddModelError("", error.Description);
                        }
                    }
                }
                else
                {
                    TempData["Title"] = "Invalid User";
                    TempData["Message"] = "No user exists with the given User Id.";
                    return View("DisplayMessage");
                }
            }
            return View(model);
        }

        public IActionResult ExternalLogin(string Provider, string ReturnUrl)
        {
            var url = Url.Action("Callback", "Account", new { returnUrl = ReturnUrl });
            var properties = SignInManager.ConfigureExternalAuthenticationProperties(Provider, url);
            return new ChallengeResult(Provider, properties);
        }

        public async Task<IActionResult> CallBack(string returnUrl)
        {
            if (string.IsNullOrEmpty(returnUrl))
            {
                returnUrl = "~/";
            }
            LoginModel model = new LoginModel();
            var info = await SignInManager.GetExternalLoginInfoAsync();
            if (info == null)
            {
                ModelState.AddModelError("", "Error loading external login information.");
                return View("Login", model);
            }

            // this method for signin the user if user is present return true or false
            var signInResult = await SignInManager.ExternalLoginSignInAsync(info.LoginProvider, info.ProviderKey, false, true);
            if (signInResult.Succeeded) // the user record is found in the local record then directly logins user in to the web page
            {
                return LocalRedirect(returnUrl);
            }
            else
            {
                // This method will get the value from the authenticated user Sepecific data like email
                var email = info.Principal.FindFirstValue(ClaimTypes.Email);
                if (email != null)
                {
                    //this method will check user is added or not in users tabel
                    var user = await UserManager.FindByEmailAsync(email);
                    if (user == null)
                    {
                        user = new IdentityUser
                        {
                            UserName = info.Principal.FindFirstValue(ClaimTypes.Email),
                            Email = info.Principal.FindFirstValue(ClaimTypes.Email),
                            PhoneNumber = info.Principal.FindFirstValue(ClaimTypes.MobilePhone),
                        };
                        var identityResult = await UserManager.CreateAsync(user);
                    }
                    await UserManager.AddLoginAsync(user, info);
                    await SignInManager.SignInAsync(user, false);
                    return LocalRedirect(returnUrl);
                }
                TempData["Title"] = "Error";
                TempData["Message"] = "Email claim not received from third party provided.";
                return RedirectToAction("DisplayMessage");
            }
        }
    }
}
