using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;


namespace Kolm_rakendust
{
    public class Numbrimang
    {
        private Form form;
        private Label juhendLabel, tulemusLabel;
        private TextBox sisendTextBox;
        private Button arvaButton;
        private Random random;
        private int õigeNumber;

        public Numbrimang(Form parentForm)
        {
            form = parentForm;
            random = new Random();
            AlustaMäng();
        }

        private void AlustaMäng()
        {
            juhendLabel = new Label
            {
                Text = "Arva number vahemikus 1 - 100:",
                Font = new System.Drawing.Font("Arial", 12),
                Location = new System.Drawing.Point(50, 50),
                Width = 300
            };

            sisendTextBox = new TextBox
            {
                Location = new System.Drawing.Point(50, 100),
                Width = 100
            };

            arvaButton = new Button
            {
                Text = "Arva!",
                Location = new System.Drawing.Point(50, 150)
            };
            arvaButton.Click += ArvaButton_Click;

            tulemusLabel = new Label
            {
                Text = "",
                Font = new System.Drawing.Font("Arial", 12),
                Location = new System.Drawing.Point(50, 200),
                Width = 300
            };

            form.Controls.Add(juhendLabel);
            form.Controls.Add(sisendTextBox);
            form.Controls.Add(arvaButton);
            form.Controls.Add(tulemusLabel);

            õigeNumber = random.Next(1, 101);
        }

        private void ArvaButton_Click(object sender, EventArgs e)
        {
            if (int.TryParse(sisendTextBox.Text, out int kasutajaArv))
            {
                if (kasutajaArv == õigeNumber)
                {
                    tulemusLabel.Text = "Õige! Arvasite numbri!";
                }
                else if (kasutajaArv < õigeNumber)
                {
                    tulemusLabel.Text = "Liiga väike! Proovige uuesti.";
                }
                else
                {
                    tulemusLabel.Text = "Liiga suur! Proovige uuesti.";
                }
            }
            else
            {
                tulemusLabel.Text = "Palun sisestage kehtiv number!";
            }
        }
    }
}
