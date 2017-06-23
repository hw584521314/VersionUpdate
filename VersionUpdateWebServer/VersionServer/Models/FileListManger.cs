using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace VersionServer.Models
{
    public class Items
    {
       public string filePathAndName;//类似于 folder1\text.txt格式
       public DateTime datetime;
    }
    public class FileListManger
    {
        List<Items> GetFileList(string appName,string version)
        {
            return null;
        }
    }
}