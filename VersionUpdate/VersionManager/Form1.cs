using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using WinSCP;

namespace VersionUpdateManager
{
   
    public partial class Form1 : Form
    {
        class RemoteFileItem
        {
            public string remotePath { get; set; }
            public DateTime lastWriteTime { get; set; }
        }
        public Form1()
        {
            InitializeComponent();
            //初始化：
            ftpServer = Program.ftpURL;
            fileListPath = Program.fileListURL;
            //调整界面
            
            //禁用面板
            panel1.Enabled = false;
            //程序启动时，开启计时器
            timer1.Enabled = true;
        }
        int tickNumber = 0;
        int tickCount = 0;
        string ftpServer;
        string fileListPath;
        private void timer1_Tick(object sender, EventArgs e)
        {
            //计时器中，检查之前的主程序是否已经退出，如果没有退出，则强制其退出，并等待其退出成功。
            bool isExists = false;
            if (tickNumber++<10)
            {

                Process[] myproc = Process.GetProcesses();

                foreach (Process item in myproc)
                {
                    if (item.Id == Program.pid)
                    {
                        isExists = true;
                        item.Kill();
                        break;
                    }
                }
                
            }
            else
            {
              
                tickCount++;
                tickNumber = 0;
            }

            if (tickCount>10)
            {//等待主程序超时
                MessageBox.Show("等待主程序退出超时");
                timer1.Enabled = false;
                Application.Exit();
                return;
            }
            if (isExists==true)
            {
                status.Text = "等待主程序退出" + new string('.', tickNumber);
            }
            else
            {   //已经没有进程了，关闭定时器
                timer1.Enabled = false;
                //使能面板
                panel1.Enabled = true;
                //异步调用fileList处理流程
                status.BeginInvoke(new Action(CompareFileList));
            }
        }
        string relatedRootFolder;
        void CompareFileList()
        {
            checkedListBox1.Items.Clear();
            Credential credForm = new Credential();
            if (credForm.ShowDialog() == DialogResult.Cancel)
            {
                return;
            }
            userName = credForm._userName;
            password = credForm._password;
            string fileListOnLocalPath = Application.StartupPath+@"\";
            string tmpFileListPath = fileListPath.Replace('\\', '/');
            string fileListOnLocalFile = Application.StartupPath + tmpFileListPath.Substring(tmpFileListPath.LastIndexOf('/'));
            //获取fileList
            if (GetFile(ftpServer, fileListPath, fileListOnLocalPath, userName, password)==false)
            {
                MessageBox.Show("无法获取更新文件列表");
                //启动原来的程序
                Process tmpProcess = new Process();
                tmpProcess.StartInfo.FileName = Program.exePath;
                tmpProcess.Start();
                //退出当前程序
                Application.Exit();
                return;
            }
           
            
            //否则，开始解析文件列表
            string[] lines = File.ReadAllLines(fileListOnLocalFile);
            string tmpFile, tmpDatetime;
            remoteFileList.Clear();
            updateFileList.Clear();
            relatedRootFolder = lines[0];//第一行为指定rootPath根目录
            for (int i=1;i<lines.Count();i++) 
            {
                string eachLine = lines[i];
                string[] item = eachLine.Split('|');
                tmpFile = Application.StartupPath+item[0].Replace(relatedRootFolder,"");
                tmpDatetime = item[1];
                RemoteFileItem tmpItem = new RemoteFileItem();
                tmpItem.remotePath = item[0];
                tmpItem.lastWriteTime = DateTime.Parse(tmpDatetime);
                remoteFileList.Add(tmpFile, tmpItem);
            }
            //文件列表解析完毕，接下来获取待更新文件列表。
            foreach(string file in remoteFileList.Keys)
            {
                if(File.Exists(file)==true)
                {
                    //比较时间
                    FileInfo info = new FileInfo(file);
                    if (info.LastWriteTime < remoteFileList[file].lastWriteTime)
                    {//服务器上的时间更新
                        updateFileList.Add(remoteFileList[file].remotePath);
                    }
                }
                else
                {//本地补存在该文件
                    updateFileList.Add(remoteFileList[file].remotePath);
                }
            }
           
            //更新界面
            for(int i=0;i< updateFileList.Count;i++)
            {
                int index=checkedListBox1.Items.Add(updateFileList[i]);
                checkedListBox1.SetItemChecked(index, true);//默认都是选中
            }
            
           
        }
       
        Dictionary<string, RemoteFileItem> remoteFileList=new Dictionary<string, RemoteFileItem>();
        List<string> updateFileList = new List<string>();//等待更新的文件
        //从FTP上获取fileList。这里弹出对话框要求输入用户名和密码。
        string userName, password;
        
        bool GetFile(string ftpServer, string filePathOnFTP, string filePathOnLocal, string _userName, string _password)
        {

            //开始获取文件列表
            try
            {
                // Setup session options
                SessionOptions sessionOptions = new SessionOptions
                {
                    Protocol = Protocol.Ftp,
                    HostName = ftpServer,
                    UserName = _userName,
                    Password = _password
                };

                using (Session session = new Session())
                {
                    // Connect
                    session.Open(sessionOptions);

                    // Upload files
                    TransferOptions transferOptions = new TransferOptions();
                    transferOptions.TransferMode = TransferMode.Binary;

                    TransferOperationResult transferResult;
                    transferResult = session.GetFiles(filePathOnFTP.Replace('\\','/'), filePathOnLocal, false, transferOptions);

                    // Throw on any error
                    transferResult.Check();
                    return true;
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("Error: {0}", e);
                return false;
            }
        }
        /// <summary>
        /// 当点击升级按钮时，会开始进行升级操作。
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Update_Click(object sender, EventArgs e)
        {
            //禁用控件
           
            Update.Enabled = false;
            checkedListBox1.Enabled = false;
           
         
           
            //获取所有选中的文件
            int totalCount = checkedListBox1.Items.Count;
            progressBar1.Maximum = totalCount;
            progressBar1.Minimum = 0;
            progressBar1.Value = 1;

            for (int i=0;i<totalCount;i++)
            {//开始从FTP下载文件了
                string fileName = checkedListBox1.Items[i].ToString();
                string relateFilePath = fileName.Replace(relatedRootFolder, "");
                string fileLocalPath = Application.StartupPath + relateFilePath.Substring(0, relateFilePath.LastIndexOf('\\'))+@"\";
              
                if (Directory.Exists(fileLocalPath)==false)
                {
                    Directory.CreateDirectory(fileLocalPath);
                }
                if (GetFile(ftpServer, fileName, fileLocalPath, userName, password)==true)
                {               
                    
                    logWin.Items.Add(string.Format("{0} 更新成功\r\n", fileName));
                }
                else
                {
                    logWin.Items.Add(string.Format("{0} 更新失败\r\n", fileName));
                }
                //更新进度条
                progressBar1.Value = i+1;
            }

            ////使能控件
            Update.Enabled = true;
            checkedListBox1.Enabled = true;
            //检查启动程序选项的状态：
            if(isAutoStart.Checked==true)
            {
                //启动原来的程序
                Process tmpProcess = new Process();
                tmpProcess.StartInfo.FileName = Program.exePath;
                tmpProcess.Start();
                //退出当前程序
                Application.Exit();
            }
           
        }

        private void cancel_Click(object sender, EventArgs e)
        {
           if( MessageBox.Show("确认退出？")==DialogResult.OK)
            {
                Application.Exit();
            }
           
        }
    }
}
