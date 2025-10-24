using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;


namespace Kolm_rakendust
{
    public class MathQuiz
    {
        private Button startButton;
        private Button submitButton;
        private Button endQuizButton;
        private Label lblQuestion1, lblQuestion2, lblQuestion3, lblQuestion4, lblTimeLeft, lblResult;
        private NumericUpDown numAnswer1, numAnswer2, numAnswer3, numAnswer4;
        private System.Windows.Forms.Timer timer;

        private int timeLeft;
        private Control parentControl;

        public MathQuiz(Control parent)
        {
            parentControl = parent;


            startButton = new Button();
            startButton.Text = "Alusta viktoriini";
            startButton.Location = new Point(150, 10);
            startButton.Size = new Size(120, 30);
            startButton.Click += StartButton_Click;

            submitButton = new Button();
            submitButton.Text = "Esita vastused";
            submitButton.Location = new Point(280, 10);
            submitButton.Size = new Size(120, 30);
            submitButton.Click += SubmitButton_Click;
            submitButton.Enabled = false;  

            endQuizButton = new Button();
            endQuizButton.Text = "Lõpeta test";
            endQuizButton.Location = new Point(410, 10);
            endQuizButton.Size = new Size(120, 30);
            endQuizButton.Click += EndQuizButton_Click;


            lblQuestion1 = new Label() { Text = "26 + 34 =", Location = new Point(150, 50), AutoSize = true };
            lblQuestion2 = new Label() { Text = "47 - 26 =", Location = new Point(150, 90), AutoSize = true };
            lblQuestion3 = new Label() { Text = "3 × 3 =", Location = new Point(150, 130), AutoSize = true };
            lblQuestion4 = new Label() { Text = "64 ÷ 8 =", Location = new Point(150, 170), AutoSize = true };


            numAnswer1 = new NumericUpDown() { Location = new Point(220, 50), Width = 60 };
            numAnswer2 = new NumericUpDown() { Location = new Point(220, 90), Width = 60 };
            numAnswer3 = new NumericUpDown() { Location = new Point(220, 130), Width = 60 };
            numAnswer4 = new NumericUpDown() { Location = new Point(220, 170), Width = 60 };

            lblTimeLeft = new Label() { Text = "Aeg: 30 sek.", Location = new Point(300, 50), AutoSize = true };
            lblResult = new Label() { Text = "", Location = new Point(330, 90), AutoSize = true, Font = new Font("Arial", 12, FontStyle.Bold) };


            timer = new System.Windows.Forms.Timer();
            timer.Interval = 1000;
            timer.Tick += Timer_Tick;

            parentControl.Controls.Add(startButton);
            parentControl.Controls.Add(submitButton);
            parentControl.Controls.Add(endQuizButton);
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
        }

        private void StartButton_Click(object sender, EventArgs e)
        {
            timeLeft = 30;
            lblTimeLeft.Text = $"Aeg: {timeLeft} sek.";
            timer.Start();
            startButton.Enabled = false;
            submitButton.Enabled = true;
        }

        private void SubmitButton_Click(object sender, EventArgs e)
        {
            timer.Stop();
            int correctAnswers = 0;

            if (numAnswer1.Value == 26 + 34) correctAnswers++;
            if (numAnswer2.Value == 47 - 26) correctAnswers++;
            if (numAnswer3.Value == 3 * 3) correctAnswers++;
            if (numAnswer4.Value == 64 / 8) correctAnswers++;

            lblResult.Text = $"Õigeid vastuseid: {correctAnswers}/4";

            submitButton.Enabled = false;
            endQuizButton.Enabled = true;
        }


        private void EndQuizButton_Click(object sender, EventArgs e)
        {

            numAnswer1.Value = 0;
            numAnswer2.Value = 0;
            numAnswer3.Value = 0;
            numAnswer4.Value = 0;

            timeLeft = 30;
            lblTimeLeft.Text = $"Aeg: {timeLeft} sek."; 
            lblResult.Text = " ";

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

        public void Show()
        {
            startButton.Visible = true;
            submitButton.Visible = true;
            endQuizButton.Visible = true;
            lblQuestion1.Visible = true;
            lblQuestion2.Visible = true;
            lblQuestion3.Visible = true;
            lblQuestion4.Visible = true;
            numAnswer1.Visible = true;
            numAnswer2.Visible = true;
            numAnswer3.Visible = true;
            numAnswer4.Visible = true;
            lblTimeLeft.Visible = true;
            lblResult.Visible = true;
        }



        public void Hide()
        {
            startButton.Visible = false;
            submitButton.Visible = false;
            endQuizButton.Visible = false;
            lblQuestion1.Visible = false;
            lblQuestion2.Visible = false;
            lblQuestion3.Visible = false;
            lblQuestion4.Visible = false;
            numAnswer1.Visible = false;
            numAnswer2.Visible = false;
            numAnswer3.Visible = false;
            numAnswer4.Visible = false;
            lblTimeLeft.Visible = false;
            lblResult.Visible = false;
        }
    }
}
