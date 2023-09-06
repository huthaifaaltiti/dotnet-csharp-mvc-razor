using Microsoft.AspNetCore.Mvc;
using System;
using System.Net;
using System.IO;
using System.Diagnostics;
using System.Threading.Tasks;
using Newtonsoft.Json;
using WebUsersStat_MVC.Models;

namespace WebUsersStat_MVC.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Index(int NationalNumber)
        {
            try
            {
                string apiUrl = $"http://localhost/WebAPI/Api/users/{NationalNumber}";

                using (WebClient webClient = new WebClient())
                {
                    string responseData = await webClient.DownloadStringTaskAsync(new Uri(apiUrl));

                    User userModel = JsonConvert.DeserializeObject<User>(responseData);

                    if (userModel != null)
                    {
                        if (userModel.IsActive)
                        {
                            // Active user
                            userModel.IsActiveStatus = "Active user.";
                        }
                        else
                        {
                            // Inactive user
                            userModel.AvgSalary = 0;
                            userModel.UserSalaryStatus = "---";
                            userModel.LargestSalary = 0;
                            userModel.IsActiveStatus = "This user is no longer active.";
                        }

                        return View(userModel);
                    }
                    else
                    {
                        // User not found
                        userModel = new User
                        {
                            AvgSalary = 0,
                            UserSalaryStatus = "---",
                            LargestSalary = 0,
                            IsActiveStatus = "User not found"
                        };

                        return View(userModel);
                    }
                }
            }
            catch (WebException ex) when ((ex.Response as HttpWebResponse)?.StatusCode == HttpStatusCode.NotFound)
            {
                // (HTTP 404)
                User userModel = new User
                {
                    Username = "---",
                    NationalNumber= 0,
                    AvgSalary = 0,
                    UserSalaryStatus = "---",
                    LargestSalary = 0,
                    IsActiveStatus = "User not found"
                };

                return View(userModel);
            }
            catch (Exception ex)
            {
                ViewBag.ErrorMessage = "An error occurred while fetching data from the API: " + ex.Message;
                return View();
            }
        }


        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
