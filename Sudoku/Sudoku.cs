using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Sudoku
{
    public class Sudoku
    {
        public TextBox[,] Grid { get; set; }
        private Int32[,] IntGrid { get; set; }
        private LinkedList<int>[,] CandidatesGrid { get; set; }

        private Int32[,] SavedState { get; set; }
        private int LastTriedCandidate { get; set; }
        private bool StateSaved { get; set; } = false;

        private int Zeroes { get; set; } = 0;
        private bool Modified { get; set; } = false;

        private bool Guessed { get; set; } = false;

        public Sudoku()
        {
            Grid = new TextBox[9, 9];
            IntGrid = new int[9, 9];
            InitCandidates();
        }

        public int GetValue(TextBox t)
        {
            int value = 0;

            try
            {
                value = Int32.Parse(t.Text);
            }
            catch (Exception ex)
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

        public void PrintIntGrid()
        {
            for (int i = 0; i < 9; i++)
            {
                for (int j = 0; j < 9; j++)
                {
                    Console.Write(IntGrid[i, j]);
                }
                Console.WriteLine();
            }
        }

        private void CopyIntToTextBoxes()
        {
            for (int x = 0; x < 9; x++)
            {
                for (int y = 0; y < 9; y++)
                {
                    Grid[x, y].Text = (IntGrid[x, y] == 0) ? "X" : IntGrid[x, y].ToString();
                }
            }
        }

        private void SaveState()
        {
            SavedState = new int[9, 9];

            for (int x = 0; x < 9; x++)
            {
                for (int y = 0; y < 9; y++)
                {
                    SavedState[x, y] = IntGrid[x, y];
                }
            }
            StateSaved = true;
        }

        private void LoadState()
        {
            for (int x = 0; x < 9; x++)
            {
                for (int y = 0; y < 9; y++)
                {
                    IntGrid[x, y] = SavedState[x, y];
                }
            }
            StateSaved = false;
        }

        private bool CheckChanges()
        {
            int newZeroes = CountZeroes();
            if (this.Zeroes != newZeroes)
            {
                this.Zeroes = newZeroes;
                this.Modified = true;
            }
            else
            {
                this.Modified = false;
            }
            return this.Modified;
        }

        private int CountZeroes()
        {
            int count = 0;
            for (int x = 0; x < 9; x++)
            {
                for (int y = 0; y < 9; y++)
                {
                    if (IntGrid[x, y] == 0)
                    {
                        count++;
                    }
                }
            }
            return count;
        }

        public void Solve()
        {
            IntGridFromTextBoxes();
            for (int i = 0; i < 100; i++)
            {
                PrintIntGrid();

                FindCandidates();
                FixCandidates();

                PrintIntGrid();
                Console.WriteLine();

                InitCandidates();
                FindCandidates();

                FixRemaindersRow();

                InitCandidates();
                FindCandidates();
                FixCandidates();

                FixRemaindersColumn();

                InitCandidates();
                FindCandidates();
                FixCandidates();

                FixRemaindersBox();

                InitCandidates();
                FindCandidates();
                FixCandidates();

                PrintIntGrid();
                Console.WriteLine();

                FillMandatoryinColumn();

                InitCandidates();
                FindCandidates();
                FixCandidates();

                FillMandatoryinLine();

                InitCandidates();
                FindCandidates();
                FixCandidates();

                FillMandatoryInBox();

                InitCandidates();
                FindCandidates();
                FixCandidates();

                //if (!hasCandidates())
                //    break;
                //InitCandidates();

                if (!this.CheckChanges() && this.Zeroes == 0)
                {
                    Console.WriteLine("Finito!");
                }
                //else if(!this.CheckChanges() && !this.Guessed)
                //{
                //    Console.WriteLine("Non so più cosa fare... ({0})", i);
                //    Console.WriteLine("Procedo per tentativi...");

                //    Guess();
                //} else if (!this.CheckChanges() && !this.Guessed)
                //{
                //    if (this.StateSaved)
                //    {
                //        LoadState();
                //        this.StateSaved = false;
                //    }
                //    Guess();
                //}
                InitCandidates();
                CopyIntToTextBoxes();
            }

        }

        public bool hasCandidates()
        {
            for (int x = 0; x < 9; x++)
            {
                for (int y = 0; y < 9; y++)
                {
                    if (CandidatesGrid[x, y].Count > 0)
                    {
                        return true;
                    }
                }
            }
            return false;
        }

        public bool CheckOrizontal(int n, int i, int j)
        {
            for (int y = 0; y < 9; y++)
            {
                if (y == j) continue;
                if (n == IntGrid[i, y])
                {
                    return false;
                }
            }
            return true;
        }

        private void Guess()
        {
            if (!this.Guessed)
                this.SaveState();
            for (int x = 0; x < 9; x++)
            {
                for (int y = 0; y < 9; y++)
                {
                    if (CandidatesGrid[x, y].Count > 1)
                    {
                        IntGrid[x, y] = CandidatesGrid[x, y].ElementAt((new Random()).Next(0, CandidatesGrid[x, y].Count - 1));
                        CandidatesGrid[x, y].Clear();
                        Console.WriteLine("Provo con {0}, in {1},{2}", IntGrid[x, y], x + 1, y + 1);
                        return;
                    }
                }
            }
            this.Guessed = true;
        }

        public bool CheckVertical(int n, int i, int j)
        {
            for (int x = 0; x < 9; x++)
            {
                if (x == i) continue;
                if (n == IntGrid[x, j])
                {
                    return false;
                }
            }
            return true;
        }

        public bool CheckBox(int n, int i, int j)
        {
            int xBox = -1;
            if (i >= 0 && i < 3)
            {
                xBox = 0;
            }
            else if (i >= 3 && i < 6)
            {
                xBox = 1;
            }
            else if (i >= 6 && i < 9)
            {
                xBox = 2;
            }

            int yBox = -1;
            if (j >= 0 && j < 3)
            {
                yBox = 0;
            }
            else if (j >= 3 && j < 6)
            {
                yBox = 1;
            }
            else if (j >= 6 && j < 9)
            {
                yBox = 2;
            }

            for (int x = xBox * 3; x < xBox * 3 + 3; x++)
            {
                for (int y = yBox * 3; y < yBox * 3 + 3; y++)
                {
                    if (x == i && y == j)
                    {
                        continue;
                    }
                    if (IntGrid[x, y] == n)
                    {
                        return false;
                    }
                }
            }
            return true;
        }

        private void InitCandidates()
        {
            CandidatesGrid = new LinkedList<int>[9, 9];
            for (int i = 0; i < 9; i++)
            {
                for (int j = 0; j < 9; j++)
                {
                    CandidatesGrid[i, j] = new LinkedList<int>();
                }
            }
        }

        public void FindCandidates()
        {
            for (int x = 0; x < 9; x++)
            {
                for (int y = 0; y < 9; y++)
                {
                    for (int n = 1; n < 10; n++)
                    {
                        if (IntGrid[x, y] != 0)
                            continue;

                        if (CheckOrizontal(n, x, y) && CheckVertical(n, x, y) && CheckBox(n, x, y))
                        {
                            CandidatesGrid[x, y].AddLast(n);
                            Console.WriteLine("Candidato: {0}, in {1},{2}", n, x + 1, y + 1);
                        }
                    }
                }
            }
        }

        public void FixCandidates()
        {
            for (int x = 0; x < 9; x++)
            {
                for (int y = 0; y < 9; y++)
                {
                    if (CandidatesGrid[x, y].Count == 1)
                    {
                        int n = CandidatesGrid[x, y].First();
                        IntGrid[x, y] = n;
                        CandidatesGrid[x, y].RemoveFirst();
                        Console.WriteLine("Fisso da Candidati: {0}, in {1},{2}", n, x + 1, y + 1);
                    }
                }
            }
        }

        public void FixRemaindersRow()
        {
            bool stop = false;

            // Controllo se nelle righe orizzontali manca un solo numero
            // e nel caso lo inserisco.
            for (int i = 0; i < 9; i++)
            {
                int zeroes = 0;
                for (int j = 0; j < 9; j++)
                {
                    if (IntGrid[i, j] == 0)
                    {
                        zeroes++;
                    }
                }

                if (zeroes == 1)
                {
                    for (int j = 0; j < 9 && !stop; j++)
                    {
                        if (IntGrid[i, j] == 0)
                        {
                            bool found = false;
                            for (int n = 1; n <= 9; n++)
                            {
                                for (int y = 0; y < 9; y++)
                                {
                                    if (n == IntGrid[i, y])
                                    {
                                        found = true;
                                    }
                                }
                                if (!found)
                                {
                                    IntGrid[i, j] = n;
                                    CandidatesGrid[i, j].Clear();
                                    Console.WriteLine("Fisso Mancante Riga: {0} in {1},{2}", n, i + 1, j + 1);
                                    stop = true;
                                    break;
                                }
                                else
                                {
                                    found = false;
                                }
                            }
                        }
                    }
                }
            }
        }

        public void FixRemaindersColumn()
        {

            bool stop = false;


            //Controllo se nelle colonne verticali manca un solo numero
            // e nel caso lo inserisco.
            for (int j = 0; j < 9; j++)
            {
                int zeroes = 0;
                for (int i = 0; i < 9; i++)
                {
                    if (IntGrid[i, j] == 0)
                    {
                        zeroes++;
                    }
                }

                if (zeroes == 1)
                {
                    for (int i = 0; i < 9 && !stop; i++)
                    {
                        if (IntGrid[i, j] == 0)
                        {
                            bool found = false;
                            for (int n = 1; n <= 9; n++)
                            {
                                for (int x = 0; x < 9; x++)
                                {
                                    if (n == IntGrid[x, j])
                                    {
                                        found = true;
                                    }
                                }
                                if (!found)
                                {
                                    IntGrid[i, j] = n;
                                    CandidatesGrid[i, j].Clear();
                                    Console.WriteLine("Fisso Mancante Colonna: {0} in {1},{2}", n, i + 1, j + 1);
                                    stop = true;
                                    break;
                                }
                                else
                                {
                                    found = false;
                                }
                            }
                        }
                    }
                }
            }
        }

        public void FixRemaindersBox()
        {

            bool stop = false;


            // Controllo se nel box manca un solo numero....
            // e nel caso lo inserisco.
            for (int xBox = 0; xBox < 3; xBox++)
            {
                for (int yBox = 0; yBox < 3; yBox++)
                {
                    int zeroes = 0;
                    for (int x = xBox * 3; x < xBox * 3 + 3; x++)
                    {
                        for (int y = yBox * 3; y < yBox * 3 + 3; y++)
                        {
                            if (IntGrid[x, y] == 0)
                            {
                                zeroes++;
                            }
                        }
                    }

                    if (zeroes == 1)
                    {
                        for (int x = xBox * 3; x < xBox * 3 + 3 && !stop; x++)
                        {
                            for (int y = yBox * 3; y < yBox * 3 + 3 && !stop; y++)
                            {
                                if (IntGrid[x, y] == 0)
                                {
                                    for (int n = 1; n <= 9; n++)
                                    {
                                        bool found = false;
                                        for (int i = xBox * 3; i < xBox * 3 + 3; i++)
                                        {
                                            for (int j = yBox * 3; j < yBox * 3 + 3; j++)
                                            {
                                                if (IntGrid[i, j] == n)
                                                {
                                                    found = true;
                                                    break;
                                                }
                                            }
                                            if (found)
                                            {
                                                break;
                                            }
                                        }
                                        if (!found)
                                        {
                                            IntGrid[x, y] = n;
                                            CandidatesGrid[x, y].Clear();
                                            Console.WriteLine("Fisso Box: {0} in {1},{2}", n, x + 1, y + 1);
                                            stop = true;
                                            break;
                                        }
                                        else
                                        {
                                            found = false;
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }
        }


        private void FillMandatoryInBox()
        {
            for (int n = 1; n <= 9; n++)
            {
                for (int xBox = 0; xBox < 3; xBox++)
                {
                    for (int yBox = 0; yBox < 3; yBox++)
                    {
                        int count = 0;
                        int _x = 0, _y = 0;

                        for (int x = xBox * 3; x < xBox * 3 + 3; x++)
                        {
                            for (int y = yBox * 3; y < yBox * 3 + 3; y++)
                            {
                                if (IntGrid[x, y] == 0 && CandidatesGrid[x, y].Contains(n))
                                {
                                    Console.WriteLine("CandidatesGrid[{0},{1}].Contains({2}).", x + 1, y + 1, n);
                                    count++;
                                    _x = x;
                                    _y = y;
                                }
                            }
                        }

                        if (count == 1)
                        {
                            IntGrid[_x, _y] = n;
                            CandidatesGrid[_x, _y].Clear();
                            Console.WriteLine("Fisso Box unico valido: {0} in {1},{2}", n, _x + 1, _y + 1);
                        }
                    }
                }
            }
        }

        private void FillMandatoryinLine()
        {
            for (int n = 1; n <= 9; n++)
            {
                int count = 0;
                int x = 0, y = 0;

                for (int i = 0; i < 9; i++)
                {
                    for (int j = 0; j < 9; j++)
                    {
                        if (IntGrid[i, j] == 0 && CandidatesGrid[i, j].Contains(n))
                        {
                            count++;
                            x = i;
                            y = j;
                        }
                    }
                }

                if (count == 1)
                {
                    IntGrid[x, y] = n;
                    CandidatesGrid[x, y].Clear();
                    Console.WriteLine("Fisso Riga unico valido: {0} in {1},{2}", n, x + 1, y + 1);
                }
            }
        }

        private void FillMandatoryinColumn()
        {
            for (int n = 1; n <= 9; n++)
            {
                int count = 0;
                int x = 0, y = 0;

                for (int j = 0; j < 9; j++)
                {
                    for (int i = 0; i < 9; i++)
                    {
                        if (IntGrid[i, j] == 0 && CandidatesGrid[i, j].Contains(n))
                        {
                            count++;
                            x = i;
                            y = j;
                        }
                    }
                }
                if (count == 1)
                {
                    IntGrid[x, y] = n;
                    CandidatesGrid[x, y].Clear();
                    Console.WriteLine("Fisso Colonna unico valido: {0} in {1},{2}", n, x + 1, y + 1);
                }
            }
        }

        private void FillMandatory()
        {
            FillMandatoryinColumn();
            FillMandatoryinLine();
            FillMandatoryInBox();
        }

        private void FixOnlyPossibleIn(int i, int j)
        {

        }
    }
}
