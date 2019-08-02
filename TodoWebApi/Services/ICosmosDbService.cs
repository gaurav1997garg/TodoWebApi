using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Cosmos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TodoWebApi.Models;

namespace TodoWebApi.Services
{
    public interface ICosmosDbService
    {
        Task<IEnumerable<Item>> GetItems(string query);
        Task<ActionResult<Item>> GetItem(string id);
        Task<bool> InsertItem(Item item);
        Task EditItem(string id, Item item);
        Task<bool> DeleteItem(string id);
     //   Item GetItem(string Id);
     //   Item Create(Item item);
     //   Item Update(string id,Item item);

     // Task Delete(string id);
    }
}
