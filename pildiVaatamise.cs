using System;
using System.Drawing;
using System.Windows.Forms;

namespace Kolm_rakendust
{
    public class pildiVaatamise
    {
        private Button showButton;
        private Button clearButton;
        private Button backgroundButton;
        private Button closeButton;
        private CheckBox stretchCheckBox;
        private PictureBox pictureBox;
        private OpenFileDialog openFileDialog;
        private ColorDialog colorDialog;
        private Button saveButton;
        private SaveFileDialog saveFileDialog;
        private Button grayscaleButton;
        private Button infoButton;

        public pildiVaatamise(Form parent)
        {
            openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "Image Files|*.bmp;*.jpg;*.jpeg;*.png;*.gif|All files|*.*";

            colorDialog = new ColorDialog();

            saveFileDialog = new SaveFileDialog();
            saveFileDialog.Filter = "PNG Image|*.png|JPEG Image|*.jpg;*.jpeg|Bitmap|*.bmp";

            pictureBox = new PictureBox
            {
                Location = new Point(150, 50),
                Size = new Size(560, 450),
                BorderStyle = BorderStyle.Fixed3D,
                SizeMode = PictureBoxSizeMode.Normal
            };

            showButton = new Button { Text = "Vali pilt...", Location = new Point(150, 10), Size = new Size(100, 30) };
            clearButton = new Button { Text = "Tühjenda pilt", Location = new Point(260, 10), Size = new Size(100, 30) };
            backgroundButton = new Button { Text = "Muuda taustavärvi", Location = new Point(370, 10), Size = new Size(120, 30) };
            closeButton = new Button { Text = "Sulge", Location = new Point(500, 10), Size = new Size(100, 30) };
            stretchCheckBox = new CheckBox { Text = "Rasta pilt", Location = new Point(610, 15), Size = new Size(100, 20) };
            saveButton = new Button { Text = "Salvesta pilt", Location = new Point(710, 10), Size = new Size(100, 30) };
            grayscaleButton = new Button { Text = "Grayscale", Location = new Point(800, 10), Size = new Size(100, 30) };
            infoButton = new Button { Text = "Info", Location = new Point(900, 10), Size = new Size(60, 30) };

            showButton.Click += ShowButton_Click;
            clearButton.Click += ClearButton_Click;
            backgroundButton.Click += BackgroundButton_Click;
            closeButton.Click += CloseButton_Click;
            stretchCheckBox.CheckedChanged += StretchCheckBox_CheckedChanged;
            saveButton.Click += SaveButton_Click;
            grayscaleButton.Click += (s, e) => ApplyGrayscale();
            infoButton.Click += (s, e) => ShowImageInfo();


            parent.Controls.Add(showButton);
            parent.Controls.Add(clearButton);
            parent.Controls.Add(backgroundButton);
            parent.Controls.Add(closeButton);
            parent.Controls.Add(stretchCheckBox);
            parent.Controls.Add(saveButton);
            parent.Controls.Add(grayscaleButton);
            parent.Controls.Add(infoButton);
            parent.Controls.Add(pictureBox);
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
            if (pictureBox.FindForm() is Form form)
            {
                form.Close();
            }
        }

        private void StretchCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            pictureBox.SizeMode = stretchCheckBox.Checked ? PictureBoxSizeMode.StretchImage : PictureBoxSizeMode.Normal;
        }

        private void SaveButton_Click(object sender, EventArgs e)
        {
            if (pictureBox.Image == null) return;
            if (saveFileDialog.ShowDialog() == DialogResult.OK)
            {
                pictureBox.Image.Save(saveFileDialog.FileName);
            }
        }

        public void ApplyGrayscale()
        {
            if (pictureBox.Image == null) return;

            Bitmap bmp = new Bitmap(pictureBox.Image);
            for (int y = 0; y < bmp.Height; y++)
            {
                for (int x = 0; x < bmp.Width; x++)
                {
                    Color c = bmp.GetPixel(x, y);
                    int gray = (int)(c.R * 0.3 + c.G * 0.59 + c.B * 0.11);
                    bmp.SetPixel(x, y, Color.FromArgb(gray, gray, gray));
                }
            }
            pictureBox.Image = bmp;
        }

        public void ShowImageInfo()
        {
            if (pictureBox.Image == null) return;

            Bitmap bmp = (Bitmap)pictureBox.Image;
            string info = $"Размер: {bmp.Width} x {bmp.Height}\nФормат пикселя: {bmp.PixelFormat}";
            MessageBox.Show(info, "Информация о картинке");
        }

        public void Show()
        {
            showButton.Visible = true;
            clearButton.Visible = true;
            backgroundButton.Visible = true;
            closeButton.Visible = true;
            stretchCheckBox.Visible = true;
            pictureBox.Visible = true;
            saveButton.Visible = true;
            grayscaleButton.Visible = true;
            infoButton.Visible = true;
        }

        public void Hide()
        {
            showButton.Visible = false;
            clearButton.Visible = false;
            backgroundButton.Visible = false;
            closeButton.Visible = false;
            stretchCheckBox.Visible = false;
            pictureBox.Visible = false;
            saveButton.Visible = false;
            grayscaleButton.Visible = false;
            infoButton.Visible = false;
        }
    }
}
