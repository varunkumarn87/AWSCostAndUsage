using System;
using Amazon.CostExplorer.Model;
using Amazon.CostExplorer;
using System.Text.Json;
using System.IO;
using System.Threading.Tasks;
using System.Collections.Generic;
using Newtonsoft.Json;
using Microsoft.Azure.Cosmos;
using Newtonsoft.Json.Linq;

namespace Advantsure.CostAndUsage
{
    public class ProcessCosmosDB
    {
        private static readonly string EndpointUri = Environment.GetEnvironmentVariable("CosmosDBURL");
        // The primary key for the Azure Cosmos account.
        private static readonly string PrimaryKey = Environment.GetEnvironmentVariable("CosmosDBPrimaryKey");

        // The Cosmos client instance
        private CosmosClient cosmosClient;

        // The database we will create
        private Database database;

        // The container we will create.
        private Container container;

        // The name of the database and container we will create
        private string databaseId = Environment.GetEnvironmentVariable("DatabaseId");
        private string containerId = Environment.GetEnvironmentVariable("ContainerId");
        // public static async Task Main(string[] args)
        // {
        //     try
        //     {
        //         Console.WriteLine("Beginning operations...\n");
        //         ProcessCosmosDB p = new ProcessCosmosDB();
        //         await p.GetStartedDemoAsync(response);

        //     }
        //     catch (CosmosException de)
        //     {
        //         Exception baseException = de.GetBaseException();
        //         Console.WriteLine("{0} error occurred: {1}", de.StatusCode, de);
        //     }
        //     catch (Exception e)
        //     {
        //         Console.WriteLine("Error: {0}", e);
        //     }
        //     finally
        //     {
        //         Console.WriteLine("Application Ended");                
        //     }
        // }
        public async Task GetStartedDemoAsync(CanonicalResponse canonicalresponse)
        {
            // Create a new instance of the Cosmos Client
            this.cosmosClient = new CosmosClient(EndpointUri, PrimaryKey);
            await this.CreateDatabaseAsync();

            //ADD THIS PART TO YOUR CODE
            await this.CreateContainerAsync();
            await this.AddItemsToContainerAsync(canonicalresponse);
        }
        private async Task CreateDatabaseAsync()
        {
            // Create a new database
            this.database = await this.cosmosClient.CreateDatabaseIfNotExistsAsync(databaseId);
            Console.WriteLine("Database Available: {0}\n", this.database.Id);
        }
        private async Task CreateContainerAsync()
        {
            // Create a new container
            this.container = await this.database.CreateContainerIfNotExistsAsync(containerId, "/id");
            Console.WriteLine("Container Available: {0}\n", this.container.Id);
        }
        private async Task AddItemsToContainerAsync(CanonicalResponse canonicalresponse)
        {            
            JObject js = JObject.Parse(JsonConvert.SerializeObject(canonicalresponse));                      
            var addjsontoCosmosResponse = await this.container.CreateItemAsync(js, new PartitionKey(js.GetValue("id").ToString()));
        }
    }

}

