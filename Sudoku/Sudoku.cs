using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Resources;
using System.Runtime.CompilerServices;
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
                    if (j % 3 == 0)
                        Console.Write(" ");
                    Console.Write(IntGrid[i, j]);
                }
                Console.WriteLine();
                if ((i + 1) % 3 == 0)
                {
                    Console.WriteLine();
                }
            }
        }

        private void CopyIntToTextBoxes()
        {
            for (int x = 0; x < 9; x++)
            {
                for (int y = 0; y < 9; y++)
                {
                    Grid[x, y].Text = (IntGrid[x, y] == 0) ? "" : IntGrid[x, y].ToString();
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
            PrintIntGrid();

            if(!CheckCoherence())
            {
                MessageBox.Show("Incoerenza nella griglia di partenza");
            }

            for (int i = 0; i < 100; i++)
            {

                Console.WriteLine("Iterazione n° {0}", i);
                FindCandidates();
                FixCandidates();

                //PrintIntGrid();
                //Console.WriteLine();

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

                //PrintIntGrid();
                //Console.WriteLine();

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

                FillColumns();

                InitCandidates();
                FindCandidates();
                FixCandidates();

                FillRows();

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
            }
            CopyIntToTextBoxes();
            PrintIntGrid();

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
                        int n = CandidatesGrid[x, y].ElementAt((new Random()).Next(0, CandidatesGrid[x, y].Count - 1));
                        if (CheckCoherence(n, x, y))
                        {
                            IntGrid[x, y] = n;
                            CandidatesGrid[x, y].Clear();
                            Console.WriteLine("Provo con {0}, in {1},{2}", IntGrid[x, y], x + 1, y + 1);
                            return;
                        }
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
                            //Console.WriteLine("Candidato: {0}, in {1},{2}", n, x + 1, y + 1);
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
                        if (CheckCoherence(n, x, y))
                        {
                            IntGrid[x, y] = n;
                            CandidatesGrid[x, y].Clear();
                            Console.WriteLine("Fisso da Candidati: {0}, in {1},{2}", n, x + 1, y + 1);
                        }
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
                                if (!found && CheckCoherence(n, i, j))
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
                                if (!found && CheckCoherence(n, i, j))
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
                                        if (!found && CheckCoherence(n, x, y))
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
                                    //Console.WriteLine("CandidatesGrid[{0},{1}].Contains({2}).", x + 1, y + 1, n);
                                    count++;
                                    _x = x;
                                    _y = y;
                                }
                            }
                        }

                        if (count == 1 && CheckCoherence(n, _x, _y))
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

                if (count == 1 && CheckCoherence(n, x, y))
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
                if (count == 1 && CheckCoherence(n, x, y))
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

        private bool CheckNumberInSlot(int n, int i, int j)
        {
            for (int x = 0; x < 9; x++)
            {
                if (IntGrid[x, j] == n)
                {
                    Console.WriteLine("{0} non può stare in {1},{2} a causa di {3} in {4},{5}", n, i, j, IntGrid[x, j], x, j);
                    return false;
                }
            }


            for (int y = 0; y < 9; y++)
            {
                if (IntGrid[i, y] == n)
                {
                    Console.WriteLine("{0} non può stare in {1},{2} a causa di {3} in {4},{5}", n, i, j, IntGrid[i, y], i, y);
                    return false;
                }
            }

            int xBox = i / 3;
            int yBox = j / 3;

            for (int x = xBox * 3; x < xBox * 3 + 3; x++)
            {
                for (int y = yBox * 3; y < yBox * 3 + 3; y++)
                {
                    if (IntGrid[x, y] == n)
                    {
                        Console.WriteLine("{0} non può stare in {1},{2} a causa di {3} in {4},{5}", n, i, j, IntGrid[x, y], x, y);
                        return false;
                    }
                }
            }

            return true;
        }

        private void CheckAndFixWholeGrid()
        {
            for (int i = 0; i < 9; i++)
            {
                for (int j = 0; j < 9; j++)
                {
                    for (int n = 1; n < 10; n++)
                    {
                        if (CheckNumberInSlot(n, i, j))
                        {
                            if (CheckCoherence(n, i, j))
                            {
                                Console.WriteLine("Controllo ed inserisco {0} in {1},{2}", n, i, j);
                                IntGrid[i, j] = n;
                            }
                        }
                        else
                        {
                            Console.WriteLine("{0} non può stare in {1},{2}", n, i, j);
                        }
                    }
                }
            }
        }

        private bool LineContains(int n, int line)
        {
            for (int j = 0; j < 9; j++)
            {
                if (IntGrid[line, j] == n)
                    return true;
            }
            return false;
        }
        private bool ColumnContains(int n, int column)
        {
            for (int i = 0; i < 9; i++)
            {
                if (IntGrid[i, column] == n)
                    return true;
            }
            return false;
        }
        private LinkedList<int> GetMissingFromLine(int line)
        {
            LinkedList<int> missing = new LinkedList<int>();

            for (int n = 1; n < 10; n++)
            {
                if (!LineContains(n, line))
                {
                    missing.AddLast(n);
                }
            }

            return missing;
        }

        private LinkedList<int> GetMissingFromColumn(int column)
        {
            LinkedList<int> missing = new LinkedList<int>();

            for (int n = 1; n < 10; n++)
            {
                if (!ColumnContains(n, column))
                {
                    missing.AddLast(n);
                }
            }

            return missing;
        }

        private void FillColumns()
        {
            for (int j = 0; j < 9; j++)
            {
                var missing = GetMissingFromColumn(j);
                foreach (var n in missing)
                {
                    int count = 0;
                    for (int i = 0; i < 9; i++)
                    {
                        if (IntGrid[i, j] == 0)
                        {
                            if (CheckNumberInSlot(n, i, j))
                            {
                                count++;
                            }
                        }
                    }
                    if (count == 1)
                    {
                        for (int i = 0; i < 9; i++)
                        {
                            if (IntGrid[i, j] == 0)
                            {
                                if (CheckNumberInSlot(n, i, j))
                                {
                                    if (CheckCoherence(n, i, j))
                                        IntGrid[i, j] = n;
                                }
                            }
                        }
                    }
                }
            }
        }

        private void FillRows()
        {
            for (int i = 0; i < 9; i++)
            {
                var missing = GetMissingFromLine(i);
                foreach (var n in missing)
                {
                    int count = 0;
                    for (int j = 0; j < 9; j++)
                    {
                        if (IntGrid[i, j] == 0)
                        {
                            if (CheckNumberInSlot(n, i, j))
                            {
                                count++;
                            }
                        }
                    }
                    if (count == 1)
                    {
                        for (int j = 0; j < 9; j++)
                        {
                            if (IntGrid[i, j] == 0)
                            {
                                if (CheckNumberInSlot(n, i, j))
                                {
                                    if (CheckCoherence(n, i, j))
                                    {
                                        IntGrid[i, j] = n;
                                    }
                                }
                            }
                        }
                    }
                }
            }
        }

        public bool CheckCoherence(int n, int i, int j)
        {
            if (!CheckHorizontalCoherence(n, i, j) || !CheckVerticalCoherence(n, i, j) || !CheckBoxCoherence(n, i, j))
            {
                return false;
            }

            return true;
        }

        public bool CheckCoherence()
        {
            IntGridFromTextBoxes();

            for (int i = 0; i < 9; i++)
            {
                for (int j = 0; j < 9; j++)
                {
                    int n = IntGrid[i, j];
                    if (n == 0) continue;
                    if (!CheckHorizontalCoherence(n, i, j) || !CheckVerticalCoherence(n, i, j) || !CheckBoxCoherence(n, i, j))
                    {
                        Grid[i, j].ForeColor = Color.Red;
                        return false;
                    }
                }
            }

            return true;
        }

        private bool CheckHorizontalCoherence(int n, int i, int j)
        {
            for (int y = 0; y < 9; y++)
            {
                if (j == y)
                    continue;

                if (IntGrid[i, y] == n)
                {
                    return false;
                }
            }
            return true;
        }

        private bool CheckVerticalCoherence(int n, int i, int j)
        {
            for (int x = 0; x < 9; x++)
            {
                if (x == i)
                    continue;

                if (IntGrid[x, j] == n)
                {
                    return false;
                }
            }
            return true;
        }

        private bool CheckBoxCoherence(int n, int i, int j)
        {
            int xBox = i / 3;
            int yBox = j / 3;

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

        public string PrepareForSave()
        {
            string save = "";
            for (int i = 0; i < 9; i++)
            {
                for (int j = 0; j < 9; j++)
                {
                    save += (Grid[i, j].Text == "") ? "0" : Grid[i, j].Text;
                }
                save += "\n";
            }
            return save;
        }

        public bool LoadFromString(string sudoku)
        {
            var strings = sudoku.Split('\n');
            
            if (strings.Length < 9)
                return false;

            for (int i = 0; i < 9; i++)
            {
                if (strings[i].Length != 9)
                    return false;
                for (int j = 0; j < 9; j++)
                {
                    string n = strings[i].Substring(j, 1);
                    Grid[i, j].Text = (n == "0") ? "" : n;
                }
            }
            return true;
        }
    }
}
