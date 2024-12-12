using Gift_Purchase_Store.Data;
using Gift_Purchase_Store.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Gift_Purchase_Store.Controllers
{
    [Authorize(Roles = "Customer")]
    public class ShippingAddressController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public ShippingAddressController(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        // List all saved addresses
        public async Task<IActionResult> AddressIndex()
        {
            var userId = _userManager.GetUserId(User);
            var addresses = await _context.ShippingAddresses
                .Where(a => a.UserId == userId)
                .ToListAsync();

            return View("AddressIndex", addresses);
        }

        // Create or edit an address
        [HttpGet]
        public async Task<IActionResult> Save(int? id)
        {
            if (id == null || id == 0) // Add new address
            {
                return View("AddressForm", new ShippingAddress());
            }

            // Edit existing address
            var address = await _context.ShippingAddresses.FindAsync(id);
            if (address == null || address.UserId != _userManager.GetUserId(User))
            {
                return Unauthorized();
            }

            return View("AddressForm", address);
        }

        [HttpPost]
        public async Task<IActionResult> Save(ShippingAddress address)
        {
            if (!ModelState.IsValid)
            {
                return View("AddressForm", address);
            }

            var userId = _userManager.GetUserId(User);

            if (address.ShippingAddressId == 0) // Add new address
            {
                address.UserId = userId;
                _context.ShippingAddresses.Add(address);
            }
            else // Edit existing address
            {
                var existingAddress = await _context.ShippingAddresses.FindAsync(address.ShippingAddressId);
                if (existingAddress == null || existingAddress.UserId != userId)
                {
                    return Unauthorized();
                }

                existingAddress.AddressLine1 = address.AddressLine1;
                existingAddress.City = address.City;
                existingAddress.State = address.State;
                existingAddress.ZipCode = address.ZipCode;
                existingAddress.PhoneNumber = address.PhoneNumber;

                _context.ShippingAddresses.Update(existingAddress);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction("AddressIndex");
        }

        // Delete an address
        public async Task<IActionResult> Delete(int id)
        {
            var address = await _context.ShippingAddresses.FindAsync(id);
            if (address == null || address.UserId != _userManager.GetUserId(User))
            {
                return Unauthorized();
            }

            _context.ShippingAddresses.Remove(address);
            await _context.SaveChangesAsync();

            return RedirectToAction("AddressIndex");
        }
    }
}
