using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.Taskbar;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.Window;

namespace Kolm_rakendust
{
    public class MatemaatilineArarvamisMang
    {
        private Label[] labels;
        private string[] icons;
        private Label firstClicked = null;
        private Label secondClicked = null;
        private Random random = new Random();
        private System.Windows.Forms.Timer flipTimer;
        private System.Windows.Forms.Timer gameTimer;
        private int matchedPairs = 0;
        private Form form;
        private int points = 0;
        private Label timeLabel;
        private Button level1Btn;
        private Button level2Btn;
        private Button level3Btn;
        private Button showScoresBtn;
        private int timeLeftSeconds;
        private bool gameActive = false;

        private const string ScoresFile = "scores.txt";

        public MatemaatilineArarvamisMang(Form form)
        {
            this.form = form ?? throw new ArgumentNullException(nameof(form));
            InitializeGame();
        }

        private void InitializeGame()
        {
            form.Text = "Matemaatiline mäng";

            timeLabel = new Label
            {
                Width = 200,
                Height = 30,
                Text = "Aeg: 00:00",
                Font = new Font("Arial", 12, FontStyle.Bold),
                Location = new Point(150, 20)
            };
            form.Controls.Add(timeLabel);

            level1Btn = new Button { Text = "Tase 1", Location = new Point(370, 20), Width = 80 };
            level1Btn.Click += (s, e) => StartLevel(1);
            form.Controls.Add(level1Btn);

            level2Btn = new Button { Text = "Tase 2", Location = new Point(460, 20), Width = 80 };
            level2Btn.Click += (s, e) => StartLevel(2);
            form.Controls.Add(level2Btn);

            level3Btn = new Button { Text = "Tase 3", Location = new Point(550, 20), Width = 80 };
            level3Btn.Click += (s, e) => StartLevel(3);
            form.Controls.Add(level3Btn);

            flipTimer = new System.Windows.Forms.Timer { Interval = 750 };
            flipTimer.Tick += FlipTimer_Tick;

            gameTimer = new System.Windows.Forms.Timer { Interval = 1000 };
            gameTimer.Tick += GameTimer_Tick;
        }

        private void StartLevel(int level)
        {
            // удалить старые карточки
            if (labels != null)
            {
                foreach (var l in labels)
                    if (l != null && form.Controls.Contains(l))
                        form.Controls.Remove(l);
            }

            // создать кнопку "Vaata tulemusi" вместе с остальными (если ещё нет)
            if (showScoresBtn == null)
            {
                showScoresBtn = new Button
                {
                    Text = "Vaata tulemusi",
                    Location = new Point(650, 20),
                    Width = 120
                };
                // <- подключаем обработчик с правильной сигнатурой
                showScoresBtn.Click += ShowScores;
                form.Controls.Add(showScoresBtn);
            }
            else
            {
                // если кнопка уже создана — просто показать её
                if (!form.Controls.Contains(showScoresBtn))
                    form.Controls.Add(showScoresBtn);
                showScoresBtn.Visible = true;
            }

            firstClicked = null;
            secondClicked = null;
            matchedPairs = 0;
            points = 0;
            gameActive = true;

            int pairs;
            switch (level)
            {
                case 1: pairs = 5; timeLeftSeconds = 30; break;
                case 2: pairs = 10; timeLeftSeconds = 50; break;
                case 3:
                default: pairs = 15; timeLeftSeconds = 60; break;
            }

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

            int startX = 150, startY = 70, x = startX, y = startY;
            int columns = 5, spacing = 110;

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
                var tmp = icons[i];
                icons[i] = icons[j];
                icons[j] = tmp;
            }
        }

        private void Label_Click(object sender, EventArgs e)
        {
            if (!gameActive || (flipTimer != null && flipTimer.Enabled)) return;

            var clickedLabel = sender as Label;
            if (clickedLabel == null || clickedLabel.Text != "?") return;

            int index = Array.IndexOf(labels, clickedLabel);
            if (index < 0 || index >= icons.Length) return;

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
            // нужно добавить ссылку на Microsoft.VisualBasic в проекте, либо сделать своё окно ввода
            string name = Microsoft.VisualBasic.Interaction.InputBox("Sisesta oma nimi:", "Mängija nimi", "Mängija");
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

        // Обработчик события Click — сигнатура должна быть (object, EventArgs)
        private void ShowScores(object sender, EventArgs e)
        {
            ShowScoresInternal();
        }

        // Вынесенная логика отображения результатов (можно вызывать откуда угодно)
        private void ShowScoresInternal()
        {
            if (!File.Exists(ScoresFile))
            {
                MessageBox.Show("Pole veel tulemusi!", "Tulemused");
                return;
            }

            try
            {
                var lines = File.ReadAllLines(ScoresFile);
                var results = lines.Select(l => l.Split(';'))
                                   .Where(a => a.Length >= 3)
                                   .Select(a => new { Aeg = a[0], Nimi = a[1], Punktid = a[2] })
                                   .ToList();

                string msg = "Tulemused:\n\n";
                foreach (var r in results.OrderByDescending(r => int.Parse(r.Punktid)))
                {
                    msg += $"{r.Nimi,-15} {r.Punktid,5} punkti ({r.Aeg})\n";
                }

                MessageBox.Show(msg, "Mängu tulemused");
            }
            catch (Exception ex)
            {
                MessageBox.Show("Viga tulemusi lugedes: " + ex.Message);
            }
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

            // Сбросить флаги
            gameActive = false;
        }

    }
}