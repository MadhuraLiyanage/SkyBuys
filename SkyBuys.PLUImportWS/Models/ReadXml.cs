using SkyBuys.Enum.Enum;
using SkyBuys.Models;
using System.Security.Principal;
using System.Xml;

namespace SkyBuys.PLUImportWS.Models
{
    public class ReadXml
    {
        public async Task<IEnumerable<ItemDefinition>> GetItemDefinition()
        {
            //Coping xml file
            TextLogger.LogToText(LoogerType.Information, "XML file coping started");
            if (!CopyXML(GlobalStaticVaiables.FtpUrl + GlobalStaticVaiables.FtpFileName, GlobalStaticVaiables.XMLFIlePath + GlobalStaticVaiables.FtpFileName,  GlobalStaticVaiables.Domain, GlobalStaticVaiables.FtpUserId, GlobalStaticVaiables.FtpPassword))
            {
                //error coping file
                TextLogger.LogToText(LoogerType.Error, "Error in coping XML file ");
            }

            TextLogger.LogToText(LoogerType.Information, "XML file reading started");

            bool addElement = false;

            //List<ItemDefinition> xmlItemDesfinitions = new List<ItemDefinition>();
            //XmlItemDesfinition xmlItemDesfinition = new XmlItemDesfinition();
            List<ItemDefinition> itemDefinitions = new List<ItemDefinition>();
            ItemDefinition itemDefinition = new ItemDefinition();
            try
            {
                using (XmlTextReader xmlReader = new XmlTextReader(GlobalStaticVaiables.XMLFIlePath + GlobalStaticVaiables.FtpFileName))
                {
                    Console.WriteLine(DateTime.Now);
                    string m;
                    while (xmlReader.Read())
                    {
                        m = xmlReader.ReadString();
                        if (m == $"\n")
                        {
                            if (itemDefinition.ItemNumber != null
                                && itemDefinition.ShortDescription != null && itemDefinition.MainCategory != null)
                            {
                                if (itemDefinition.LongDescription == null)
                                {
                                    itemDefinition.LongDescription = itemDefinition.ShortDescription;
                                }
                                if (itemDefinition.LongDescription == "NOT IN USE")
                                {
                                    itemDefinition.LongDescription = itemDefinition.ShortDescription;
                                }
                                itemDefinitions.Add(itemDefinition);
                                addElement = false;
                                itemDefinition = new ItemDefinition();
                            }
                        }
                        switch (xmlReader.Name.ToString())
                        {
                            case "ITEM_NUMBER":
                                itemDefinition.ItemNumber = m;
                                break;
                            case "DESCRIPTION":
                                itemDefinition.ShortDescription = m.Replace($"\n", "");
                                break;
                            case "LONG_DESCRIPTION":
                                itemDefinition.LongDescription = m;
                                break;
                            case "CATEGORY_CODE": //Main category
                                itemDefinition.MainCategory = m;
                                break;
                            case "ATTRIBUTE_CATEGORY": //sub category
                                itemDefinition.SubCategory = m;
                                break;
                            case "ATTRIBUTE5 ": //brand
                                itemDefinition.Brand = m;
                                break;
                            case "ATTRIBUTE7 ": //size
                                itemDefinition.ItemSize = m;
                                break;
                        }
                    }
                }
                TextLogger.LogToText(LoogerType.Information, $"Item definition count : {itemDefinitions.Count}");
                TextLogger.LogToText(LoogerType.Information, "XML file reading completed successfully");
                //group item definition oblect list
                /*
                public string ItemNumber { get; set; }
                public string ShortDescription { get; set; }
                public string LongDescription { get; set; }
                public string MainCategory { get; set; }
                public string SubCategory { get; set; }
                public string Brand { get; set; }
                public string ItemSize { get; set; }
                */
                /*

                var itemDefinitions = xmlItemDesfinitions
                        .GroupBy(itm => new { 
                            itm.ItemNumber, 
                            itm.ShortDescription,
                            itm.LongDescription,
                            itm.MainCategory,
                            itm.SubCategory,
                            itm.Brand,
                            itm.ItemSize
                        })
                    .Select(gItm => new ItemDefinition()
                        {
                            ItemNumber = gItm.Key.ItemNumber,
                            ShortDescription = gItm.Key.ShortDescription,
                            LongDescription = gItm.Key.LongDescription,
                            MainCategory = gItm.Key.MainCategory,
                            SubCategory = gItm.Key.SubCategory,
                            Brand = gItm.Key.Brand,
                            ItemSize = gItm.Key.ItemSize
                        }
                     );*/
                TextLogger.LogToText(LoogerType.Information, "XML file reading completed successfully");
                return (itemDefinitions);
            }
            catch(Exception ex)
            {
                TextLogger.LogToText(LoogerType.Error, $"Error reading XML. Exception : {ex.Message}");
                return new List<ItemDefinition>();
            }
        }


        private bool CopyXML(string sourceFilePath, string targetPath, string domain, string userID, string password)
        {
            try
            {
                using (WindowsLogin wi = new WindowsLogin(userID, domain, password))
                {
                    #if NET461
                        using (user.Impersonate())
                    #else
                        WindowsIdentity.RunImpersonated(wi.Identity.AccessToken, () =>
                    #endif
                {
                    WindowsIdentity useri = WindowsIdentity.GetCurrent();
                    File.Copy(sourceFilePath, targetPath, true);


                }
                    #if !NET461
                        );
                    #endif
                }
                return true;
            }
            catch(Exception ex)
            {
                TextLogger.LogToText(LoogerType.Error, $"Error coping XML file from {sourceFilePath}. Exception : {ex.Message}");
            }
            return false;
        }
    }
}
