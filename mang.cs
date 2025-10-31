 using Microsoft.VisualBasic;
using System;
using System.Drawing;
using System.IO;
using System.Windows.Forms;

namespace Kolm_rakendust
{
    public class MatemaatilineArarvamisMangForm
    {
        private Form form;
        private Label timeLabel;
        private Button[] levelBtns;
        private Button showScoresBtn;
        private Label[] labels;
        private string[] icons;
        private Label firstClicked, secondClicked;
        private Random random = new Random();
        private System.Windows.Forms.Timer flipTimer;
        private System.Windows.Forms.Timer gameTimer;
        private int matchedPairs, points, timeLeftSeconds;
        private bool gameActive;
        private const string ScoresFile = "scores.txt";
        private Button addTimeBtn;
        private bool bonusUsed = false;

        public MatemaatilineArarvamisMangForm(Form form)
        {
            this.form = form;
            form.Text = "Matemaatiline mäng";

            timeLabel = new Label();
            timeLabel.Text = "Aeg: 00:00";
            timeLabel.Font = new Font("Arial", 12, FontStyle.Bold);
            timeLabel.Location = new Point(150, 20);
            timeLabel.Width = 200;
            timeLabel.Height = 30;
            form.Controls.Add(timeLabel);

            levelBtns = new Button[3];
            for (int i = 0; i < 3; i++)
            {
                levelBtns[i] = new Button();
                levelBtns[i].Text = "Tase " + (i + 1);
                levelBtns[i].Location = new Point(370 + i * 90, 20);
                levelBtns[i].Width = 80;
                if (i == 0) levelBtns[i].Click += Level1_Click;
                if (i == 1) levelBtns[i].Click += Level2_Click;
                if (i == 2) levelBtns[i].Click += Level3_Click;
                form.Controls.Add(levelBtns[i]);
            }

            showScoresBtn = new Button();
            showScoresBtn.Text = "Vaata tulemusi";
            showScoresBtn.Location = new Point(650, 20);
            showScoresBtn.Width = 120;
            showScoresBtn.Click += ShowScoresBtn_Click;
            form.Controls.Add(showScoresBtn);
            addTimeBtn = new Button();
            addTimeBtn.Text = "+10 sek";
            addTimeBtn.Location = new Point(780, 20);
            addTimeBtn.Width = 80;
            addTimeBtn.Enabled = false; // кнопка активна только во время игры
            addTimeBtn.Click += AddTimeBtn_Click;
            form.Controls.Add(addTimeBtn);

            flipTimer = new System.Windows.Forms.Timer();
            flipTimer.Interval = 750;
            flipTimer.Tick += FlipTimer_Tick;

            gameTimer = new System.Windows.Forms.Timer();
            gameTimer.Interval = 1000;
            gameTimer.Tick += GameTimer_Tick;
        }

        private void Level1_Click(object sender, EventArgs e)
        {
            StartLevel(1);
        }
        private void Level2_Click(object sender, EventArgs e)
        {
            StartLevel(2);
        }
        private void Level3_Click(object sender, EventArgs e)
        {
            StartLevel(3);
        }
        private void StartLevel(int level)
        {
            bonusUsed = false;
            addTimeBtn.Enabled = true;

            // Убираем старые карточки
            RemoveOldLabels();

            // Сбрасываем переменные игры
            firstClicked = null;
            secondClicked = null;
            matchedPairs = 0;
            points = 0;
            gameActive = true;

            // Определяем количество пар в зависимости от уровня
            int pairs = 0;
            if (level == 1) pairs = 5;
            if (level == 2) pairs = 10;
            if (level == 3) pairs = 15;

            // Определяем время на уровень
            if (level == 1) timeLeftSeconds = 30;
            if (level == 2) timeLeftSeconds = 50;
            if (level == 3) timeLeftSeconds = 60;

            // Создаём массив иконок
            icons = new string[pairs * 2];
            for (int i = 0; i < pairs; i++)
            {
                icons[i * 2] = (i + 1).ToString();
                icons[i * 2 + 1] = (i + 1).ToString();
            }

            // Перемешиваем иконки
            ShuffleIcons();

            // Создаём метки для карточек
            labels = new Label[pairs * 2];
            int x = 150;
            int y = 70;
            int spacing = 110;
            int cols = 5;

            for (int i = 0; i < labels.Length; i++)
            {
                labels[i] = new Label();
                labels[i].Width = 100;
                labels[i].Height = 100;
                labels[i].Text = "?";
                labels[i].Font = new Font("Arial", 24, FontStyle.Bold);
                labels[i].TextAlign = ContentAlignment.MiddleCenter;

                if (level == 1) labels[i].BackColor = Color.LightGray;
                if (level == 2) labels[i].BackColor = Color.LightPink;
                if (level == 3) labels[i].BackColor = Color.LightYellow;

                labels[i].BorderStyle = BorderStyle.FixedSingle;
                labels[i].Location = new Point(x, y);
                labels[i].Click += CardLabel_Click;
                form.Controls.Add(labels[i]);

                x += spacing;
                if ((i + 1) % cols == 0)
                {
                    x = 150;
                    y += spacing;
                }
            }

            UpdateTimeLabel();
            gameTimer.Start();
        }


        private void RemoveOldLabels()
        {
            if (labels != null)
            {
                for (int i = 0; i < labels.Length; i++)
                {
                    form.Controls.Remove(labels[i]);
                }
            }
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

        private void CardLabel_Click(object sender, EventArgs e)
        {
            if (!gameActive || flipTimer.Enabled) return;

            var lbl = sender as Label;
            if (lbl == null || lbl.Text != "?") return;

            lbl.Text = icons[Array.IndexOf(labels, lbl)];

            if (firstClicked == null) { firstClicked = lbl; return; }

            secondClicked = lbl;

            if (firstClicked.Text == secondClicked.Text)
            {
                matchedPairs++; points += 10;
                firstClicked.BackColor = secondClicked.BackColor = Color.LightGreen;
                firstClicked = secondClicked = null;

                if (matchedPairs == icons.Length / 2)
                {
                    gameTimer.Stop(); gameActive = false;
                    string player = Interaction.InputBox("Sisesta oma nimi:", "Mängija nimi", "Mängija");
                    if (string.IsNullOrEmpty(player)) player = "Tundmatu";
                    try { File.AppendAllLines(ScoresFile, new[] { $"{DateTime.Now:yyyy-MM-dd HH:mm};{player};{points}" }); } catch { }
                    MessageBox.Show("Võit! Punktid: " + points);
                    addTimeBtn.Enabled = false;

                }
            }
            else flipTimer.Start();
        }

        private void AddTimeBtn_Click(object sender, EventArgs e)
        {
            if (bonusUsed || !gameActive) return; // можно использовать только 1 раз

            timeLeftSeconds += 10;
            bonusUsed = true;
            addTimeBtn.Enabled = false;
            UpdateTimeLabel();
        }

        private void FlipTimer_Tick(object sender, EventArgs e)
        {
            flipTimer.Stop();
            if (firstClicked != null) firstClicked.Text = "?";
            if (secondClicked != null) secondClicked.Text = "?";
            firstClicked = null;
            secondClicked = null;
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
                addTimeBtn.Enabled = false;

            }
        }

        private void UpdateTimeLabel()
        {
            int min = timeLeftSeconds / 60;
            int sec = timeLeftSeconds % 60;
            timeLabel.Text = "Aeg: " + min.ToString("00") + ":" + sec.ToString("00");
        }

        private void ShowScoresBtn_Click(object sender, EventArgs e)
        {
            if (!File.Exists(ScoresFile))
            {
                MessageBox.Show("Tulemusi pole!");
                return;
            }
            string[] lines = File.ReadAllLines(ScoresFile);
            string msg = "Tulemused:\n\n";
            for (int i = 0; i < lines.Length; i++)
            {
                string[] p = lines[i].Split(';');
                msg += p[1].PadRight(15) + " " + p[2].PadLeft(5) + " punkti (" + p[0] + ")\n";
            }
            MessageBox.Show(msg, "Mängu tulemused");
        }

        public void Hide()
        {
            gameTimer.Stop();
            flipTimer.Stop();
            if (labels != null)
            {
                for (int i = 0; i < labels.Length; i++)
                {
                    form.Controls.Remove(labels[i]);
                }
            }
            form.Controls.Remove(timeLabel);
            for (int i = 0; i < levelBtns.Length; i++)
            {
                form.Controls.Remove(levelBtns[i]);
            }
            form.Controls.Remove(showScoresBtn);
            gameActive = false;
            form.Controls.Remove(addTimeBtn);

        }
    }
}
