using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SkyBuys.PLUImportWS
{
    public class GlobalVariables
    {
        public int Interval { get; set; }
        public string LogFilePath { get; set; }
        public string FtpUrl { get; set; }
        public string FtpFileName { get; set; }
        public string FileExtractionTimes { get; set; }
        public string FtpUserId { get; set; }
        public string FtpPassword { get; set; }
        public string Domain { get; set; }
        public string XMLFIlePath { get; set; }
    }

    public static class GlobalStaticVaiables
    {
        public static string DbConnectionString;
        public static int Interval { get; set; }
        public static string LogFilePath { get; set; }
        public static string FtpUrl { get; set; }
        public static string FtpFileName { get; set; }
        public static string[] FileExtractionTimes { get; set; }
        public static string FtpUserId { get; set; }
        public static string FtpPassword { get; set; }
        public static string Domain { get; set; }
        public static string XMLFIlePath { get; set; }
    }
}

