using Course.Models;
using Course.Services.Contarcts;
using Microsoft.AspNetCore.Mvc;

namespace Course.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UsersController : ControllerBase
    {
        private readonly IUserService _userService;
        public UsersController(IUserService userService) => _userService = userService;

        [HttpGet]
        public IActionResult GetAll() => Ok(_userService.GetAllUsers());

        [HttpGet("{id}")]
        public IActionResult GetById(string id)
        {
            var user = _userService.GetUserById(id);
            if (user == null) return NotFound();
            return Ok(user);
        }

        [HttpPost]
        public IActionResult Create([FromBody] User user)
        {
            _userService.AddUser(user);
            return CreatedAtAction(nameof(GetById), new { id = user.Id }, user);
        }

        [HttpPut("{id}")]
        public IActionResult Update(string id, [FromBody] User user)
        {
            _userService.UpdateUser(id, user);
            return NoContent();
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(string id)
        {
            _userService.DeleteUser(id);
            return NoContent();
        }
    }
}
