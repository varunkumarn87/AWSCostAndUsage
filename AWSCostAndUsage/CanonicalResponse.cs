using System;
using System.Collections.Generic;
using Amazon.Runtime.Internal;
using System.Globalization;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
namespace Advantsure.CostAndUsage
{
    public partial class CanonicalResponse
    {
        [JsonProperty("id")]
        public Guid id { get; set; }

        [JsonProperty("Company")]
        public string Company { get; set; }

        [JsonProperty("Product")]
        public string Product { get; set; }

        [JsonProperty("Provider")]
        public string Provider { get; set; }

        //[JsonProperty("GroupDefinitions")]
        //public object[] GroupDefinitions { get; set; }

        //[JsonProperty("NextPageToken")]
        //public object NextPageToken { get; set; }

        [JsonProperty("ResultsByTime")]
        public List<ResultByTime> ResultsByTime { get; set; }

        [JsonProperty("ResponseMetadata")]
        public ResponseMetadata ResponseMetadata { get; set; }

            [JsonProperty("Resources")]
            public List<ResourceInfo> Resources { get; set; }

        }

    public partial class ResponseMetadata
    {
        [JsonProperty("RequestId")]
        public Guid RequestId { get; set; }

        [JsonProperty("Metadata")]
        public IDictionary<string, string> Metadata { get; set; }
    }

        public partial class ResourceInfo
    {
            [JsonProperty("RequestId")]
            public string Client { get; set; }

            [JsonProperty("Resource")]
            public string Resource { get; set; }

            [JsonProperty("Amount")]
            public string Amount { get; set; }
    }
   

    public class MetricValue
    {
        //
        // Summary:
        //     Gets and sets the property Amount.
        //     The actual number that represents the metric.
        public string Amount { get; set; }
        //
        // Summary:
        //     Gets and sets the property Unit.
        //     The unit that the metric is given in.
        public string Unit { get; set; }
    }

    public class Group
    {
        //
        // Summary:
        //     Gets and sets the property Keys.
        //     The keys that are included in this group.
        public List<string> Keys { get; set; }
        //
        // Summary:
        //     Gets and sets the property Metrics.
        //     The metrics that are included in this group.
        public Dictionary<string, MetricValue> Metrics { get; set; }
    }
    public class ResultByTime
    {
        [JsonProperty("Estimated")]
        public bool Estimated { get; set; }

        [JsonProperty("Groups")]
        public List<Group> Groups { get; set; }

        [JsonProperty("TimePeriod")]
        public DateInterval TimePeriod { get; set; }

        [JsonProperty("Total")]
        public Dictionary<string, MetricValue> Total { get; set; }
    }

    public class DateInterval
    {

        public string End { get; set; }

        public string Start { get; set; }
    }

    public partial class TimePeriod
    {
        [JsonProperty("End")]
        public DateTimeOffset End { get; set; }

        [JsonProperty("Start")]
        public DateTimeOffset Start { get; set; }
    }

    public partial class Total
    {
        [JsonProperty("BlendedCost")]
        public BlendedCost BlendedCost { get; set; }

        [JsonProperty("UnblendedCost")]
        public BlendedCost UnblendedCost { get; set; }

        [JsonProperty("UsageQuantity")]
        public BlendedCost UsageQuantity { get; set; }
    }

    public partial class BlendedCost
    {
        [JsonProperty("Amount")]
        public string Amount { get; set; }

        [JsonProperty("Unit")]
        public string Unit { get; set; }
    }
}
