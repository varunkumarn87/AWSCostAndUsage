using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Amazon.CostExplorer;
using Amazon.CostExplorer.Model;
using AutoMapper;


namespace Advantsure.CostAndUsage
{
    public static class AWSCostAndUsage
    {
        [FunctionName("AWSCostAndUsage")]
        public static async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = null)] HttpRequest req,
            ILogger log)
        {
            log.LogInformation("application started..");
            try
            {
                string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
                log.LogInformation("requestBody : " + requestBody);
                GetCostAndUsageRequest costAndUsageRequest = JsonConvert.DeserializeObject<GetCostAndUsageRequest>(requestBody);                
                //GetCostAndUsageRequest costAndUsageRequest = BuildAWSCostAndUsageRequest(data);
                log.LogInformation("Build JSON Completed and AWS call initiated..");
                GetCostAndUsageResponse costAndUsageResponse = CallAWSCostAndUsageAPI(costAndUsageRequest);
                log.LogInformation("AWS call Completed..");

                CanonicalResponse canonicalresponse = CreateCanonicalData(costAndUsageResponse);
                
                log.LogInformation("Canonical response completed..");
                // Insert JSON Response into Cosmos DB            
                ProcessCosmosDB cosmosDB = new ProcessCosmosDB();
                log.LogInformation("DB initiated..");
                await cosmosDB.GetStartedDemoAsync(canonicalresponse);
                log.LogInformation("processed response completed..");
                return new OkObjectResult(costAndUsageResponse);
            }
            catch(Exception ex)
            {
                log.LogInformation("Error occured in AWS Cost and Usage "+ex.Message.ToString());
                return new BadRequestObjectResult(ex.Message.ToString());
            }
        }

        //private static GetCostAndUsageRequest BuildAWSCostAndUsageRequest(object data)
        //{

        //    GetCostAndUsageRequest costAndUsageRequest = new GetCostAndUsageRequest();
        //    //string jsonString = File.ReadAllText(@"costandusageresquest.json");
        //    costAndUsageRequest = JsonConvert.DeserializeObject<GetCostAndUsageRequest>(data.ToString());
        //    return costAndUsageRequest;
        //}
        private static GetCostAndUsageResponse CallAWSCostAndUsageAPI(GetCostAndUsageRequest costAndUsageRequest)
        {
            
            var client = new AmazonCostExplorerClient(
                awsAccessKeyId: Environment.GetEnvironmentVariable("awsAccessKeyId"),
                awsSecretAccessKey: Environment.GetEnvironmentVariable("awsSecretAccessKey"),
                Amazon.RegionEndpoint.USEast1);           
            
                GetCostAndUsageResponse costAndUsageResponse = client.GetCostAndUsageAsync(costAndUsageRequest).Result;
                
                return costAndUsageResponse;     
        }

        private static CanonicalResponse CreateCanonicalData(GetCostAndUsageResponse awsResponse)
        {
            var config = new MapperConfiguration(cfg => { cfg.CreateMap<Amazon.CostExplorer.Model.ResultByTime, ResultByTime>();
                                                          cfg.CreateMap<Amazon.CostExplorer.Model.DateInterval, DateInterval>();
                                                          cfg.CreateMap<Amazon.CostExplorer.Model.MetricValue, MetricValue>();
                                                          cfg.CreateMap<Amazon.CostExplorer.Model.Group, Group>();
                                                          cfg.CreateMap<Amazon.Runtime.ResponseMetadata, ResponseMetadata>();                                                          
                                                          cfg.CreateMap<GetCostAndUsageResponse, CanonicalResponse>(); });
            IMapper mapper = config.CreateMapper();
            CanonicalResponse genericUsage = mapper.Map<GetCostAndUsageResponse, CanonicalResponse>(awsResponse);
            genericUsage.id = Guid.NewGuid();
           // genericUsage.id = awsResponse.ResponseMetadata.RequestId;
            genericUsage.Company = "Advantasure";
            genericUsage.Product = "Symphony";
            genericUsage.Provider = "AWS";
            return genericUsage;

        }
    }
}
