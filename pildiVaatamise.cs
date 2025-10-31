using System;
using System.Drawing;
using System.Windows.Forms;

namespace Kolm_rakendust
{
    public class pildiVaatamise
    {
        Button btnShow, btnClear, btnBackground, btnClose, btnSave, btnInfo, btnApplyFilter;
        CheckBox chStretch;
        PictureBox pic;
        ComboBox cmbFilters; //  выпадающий список
        OpenFileDialog openFile;
        SaveFileDialog saveFile;
        ColorDialog colorDialog;

        private Image originalImage = null; 


        public pildiVaatamise(Form parent)
        {
            openFile = new OpenFileDialog();
            openFile.Filter = "Image Files|*.bmp;*.jpg;*.jpeg;*.png;*.gif|All files|*.*";

            saveFile = new SaveFileDialog();
            saveFile.Filter = "PNG Image|*.png|JPEG Image|*.jpg;*.jpeg|Bitmap|*.bmp";
            colorDialog = new ColorDialog();

            


            pic = new PictureBox()
            {
                Location = new Point(150, 50),
                Size = new Size(560, 450),
                BorderStyle = BorderStyle.Fixed3D,
                SizeMode = PictureBoxSizeMode.Normal
            };

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

            btnInfo = new Button()
            {
                Text = "Info",
                Location = new Point(820, 10),
                Size = new Size(60, 30)
            };
            btnInfo.Click += (s, e) => ShowImageInfo();

            // 🆕 Выпадающий список фильтров
            cmbFilters = new ComboBox()
            {
                Location = new Point(710, 60),
                Size = new Size(120, 30),
                DropDownStyle = ComboBoxStyle.DropDownList // запрет ввода текста
            };

            // 🆕 Добавляем варианты фильтров
            cmbFilters.Items.AddRange(new string[]
            {
                "Originaal",
                "Grayscale",
                "Red",
                "Green",
                "Blue",
                "Sepia"
            });
            cmbFilters.SelectedIndex = 0;

            // 🆕 Кнопка "Rakenda" (применить фильтр)
            btnApplyFilter = new Button()
            {
                Text = "Rakenda",
                Location = new Point(840, 60),
                Size = new Size(80, 30)
            };
            btnApplyFilter.Click += BtnApplyFilter_Click;

            chStretch = new CheckBox()
            {
                Text = "Rasta pilt",
                Location = new Point(610, 15),
                AutoSize = true
            };
            chStretch.CheckedChanged += ChStretch_CheckedChanged;

            // --- Добавляем всё на форму ---
            parent.Controls.Add(pic);
            parent.Controls.Add(btnShow);
            parent.Controls.Add(btnClear);
            parent.Controls.Add(btnBackground);
            parent.Controls.Add(btnClose);
            parent.Controls.Add(chStretch);
            parent.Controls.Add(btnSave);
            parent.Controls.Add(btnInfo);
            parent.Controls.Add(cmbFilters);
            parent.Controls.Add(btnApplyFilter);
        }

        // --- Кнопка: выбрать картинку ---
        private void BtnShow_Click(object sender, EventArgs e)
        {
            if (openFile.ShowDialog() == DialogResult.OK)
            {
                pic.Load(openFile.FileName);
                originalImage = (Image)pic.Image.Clone(); // сохраняем оригинал
            }
        }

        // --- Кнопка: очистить ---
        private void BtnClear_Click(object sender, EventArgs e)
        {
            pic.Image = null;
            originalImage = null;
        }

        // --- Кнопка: изменить фон ---
        private void BtnBackground_Click(object sender, EventArgs e)
        {
            if (colorDialog.ShowDialog() == DialogResult.OK)
            {
                pic.BackColor = colorDialog.Color;
            }
        }

        // --- Кнопка: закрыть форму ---
        private void BtnClose_Click(object sender, EventArgs e)
        {
            if (pic.FindForm() is Form form)
            {
                form.Close();
            }
        }

        // --- Чекбокс: растягивание ---
        private void ChStretch_CheckedChanged(object sender, EventArgs e)
        {
            pic.SizeMode = chStretch.Checked
                ? PictureBoxSizeMode.StretchImage
                : PictureBoxSizeMode.Normal;
        }

        // --- Кнопка: сохранить ---
        private void BtnSave_Click(object sender, EventArgs e)
        {
            if (pic.Image == null) return;

            if (saveFile.ShowDialog() == DialogResult.OK)
            {
                pic.Image.Save(saveFile.FileName);
            }
        }

        // --- Кнопка: применить выбранный фильтр ---
        private void BtnApplyFilter_Click(object sender, EventArgs e)
        {
            if (originalImage == null) return;

            string selected = cmbFilters.SelectedItem.ToString();

            switch (selected)
            {
                case "Originaal":
                    pic.Image = (Image)originalImage.Clone();
                    break;
                case "Grayscale":
                    ApplyGrayscale();
                    break;
                case "Red":
                    ApplyColorFilter("red");
                    break;
                case "Green":
                    ApplyColorFilter("green");
                    break;
                case "Blue":
                    ApplyColorFilter("blue");
                    break;
                case "Sepia":
                    ApplySepia();
                    break;
            }
        }

        // --- Фильтр: Grayscale ---
        public void ApplyGrayscale()
        {
            Bitmap bmp = new Bitmap(originalImage);
            for (int y = 0; y < bmp.Height; y++)
                for (int x = 0; x < bmp.Width; x++)
                {
                    Color c = bmp.GetPixel(x, y);
                    int gray = (int)(c.R * 0.3 + c.G * 0.59 + c.B * 0.11);
                    bmp.SetPixel(x, y, Color.FromArgb(gray, gray, gray));
                }
            pic.Image = bmp;
        }

        // --- Цветные фильтры ---
        public void ApplyColorFilter(string color)
        {
            Bitmap bmp = new Bitmap(originalImage);
            for (int y = 0; y < bmp.Height; y++)
                for (int x = 0; x < bmp.Width; x++)
                {
                    Color c = bmp.GetPixel(x, y);
                    int r = c.R, g = c.G, b = c.B;

                    switch (color)
                    {
                        case "red": g = 0; b = 0; break;
                        case "green": r = 0; b = 0; break;
                        case "blue": r = 0; g = 0; break;
                    }

                    bmp.SetPixel(x, y, Color.FromArgb(r, g, b));
                }
            pic.Image = bmp;
        }

        // --- Сепия ---
        public void ApplySepia()
        {
            Bitmap bmp = new Bitmap(originalImage);
            for (int y = 0; y < bmp.Height; y++)
                for (int x = 0; x < bmp.Width; x++)
                {
                    Color c = bmp.GetPixel(x, y);
                    int tr = (int)(0.393 * c.R + 0.769 * c.G + 0.189 * c.B);
                    int tg = (int)(0.349 * c.R + 0.686 * c.G + 0.168 * c.B);
                    int tb = (int)(0.272 * c.R + 0.534 * c.G + 0.131 * c.B);

                    bmp.SetPixel(x, y, Color.FromArgb(
                        Math.Min(255, tr),
                        Math.Min(255, tg),
                        Math.Min(255, tb)
                    ));
                }
            pic.Image = bmp;
        }

        // --- Информация ---
        public void ShowImageInfo()
        {
            if (pic.Image == null) return;

            Bitmap bmp = (Bitmap)pic.Image;
            string info = $"Suurus: {bmp.Width} x {bmp.Height}\nFormaat: {bmp.PixelFormat}";
            MessageBox.Show(info, "Informatsioon");
        }

        // --- Показать элементы ---
        public void Show()
        {
            btnShow.Visible = true;
            btnClear.Visible = true;
            btnBackground.Visible = true;
            btnClose.Visible = true;
            chStretch.Visible = true;
            btnSave.Visible = true;
            btnInfo.Visible = true;
            cmbFilters.Visible = true;
            btnApplyFilter.Visible = true;
            pic.Visible = true;
        }

        // --- Скрыть элементы ---
        public void Hide()
        {
            btnShow.Visible = false;
            btnClear.Visible = false;
            btnBackground.Visible = false;
            btnClose.Visible = false;
            chStretch.Visible = false;
            btnSave.Visible = false;
            btnInfo.Visible = false;
            cmbFilters.Visible = false;
            btnApplyFilter.Visible = false;
            pic.Visible = false;
        }
    }
}
