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

        public UpdateItem CheckUpdate(string appName,string currentVersion)
        {
            //读取配置文件
            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.Load(AppDomain.CurrentDomain.BaseDirectory + @"\VersionUpdateConfig.xml");
            XmlNodeList webApi = xmlDoc.GetElementsByTagName("WebAPIURL");
            string webApiURL = "";
            if (webApi.Count>0)
            {
                webApiURL = webApi[0].InnerText;
            }
            
            string url = string.Format(webApiURL+@"?appName={0}&currentVersion={1}",appName,currentVersion);

            WebClient client = new WebClient();
            //上传并接收数据
            UpdateItem tmpItem;
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
                Console.WriteLine("调用web API失败");
                tmpItem = new UpdateItem();
                tmpItem.isShouldUpdate = false;
                tmpItem.FileListURL = "";
                tmpItem.FTPURL = "";
                return tmpItem;
            }
            

        }
        public void StartUpdate(UpdateItem updateItem,int processId,string currentExePath)
        {
            
          
            if(updateItem.isShouldUpdate==false)
            {//补应该升级，则直接退出
                return;
            }

            Environment.SetEnvironmentVariable("ProcessId", processId.ToString());
            Environment.SetEnvironmentVariable("ExePath", currentExePath);
            Environment.SetEnvironmentVariable("FileListURL", updateItem.FileListURL);
            Environment.SetEnvironmentVariable("FTPURL", updateItem.FTPURL);

            string updateManager = Path.GetDirectoryName(currentExePath) + @"\VersionManager.exe";
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

        }
    }
}
