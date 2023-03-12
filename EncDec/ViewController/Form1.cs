using EncDec.Cryptography;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace EncDec
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        public Bitmap bmp;

        private void Form1_Load(object sender, EventArgs e)
        {
            oSToolStripMenuItem.Text = Info.getOS();
            lANIPToolStripMenuItem.Text = Info.getLANIP();
            wANIPToolStripMenuItem.Text = Info.getWANIP();

            pictureBox1.AllowDrop = true;
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            dateToolStripMenuItem.Text = Info.getDate();
            timeToolStripMenuItem.Text = Info.getTime();
            stopwatchToolStripMenuItem.Text = Info.getStopwatch();
        }

        public void clearControls(Control.ControlCollection ctrlCollection)
        {
            foreach (Control ctrl in ctrlCollection)
            {
                if (ctrl is TextBoxBase)
                {
                    ctrl.Text = String.Empty;
                }
                else if (ctrl is RadioButton)
                {
                    ((RadioButton)ctrl).Checked = false;
                }
                else if (ctrl is PictureBox)
                {
                    ((PictureBox)ctrl).Image = null;
                }
                else
                {
                    clearControls(ctrl.Controls);
                }
                List<Label> labels = new List<Label> { label3, label4, label6, label8 };
                foreach (Label label in labels) 
                {
                    label.Text = "file path will be indicated here";
                }
            }
        }

        private void clearToolStripMenuItem_Click(object sender, EventArgs e)
        {
            clearControls(this.Controls);
        }

        private void restartToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Application.Restart();
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Environment.Exit(0);
        }

        private void aboutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MessageBox.Show(this, "EncDec\r\n"
            + "Artur Zhadan\r\n"
            + "2020",
            "Message",
            MessageBoxButtons.OK,
            MessageBoxIcon.Information);
        }

        private void textFileToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                OpenFileDialog ofd = new OpenFileDialog();

                ofd.Filter = "Text files(*.txt)|*.txt|All files(*.*)|*.*";

                if (ofd.ShowDialog() == DialogResult.Cancel)
                {
                    return;
                }

                string fileName = ofd.FileName;
                string fileText = File.ReadAllText(fileName);

                textBox1.Text = fileText;
                label3.Text = fileName;

                MessageBox.Show(this, "Success!",
                    "Message",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show(this, "Something went wrong!"
                    + "\r\n"
                    + "Check input data.",
                    "Message",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information);
            }
        }

        private void imageToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                OpenFileDialog ofd = new OpenFileDialog();

                ofd.Filter = "Image Files(*.jpg; *.jpeg; *.gif; *.bmp; *.png)|*.jpg; *.jpeg; *.gif; *.bmp; *.png";

                if (ofd.ShowDialog() == DialogResult.Cancel)
                {
                    return;
                }

                string fileName = ofd.FileName;

                bmp = new Bitmap(ofd.FileName);
                pictureBox1.Image = bmp;
                label4.Text = fileName;

                MessageBox.Show(this, "Success!",
                    "Message",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show(this, "Something went wrong!"
                    + "\r\n"
                    + "Check input data.",
                    "Message",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information);
            }
        }

        private void textFileToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            try 
            {
                SaveFileDialog sfd = new SaveFileDialog();

                sfd.Filter = "Text files(*.txt)|*.txt|All files(*.*)|*.*";

                if (sfd.ShowDialog() == DialogResult.Cancel)
                {
                    return;
                }

                string fileName = sfd.FileName;

                File.WriteAllText(fileName, textBox3.Text);

                label6.Text = fileName;

                MessageBox.Show(this, "Success!",
                    "Message",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show(this, "Something went wrong!"
                    + "\r\n"
                    + "Check input data.",
                    "Message",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information);
            }
        }

        private void imageToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            try 
            {
                SaveFileDialog sfd = new SaveFileDialog();

                sfd.FileName = "Image";
                sfd.Filter = "JPEG Image (.png)|*.png";
                sfd.Title = "Save image";

                if (sfd.ShowDialog() == DialogResult.Cancel)
                {
                    return;
                }

                string fileName = sfd.FileName;

                int width = Convert.ToInt32(pictureBox1.Width);
                int height = Convert.ToInt32(pictureBox1.Height);

                Bitmap bmpPreview = new Bitmap(width, height);

                pictureBox2.DrawToBitmap(bmpPreview, new Rectangle(0, 0, width, height));
                
                bmp.Save(sfd.FileName, ImageFormat.Png);

                label8.Text = fileName;

                MessageBox.Show(this, "Success!",
                    "Message",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show(this, "Something went wrong!"
                    + "\r\n"
                    + "Check input data.",
                    "Message",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information);
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                if (radioButton4.Checked == true)
                {
                    if (radioButton1.Checked == true)
                    {
                        RSAHelper.MakeKey();
                        textBox3.Text = RSAHelper.Encrypt(textBox1.Text);
                    }
                    if (radioButton2.Checked == true)
                    {
                        textBox3.Text = AESHelper.Encrypt(textBox1.Text, textBox2.Text);
                    }
                    if (radioButton3.Checked == true)
                    {
                        textBox3.Text = SHAHelper.Encrypt(textBox1.Text);
                    }
                    if (radioButton6.Checked == true)
                    {
                        if (pictureBox1.Image != null)
                        {
                            string text = textBox1.Text;
                            bmp = LSB.embedText(text, bmp);
                            pictureBox2.Image = bmp;
                        }
                    }
                }
                if (radioButton5.Checked == true)
                {
                    if (radioButton1.Checked == true)
                    {
                        textBox3.Text = Convert.ToString(RSAHelper.Decrypt(textBox1.Text));
                    }
                    if (radioButton2.Checked == true)
                    {
                        textBox3.Text = AESHelper.Decrypt(textBox1.Text, textBox2.Text);
                    }
                    if (radioButton3.Checked == true)
                    {
                        textBox3.Text = "HASH cannot be decrypted, only compared.";
                    }
                    if (radioButton6.Checked == true)
                    {
                        if (pictureBox1.Image != null)
                        {
                            string extractedText = LSB.extractText(bmp);

                            textBox3.Text = extractedText;
                        }
                    }
                }
            }
            catch (Exception ex) 
            {
                MessageBox.Show(this, "Something went wrong!"
                    + "\r\n"
                    + "Check input data.",
                    "Message",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information);
            }
        }

        private void pictureBox1_DragDrop(object sender, DragEventArgs e)
        {
            try 
            {
                foreach (string pic in ((string[])e.Data.GetData(DataFormats.FileDrop)))
                {
                    bmp = new Bitmap(Bitmap.FromFile(pic));
                    pictureBox1.Image = bmp;
                }
                MessageBox.Show(this, "Success!",
                    "Message",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show(this, "Something went wrong!"
                    + "\r\n"
                    + "Check input data.",
                    "Message",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information);
            }
        }

        private void pictureBox1_DragEnter(object sender, DragEventArgs e)
        {
            e.Effect = DragDropEffects.Copy;
        }

        private void pictureBox1_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                pictureBox1.DoDragDrop(pictureBox1.Image,
                    DragDropEffects.Copy | DragDropEffects.Move);
            }
        }
    }
}
