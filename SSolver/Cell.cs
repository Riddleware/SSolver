using System;
using System.Collections.Generic;
using System.Linq;

namespace SSolver
{
    public class Cell
    {
        private Grid _Grid;
        public int X { get; }
        public int Y { get; }
        public int Value { get; set; }
        
        public bool Orig { get; }

        public bool Guess { get; set; }

        public readonly List<int> PossibleValues = new();
        private List<Cell> _row;
        private List<Cell> _col;
        private List<Cell> _box;

        public Cell(Grid p, Cell c)
        {
            _Grid = p;
            X = c.X;
            Y = c.Y;
            Value = c.Value;
            Orig = c.Orig;

            PossibleValues = new List<int>();
            PossibleValues.AddRange(c.PossibleValues);
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
                PossibleValues = new List<int> {1, 2, 3, 4, 5, 6, 7, 8, 9};
            }
        }
        
        private bool CanHave(int i)
        {
            if (Value != 0)
                return false;
            
            if (_row == null)
            {
                _row = _Grid.GetRow(X);
                _col = _Grid.GetCol(Y);
                _box = _Grid.GetBox(X, Y);
            }
            
            return !(_row.Any(t => t.Value == i) 
                     || _col.Any(t => t.Value == i) 
                     || _box.Any(t => t.Value == i));
        }

        public bool Check()
        {
            for (var i = 1; i < 10; i++)
            {
                if (_row.Find(c => c.Value == i) == null
                    || _col.Find(c => c.Value == i) == null
                    || _box.Find(c => c.Value == i) == null
                )
                    return false;
            }

            return true;
        }

        public bool Solve()
        {
            if (_row == null)
            {
                _row = _Grid.GetRow(X);
                _col = _Grid.GetCol(Y);
                _box = _Grid.GetBox(X, Y);
            }
            
            if (Value != 0)
                return false;

            var somethingChanged = false;
            
            for (int i = 1; i < 10; i++)
            {
                if (PossibleValues.Contains(i))
                {
                    if (_row.Any(t => t.Value == i)
                        || _col.Any(t => t.Value == i)
                        || _box.Any(t => t.Value == i))
                        PossibleValues.Remove(i);
                }

                if (PossibleValues.Count == 1)
                {
                    Value = PossibleValues[0];
                    somethingChanged = true;
                }
            }

            if (Value == 0)
            {
                foreach (var p in PossibleValues)
                {
                    var canHave = _row.Where(z => z != this).Any(c => c.CanHave(p))
                                  && _col.Where(z => z != this).Any(c => c.CanHave(p))
                                  && _box.Where(z => z != this).Any(c => c.CanHave(p));

                    if (!canHave)
                    {
                        Value = p;
                        somethingChanged = true;
                        break;
                    }
                }
            }

            return somethingChanged;
        }
    }
}