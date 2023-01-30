using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TodoAPI_JwtAuth.Entities;
using TodoAPI_JwtAuth.Models;

namespace TodoAPI_JwtAuth.Controllers
{
    [Authorize]
    [Route("[controller]/[action]")]
    [ApiController]
    public class TodoController : ControllerBase
    {
        private DatabaseContext _databaseContext;

        public TodoController(DatabaseContext databaseContext)
        {
            _databaseContext = databaseContext;
        }

        [AllowAnonymous]

        [HttpGet]
        public IActionResult List()
        {
            return Ok(_databaseContext.Todos.ToList());
        }

        [AllowAnonymous]
        [HttpGet("{count}/{order}")]
        public IActionResult GetList(int count, string order)
        {
            List<TodoItem> items = null;

            if (order == "asc")
            {
                items = _databaseContext.Todos.OrderBy(x => x.Id).Take(count).ToList();
            }
            else if (order == "desc")
            {
                items = _databaseContext.Todos.OrderByDescending(x => x.Id).Take(count).ToList();
            }
            else
            {
                items = _databaseContext.Todos.Take(count).ToList();
            }

            return Ok(items);
        }

        [HttpGet("{id}")]
        public IActionResult GetById(int id)
        {
            TodoItem item = _databaseContext.Todos.Find(id);

            if (item == null)
            {
                return NotFound(id);
            }

            return Ok(item);
        }

        [Authorize(Roles ="admin")]
        [HttpPost]
        public IActionResult Create(TodoCreateModel model)
        {
            if (model.Text == "string")
            {
                return BadRequest(model); //status code:400
            }

            TodoItem item = new TodoItem();

            item.Text = model.Text;
            item.Issue = model.Issue;
            item.DueDate = model.DueDate;
            item.Completed = false;


            _databaseContext.Todos.Add(item);
            _databaseContext.SaveChanges();

            return Created("", item);     //status code:201


        }
        [Authorize(Roles = "admin")]
        [HttpPut("{id}")]
        public IActionResult Edit([FromRoute] int id, [FromBody] TodoUpdateModel model)
        {
            TodoItem item = _databaseContext.Todos.Find(id);
            if (item == null)
            {
                return NotFound(id);
            }

            item.Text = model.Text;
            item.Issue = model.Issue;
            item.DueDate = model.DueDate;
            item.Completed = model.Completed;

            _databaseContext.SaveChanges();
            return Ok(item);
        }

        [Authorize(Roles = "admin")]
        [HttpDelete("{id}")]
        public IActionResult Remove([FromRoute] int id)
        {
            TodoItem item = _databaseContext.Todos.Find(id);
            if (item == null)
            {
                return NotFound(id);
            }

            _databaseContext.Todos.Remove(item);
            _databaseContext.SaveChanges();

            return Ok();
        }

    }
}
