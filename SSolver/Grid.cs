using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

namespace SSolver
{
    public class Grid : List<List<Cell>>
    {
        private readonly int _speed;
        
        private Grid(Grid c)
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

        private void Assign(Grid c)
        {
            foreach (var row in c)
                foreach (var cell in row)
                    this[cell.X][cell.Y] = cell;
        }

        public Grid(List<int> nums, int speed = 500)
        {
            _speed = speed;
            for (int i = 0; i < 81; i++)
            {
                if (i % 9 == 0)
                    Add(new List<Cell>());

                this[i / 9].Add(new Cell(this, i / 9, i % 9, nums[i]));
            }
        }

        private bool Check()
        {
            foreach (var row in this)
            foreach (var cell in row)
                if (!cell.Check())
                    return false;

            return true;
        }

        public List<Cell> GetRow(int x) => this[x];

        public List<Cell> GetCol(int y) => this.Select(row => row[y]).ToList();

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

        private bool SolveSimple()
        {
            bool somethingChanged;
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

        private bool SolveWithGuesswork()
        {
            var emptyCells = new List<Cell>();
            foreach (var row in this)
            {
                emptyCells.AddRange(row.FindAll(r => r.Value == 0));
            }

            foreach (var cell in emptyCells)
            {
                foreach (var pos in cell.PossibleValues)
                {
                    var copy = new Grid(this);
                    copy[cell.X][cell.Y].Value = pos;
                    copy[cell.X][cell.Y].Guess = true;
                    if (copy.SolveSimple() && copy.Check())
                    {
                        this.Assign(copy);
                        Print("====");
                        return true;
                    }

                    Thread.Sleep(_speed);
                }
            }

            return false;
        }

        private bool Solved()
        {
            foreach (var row in this)
            {
                if (row.Any(c => c.Value == 0))
                {
                    return false;
                }
            }

            return true;
        }

        private int pp = 0;

        private void Print(string p = "s")
        {
            var oc = Console.ForegroundColor;
            Console.Clear();
            Console.WriteLine($"{(char) 169}-------------{(char) 170}");
            foreach (var row in this)
            {
                foreach (var col in row)
                {
                    Console.ForegroundColor = col.Orig ? ConsoleColor.Blue 
                                                       : col.Value == 0 ? ConsoleColor.White 
                                                       : col.Guess ? ConsoleColor.Red : ConsoleColor.Green;
                    Console.Write($"{col.Value}  ");
                }

                Console.Write("\r\n");
            }

            Console.ForegroundColor = oc;
            Console.WriteLine($"{p}=={pp++}++++++++");
        }
        
        public bool Solve()
        {
            return SolveSimple() || SolveWithGuesswork();
        }
    }
}
