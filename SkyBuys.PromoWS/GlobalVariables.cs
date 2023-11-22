﻿namespace SkyBuys.PromoWS
{
    public class GlobalVariables
    {
        public int Interval { get; set; }
        public string LogFilePath { get; set; }
        public string SkyBuysFilePath { get; set; }
        public string SkyBuysPromFileName { get; set; }
        public string SkyBuysApiBaseUrl { get; set; }
        public string SkyBuysApiLoginEndpoint { get; set; }
        public string SkyBuysOffersEndpoint { get; set; }
        public string SkyBuysApiLoginName { get; set; }
        public string SkyBuysApiPassword { get; set; }
        public string RunOnScheduledTimes { get; set; }
        public string ExcludeTimeRange { get; set; }
    }


    public static class GlobalStaticVaiables
    {
        public static string DbConnectionString;
        public static int Interval { get; set; }
        public static string LogFilePath { get; set; }
        public static string SkyBuysFilePath { get; set; }
        public static string SkyBuysPromFileName { get; set; }
        public static string SkyBuysApiBaseUrl { get; set; }
        public static string SkyBuysApiLoginEndpoint { get; set; }
        public static string SkyBuysOffersEndpoint { get; set; }
        public static string SkyBuysApiLoginName { get; set; }
        public static string SkyBuysApiPassword { get; set; }
        public static string[] RunOnScheduledTimes { get; set; }
        public static string[] ExcludeTimeRange { get; set; }
    }
}

