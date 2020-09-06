using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Sudoku
{
    public partial class Form1 : Form
    {
        public Sudoku Sudoku { get; set; }

        public Form1()
        {
            InitializeComponent();

            Sudoku.Grid = new TextBox[9, 9];

            this.Sudoku.Grid[0, 0] = textBox1;
            this.Sudoku.Grid[0, 1] = textBox2;
            this.Sudoku.Grid[0, 2] = textBox3;
            this.Sudoku.Grid[0, 3] = textBox4;
            this.Sudoku.Grid[0, 4] = textBox5;
            this.Sudoku.Grid[0, 5] = textBox6;
            this.Sudoku.Grid[0, 6] = textBox7;
            this.Sudoku.Grid[0, 7] = textBox8;
            this.Sudoku.Grid[0, 8] = textBox9;

            this.Sudoku.Grid[1, 0] = textBox10;
            this.Sudoku.Grid[1, 1] = textBox11;
            this.Sudoku.Grid[1, 2] = textBox12;
            this.Sudoku.Grid[1, 3] = textBox13;
            this.Sudoku.Grid[1, 4] = textBox14;
            this.Sudoku.Grid[1, 5] = textBox15;
            this.Sudoku.Grid[1, 6] = textBox16;
            this.Sudoku.Grid[1, 7] = textBox17;
            this.Sudoku.Grid[1, 8] = textBox18;

            this.Sudoku.Grid[2, 0] = textBox19;
            this.Sudoku.Grid[2, 1] = textBox20;
            this.Sudoku.Grid[2, 2] = textBox21;
            this.Sudoku.Grid[2, 3] = textBox22;
            this.Sudoku.Grid[2, 4] = textBox23;
            this.Sudoku.Grid[2, 5] = textBox24;
            this.Sudoku.Grid[2, 6] = textBox25;
            this.Sudoku.Grid[2, 7] = textBox26;
            this.Sudoku.Grid[2, 8] = textBox27;

            this.Sudoku.Grid[3, 0] = textBox28;
            this.Sudoku.Grid[3, 1] = textBox29;
            this.Sudoku.Grid[3, 2] = textBox30;
            this.Sudoku.Grid[3, 3] = textBox31;
            this.Sudoku.Grid[3, 4] = textBox32;
            this.Sudoku.Grid[3, 5] = textBox33;
            this.Sudoku.Grid[3, 6] = textBox34;
            this.Sudoku.Grid[3, 7] = textBox35;
            this.Sudoku.Grid[3, 8] = textBox36;




            this.Sudoku.Grid[4, 0] = textBox37;
            this.Sudoku.Grid[4, 1] = textBox38;
            this.Sudoku.Grid[4, 2] = textBox39;
            this.Sudoku.Grid[4, 3] = textBox40;
            this.Sudoku.Grid[4, 4] = textBox41;
            this.Sudoku.Grid[4, 5] = textBox42;
            this.Sudoku.Grid[4, 6] = textBox43;
            this.Sudoku.Grid[4, 7] = textBox44;
            this.Sudoku.Grid[4, 8] = textBox45;

            this.Sudoku.Grid[5, 0] = textBox46;
            this.Sudoku.Grid[5, 1] = textBox47;
            this.Sudoku.Grid[5, 2] = textBox48;
            this.Sudoku.Grid[5, 3] = textBox49;
            this.Sudoku.Grid[5, 4] = textBox50;
            this.Sudoku.Grid[5, 5] = textBox51;
            this.Sudoku.Grid[5, 6] = textBox52;
            this.Sudoku.Grid[5, 7] = textBox53;
            this.Sudoku.Grid[5, 8] = textBox54;

            this.Sudoku.Grid[6, 0] = textBox55;
            this.Sudoku.Grid[6, 1] = textBox56;
            this.Sudoku.Grid[6, 2] = textBox57;
            this.Sudoku.Grid[6, 3] = textBox58;
            this.Sudoku.Grid[6, 4] = textBox59;
            this.Sudoku.Grid[6, 5] = textBox60;
            this.Sudoku.Grid[6, 6] = textBox61;
            this.Sudoku.Grid[6, 7] = textBox62;
            this.Sudoku.Grid[6, 8] = textBox63;

            this.Sudoku.Grid[7, 0] = textBox64;
            this.Sudoku.Grid[7, 1] = textBox65;
            this.Sudoku.Grid[7, 2] = textBox66;
            this.Sudoku.Grid[7, 3] = textBox67;
            this.Sudoku.Grid[7, 4] = textBox68;
            this.Sudoku.Grid[7, 5] = textBox69;
            this.Sudoku.Grid[7, 6] = textBox70;
            this.Sudoku.Grid[7, 7] = textBox71;
            this.Sudoku.Grid[7, 8] = textBox72;


            this.Sudoku.Grid[8, 0] = textBox73;
            this.Sudoku.Grid[8, 1] = textBox74;
            this.Sudoku.Grid[8, 2] = textBox75;
            this.Sudoku.Grid[8, 3] = textBox76;
            this.Sudoku.Grid[8, 4] = textBox77;
            this.Sudoku.Grid[8, 5] = textBox78;
            this.Sudoku.Grid[8, 6] = textBox79;
            this.Sudoku.Grid[8, 7] = textBox80;
            this.Sudoku.Grid[8, 8] = textBox81;
        }

        private void button1_Click(object sender, EventArgs e)
        {

        }
    }
}
