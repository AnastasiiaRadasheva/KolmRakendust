using Microsoft.VisualBasic;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows.Forms;

namespace Kolm_rakendust
{
    public class MatemaatilineArarvamisMangForm
    {
        private Form form;
        private Label timeLabel;
        private Button level1Btn, level2Btn, level3Btn, showScoresBtn;
        private Label[] labels;
        private string[] icons;
        private Label firstClicked, secondClicked;
        private Random random = new Random();
        private System.Windows.Forms.Timer flipTimer, gameTimer;
        private int matchedPairs;
        private int points;
        private int timeLeftSeconds;
        private bool gameActive;
        private const string ScoresFile = "scores.txt";

        public MatemaatilineArarvamisMangForm(Form form)
        {
            this.form = form ?? throw new ArgumentNullException(nameof(form));
            InitializeComponents();
        }

        private void InitializeComponents()
        {
            form.Text = "Matemaatiline mäng";

            // Время
            timeLabel = new Label
            {
                Text = "Aeg: 00:00",
                Font = new Font("Arial", 12, FontStyle.Bold),
                Location = new Point(150, 20),
                Width = 200,
                Height = 30
            };
            form.Controls.Add(timeLabel);

            // Кнопки уровней
            level1Btn = CreateLevelButton("Tase 1", 370, Level1_Click);
            level2Btn = CreateLevelButton("Tase 2", 460, Level2_Click);
            level3Btn = CreateLevelButton("Tase 3", 550, Level3_Click);

            // Таймеры
            flipTimer = new System.Windows.Forms.Timer { Interval = 750 };
            flipTimer.Tick += FlipTimer_Tick;

            gameTimer = new System.Windows.Forms.Timer { Interval = 1000 };
            gameTimer.Tick += GameTimer_Tick;

            // Кнопка просмотра результатов
            showScoresBtn = new Button
            {
                Text = "Vaata tulemusi",
                Location = new Point(650, 20),
                Width = 120
            };
            showScoresBtn.Click += ShowScoresBtn_Click;
            form.Controls.Add(showScoresBtn);
        }

        private Button CreateLevelButton(string text, int x, EventHandler clickHandler)
        {
            var btn = new Button
            {
                Text = text,
                Location = new Point(x, 20),
                Width = 80
            };
            btn.Click += clickHandler;
            form.Controls.Add(btn);
            return btn;
        }

        private void Level1_Click(object sender, EventArgs e) => StartLevel(1);
        private void Level2_Click(object sender, EventArgs e) => StartLevel(2);
        private void Level3_Click(object sender, EventArgs e) => StartLevel(3);

        private void StartLevel(int level)
        {
            ClearOldLabels();

            firstClicked = null;
            secondClicked = null;
            matchedPairs = 0;
            points = 0;
            gameActive = true;

            int pairs = level switch
            {
                1 => 5,
                2 => 10,
                3 => 15,
                _ => 5
            };

            timeLeftSeconds = level switch
            {
                1 => 30,
                2 => 50,
                3 => 60,
                _ => 30
            };

            CreateIcons(pairs);
            ShuffleIcons();
            CreateCardLabels(pairs * 2);

            UpdateTimeLabel();
            gameTimer.Start();
        }

        private void ClearOldLabels()
        {
            if (labels != null)
            {
                foreach (var lbl in labels)
                    if (lbl != null && form.Controls.Contains(lbl))
                        form.Controls.Remove(lbl);
            }
        }

        private void CreateIcons(int pairs)
        {
            var list = new List<string>();
            for (int i = 1; i <= pairs; i++)
            {
                list.Add(i.ToString());
                list.Add(i.ToString());
            }
            icons = list.ToArray();
        }

        private void ShuffleIcons()
        {
            for (int i = 0; i < icons.Length; i++)
            {
                int j = random.Next(icons.Length);
                var tmp = icons[i];
                icons[i] = icons[j];
                icons[j] = tmp;
            }
        }

        private void CreateCardLabels(int totalCards)
        {
            labels = new Label[totalCards];
            int startX = 150, startY = 70, spacing = 110, columns = 5;
            int x = startX, y = startY;

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
                labels[i].Click += CardLabel_Click;
                form.Controls.Add(labels[i]);

                x += spacing;
                if ((i + 1) % columns == 0)
                {
                    x = startX;
                    y += spacing;
                }
            }
        }

        private void CardLabel_Click(object sender, EventArgs e)
        {
            if (!gameActive || flipTimer.Enabled) return;

            var clickedLabel = sender as Label;
            if (clickedLabel == null || clickedLabel.Text != "?") return;

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
                points += 10;
                firstClicked.BackColor = Color.LightGreen;
                secondClicked.BackColor = Color.LightGreen;
                ResetClickedLabels();

                if (matchedPairs == icons.Length / 2)
                {
                    gameTimer.Stop();
                    gameActive = false;
                    string player = PromptForName();
                    SaveScore(player, points);
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
            if (firstClicked != null) firstClicked.Text = "?";
            if (secondClicked != null) secondClicked.Text = "?";
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

        private string PromptForName()
        {
            string name = Interaction.InputBox("Sisesta oma nimi:", "Mängija nimi", "Mängija");
            return string.IsNullOrWhiteSpace(name) ? "Tundmatu" : name;
        }

        private void SaveScore(string player, int score)
        {
            try
            {
                string line = $"{DateTime.Now:yyyy-MM-dd HH:mm};{player};{score}";
                File.AppendAllLines(ScoresFile, new[] { line });
            }
            catch (Exception ex)
            {
                MessageBox.Show("Tulemuse salvestamine ebaõnnestus: " + ex.Message);
            }
        }

        private void ShowScoresBtn_Click(object sender, EventArgs e)
        {
            ShowScoresInternal();
        }

        private void ShowScoresInternal()
        {
            string[] lines = File.ReadAllLines(ScoresFile);

            string msg = "Tulemused:\n\n";

            for (int i = 0; i < lines.Length; i++)
            {
                string[] parts = lines[i].Split(';');
                msg += parts[1].PadRight(15) + " " + parts[2].PadLeft(5) + " punkti (" + parts[0] + ")\n";
            }

            MessageBox.Show(msg, "Mängu tulemused");
        }


        public void Hide()
        {
            gameTimer?.Stop();
            flipTimer?.Stop();

            if (labels != null)
            {
                foreach (var l in labels)
                    form.Controls.Remove(l);
                labels = null;
            }

            form.Controls.Remove(timeLabel);
            form.Controls.Remove(level1Btn);
            form.Controls.Remove(level2Btn);
            form.Controls.Remove(level3Btn);
            form.Controls.Remove(showScoresBtn);

            gameActive = false;
        }
    }
}
