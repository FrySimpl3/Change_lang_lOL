using IWshRuntimeLibrary;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using IWshRuntimeLibrary;
using static System.Net.Mime.MediaTypeNames;
using System.Reflection;


namespace Change_lOL
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }
        string[] localeCode;
        string installPath;
        void errorlog(string error)
        {
            try
            {
                System.Threading.Thread t = new System.Threading.Thread(() => {

                    string _date = System.DateTime.Now.ToString("[ HH:mm:ss dd/MM/yyyy ]\t");
                    System.IO.File.AppendAllText("Fry.ini", _date + error + "\n");
                });
                t.IsBackground = true;
                t.Start();
            }
            catch (Exception msg)
            {
                MessageBox.Show(msg.ToString());
            }
        }
        void load()
        {
            string pathRegion = "region.txt";

            if (!System.IO.File.Exists(pathRegion))
            {
                using (var client = new System.Net.WebClient())
                {
                    client.DownloadFile("https://cdn.discordapp.com/attachments/1019878981450870864/1061956348172054568/region.txt", pathRegion);
                }
            }
            if (System.IO.File.Exists(pathRegion))
            {
                string[] arrTemp = System.IO.File.ReadAllLines(pathRegion);
                List<string> termsListlocaleCode = new List<string>();
                for (int i = 0; i < arrTemp.Length; i++)
                {
                    string[] Temp = arrTemp[i].Split('|');
                    termsListlocaleCode.Add(Temp[0]);
                    comboBox1.Items.Add(Temp[1].Trim());
                }
                comboBox1.SelectedIndex = 7;
                localeCode = termsListlocaleCode.ToArray();
                installPath = Microsoft.Win32.Registry.GetValue(@"HKEY_CURRENT_USER\SOFTWARE\Microsoft\Windows\CurrentVersion\Uninstall\Riot Game league_of_legends.live", "InstallLocation", "").ToString();
            }
        }
        private void Form1_Load(object sender, EventArgs e)
        {
            try
            {
                load();
            }
            catch (Exception msg)
            {
                errorlog(msg.ToString());
            }
        }
        private void CreateShortcut()
        {
            object shDesktop = (object)"Desktop";
            WshShell shell = new WshShell();
            string shortcutAddress = (string)shell.SpecialFolders.Item(ref shDesktop) + @"\"+ comboBox1.SelectedItem.ToString() + ".lnk";
            IWshShortcut shortcut = (IWshShortcut)shell.CreateShortcut(shortcutAddress);
            shortcut.Description = "Code By Fry#1714";
            int index = comboBox1.SelectedIndex;
            shortcut.TargetPath = installPath+"\\LeagueClient.exe";
            shortcut.Arguments = "--locale=" + localeCode[index].Trim();
            string iconPath = System.Environment.CurrentDirectory + "\\icon.ico";
            if (System.IO.File.Exists(iconPath))
            {
                shortcut.IconLocation = iconPath;
            }
            shortcut.Save();
        }
        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                CreateShortcut();
            }
            catch (Exception msg)
            {
                errorlog(msg.ToString());
            }
        }
    }
}
