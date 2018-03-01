using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;

namespace Djm.OGame.Web.Api.Controllers
{
    [Route("api/[controller]")]
    public class ValuesController : Controller
    {
        public static readonly List<string> Values = new List<string>()
        {
            "value1",
            "value2",
            "value3",
        };

        // GET api/values
        [HttpGet]
        public IEnumerable<string> Get()
        {
            return Values;
        }

        // GET api/values/5
        [HttpGet("{id}")]
        public string Get(int id)
        {
            return Values[id];
        }

        // POST api/values
        [HttpPost]
        public IActionResult Post([FromBody]CreateValueBindingModel bindingModel)
        {
            if (bindingModel == null)
                return BadRequest("Body empty");

            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            
            Values.Add(bindingModel.Value);

            return Created(Url.Action("Get", new { id = Values.Count - 1}), bindingModel.Value);
        }

        // PUT api/values/5
        [HttpPut("{id:int}")]
        public IActionResult Put(int _id, [FromBody]CreateValueBindingModel bindingModel)
        {
            if (bindingModel == null)
                return BadRequest("Body empty");

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            Values[_id] = bindingModel.Value;

            return Created(Url.Action("Get", new { id = _id }), bindingModel.Value);
        }

        // DELETE api/values/5
        [HttpDelete("{id:int}")]
        public IActionResult Delete(int id)
        {
            Values.RemoveAt(id);

            return NoContent();
        }
    }

    public class CreateValueBindingModel
    {
        [Required]
        [MinLength(4)]
        public string Value { get; set; }
    }
}
