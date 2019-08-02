using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Cosmos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TodoWebApi.Models;

namespace TodoWebApi.Services
{
    public class CosmosDbService :ICosmosDbService
    {
        private Container _container;

        public CosmosDbService(CosmosClient dbClient,string databaseName,string containerName)
        {
            this._container = dbClient.GetContainer(databaseName, containerName);
        }

        public  async Task<IEnumerable<Item>> GetItems(string queryString)
        {
            var query = this._container.GetItemQueryIterator<Item>(new QueryDefinition(queryString));
            List<Item> results = new List<Item>();
            while (query.HasMoreResults)
            {
                var response =await query.ReadNextAsync();
                results.AddRange(response.ToList());
            }
            return results;

        }

        public async Task<ActionResult<Item>> GetItem(string id)
        { ItemResponse<Item> response;
            try
            {
                response = await this._container.ReadItemAsync<Item>(id, new PartitionKey(id));
            }
            catch (Exception) { return null; }
            return response.Resource;
        }

        public async  Task<bool> InsertItem(Item item)
        {
            try
            {
                await this._container.CreateItemAsync<Item>(item, new PartitionKey(item.Id));
            }
            catch (Exception)
            {
                return false;
            }
            return true;
            
        }

        public async Task EditItem(string id,Item item)
        {
            ItemResponse<Item> response = await this._container.ReplaceItemAsync(item, id);

        }

        public async Task<bool> DeleteItem(string id)
        {
            ItemResponse<Item> response;
            try
            {
                response = await this._container.DeleteItemAsync<Item>(id, new PartitionKey(id));
            }
            catch (Exception)
            {
                return false;
            } 
             return true;
        }


    }
}

