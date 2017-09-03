using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Tinygubackend.Models;

namespace Tinygubackend.Controllers
{
  [Route("api/[controller]")]
  public class ValuesController : Controller
  {
    private TinyguContext _tinyguContext;
    public ValuesController(TinyguContext tinyguContext)
    {
      _tinyguContext = tinyguContext;
    }
    
    // GET api/values
    [HttpGet]
    public string Get()
    {
      var query = _tinyguContext.Users.Where(u => u.Name == "Lars");
      foreach (User user in query)
      {
      }
      return "No links";
    }

    // GET api/values/5
    [HttpGet("{id}")]
    public string Get(int id)
    {
      return "value";
    }

    // POST api/values
    [HttpPost]
    public void Post([FromBody] string value)
    {
    }

    // PUT api/values/5
    [HttpPut("{id}")]
    public void Put(int id, [FromBody] string value)
    {
    }

    // DELETE api/values/5
    [HttpDelete("{id}")]
    public void Delete(int id)
    {
    }
  }
}
