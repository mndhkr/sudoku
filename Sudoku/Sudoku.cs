using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Sudoku
{
    public class Sudoku
    {
        public TextBox[,] Grid { get; set; }
        private Int32[,] IntGrid { get; set; }
        private LinkedList<int>[,] SolutionGrid { get; set; }

        public int GetValue(TextBox t)
        {
            int value = 0;

            try
            {
                value = Int32.Parse(t.Text);
            } catch (Exception ex)
            {
                value = 0;
            }

            return value;
        }

        private void IntGridFromTextBoxes()
        {
            IntGrid = new int[9, 9];

            for (int i = 0; i < 9; i++)
            {
                for (int j = 0; j < 9; j++)
                {
                    IntGrid[i, j] = GetValue(Grid[i, j]);
                }
            }
        }

        public void Solve()
        {
            SolutionGrid = new LinkedList<int>[9, 9];


            for (int i = 0; i < 9; i++)
            {
                for (int j = 0; j < 9; j++)
                {
                    SolutionGrid[i, j] = new LinkedList<int>();
                }
            }
        }

        public bool CheckOrizontal(int n, int i, int j)
        {
            for(int y = 0; y < 9; y++)
            {
                if (y == j) continue;
                if(n == IntGrid[i,y])
                {
                    return false;
                }
            }
            return true;
        }
    }
}
