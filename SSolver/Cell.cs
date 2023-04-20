using System;
using System.Collections.Generic;
using System.Linq;

namespace SSolver
{
    public class Cell
    {
        private Grid _Grid;
        public int X { get; set; }
        public int Y { get; set; }
        public int Value { get; set; }
        
        public bool Orig { get; set; }

        public bool Guess { get; set; }

        public List<int> poss = new List<int>();
        protected List<Cell> Row;
        protected List<Cell> Col;
        protected List<Cell> Box;

        public Cell(Grid p, Cell c)
        {
            _Grid = p;
            X = c.X;
            Y = c.Y;
            Value = c.Value;
            Orig = c.Orig;

            poss = new List<int>();
            poss.AddRange(c.poss);
        }

        public bool Check()
        {
            bool valid = true;

            for (int i = 1; i < 10; i++)
            {
                if (Row.Find(c => c.Value == i) == null
                    || Col.Find(c => c.Value == i) == null
                    || Box.Find(c => c.Value == i) == null
                )
                    return false;
            }

            return valid;
        }

        public Cell(Grid grid, int x, int y, int value)
        {
            _Grid = grid;
            X = x;
            Y = y;
            Value = value;
            Orig = value != 0;
            
            if (value == 0)
            {
                poss = new List<int> {1, 2, 3, 4, 5, 6, 7, 8, 9};
            }
        }

        public bool Solve()
        {
            if (Row == null)
            {
                Row = _Grid.GetRow(X, Y);
                Col = _Grid.GetCol(X, Y);
                Box = _Grid.GetBox(X, Y);
            }

            bool somethingChanged = false;
            if (Value != 0)
                return false;

            for (int i = 1; i < 10; i++)
            {
                if (poss.Contains(i))
                {
                    var c = Row.Find(t => t.Value == i);
                    if (c != null)
                        poss.Remove(i);
                    else
                    {
                        c = Col.Find(t => t.Value == i);
                        if (c != null)
                            poss.Remove(i);
                        else
                        {
                            c = Box.Find(t => t.Value == i);
                            if (c != null)
                                poss.Remove(i);
                        }
                    }
                }

                if (poss.Count == 1)
                {
                    Value = poss[0];
                    somethingChanged = true;
                    Console.Beep();//1000 * Value, 100);
                }
            }

            if (Value == 0)
            {
                for (int p = 0; p < poss.Count; p++)
                {
                    bool canHave = false;

                    foreach (var c in Row.Where(z => z != this))
                    {   
                        if (c.CanHave(poss[p]))
                        {
                            canHave = true;
                            break;
                        }
                    }

                    if (canHave)
                    {
                        canHave = Col.Where(z => z != this).Any(c => c.CanHave(poss[p]));
                    }

                    if (canHave)
                    {
                        canHave = false;
                        foreach (var c in Box.Where(z => z != this))
                        {
                            if (c.CanHave(poss[p]))
                            {
                                canHave = true;
                                break;
                            }
                        }
                    }

                    if (!canHave)
                    {
                        Value = poss[p];
                        Console.Beep();//1000 * Value, 100);
                        somethingChanged = true;
                        break;
                    }
                }
            }

            return somethingChanged;
        }

        bool CanHave(int i)
        {
            if (Row == null)
            {
                Row = _Grid.GetRow(X, Y);
                Col = _Grid.GetCol(X, Y);
                Box = _Grid.GetBox(X, Y);
            }

            if (Value != 0)
                return false;

            if (Row.Find(t => t.Value == i) != null
                || Col.Find(t => t.Value == i) != null
                || Box.Find(t => t.Value == i) != null)
                return false;

            return true;
        }
    }
}