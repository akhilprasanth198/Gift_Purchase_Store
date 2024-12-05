using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Gift_Purchase_Store.Controllers
{
    public class AdminController : Controller
    {
        [Authorize(Roles = "Admin")]
        
            public IActionResult Dashboard()
            {
                return View();
            }

            public IActionResult ManageUsers()
            {
                // Implement logic to manage users
                return View();
            }
        }

    }

