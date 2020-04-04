using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace pk4toBFG
{
    public partial class Form1 : Form
    {
        public static List<string> ml { set; get; }
        public Form1()
        {
            InitializeComponent();
            ml = new List<string>();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Stream myStream = null;
            OpenFileDialog openFileDialog1 = new OpenFileDialog();
            string filter = "pk4 files (*.pk4)|*.pk4";
            openFileDialog1.Filter = filter;
            openFileDialog1.FilterIndex = 1;
            openFileDialog1.RestoreDirectory = true;

            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    if ((myStream = openFileDialog1.OpenFile()) != null)
                    {
                        using (myStream)
                        {
                            ml.Add(openFileDialog1.FileName);
                            string[] brokenPath = openFileDialog1.FileName.Split('\\');
                            listBox1.Items.Add(brokenPath.Last());
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error: Could not read file from disk. Original error: " + ex.Message);
                }
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            ml.RemoveAt(listBox1.SelectedIndex);
            listBox1.Items.Remove(listBox1.SelectedItem);
            
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (ml.Count <= 0)
            {
                MessageBox.Show("No pk4 files selected");
                return;
            }
            if (textBox1.Text == "")
            {
                MessageBox.Show("No Directory");
                return;
            }
            for (int i = 0; i < ml.Count; i++) {
                using (ZipArchive archive = ZipFile.OpenRead(ml[i].ToString()))
                {
                    if (i == 0)
                    {
                        archive.ExtractToDirectory(textBox1.Text);
                    }
                    else
                    {
                        foreach (ZipArchiveEntry file in archive.Entries)
                        {
                            string fullname = Path.Combine(textBox1.Text, file.FullName);
                            if(file.Name == "" || !File.Exists(fullname))
                            {
                                Directory.CreateDirectory(Path.GetDirectoryName(fullname));
                                if(file.Name != "")
                                {
                                    file.ExtractToFile(fullname, true);
                                }
                                continue;
                            }
                            file.ExtractToFile(fullname, true);
                        }
                    }
                }

                        }
            MessageBox.Show("Extraction Complete!!");
        }
    }
}
