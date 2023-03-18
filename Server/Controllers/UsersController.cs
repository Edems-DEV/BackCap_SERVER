using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Server.DatabaseTables;

namespace Server.Controllers;
[Route("api/[controller]")]
[ApiController]
public class UsersController : ControllerBase
{
    private readonly MyContext context = new MyContext();
    
    // GET: api/users?limit=25&offset=50
    [HttpGet]
    public List<User> Get(int limit = 10, int offset = 0)
    {
        var people = context.User
            //.Include(x => x.Group)
            //.OrderBy(p => p.Id)
            .Skip(offset)
            .Take(limit)
            .ToList();
        return people;
    }

    // GET: for stats
    [HttpGet("count")]
    public IActionResult GetCount()
    {
        return Ok(context.User.Count()); //idk if it works
    }

    // GET api/users/5
    [HttpGet("{id}")]
    public User Get(int id)
    {
        return context.User.Find(id);
    }

    // POST api/users
    [HttpPost]
    public void Post([FromBody] string value)
    {
    }

    // PUT api/users/5
    [HttpPut("{id}")]
    public void Put(int id, [FromBody] string value)
    {
    }

    // DELETE api/users/5
    [HttpDelete("{id}")]
    public void Delete(int id)
    {
    }
}

// return data from database