using System;
using System.Drawing;
using System.Windows.Forms;

namespace Kolm_rakendust
{
    public partial class Form1 : Form
    {
        TreeView tree;
        Button btn;
        Label lbl;
        private pildiVaatamise pildiVaataja;
        private MatemaatilineArarvamisMangForm mathGame;

        private MathQuiz mathQuiz;

        public Form1()
        {
            InitializeComponent();

            mathQuiz = new MathQuiz(this);
            mathQuiz.Hide();

            mathGame = new MatemaatilineArarvamisMangForm(this);
            mathGame.Hide();

            pildiVaataja = new pildiVaatamise(this);
            pildiVaataja.Hide();

            mathQuiz = new MathQuiz(this);
            mathQuiz.Hide();

            InitializeComponent();
            this.Height = 800;
            this.Width = 1000;
            this.Text = "Kolm rakendus ";

            tree = new TreeView();
            tree.Dock = DockStyle.Left;
            tree.AfterSelect += Tree_AfterSelect;

            TreeNode tn = new TreeNode("Elemendid");
            tn.Nodes.Add(new TreeNode("Pildi vaatamise programm"));
            tn.Nodes.Add(new TreeNode("Matemaatiline äraarvamismäng"));
            tn.Nodes.Add(new TreeNode("Sarnaste piltide leidmise mäng"));
            tn.Nodes.Add(new TreeNode("Välja"));

            btn = new Button();
            btn.Text = "Vajuta siia";
            btn.Location = new Point(150, 80);
            btn.Height = 80;
            btn.Width = 100;

            lbl = new Label();
            lbl.Text = "Elementide loomine c# abil";
            lbl.Font = new Font("Arial", 24);
            lbl.Size = new Size(400, 30);
            lbl.Location = new Point(150, 0);
            lbl.MouseHover += Lbl_MouseHover;
            lbl.MouseLeave += Lbl_MouseLeave;


            tree.Nodes.Add(tn);
            this.Controls.Add(tree);


            pildiVaataja = new pildiVaatamise(this);
            pildiVaataja.Hide();

        }

        private void Tree_AfterSelect(object sender, TreeViewEventArgs e)
        {

            if (e.Node.Text == "Pildi vaatamise programm")
            {
                pildiVaataja.Show();
                mathQuiz.Hide();
                mathGame.Hide();

            }
            else if (e.Node.Text == "Matemaatiline äraarvamismäng")
            {
                mathQuiz.Show();
                pildiVaataja.Hide();
                mathGame.Hide();

            }
            else if (e.Node.Text == "Sarnaste piltide leidmise mäng")
            {
                mathGame = new MatemaatilineArarvamisMangForm(this);
                pildiVaataja.Hide();
                mathQuiz.Hide();
                
            }
        }

        private void Lbl_MouseLeave(object sender, EventArgs e)
        {
            lbl.BackColor = Color.Transparent;
            Form1 Form = new Form1();
            Form.Show();
            this.Hide();
        }

        private void Lbl_MouseHover(object sender, EventArgs e)
        {
            lbl.BackColor = Color.FromArgb(200, 10, 20);
        }
    }
}
