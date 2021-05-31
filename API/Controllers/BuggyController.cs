using System.Runtime.InteropServices.ComTypes;
using API.Data;
using API.Entities;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    public class BuggyController : BaseApiController
    {
        public readonly DataContext _context;
        public BuggyController(DataContext context)
        {
            _context = context;
        }

         [HttpGet("auth")]
        public ActionResult<string> GetSecret(){
            
            return "Secret text";
        }


        [HttpGet("not-found")]
        public ActionResult<AppUser> GetNotFound(){
            var thing = _context.Users.Find(-1);

            if(thing == null) return NotFound();

            return Ok(thing);
        }

        [HttpGet("server-error")]
        public ActionResult<string> GetServerError(){
            var thing = _context.Users.Find(-1);
            var thingToReturn = thing.ToString();
            
            return thingToReturn;
        }

        [HttpGet("bad-request")]
        public ActionResult<AppUser> GetBadRequest(){
            return BadRequest("This is not good request");
        }
    }
}