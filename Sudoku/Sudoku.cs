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
        private LinkedList<int>[,] CandidatesGrid { get; set; }

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
            CandidatesGrid = new LinkedList<int>[9, 9];


            for (int i = 0; i < 9; i++)
            {
                for (int j = 0; j < 9; j++)
                {
                    CandidatesGrid[i, j] = new LinkedList<int>();
                }
            }

            FindCandidates();
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

        public bool CheckVertical(int n, int i, int j)
        {
            for (int x = 0; x < 9; x++)
            {
                if (x == i) continue;
                if (n == IntGrid[j, x])
                {
                    return false;
                }
            }
            return true;
        }

        public bool CheckBox(int n, int i, int j)
        {
            int xBox = -1;
            if(i >= 0 && i < 3)
            {
                xBox = 0;
            } else if (i >= 3 && i < 6)
            {
                xBox = 1;
            } else if(i >= 6 && i < 9)
            {
                xBox = 2;
            }

            int yBox = -1;
            if(j >= 0 && j < 3)
            {
                yBox = 0;
            } else if(j >= 3 && j < 6)
            {
                yBox = 1;
            } else if (j >= 6 && j < 9)
            {
                yBox = 2;
            }

            for(int x = xBox*3; x < xBox*3 + 3; x++)
            {
                for(int y = yBox*3; y < yBox*3+3; y++)
                {
                    if(x == i && y == j)
                    {
                        continue;
                    }
                    if(IntGrid[x,y] == n)
                    {
                        return false;
                    }
                }
            }
            return true;
        }

        public void FindCandidates()
        {
            for(int x = 0; x < 9; x++)
            {
                for(int y = 0; y < 9; y++)
                {
                    for(int n = 1; n < 10; n++)
                    {
                        if(CheckOrizontal(n, x, y) && CheckVertical(n, x, y) && CheckBox(n, x, y))
                        {
                            CandidatesGrid[x, y].AddLast(n);
                            Console.WriteLine("Aggiungo {}, in {},{}", n, x, y);
                        }
                    }
                }
            }
        }
    }
}
