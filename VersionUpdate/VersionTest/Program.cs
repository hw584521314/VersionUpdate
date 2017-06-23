using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using VersionUpdate;

namespace VersionTest
{
    class Program
    {
        static void Main(string[] args)
        {
            VersionUpdateTool tmpTool = new VersionUpdateTool();
            try
            {
                string appName = System.Reflection.Assembly.GetExecutingAssembly().GetName().Name;
               string version= System.Reflection.Assembly.GetExecutingAssembly().GetName().Version.ToString();
               UpdateItem tmpItem = tmpTool.CheckUpdate(appName, version);
                if(tmpItem.isShouldUpdate==true)
                {
                    Console.WriteLine("准备升级");
                    tmpTool.StartUpdate(tmpItem, Process.GetCurrentProcess().Id, @"E:\创业学习\版本升级器\VersionUpdate\VersionTest\bin\Debug\VersionTest.exe");
                    while(true)
                    {
                        Thread.Sleep(500);
                        Console.WriteLine("正在升级");
                    }
                }
                else
                {
                    Console.WriteLine("不用升级");
                    Console.ReadKey();
                }
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
    }
}
