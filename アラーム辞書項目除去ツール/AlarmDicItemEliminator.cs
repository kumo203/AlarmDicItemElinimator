using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
//using System.Threading.Tasks;
using System.Windows.Forms;

namespace アラーム辞書項目除去ツール
{
    public partial class AlarmDicItemEliminator : Form
    {
        public AlarmDicItemEliminator()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (folderBrowserDialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                textBox1.Text = folderBrowserDialog.SelectedPath;
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (Directory.Exists(textBox1.Text)==false) {
                MessageBox.Show("Folder doesn't exist", "Folder Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            string titleSave = this.Text;
            foreach (var f in Directory.GetFiles(textBox1.Text, "*.elg"))
            {
                this.Text = "Processing: " + f;
                bool changed = false;
                string newName = f + ".bak";

                File.Move(f, newName);
                var lines = File.ReadAllLines(newName);
                StringBuilder sb = new StringBuilder();
                foreach (var line in lines)
                {
                    if (Regex.Match(line, Properties.Settings.Default.MatchString).Success)
                    {
                        changed = true;
                    }
                    else
                    {
                        sb.AppendLine(line);
                    }
                }
                if (changed)
                {
                    File.WriteAllText(f, sb.ToString());
                }
                else
                {
                    File.Move(newName, f);
                }
            }
            this.Text = titleSave;
            MessageBox.Show("Elimination has been ended", "End");
        }
    }
}
