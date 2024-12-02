using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using webapp.Model;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;
using System.Linq;
using System;

namespace webapp.Controllers
{
    public class HomeController : Controller
    {
        private readonly UserContext _context;
        private readonly ILogger<HomeController> _logger;

        public HomeController(UserContext context, ILogger<HomeController> logger)
        {
            _context = context;
            _logger = logger;
        }

      
        public async Task<IActionResult> Index()
        {
            var users = await _context.Users.ToListAsync();
            return View(users);
        }

      
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Id == id);
            if (user != null)
            {
                _context.Users.Remove(user);
                await _context.SaveChangesAsync();
                TempData["Message"] = "User deleted successfully!";
                TempData["MessageType"] = "success";
            }
            else
            {
                TempData["Message"] = "User not found.";
                TempData["MessageType"] = "error";
            }

            return RedirectToAction(nameof(Index));
        }



        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddUser(User user)
        {
            _logger.LogInformation("AddUser POST action invoked.");

          
            if (ModelState.IsValid)
            {
                try
                {
                    _logger.LogInformation("ModelState is valid. Adding user...");

                   
                    if (string.IsNullOrEmpty(user.Postal_Code))
                    {
                        user.Postal_Code = null; 
                    }

                   
                    _context.Users.Add(user);
                    await _context.SaveChangesAsync();

                   
                    TempData["Message"] = "User added successfully!";
                    TempData["MessageType"] = "success";

                   
                    return RedirectToAction(nameof(Index));
                }
                catch (DbUpdateException dbEx)
                {
                    
                    _logger.LogError($"Database error: {dbEx.Message}");
                    TempData["Message"] = "Error adding user to the database: " + dbEx.Message;
                    TempData["MessageType"] = "error";
                }
                catch (Exception ex)
                {
                   
                    _logger.LogError($"Error adding user: {ex.Message}");
                    TempData["Message"] = "Error adding user to the database: ";
                    TempData["MessageType"] = "error";
                }
            }
            else
            {
               
                foreach (var error in ModelState.Values.SelectMany(v => v.Errors))
                {
                    _logger.LogError($"Model error: {error.ErrorMessage}");
                }

              
                TempData["Message"] = "There was an error adding the user.";
                TempData["MessageType"] = "error";
            }

            return View("Index", await _context.Users.ToListAsync());
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UpdateUser(User updatedUser)
        {
            if (!ModelState.IsValid)
            {
                TempData["Message"] = "There were validation errors. Please check your input.";
                TempData["MessageType"] = "error";
                return RedirectToAction(nameof(Index)); 
            }

            var user = await _context.Users.FirstOrDefaultAsync(u => u.Id == updatedUser.Id);
            if (user == null)
            {
                TempData["Message"] = "User not found.";
                TempData["MessageType"] = "error";
                return RedirectToAction(nameof(Index));
            }

           
            user.Name = updatedUser.Name;
            user.Email = updatedUser.Email;
            user.City = updatedUser.City;
            user.Region = updatedUser.Region;
            user.Country = updatedUser.Country;
            user.Postal_Code = updatedUser.Postal_Code;
            user.Phone = updatedUser.Phone;
            user.Address = updatedUser.Address;

            try
            {
                await _context.SaveChangesAsync();
                TempData["Message"] = "User updated successfully!";
                TempData["MessageType"] = "success";
            }
            catch (Exception ex)
            {
               
                Console.WriteLine($"Error updating user: {ex.Message}");
                TempData["Message"] = "There was an error while updating the user.";

                TempData["MessageType"] = "error";
            }

            foreach (var error in ModelState.Values.SelectMany(v => v.Errors))
            {
                _logger.LogError($"Validation error: {error.ErrorMessage}");
                Console.WriteLine($"Validation error: {error.ErrorMessage}");
            }


            return RedirectToAction(nameof(Index));
        }
    }
    }
