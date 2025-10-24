using System;
using System.Drawing;
using System.Windows.Forms;

namespace Kolm_rakendust
{
    public class MatemaatilineArarvamisMang
    {
        private Label[] labels;
        private string[] icons = { "1", "2", "3", "4", "5", "6", "7", "8", "1", "2", "3", "4", "5", "6", "7", "8" };
        private Label firstClicked = null;
        private Label secondClicked = null;
        private Random random = new Random();
        private System.Windows.Forms.Timer timer;  
        private int matchedPairs = 0;
        private Form form;

        public MatemaatilineArarvamisMang(Form form)
        {
            this.form = form;
            InitializeGame();
        }

        private void InitializeGame()
        {
            form.Text = "Матч-игра";



            labels = new Label[16];
            int x = 150;
            int y = 150;


            ShuffleIcons();


            for (int i = 0; i < labels.Length; i++)
            {
                labels[i] = new Label
                {
                    Width = 100,
                    Height = 100,
                    Text = "?",
                    Font = new Font("Arial", 24, FontStyle.Bold),
                    TextAlign = ContentAlignment.MiddleCenter,
                    BackColor = Color.LightGray,
                    BorderStyle = BorderStyle.FixedSingle,
                    Location = new Point(x, y)
                };

                labels[i].Click += Label_Click;
                form.Controls.Add(labels[i]);

                x += 110;  
                if (x >= 600)
                {
                    x = 150;
                    y += 110;  
                }
            }


            timer = new System.Windows.Forms.Timer 
            {
                Interval = 750
            };
            timer.Tick += Timer_Tick;
        }

        private void ShuffleIcons()
        {

            for (int i = 0; i < icons.Length; i++)
            {
                int j = random.Next(icons.Length);
                string temp = icons[i];
                icons[i] = icons[j];
                icons[j] = temp;
            }
        }

        private void Label_Click(object sender, EventArgs e)
        {
            // Если таймер сейчас работает (ожидание скрытия), игнорируем дополнительные клики
            if (timer != null && timer.Enabled)
                return;

            var clickedLabel = sender as Label;
            if (clickedLabel == null)
                return;

            if (clickedLabel.Text != "?")
                return;


            int index = Array.IndexOf(labels, clickedLabel);
            clickedLabel.Text = icons[index];


            if (firstClicked == null)
            {
                firstClicked = clickedLabel;
                return;
            }


            secondClicked = clickedLabel;

     
            if (firstClicked.Text == secondClicked.Text)
            {
                matchedPairs++;
                ResetClickedLabels();
              
                if (matchedPairs == icons.Length / 2)
                {
                    MessageBox.Show("Вы победили!");
                }
            }
            else
            {
                timer.Start();
            }
        }

        private void Timer_Tick(object sender, EventArgs e)
        {
            timer.Stop();

            if (firstClicked != null)
                firstClicked.Text = "?";
            if (secondClicked != null)
                secondClicked.Text = "?";

            ResetClickedLabels();
        }

        private void ResetClickedLabels()
        {
            firstClicked = null;
            secondClicked = null;
        }
    }
}
