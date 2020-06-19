using System;
using System.Collections.Generic;
using System.Text;

namespace AWSCostAndUsage
{
    public class TimePeriod
    {

        /// <summary>
        /// 
        /// </summary>
        public string Start { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string End { get; set; }
    }

    public class Tags
    {

        /// <summary>
        /// 
        /// </summary>
        public string Key { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public List<string> Values { get; set; }
    }

    public class DIMENSION
    {

        /// <summary>
        /// 
        /// </summary>
        public string Key { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public List<string> Values { get; set; }
    }

    public class Filter
    {

        /// <summary>
        /// 
        /// </summary>
        public Tags Tags { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public DIMENSION DIMENSION { get; set; }
    }

    public class GroupByItem
    {

        /// <summary>
        /// 
        /// </summary>
        public string Type { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string Key { get; set; }
    }

    public class CanonicalRequest
    {

        /// <summary>
        /// 
        /// </summary>
        public TimePeriod TimePeriod { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string Granularity { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public Filter Filter { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public List<GroupByItem> GroupBy { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public List<string> Metrics { get; set; }
    }


}
