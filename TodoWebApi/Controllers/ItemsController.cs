using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TodoWebApi.Models;
using TodoWebApi.Services;

namespace TodoWebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ItemsController : ControllerBase
    {
        private readonly ICosmosDbService _cosmosDbService;
        public ItemsController(ICosmosDbService cosmosDbService)
        {
            _cosmosDbService = cosmosDbService;
        }
        [HttpGet]
        [ActionName("GetAllItems")]
        public Task<IEnumerable<Item>> GetItems()
        {
            //  var items= _cosmosDbService.GetItems("SELECT * FROM c");
            return _cosmosDbService.GetItems("SELECT * FROM c");

        }

        [HttpGet("{id}")]
        [ActionName("GetSingleItem")]
        public async Task<ActionResult<Item>> GetItem(string id)
        {
            var todo = await _cosmosDbService.GetItem(id);
            if (todo == null)
            {
                return NotFound();
            }
            else return todo;
        }

        [HttpPost]
        public async Task<ActionResult<Item>> PostItem(Item item)
        {
            if (await _cosmosDbService.InsertItem(item))
            {
                return NoContent();
            }
            return BadRequest();
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> EditItem(string id,Item item)
        {
            if (id != item.Id)
            {
                return BadRequest();
            }
            await _cosmosDbService.EditItem(id, item);
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteItem(string id) {
             if(await _cosmosDbService.DeleteItem(id))
            {
                return NoContent();
            }
            return NotFound();
        }
    }
}