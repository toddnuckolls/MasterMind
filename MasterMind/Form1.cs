using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics.Eventing.Reader;
using System.Drawing;
using System.Linq;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MasterMind
{
    public partial class Form1 : Form
    {
        int column = 1;
        int row = 1;
        public Form1()
        {
            InitializeComponent();

            
            foreach (GroupBox group in panel1.Controls.OfType<GroupBox>())
            {
                
                foreach(Button button in group.Controls.OfType<Button>())
                {
                    button.Tag = column;
                    column++;
                }
                column = 1;
                group.Tag = row;
                row++;
            }
            column = 1;
            row = 1;
        }
        
        private void Form1_Load(object sender, EventArgs e)
        {
            Random rnd = new Random();
            foreach (Button b in grpSecretCode.Controls)
            {
                
                int colorNum = rnd.Next(1, 6);
                
                if(colorNum == 1)
                    b.BackColor = Color.White;
                else if(colorNum == 2)
                    b.BackColor = Color.Red;
                else if (colorNum == 3)
                    b.BackColor = Color.Blue;
                else if(colorNum == 4)
                    b.BackColor= Color.Green;
                else if(colorNum == 5)
                    b.BackColor = Color.Orange;
                else
                    b.BackColor = Color.Black;
            }
        }

        private void ColorClick(Color color)
        {
            if (row != 8)
            {
                if (column == 4)
                {   
                    WhichButton(row, column).BackColor = color;
                    foreach(GroupBox g in panel1.Controls.OfType<GroupBox>())
                    {
                        if (g.Tag.ToString() == row.ToString())
                        {
                            if (CompareCodes(g))
                            {
                                grpSecretCode.Visible = true;
                                lblResult.Visible = true;
                            }  
                            else
                            {
                                CountPins(g);
                                row++;
                                column = 1;
                                break;
                            }
                        }
                    }
                }
                else
                {
                    WhichButton(row, column).BackColor = color;
                    column++;
                }
            }
            else
            {
                if (column != 4)
                {
                    WhichButton(row, column).BackColor = color;
                    column++;
                }
                else
                {
                    WhichButton(row, column).BackColor = color;
                    foreach (GroupBox g in panel1.Controls.OfType<GroupBox>())
                    {
                        if (g.Tag.ToString() == row.ToString())
                        {
                            if (CompareCodes(g))
                            {
                                grpSecretCode.Visible = true;
                                lblResult.Visible = true;
                                btnBlack.Enabled = false;
                                btnRed.Enabled = false;
                                btnGreen.Enabled = false;
                                btnBlue.Enabled = false;
                                btnOrange.Enabled = false;
                                btnWhite.Enabled = false;
                            }

                            else
                            {
                                lblResult.Text = "You lose";
                                lblResult.Visible = true;
                                grpSecretCode.Visible = true;
                                btnBlack.Enabled = false;
                                btnRed.Enabled = false;
                                btnGreen.Enabled = false;
                                btnBlue.Enabled = false;
                                btnOrange.Enabled = false;
                                btnWhite.Enabled = false;
                            }
                        }
                    }
                    
                }
            }
        }
        
        private bool CompareCodes(GroupBox g)
        {
            Color[] guess = new Color[4];
            int k = 0;
            foreach (Button b in g.Controls)
            {
                guess[k] = b.BackColor;
                k++;
            }
            Color[] code = { btnCode1.BackColor, btnCode2.BackColor, btnCode3.BackColor, btnCode4.BackColor };


            return guess.SequenceEqual(code);
        }

        private void CountPins(GroupBox g)
        {
            Color[] guess = new Color[4];
            int k = 0;
            foreach (Button b in g.Controls)
            {
                guess[k] = b.BackColor;
                k++;
            }
            Color[] code = {btnCode1.BackColor, btnCode2.BackColor, btnCode3.BackColor, btnCode4.BackColor};

            bool[] guessChecked = new bool[4];
            bool[] codeChecked = new bool[4];
            int redPins = 0;
            int whitePins = 0;

            // Count red pins
            for (int i = 0; i < 4; i++)
            {
                if (guess[i] == code[i])
                {
                    redPins++;
                    guessChecked[i] = true;
                    codeChecked[i] = true;
                }
            }

            // Count white pins
            for (int i = 0; i < 4; i++)
            {
                if (!guessChecked[i])
                {
                    for (int j = 0; j < 4; j++)
                    {
                        if (!codeChecked[j] && guess[i] == code[j])
                        {
                            whitePins++;
                            codeChecked[j] = true;
                            break;
                        }
                    }
                }
            }
            foreach(Label lbl in panel1.Controls.OfType<Label>())
            {
                if(lbl.Text == "null")
                {
                    if (lbl.Tag.ToString() == row.ToString())
                    {
                        lbl.Visible = true;
                        lbl.Text = "black pins: " + redPins + " white pins: " + whitePins;
                    }
                    
                }
                   
            }
                

        }

        private Button WhichButton(int row, int column)
        {
            foreach (GroupBox group in panel1.Controls.OfType<GroupBox>())
            {
                if(group.Tag.ToString() == row.ToString())
                {
                    foreach(Button b in group.Controls)
                    {
                        if(b.Tag.ToString() == column.ToString())
                        {
                            return b;
                        }
                    }
                }
            }
            return button1;
        }

        private void btnWhite_Click(object sender, EventArgs e)
        {
            ColorClick(Color.White);
        }

        private void btnRed_Click(object sender, EventArgs e)
        {
            ColorClick(Color.Red);
        }

        private void btnBlue_Click(object sender, EventArgs e)
        {
            ColorClick(Color.Blue);
        }

        private void btnGreen_Click(object sender, EventArgs e)
        {
            ColorClick(Color.Green);
        }

        private void btnOrange_Click(object sender, EventArgs e)
        {
            ColorClick(Color.Orange);
        }

        private void btnBlack_Click(object sender, EventArgs e)
        {
            ColorClick(Color.Black);
        }

        private void btnUndo_Click(object sender, EventArgs e)
        {
            if(column != 1)
            {
                column--;
                foreach(GroupBox g in panel1.Controls.OfType<GroupBox>())
                {
                    if(g.Tag.ToString() == row.ToString())
                    {
                        foreach (Button b in g.Controls)
                        {
                            if (b.Tag.ToString() == column.ToString())
                            {
                                b.BackColor = SystemColors.ControlDarkDark;
                                break;
                                
                            }
                        }
                        break;
                    }
                }
            }
        }

        private void btnNewGame_Click(object sender, EventArgs e)
        {
            btnBlack.Enabled = true;
            btnGreen.Enabled = true;
            btnOrange.Enabled = true;
            btnRed.Enabled = true;
            btnBlue.Enabled = true;
            btnWhite.Enabled = true;
            lblResult.Visible = false;
            lblResult.Text = "You win!";
            row = 1;
            column = 1;
            foreach(GroupBox g in panel1.Controls.OfType<GroupBox>())
            {
                foreach(Button b in g.Controls)
                {
                    b.BackColor = SystemColors.ControlDarkDark;
                }
            }
            foreach(Label l in panel1.Controls.OfType<Label>())
            {
                l.Visible = false;
                l.Text = "null";
            }
            Random rnd = new Random();
            foreach (Button b in grpSecretCode.Controls)
            {

                int colorNum = rnd.Next(1, 6);

                if (colorNum == 1)
                    b.BackColor = Color.White;
                else if (colorNum == 2)
                    b.BackColor = Color.Red;
                else if (colorNum == 3)
                    b.BackColor = Color.Blue;
                else if (colorNum == 4)
                    b.BackColor = Color.Green;
                else if (colorNum == 5)
                    b.BackColor = Color.Orange;
                else
                    b.BackColor = Color.Black;
            }
            grpSecretCode.Visible = false;
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
