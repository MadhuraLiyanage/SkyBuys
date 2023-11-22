using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SkyBuys.SohWS
{
    public class GlobalVariables
    {
        public int Interval { get; set; }
        public string LogFilePath { get; set; }
        public string SohURI { get; set; }
        public string SohMethod { get; set; }
        public string ApiToken { get; set; }
        public string OrganizationID { get; set; }
        public string SohExtractionTimes { get; set; }
    }


    public static class GlobalStaticVaiables
    {
        public static string DbConnectionString;
        public static int Interval { get; set; }
        public static string LogFilePath { get; set; }
        public static string SohURI { get; set; }
        public static string SohMethod { get; set; }
        public static string ApiToken { get; set; }
        public static string[] OrganizationID { get; set; }
        public static string[] SohExtractionTimes { get; set; }
    }
}

