using System;
using System.Drawing;
using System.Windows.Forms;
using System.Xml.Serialization;
using System.Drawing;


namespace Kolm_rakendust
{
    public class pildiVaatamise
    {
        private Button showButton;
        private Button clearButton;
        private Button backgroundButton;
        private Button closeButton;
        private Button filer;
        private CheckBox stretchCheckBox;
        private PictureBox pictureBox;
        private OpenFileDialog openFileDialog;
        private ColorDialog colorDialog;
        private Control parentControl;

        public pildiVaatamise(Control parent)
        {
            parentControl = parent;


            openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "Image Files|*.bmp;*.jpg;*.jpeg;*.png;*.gif|All files|*.*";

            colorDialog = new ColorDialog();

            showButton = new Button();
            showButton.Text = "Vali pilt...";
            showButton.Location = new Point(150, 10);
            showButton.Size = new Size(100, 30);
            showButton.Click += ShowButton_Click;

            clearButton = new Button();
            clearButton.Text = "Tühjenda pilt";
            clearButton.Location = new Point(260, 10);
            clearButton.Size = new Size(100, 30);
            clearButton.Click += ClearButton_Click;


            backgroundButton = new Button();
            backgroundButton.Text = "Muuda taustavärvi";
            backgroundButton.Location = new Point(370, 10);
            backgroundButton.Size = new Size(120, 30);
            backgroundButton.Click += BackgroundButton_Click;


            closeButton = new Button();
            closeButton.Text = "Sulge";
            closeButton.Location = new Point(500, 10);
            closeButton.Size = new Size(100, 30);
            closeButton.Click += CloseButton_Click;


            stretchCheckBox = new CheckBox();
            stretchCheckBox.Text = "Rasta pilt";
            stretchCheckBox.Location = new Point(610, 15);
            stretchCheckBox.Size = new Size(100, 20);
            stretchCheckBox.CheckedChanged += StretchCheckBox_CheckedChanged;


            filer = new Button();
            filer.Text = "Filtrid";
            filer.Location = new Point(700, 10);
            filer.Size = new Size(100, 30);
            filer.Click += filer_Click;





            pictureBox = new PictureBox();
            pictureBox.Location = new Point(150, 50);
            pictureBox.Size = new Size(560, 450);
            pictureBox.BorderStyle = BorderStyle.Fixed3D;
            pictureBox.SizeMode = PictureBoxSizeMode.Normal;


            parentControl.Controls.Add(showButton);
            parentControl.Controls.Add(clearButton);
            parentControl.Controls.Add(backgroundButton);
            parentControl.Controls.Add(closeButton);
            parentControl.Controls.Add(stretchCheckBox);
            parentControl.Controls.Add(pictureBox);
        }
        private void filer_Click(object sender, EventArgs e)
        {
            string[] Valikud =
            {
                "Tumedam.",
                "Heledam!",
                "Jätka, sa teed suurepärast tööd!",
                "Kas tead? Juhuslikud sõnumid on lõbusad.",
                "Siin on väike üllatus!"
            };
        }
        private void ShowButton_Click(object sender, EventArgs e)
        {

            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                pictureBox.Load(openFileDialog.FileName);
            }
        }

        private void ClearButton_Click(object sender, EventArgs e)
        {

            pictureBox.Image = null;
        }

        private void BackgroundButton_Click(object sender, EventArgs e)
        {

            if (colorDialog.ShowDialog() == DialogResult.OK)
            {
                pictureBox.BackColor = colorDialog.Color;
            }
        }

        private void CloseButton_Click(object sender, EventArgs e)
        {

            if (parentControl is Form form)
            {
                form.Close();
            }
        }

        private void StretchCheckBox_CheckedChanged(object sender, EventArgs e)
        {

            if (stretchCheckBox.Checked)
                pictureBox.SizeMode = PictureBoxSizeMode.StretchImage;
            else
                pictureBox.SizeMode = PictureBoxSizeMode.Normal;
        }

        public void Show()
        {
            showButton.Visible = true;
            clearButton.Visible = true;
            backgroundButton.Visible = true;
            closeButton.Visible = true;
            stretchCheckBox.Visible = true;
            pictureBox.Visible = true;
        }

        public void Hide()
        {
            showButton.Visible = false;
            clearButton.Visible = false;
            backgroundButton.Visible = false;
            closeButton.Visible = false;
            stretchCheckBox.Visible = false;
            pictureBox.Visible = false;
        }
    }
}
