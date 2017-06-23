using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace VersionUpdateManager
{
    public partial class Credential : Form
    {
        string credFile;
        public Credential()
        {
            InitializeComponent();
            credFile = Application.StartupPath + @"\cred.dat";
        }
        public string _userName { get; set; }
        public string _password { get; set; }
        private void OK_Click(object sender, EventArgs e)
        {
            _userName = userName.Text;
            _password = password.Text;
            //写文件
            string[] lines = new string[] { _userName, _password };
            File.WriteAllLines(credFile,lines);
            DialogResult = DialogResult.OK;
        }

        private void Cancel_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
        }

        private void Credential_Load(object sender, EventArgs e)
        {
            //读取本地文件，看是否有临时文件，如果有，则直接读取用户名和密码
           
            if (File.Exists(credFile))
            {
                string[] lines = File.ReadAllLines(credFile);
                userName.Text = lines[0];
                password.Text = lines[1];
            }

        }
    }
}
