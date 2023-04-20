using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;

namespace SSolver
{
    public class Grid : List<List<Cell>>
    {
        private int _speed;
        
        public Grid(Grid c)
        {
            _speed = c._speed;
            pp = c.pp;
            
            foreach (var row in c)
            {
                var r = new List<Cell>();
                foreach (var cell in row)
                    r.Add(new Cell(this, cell));
                Add(r);
            }
        }

        public Grid() : base()
        {
        }

        protected void Equals(Grid c)
        {
            foreach (var row in c)
            foreach (var cell in row)
                this[cell.X][cell.Y] = cell;
        }

        public Grid(List<int> nums, int speed = 500) : base()
        {
            _speed = speed;
            for (int i = 0; i < 81; i++)
            {
                if (i % 9 == 0)
                    Add(new List<Cell>());

                this[i / 9].Add(new Cell(this, i / 9, i % 9, nums[i]));
            }
        }

        public bool Check()
        {
            foreach (var row in this)
            foreach (var cell in row)
                if (!cell.Check())
                    return false;

            return true;
        }

        public List<Cell> GetRow(int x, int y)
        {
            return this[x];
        }

        public List<Cell> GetCol(int x, int y)
        {
            var col = new List<Cell>();
            foreach (var row in this)
                col.Add(row[y]);

            return col;
        }

        public List<Cell> GetBox(int x, int y)
        {
            var box = new List<Cell>();
            var xMod = x % 3;

            GetColBox(x - xMod);
            GetColBox(x - xMod + 1);
            GetColBox(x - xMod + 2);

            return box;

            void GetColBox(int lineNum)
            {
                var yMod = y % 3;

                box.Add(this[lineNum][y - yMod]);
                box.Add(this[lineNum][y - yMod + 1]);
                box.Add(this[lineNum][y - yMod + 2]);
            }
        }

        public bool SolveSimple()
        {
            bool somethingChanged = false;
            do
            {
                somethingChanged = false;
                foreach (var row in this)
                foreach (var cell in row)
                {
                    if (cell.Solve())
                    {
                        somethingChanged = true;
                        
                        Print();
                        Thread.Sleep(_speed);
                    }
                }
            } while (!Solved() && somethingChanged);

            Print();
            Thread.Sleep(_speed);
            return somethingChanged;
        }

        bool SolveWithGuesswork()
        {
            var emptyCells = new List<Cell>();
            foreach (var row in this)
            {
                emptyCells.AddRange(row.FindAll(r => r.Value == 0));
            }

            foreach (var cell in emptyCells)
            {
                foreach (var pos in cell.poss)
                {
                    //Print("g");
                    var copy = new Grid(this);
                    copy[cell.X][cell.Y].Value = pos;
                    copy[cell.X][cell.Y].Guess = true;
                    if (copy.SolveSimple() && copy.Check())
                    {
                        this.Equals(copy);
                        Print("g");
                        return true;
                    }

                    Thread.Sleep(_speed);
                }
            }

            return false;
        }

        public bool Solve()
        {
            return SolveSimple() || SolveWithGuesswork();
        }

        bool Solved()
        {
            foreach (var row in this)
            foreach (var col in row)
                if (col.Value == 0)
                    return false;

            return true;
        }

        private int pp = 0;

        public void Print(string p = "s")
        {
            var oc = Console.ForegroundColor;
            Console.Clear();
            //Console.WriteLine($"{(char) 169}-------------{(char) 170}");
            foreach (var row in this)
            {
                string r = "";
                foreach (var col in row)
                {
                    //r += $"{col.Value} ";
                    Console.ForegroundColor = col.Orig ? ConsoleColor.Blue 
                                                       : col.Value ==0 ? ConsoleColor.White 
                                                       : col.Guess ? ConsoleColor.Red : ConsoleColor.Green;
                    Console.Write($"{col.Value}  ");
                }

                Console.Write("\r\n");
            }

            //Console.WriteLine(string.Format("{0}-------------{1}", (char) 192, (char) 217));
            Console.ForegroundColor = oc;
            //Console.WriteLine($"{p}=={pp++}++++++++");
        }
    }
}
