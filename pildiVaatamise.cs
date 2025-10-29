using System;
using System.Drawing;
using System.Windows.Forms;

namespace Kolm_rakendust
{
    public class pildiVaatamise
    {
        Button btnShow, btnClear, btnBackground, btnClose, btnSave, btnGray, btnInfo;
        CheckBox chStretch;
        PictureBox pic;
        OpenFileDialog openFile;
        SaveFileDialog saveFile;
        ColorDialog colorDialog;

        public pildiVaatamise(Form parent)
        {
            openFile = new OpenFileDialog();
            openFile.Filter = "Image Files|*.bmp;*.jpg;*.jpeg;*.png;*.gif|All files|*.*";

            saveFile = new SaveFileDialog();
            saveFile.Filter = "PNG Image|*.png|JPEG Image|*.jpg;*.jpeg|Bitmap|*.bmp";

            colorDialog = new ColorDialog();

            pic = new PictureBox();
            pic.Location = new Point(150, 50);
            pic.Size = new Size(560, 450);
            pic.BorderStyle = BorderStyle.Fixed3D;
            pic.SizeMode = PictureBoxSizeMode.Normal;

            btnShow = new Button()
            {
                Text = "Vali pilt...",
                Location = new Point(150, 10),
                Size = new Size(100, 30)
            };
            btnShow.Click += BtnShow_Click;

            btnClear = new Button()
            {
                Text = "Tühjenda pilt",
                Location = new Point(260, 10),
                Size = new Size(100, 30)
            };
            btnClear.Click += BtnClear_Click;

            btnBackground = new Button()
            {
                Text = "Muuda taustavärvi",
                Location = new Point(370, 10),
                Size = new Size(120, 30)
            };
            btnBackground.Click += BtnBackground_Click;

            btnClose = new Button()
            {
                Text = "Sulge",
                Location = new Point(500, 10),
                Size = new Size(100, 30)
            };
            btnClose.Click += BtnClose_Click;

            btnSave = new Button()
            {
                Text = "Salvesta pilt",
                Location = new Point(710, 10),
                Size = new Size(100, 30)
            };
            btnSave.Click += BtnSave_Click;

            btnGray = new Button()
            {
                Text = "Grayscale",
                Location = new Point(820, 10),
                Size = new Size(100, 30)
            };
            btnGray.Click += (s, e) => ApplyGrayscale();

            btnInfo = new Button()
            {
                Text = "Info",
                Location = new Point(930, 10),
                Size = new Size(60, 30)
            };
            btnInfo.Click += (s, e) => ShowImageInfo();

            // --- МАРКЕРНАЯ ГАЛОЧКА (Stretch) ---
            chStretch = new CheckBox()
            {
                Text = "Rasta pilt",
                Location = new Point(610, 15),
                AutoSize = true
            };
            chStretch.CheckedChanged += ChStretch_CheckedChanged;

            // --- ДОБАВЛЕНИЕ НА ФОРМУ ---
            parent.Controls.Add(pic);
            parent.Controls.Add(btnShow);
            parent.Controls.Add(btnClear);
            parent.Controls.Add(btnBackground);
            parent.Controls.Add(btnClose);
            parent.Controls.Add(chStretch);
            parent.Controls.Add(btnSave);
            parent.Controls.Add(btnGray);
            parent.Controls.Add(btnInfo);
        }

        // --- СОБЫТИЯ ---
        private void BtnShow_Click(object sender, EventArgs e)
        {
            if (openFile.ShowDialog() == DialogResult.OK)
            {
                pic.Load(openFile.FileName);
            }
        }

        private void BtnClear_Click(object sender, EventArgs e)
        {
            pic.Image = null;
        }

        private void BtnBackground_Click(object sender, EventArgs e)
        {
            if (colorDialog.ShowDialog() == DialogResult.OK)
            {
                pic.BackColor = colorDialog.Color;
            }
        }

        private void BtnClose_Click(object sender, EventArgs e)
        {
            if (pic.FindForm() is Form form)
            {
                form.Close();
            }
        }

        private void ChStretch_CheckedChanged(object sender, EventArgs e)
        {
            pic.SizeMode = chStretch.Checked
                ? PictureBoxSizeMode.StretchImage
                : PictureBoxSizeMode.Normal;
        }

        private void BtnSave_Click(object sender, EventArgs e)
        {
            if (pic.Image == null) return;

            if (saveFile.ShowDialog() == DialogResult.OK)
            {
                pic.Image.Save(saveFile.FileName);
            }
        }

        // --- ДОП. ФУНКЦИИ ---
        public void ApplyGrayscale()
        {
            if (pic.Image == null) return;

            Bitmap bmp = new Bitmap(pic.Image);
            for (int y = 0; y < bmp.Height; y++)
            {
                for (int x = 0; x < bmp.Width; x++)
                {
                    Color c = bmp.GetPixel(x, y);
                    int gray = (int)(c.R * 0.3 + c.G * 0.59 + c.B * 0.11);
                    bmp.SetPixel(x, y, Color.FromArgb(gray, gray, gray));
                }
            }
            pic.Image = bmp;
        }

        public void ShowImageInfo()
        {
            if (pic.Image == null) return;

            Bitmap bmp = (Bitmap)pic.Image;
            string info = $"Suurus: {bmp.Width} x {bmp.Height}\nFormaat: {bmp.PixelFormat}";
            MessageBox.Show(info, "Informatsioon");
        }

        public void Show()
        {
            btnShow.Visible = true;
            btnClear.Visible = true;
            btnBackground.Visible = true;
            btnClose.Visible = true;
            chStretch.Visible = true;
            btnSave.Visible = true;
            btnGray.Visible = true;
            btnInfo.Visible = true;
            pic.Visible = true;
        }

        public void Hide()
        {
            btnShow.Visible = false;
            btnClear.Visible = false;
            btnBackground.Visible = false;
            btnClose.Visible = false;
            chStretch.Visible = false;
            btnSave.Visible = false;
            btnGray.Visible = false;
            btnInfo.Visible = false;
            pic.Visible = false;
        }
    }
}
