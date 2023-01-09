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
        private void Form1_Load(object sender, EventArgs e)
        {
            string pathRegion = "region.txt";
            if (System.IO.File.Exists(pathRegion))
            {
                string[] arrTemp = System.IO.File.ReadAllLines(pathRegion);
                List<string> termsListlocaleCode = new List<string>();
                for (int i = 0; i < arrTemp.Length; i++)
                {
                    string[] Temp = arrTemp[i].Split('|');
                    termsListlocaleCode.Add(Temp[0].Trim());
                    comboBox1.Items.Add(Temp[1].Trim());
                }
                comboBox1.SelectedIndex = 7;
                localeCode = termsListlocaleCode.ToArray();
                installPath = Microsoft.Win32.Registry.GetValue(@"HKEY_CURRENT_USER\SOFTWARE\Microsoft\Windows\CurrentVersion\Uninstall\Riot Game league_of_legends.live", "InstallLocation", "").ToString();
            }
            else
            {
                DialogResult rs = MessageBox.Show("not Exists region.txt yes Dowload FIle","Fry1714",MessageBoxButtons.YesNo);
                if (rs == DialogResult.Yes)
                {
                    using (var client = new System.Net.WebClient())
                    {
                        client.DownloadFile("https://cdn.discordapp.com/attachments/1019878981450870864/1061956348172054568/region.txt", pathRegion);
                    }
                }
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
            CreateShortcut();
        }
    }
}
