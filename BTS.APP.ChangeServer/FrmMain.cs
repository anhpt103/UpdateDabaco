using BTS.APP.ChangeServer;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows.Forms;

namespace BTS.APP.CS
{
    public partial class FrmMain : Form
    {
        private static string DIRECTORY_PATH = @"C:\Temp";
        private static string FILE_PATH = DIRECTORY_PATH + @"\SERVER.txt";
        private static int? CURRENT_ID = null;
        private static int? NEXT_ID = null;
        private List<DummyData> ListDummyData = new List<DummyData>();
        public FrmMain()
        {
            InitializeComponent();
            ListDummyData = new List<DummyData>()
            {
                new DummyData()
                {
                    Id = 1,
                    Name = "CƠ SỞ DỮ LIỆU ORACLE",
                },
                new DummyData()
                {
                    Id = 2,
                    Name = "CƠ SỞ DỮ LIỆU SQL SERVER",
                }
            };
        }

        private string CreateDirectory()
        {
            try
            {
                if (!Directory.Exists(DIRECTORY_PATH))
                {
                    Directory.CreateDirectory(DIRECTORY_PATH);
                }
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
            return "";
        }

        private string CreateFileIfNotExists()
        {
            try
            {
                if (!File.Exists(FILE_PATH)) // If file does not exists
                {
                    File.Create(FILE_PATH).Close(); // Create file
                    using (StreamWriter sw = File.AppendText(FILE_PATH))
                    {
                        sw.WriteLine(0); // Write text to .txt file
                        CURRENT_ID = 0;
                    }
                }
                else
                {

                }
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
            return "";
        }

        private string DrawUI(int? status)
        {
            try
            {
                if (status == null || status == 0)
                {
                    lblServerName.Text = "CHƯA CHỌN MÁY CHỦ CƠ SỞ DỮ LIỆU";
                    btnChangeServer.Text = $"CHUYỂN ĐỔI -> CƠ SỞ DỮ LIỆU ORACLE";
                    lblServerName.Location = new Point(40, 70);
                    NEXT_ID = 1;
                }
                else
                {
                    var data = ListDummyData.First(x => x.Id == status);
                    if (data == null) btnChangeServer.Enabled = false;
                    else
                    {
                        lblServerName.Text = data.Name;

                        if (status == 1)
                        {
                            lblServerName.Location = new Point(90, 70);
                            NEXT_ID = 2;
                        }
                        else if (status == 2)
                        {
                            lblServerName.Location = new Point(85, 70);
                            NEXT_ID = 1;
                        }

                        var nextData = ListDummyData.First(x => x.Id == NEXT_ID);
                        if (nextData == null) { btnChangeServer.Enabled = false; return ""; }

                        btnChangeServer.Text = $"CHUYỂN ĐỔI -> {nextData.Name}";
                        btnChangeServer.Enabled = true;
                    }
                }
            }
            catch (Exception ex)
            {
                return ex.Message;
            }

            return "";
        }

        private string ReadContentTextFile()
        {
            try
            {
                string[] lines = File.ReadAllLines(FILE_PATH);
                foreach (string line in lines)
                {
                    int value = 0;
                    int.TryParse(line, out value);
                    CURRENT_ID = value;
                }
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
            return "";
        }

        private void FrmMain_Load(object sender, System.EventArgs e)
        {
            string msg = CreateDirectory();
            if (msg.Length > 0) { MessageBox.Show(msg); return; }

            msg = CreateFileIfNotExists();
            if (msg.Length > 0) { MessageBox.Show(msg); return; }

            msg = ReadContentTextFile();
            if (msg.Length > 0) { MessageBox.Show(msg); return; }

            msg = DrawUI(CURRENT_ID);
            if (msg.Length > 0) { MessageBox.Show(msg); return; }
        }

        private void btnChangeServer_Click(object sender, EventArgs e)
        {
            File.WriteAllText(FILE_PATH, String.Empty);
            using (StreamWriter sw = File.AppendText(FILE_PATH))
            {
                sw.WriteLine(NEXT_ID);
                CURRENT_ID = NEXT_ID;
            }

            string msg = DrawUI(CURRENT_ID);
            if (msg.Length > 0) { MessageBox.Show(msg); return; }
        }
    }
}
