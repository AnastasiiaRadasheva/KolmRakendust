using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace Kolm_rakendust
{
    public class MathQuiz
    {
        private Button startButton;
        private Button submitButton;
        private Button endQuizButton;
        private Button showScoresButton;

        private Label lblQuestion1, lblQuestion2, lblQuestion3, lblQuestion4, lblTimeLeft, lblResult;
        private NumericUpDown numAnswer1, numAnswer2, numAnswer3, numAnswer4;
        private System.Windows.Forms.Timer timer;

        private int timeLeft;
        private int points = 0;
        private static List<int> allScores = new List<int>();
        private Control parentControl;

        private int[] correctAnswers = new int[4];
        private Random random = new Random();

        public MathQuiz(Control parent)
        {
            parentControl = parent;


            startButton = new Button()
            {
                Text = "Alusta viktoriini",
                Location = new Point(150, 10),
                Size = new Size(120, 30)
            };
            startButton.Click += StartButton_Click;

            submitButton = new Button()
            {
                Text = "Esita vastused",
                Location = new Point(280, 10),
                Size = new Size(120, 30),
                Enabled = false
            };
            submitButton.Click += SubmitButton_Click;

            endQuizButton = new Button()
            {
                Text = "Lõpeta test",
                Location = new Point(410, 10),
                Size = new Size(120, 30),
                Enabled = false
            };
            endQuizButton.Click += EndQuizButton_Click;

            showScoresButton = new Button()
            {
                Text = "Tulemused",
                Location = new Point(540, 10),
                Size = new Size(120, 30)
            };
            showScoresButton.Click += (s, e) => ShowScores();

            // --- вопросы и поля ---
            lblQuestion1 = new Label() { Location = new Point(150, 50), AutoSize = true };
            lblQuestion2 = new Label() { Location = new Point(150, 90), AutoSize = true };
            lblQuestion3 = new Label() { Location = new Point(150, 130), AutoSize = true };
            lblQuestion4 = new Label() { Location = new Point(150, 170), AutoSize = true };

            numAnswer1 = new NumericUpDown() { Location = new Point(250, 50), Width = 60 };
            numAnswer2 = new NumericUpDown() { Location = new Point(250, 90), Width = 60 };
            numAnswer3 = new NumericUpDown() { Location = new Point(250, 130), Width = 60 };
            numAnswer4 = new NumericUpDown() { Location = new Point(250, 170), Width = 60 };

            lblTimeLeft = new Label() { Text = "Aeg: 30 sek.", Location = new Point(330, 50), AutoSize = true };
            lblResult = new Label() { Text = "", Location = new Point(330, 90), AutoSize = true, Font = new Font("Arial", 12, FontStyle.Bold) };

            timer = new System.Windows.Forms.Timer { Interval = 1000 };
            timer.Tick += Timer_Tick;

            // --- добавляем элементы ---
            parentControl.Controls.Add(startButton);
            parentControl.Controls.Add(submitButton);
            parentControl.Controls.Add(endQuizButton);
            parentControl.Controls.Add(showScoresButton);
            parentControl.Controls.Add(lblQuestion1);
            parentControl.Controls.Add(lblQuestion2);
            parentControl.Controls.Add(lblQuestion3);
            parentControl.Controls.Add(lblQuestion4);
            parentControl.Controls.Add(numAnswer1);
            parentControl.Controls.Add(numAnswer2);
            parentControl.Controls.Add(numAnswer3);
            parentControl.Controls.Add(numAnswer4);
            parentControl.Controls.Add(lblTimeLeft);
            parentControl.Controls.Add(lblResult);

            // скрываем вопросы и ответы до старта
            SetQuizVisible(false);
        }

        private void GenerateQuestions()
        {
            for (int i = 0; i < 4; i++)
            {
                string question;
                int a, b, answer;
                char op;

                switch (random.Next(4))
                {
                    case 0:
                        op = '+';
                        a = random.Next(1, 50);
                        b = random.Next(1, 50);
                        answer = a + b;
                        break;
                    case 1:
                        op = '-';
                        a = random.Next(10, 100);
                        b = random.Next(1, a);
                        answer = a - b;
                        break;
                    case 2:
                        op = '×';
                        a = random.Next(1, 12);
                        b = random.Next(1, 12);
                        answer = a * b;
                        break;
                    default:
                        op = '÷';
                        b = random.Next(2, 10);
                        answer = random.Next(2, 10);
                        a = b * answer;
                        break;
                }

                question = $"{a} {op} {b} =";
                correctAnswers[i] = answer;

                switch (i)
                {
                    case 0: lblQuestion1.Text = question; break;
                    case 1: lblQuestion2.Text = question; break;
                    case 2: lblQuestion3.Text = question; break;
                    case 3: lblQuestion4.Text = question; break;
                }
            }
        }

        private void StartButton_Click(object sender, EventArgs e)
        {
            GenerateQuestions();      // генерируем новые вопросы
            SetQuizVisible(true);     // показываем их

            timeLeft = 30;
            lblTimeLeft.Text = $"Aeg: {timeLeft} sek.";
            timer.Start();

            startButton.Enabled = false;
            submitButton.Enabled = true;
            endQuizButton.Enabled = true;
            lblResult.Text = "";
        }

        private void SubmitButton_Click(object sender, EventArgs e)
        {
            timer.Stop();
            int correct = 0;
            NumericUpDown[] answers = { numAnswer1, numAnswer2, numAnswer3, numAnswer4 };

            for (int i = 0; i < 4; i++)
            {
                if (answers[i].Value == correctAnswers[i]) correct++;
            }

            points = correct * 10;
            allScores.Add(points);

            lblResult.Text = $"Õigeid vastuseid: {correct}/4  |  Punktid: {points}";
            submitButton.Enabled = false;
        }

        private void EndQuizButton_Click(object sender, EventArgs e)
        {
            timer.Stop();

            foreach (var num in new[] { numAnswer1, numAnswer2, numAnswer3, numAnswer4 })
                num.Value = 0;

            lblResult.Text = "";
            lblTimeLeft.Text = "Aeg: 30 sek.";

            SetQuizVisible(false);   // скрываем вопросы и поля

            startButton.Enabled = true;
            submitButton.Enabled = false;
            endQuizButton.Enabled = false;
        }

        private void Timer_Tick(object sender, EventArgs e)
        {
            if (timeLeft > 0)
            {
                timeLeft--;
                lblTimeLeft.Text = $"Aeg: {timeLeft} sek.";
            }
            else
            {
                timer.Stop();
                SubmitButton_Click(sender, e);
            }
        }

        private void ShowScores()
        {
            if (allScores.Count == 0)
            {
                MessageBox.Show("Veel pole ühtegi tulemust!", "Tulemused");
                return;
            }

            string results = ""; // создаём пустую строку для результатов

            for (int i = 0; i < allScores.Count; i++)
            {
                results += (i + 1) + ". " + allScores[i] + " punkti\n"; // добавляем каждый результат
            }

            MessageBox.Show(results, "Tulemused");
        }

        // Управление видимостью вопросов и ответов
        private void SetQuizVisible(bool visible)
        {
            lblQuestion1.Visible = visible;
            lblQuestion2.Visible = visible;
            lblQuestion3.Visible = visible;
            lblQuestion4.Visible = visible;
            numAnswer1.Visible = visible;
            numAnswer2.Visible = visible;
            numAnswer3.Visible = visible;
            numAnswer4.Visible = visible;
        }

        public void Show()
        {
            startButton.Visible = true;
            submitButton.Visible = true;
            endQuizButton.Visible = true;
            showScoresButton.Visible = true;
            lblTimeLeft.Visible = true;
            lblResult.Visible = true;
            SetQuizVisible(false); // вопросы спрятаны до старта
        }

        public void Hide()
        {
            startButton.Visible = false;
            submitButton.Visible = false;
            endQuizButton.Visible = false;
            showScoresButton.Visible = false;
            lblTimeLeft.Visible = false;
            lblResult.Visible = false;
            SetQuizVisible(false);
        }
    }
}
