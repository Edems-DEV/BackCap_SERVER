using Microsoft.AspNetCore.Mvc;
using Server.DatabaseTables;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Server.Controllers;
[Route("api/[controller]")]
[ApiController]
public class Test : ControllerBase
{
    private readonly MyContext context = new MyContext();
    // GET: api/<Test>
    [HttpGet]
    public List<User> Get()
    {
        List<User> users = new List<User>();
        users = context.User.ToList();
        return users;
    }

    // GET api/<Test>/5
    [HttpGet("{id}")]
    public string Get(int id)
    {
        return "value";
    }

    // POST api/<Test>
    [HttpPost]
    public void Post([FromBody] User user)
    {
        context.User.Add(user);
        context.SaveChanges();
    }

    // PUT api/<Test>/5
    [HttpPut("{id}")]
    public void Put(int id, [FromBody] string value)
    {
    }

    // DELETE api/<Test>/5
    [HttpDelete("{id}")]
    public void Delete(int id)
    {
    }
}
