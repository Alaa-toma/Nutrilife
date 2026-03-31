using firstapi.Data;
using firstapi.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace firstapi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        ApplicationDbContext context = new ApplicationDbContext();

        [HttpGet()]
        public IActionResult GetAll()
        {
            var users = context.Users.ToList();
            return Ok(new {message="done", users});
        }

        [HttpGet("{id}")]
        public IActionResult Getbyid(int id) {
            var user = context.Users.Find(id);
            return Ok(user);
        }

        [HttpDelete("{id}")]
        public IActionResult delete(int id)
        {
            var user = context.Users.Find(id);
            context.Users.Remove(user);
            context.SaveChanges();
            return Ok(user);
        }
        [HttpPatch("{id}")]
        public IActionResult update(User request, int id)
        {
            var user = context.Users.Find(id);
            if (user is not null) {
                request.Id = id;
                user.Name=request.Name;
                user.Email=request.Email;
                context.SaveChanges();

                return Ok(new { message = "done", request });
            }
            else
            {
                return BadRequest();
            }
           

        }


        [HttpPost()]
        public IActionResult create(User request)
        {
            context.Users.Add(request);
            context.SaveChanges();
            return Ok();
        }
    }
}
