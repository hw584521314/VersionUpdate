using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace VersionUpdateManager
{
    static class Program
    {
        public static int pid;
        public static string exePath;
        public static string fileListURL;
        public static string ftpURL;
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        /// 
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            //获取参数
            string[] args=Environment.GetCommandLineArgs();
            if(args.Contains("-d"))
            {
                //说明是调试模式：
                pid = 55555;
                exePath = @"c:\windows\system32\notepad.exe";
                fileListURL = @"\V02\fileList.txt";
                ftpURL = "192.168.5.100";
            }
            else
            {//接下来开始获取升级程序的环境变量。
                try
                {
                    pid = int.Parse(Environment.GetEnvironmentVariable("ProcessId"));
                    exePath = Environment.GetEnvironmentVariable("ExePath");
                    fileListURL = Environment.GetEnvironmentVariable("FileListURL");
                    ftpURL = Environment.GetEnvironmentVariable("FTPURL");
                }
                catch (Exception ex)
                {//有错误直接退出
                    MessageBox.Show(ex.Message);
                    return;
                }
            }
            
           
            
            Application.Run(new Form1());
        }
    }
}
