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

    // GET: api/users?limit=25&offset=50&orderBy=id&isAscending=false
    [HttpGet]
    public IActionResult Get(int limit = 10, int offset = 0, string orderBy = null, bool isAscending = true)
    {
        List<User> query;
        if (orderBy != null)
        {
            query = isAscending ?
                   context.User.OrderBy(s => s.GetType().GetProperty(orderBy).GetValue(s)).ToList() :
                   context.User.OrderByDescending(s => s.GetType().GetProperty(orderBy).GetValue(s)).ToList();
            query = query
                    .Skip(offset)
                    .Take(limit)
                    .ToList();
        }
        else
        {
            query = context.User
                .Skip(offset)
                .Take(limit)
                .ToList();
        }

        if (query == null || query.Count == 0)
        {
            return NoContent(); //204
        }

        return Ok(query); //200
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
    public IActionResult Delete(int id)
    {
        var user = context.User.Find(id);
        if (user == null)
        {
            return NotFound();
        }
        context.User.Remove(user);
        context.SaveChanges();
        return Ok($"Delete request received for user id {id}.");
    }
}

// return data from database
// -------------------------
// don't return Password !!!