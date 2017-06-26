using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Json;
using System.IO;
using System.Diagnostics;
using System.Xml;
using log4net;
using log4net.Config;

namespace VersionUpdate
{
   
    public class UpdateItem
    {
        public bool isShouldUpdate { set; get; }
        public string FileListURL { get; set; }
        public string FTPURL { get; set; }
    }
    public class JsonTools
    {
        // 从一个对象信息生成Json串
        public static string ObjectToJson(object obj)
        {
            DataContractJsonSerializer serializer = new DataContractJsonSerializer(obj.GetType());
            MemoryStream stream = new MemoryStream();
            serializer.WriteObject(stream, obj);
            byte[] dataBytes = new byte[stream.Length];
            stream.Position = 0;
            stream.Read(dataBytes, 0, (int)stream.Length);
            return Encoding.UTF8.GetString(dataBytes);
        }
        // 从一个Json串生成对象信息
        public static object JsonToObject(string jsonString, object obj)
        {
            DataContractJsonSerializer serializer = new DataContractJsonSerializer(obj.GetType());
            MemoryStream mStream = new MemoryStream(Encoding.UTF8.GetBytes(jsonString));
            return serializer.ReadObject(mStream);
        }
    }
    public class VersionUpdateTool
    {
        ILog log;
        public VersionUpdateTool()
        {
            XmlConfigurator.Configure(new System.IO.FileInfo("log4net.config"));
            log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        }
        public UpdateItem CheckUpdate(string appName,string currentVersion)
        {
            log.Info("appName="+appName+" currentVersion="+currentVersion);
            //读取配置文件
            UpdateItem tmpItem= new UpdateItem();
            tmpItem.isShouldUpdate = false;
            tmpItem.FileListURL = "";
            tmpItem.FTPURL = "";

            XmlDocument xmlDoc = new XmlDocument();
            try
            {
                xmlDoc.Load(AppDomain.CurrentDomain.BaseDirectory + @"\VersionUpdateConfig.xml");
            }
            catch(Exception ex)
            {
                log.Info("读取VersionUpdateConfig时");
                log.Error(ex.Message);
                log.Error(ex.StackTrace);
            }
            XmlNodeList webApi = xmlDoc.GetElementsByTagName("WebAPIURL");
            if(webApi.Count==0)
            {
                log.Error("未读取到WebAPIURL节点");              
                return tmpItem;
            }
            string webApiURL = "";
            if (webApi.Count>0)
            {
                webApiURL = webApi[0].InnerText;
            }
            
            string url = string.Format(webApiURL+@"?appName={0}&currentVersion={1}",appName,currentVersion);
            log.Info("WebAPI的URL为：" + url);
            WebClient client = new WebClient();
            //上传并接收数据
           
            try
            {
                Byte[] responseData = client.DownloadData(url);
                //接收返回的json的流数据，并转码
                string srcString = Encoding.UTF8.GetString(responseData);
                tmpItem = (UpdateItem)JsonTools.JsonToObject(srcString, new UpdateItem());
                
                return tmpItem;
            }
            catch(Exception ex)
            {
                log.Error("调用webAPI时："+ex.Message);
                return tmpItem;
            }
            

        }
        public void StartUpdate(UpdateItem updateItem,int processId,string currentExePath)
        {
            
          
            if(updateItem.isShouldUpdate==false)
            {//补应该升级，则直接退出
                log.Info("不需要升级");
                return;
            }
            log.Info("需要升级");

            Environment.SetEnvironmentVariable("ProcessId", processId.ToString());
            Environment.SetEnvironmentVariable("ExePath", currentExePath);
            Environment.SetEnvironmentVariable("FileListURL", updateItem.FileListURL);
            Environment.SetEnvironmentVariable("FTPURL", updateItem.FTPURL);
            log.Info("FileListURL=" + updateItem.FileListURL);
            log.Info("ExePath=" + currentExePath);
            log.Info("FTPURL=" + updateItem.FTPURL);
            

            string updateManager = Path.GetDirectoryName(currentExePath) + @"\VersionManager.exe";
            log.Info("updateManagerPath=" + updateManager);
            //执行程序
            Process process1 = new Process();
            process1.StartInfo.FileName = updateManager;
            process1.StartInfo.CreateNoWindow = true;
            
            //接下来开始设置升级程序的环境变量。
            //process1.StartInfo.EnvironmentVariables["ProcessId"] = processId.ToString();
            //process1.StartInfo.EnvironmentVariables["ExePath"] = currentExePath;
            //process1.StartInfo.EnvironmentVariables["FileListURL"] = updateItem.FileListURL;
            //process1.StartInfo.EnvironmentVariables["FTPURL"] = updateItem.FTPURL;
            //process1.StartInfo.UseShellExecute = false;

            process1.Start();
            log.Info("开始执行UpdateManager");
        }
    }
}
