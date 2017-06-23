using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Xml;

namespace VersionServer.Controllers
{
    public class UpdateItem
    {
        public bool isShouldUpdate { set; get; }
        public string FileListURL { get; set; }
        public string FTPURL { get; set; }
    }
    public class CheckUpdateController : ApiController
    {

        public UpdateItem GetCheck(string appName,string currentVersion)
        {
            UpdateItem tmpUpdateItem = new UpdateItem();
            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.Load(AppDomain.CurrentDomain.BaseDirectory + @"\App_Data\AppVersionConfigure.xml");
            XmlNodeList apps = xmlDoc.GetElementsByTagName("Applications");
            foreach (XmlNode app in apps[0].ChildNodes)
            {
                if(app["Name"].InnerText==appName)
                {
                    //计算版本号时，只计算.的前2位。
                    string NewestVersion = app["NewestVersion"].InnerText;
                    string[] numberArray= NewestVersion.Split('.');
                    int newestNumber = 0;
                   
                    for(int i=0;i<Math.Min(numberArray.Count(),2); i++)
                    {
                        newestNumber += int.Parse( numberArray[i]);
                    }
                    numberArray = currentVersion.Split('.');
                    int currentNumber = 0;
                    for (int i = 0; i < Math.Min(numberArray.Count(),2); i++)
                    {
                        currentNumber += int.Parse(numberArray[i]);
                    }
                    if (newestNumber > currentNumber)
                    {
                        //获取最新得版本得filelistURL和FTPURL。

                        tmpUpdateItem.isShouldUpdate = true;
                        GetVersionInfo(appName, NewestVersion, ref tmpUpdateItem);
                        return tmpUpdateItem;
                    }
                }
            }

            tmpUpdateItem.isShouldUpdate = false;
            tmpUpdateItem.FTPURL = "";
            tmpUpdateItem.FileListURL = "";
            return tmpUpdateItem;
           
        }
        private void GetVersionInfo(string appName, string version, ref UpdateItem update)
        {
            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.Load(AppDomain.CurrentDomain.BaseDirectory + @"\App_Data\AppVersionConfigure.xml");
            XmlNodeList apps = xmlDoc.GetElementsByTagName("Configuration");
            foreach (XmlNode configure in apps)
            {
                
                    if (configure.ChildNodes[0].InnerText == appName && configure.ChildNodes[1].InnerText == version)
                    {
                        update.FileListURL = configure.ChildNodes[2].InnerText;
                        update.FTPURL = configure.ChildNodes[3].InnerText;
                        return;
                    }
 
            }
            update.FileListURL = "";
            update.FTPURL = "";
            return;
        }
           
    }
}
