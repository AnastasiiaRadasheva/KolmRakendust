using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace Kolm_rakendust
{
    public class MatemaatilineArarvamisMang
    {
        private Label[] labels;
        private string[] icons;
        private Label firstClicked = null;
        private Label secondClicked = null;
        private Random random = new Random();
        private System.Windows.Forms.Timer flipTimer; // kaartide pööramise taimer
        private System.Windows.Forms.Timer gameTimer; // mängu aja taimer
        private int matchedPairs = 0;
        private Form form;
        private int points = 0;
        private Label timeLabel;
        private Button level1Btn;
        private Button level2Btn;
        private Button level3Btn;
        private int timeLeftSeconds;
        private bool gameActive = false;

        public MatemaatilineArarvamisMang(Form form)
        {
            this.form = form;
            InitializeGame();
        }

        private void InitializeGame()
        {
            form.Text = "Matemaatiline mäng";

            // aja näitamine: tunnid ja minutid
            timeLabel = new Label
            {
                Width = 200,
                Height = 30,
                Text = "Aeg: 00:00",
                Font = new Font("Arial", 12, FontStyle.Bold),
                Location = new Point(150, 20)
            };
            form.Controls.Add(timeLabel);

            level1Btn = new Button
            {
                Text = "Tase 1",
                Location = new Point(370, 20),
                Width = 80
            };
            level1Btn.Click += (s, e) => StartLevel(1);
            form.Controls.Add(level1Btn);

            level2Btn = new Button
            {
                Text = "Tase 2",
                Location = new Point(460, 20),
                Width = 80
            };
            level2Btn.Click += (s, e) => StartLevel(2);
            form.Controls.Add(level2Btn);

            level3Btn = new Button
            {
                Text = "Tase 3",
                Location = new Point(550, 20),
                Width = 80
            };
            level3Btn.Click += (s, e) => StartLevel(3);
            form.Controls.Add(level3Btn);

            // taimerid
            flipTimer = new System.Windows.Forms.Timer
            {
                Interval = 750
            };
            flipTimer.Tick += FlipTimer_Tick;

            gameTimer = new System.Windows.Forms.Timer
            {
                Interval = 1000
            };
            gameTimer.Tick += GameTimer_Tick;

            // alustame esimese tasemega
            StartLevel(1);
        }

        private void StartLevel(int level)
        {
            // eemaldame vanad kaardid, kui olemas
            if (labels != null)
            {
                foreach (var l in labels)
                {
                    if (l != null && form.Controls.Contains(l))
                        form.Controls.Remove(l);
                }
            }

            firstClicked = null;
            secondClicked = null;
            matchedPairs = 0;
            points = 0;
            gameActive = true;

            int pairs;
            switch (level)
            {
                case 1:
                    pairs = 9; // 8 kaarti
                    timeLeftSeconds = 30;
                    break;
                case 2:
                    pairs = 12; // 12 kaarti
                    timeLeftSeconds = 50;
                    break;
                case 3:
                default:
                    pairs = 15; // 16 kaarti
                    timeLeftSeconds = 50;
                    break;
            }

            // kaardid
            var list = new List<string>();
            for (int i = 1; i <= pairs; i++)
            {
                list.Add(i.ToString());
                list.Add(i.ToString());
            }
            icons = list.ToArray();
            ShuffleIcons();

            int totalCards = pairs * 2;
            labels = new Label[totalCards];

            int startX = 150;
            int startY = 70; // kaardid algavad siit
            int x = startX;
            int y = startY;
            int columns = 5; // veerud
            int spacing = 110;

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

                x += spacing;
                if ((i + 1) % columns == 0)
                {
                    x = startX;
                    y += spacing;
                }
            }

            UpdateTimeLabel();
            gameTimer.Start();
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

            if (!gameActive || (flipTimer != null && flipTimer.Enabled))
                return;

            var clickedLabel = sender as Label;
            if (clickedLabel == null)
                return;

            if (clickedLabel.Text != "?")
                return;

            int index = Array.IndexOf(labels, clickedLabel);
            if (index < 0 || index >= icons.Length)
                return;

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
                points += 10;
                firstClicked.BackColor = Color.LightGreen;
                secondClicked.BackColor = Color.LightGreen;
                ResetClickedLabels();

                if (matchedPairs == icons.Length / 2)
                {
                    gameTimer.Stop();
                    gameActive = false;
                    MessageBox.Show($"Võit! Punktid: {points}");
                }
            }
            else
            {
                flipTimer.Start();
            }
        }

        private void FlipTimer_Tick(object sender, EventArgs e)
        {
            flipTimer.Stop();

            if (firstClicked != null)
                firstClicked.Text = "?";
            if (secondClicked != null)
                secondClicked.Text = "?";

            ResetClickedLabels();
        }

        private void GameTimer_Tick(object sender, EventArgs e)
        {
            timeLeftSeconds--;
            UpdateTimeLabel();

            if (timeLeftSeconds <= 0)
            {
                gameTimer.Stop();
                gameActive = false;
                // mäng läbi
                MessageBox.Show("Aeg on otsas!");
            }
        }

        private void UpdateTimeLabel()
        {
            int minutes = timeLeftSeconds / 60;
            int seconds = timeLeftSeconds % 60;
            timeLabel.Text = $"Aeg: {minutes:00}:{seconds:00}";
        }

        private void ResetClickedLabels()
        {
            firstClicked = null;
            secondClicked = null;
        }
    }
}