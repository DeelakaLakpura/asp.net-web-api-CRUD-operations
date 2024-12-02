using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Configuration;
using System.Linq;
using System.Threading.Tasks;
using webapp.Model;

namespace webapp.Controllers
{
    [ApiController]
    [Route("api/user")]
    public class UserController : ControllerBase
    {
        private readonly UserContext _context;

        public UserController(UserContext context)
        {
            _context = context;
        }

     
        [HttpGet]
        public async Task<IActionResult> GetUsers()
        {
            var users = await _context.Users.ToListAsync();
            return Ok(users); 
        }

    
        [HttpGet("{id}")]
        public async Task<IActionResult> GetUser(long id)
        {
            var user = await _context.Users.FindAsync(id);
            if (user == null)
            {
                return NotFound();
            }
            return Ok(user);  
        }

        
        [HttpPost]
        public async Task<IActionResult> CreateUser([FromBody] User user)
        {
            if (user == null)
            {
                return BadRequest();
            }

            _context.Users.Add(user);
            await _context.SaveChangesAsync();
            return CreatedAtAction(nameof(GetUser), new { id = user.Id }, user);  // Return created user
        }

        
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateUser(long id, [FromBody] User user)
        {
            if (id != user.Id)
            {
                return BadRequest("User ID mismatch.");
            }

            var existingUser = await _context.Users.FindAsync(id);
            if (existingUser == null)
            {
                return NotFound();
            }

           
            existingUser.Name = user.Name;
            existingUser.Email = user.Email;
            existingUser.Address = user.Address;
            existingUser.Phone = user.Phone;
            existingUser.Country = user.Country;
            existingUser.Postal_Code = user.Postal_Code;


            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!UserExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent(); 
        }

      
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUser(long id)
        {
            var user = await _context.Users.FindAsync(id);
            if (user == null)
            {
                return NotFound();
            }

            _context.Users.Remove(user);
            await _context.SaveChangesAsync();

            return NoContent(); 
        }

        private bool UserExists(long id)
        {
            return _context.Users.Any(e => e.Id == id);
        }
    }


    public class UserMvcController : Controller
    {
        private readonly UserContext _context;

        public UserMvcController(UserContext context)
        {
            _context = context;
        }

      
        public async Task<IActionResult> Index()
        {
            var users = await _context.Users.ToListAsync();
            if (users == null || !users.Any())
            {
               
                Console.WriteLine("No users found.");
            }
            return View(users);
        }



        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Delete(int id)
        {
            var user = _context.Users.Find(id);
            if (user == null)
            {
                TempData["Error"] = "User  not found.";
                return RedirectToAction("Index");
            }

            _context.Users.Remove(user);
            _context.SaveChanges();

            TempData["Success"] = "User  deleted successfully.";
            return RedirectToAction("Index");
        }


    }
}
