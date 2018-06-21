using System;
using System.Collections.Generic;
using System.Linq;
using MineSweeper.Core.Models.Cell;

namespace MineSweeper.Core.Models.Field
{
    class CustomBasicField : IField
    {
        public int Width { get; }
        public int Height { get; }
        public int Mines { get; }
        public bool IsDefused => CheckFieldFuseState();
        public ICell[,] Cells { get; private set; }

        public CustomBasicField(int width, int height, int mineCount)
        {
            if (width < 2 || width > 100)
            {
                throw new ArgumentException("Width must be in range of 2-100!");
            }

            if (height < 4 || height > 150)
            {
                throw new ArgumentException("Width must be in range of 4-150!");
            }

            if (mineCount < 1 || mineCount > width * height / 2)
            {
                throw new ArgumentException("Mine count must be in range of 1 to 50% of field!");
            }

            Width = width;
            Height = height;
            Mines = mineCount;

            InitField();
        }

        private void InitField()
        {
            Cells = new ICell[Width, Height];
            FillMines();
            FillEmpty();
        }

        private void FillMines()
        {
            var rand = new Random();
            for (var i = 0; i < Mines;)
            {
                var x = rand.Next(0, Width);
                var y = rand.Next(0, Height);
                if (!(Cells[x, y] is null))
                {
                    continue;
                }

                Cells[x, y] = new FusedCell();
                i++;
            }
        }

        private void FillEmpty()
        {
            for (var x = 0; x < Width; x++)
            {
                for (var y = 0; y < Height; y++)
                {
                    if (!(Cells[x, y] is null))
                    {
                        continue;
                    }

                    Cells[x, y] = new EmptyCell {NeighbourMineCount = GetNeighbourMineCount(x, y)};
                }
            }
        }

        private int GetNeighbourMineCount(int x, int y)
        {
            var coords = GetNeighboursCoords(x, y);

            return coords.Count(pair => Cells[pair[0], pair[1]] is FusedCell);
        }

        private IEnumerable<int[]> GetNeighboursCoords(int x, int y)
        {
            var result = new List<int[]>();
            var lowerX = x < 1 ? x : x - 1;
            var upperX = x == Width - 1 ? x : x + 1;
            var lowerY = y < 1 ? y : y - 1;
            var upperY = y == Height - 1 ? y : y + 1;

            for (var row = lowerX; row <= upperX; row++)
            {
                for (var col = lowerY; col <= upperY; col++)
                {
                    if (x == row && y == col)
                    {
                        // Skip the field itself
                        continue;
                    }

                    if (!(Cells[row, col] is null))
                    {
                        result.Add(new[] {row, col});
                    }
                }
            }

            return result;
        }

        public bool TryOpenCell(int x, int y)
        {
            if (x < 0 || x > Width || y < 0 || y > Height)
            {
                throw new ArgumentOutOfRangeException();
            }

            try
            {
                Cells[x, y].Open();
                if (Cells[x, y] is EmptyCell target && target.NeighbourMineCount == 0)
                {
                    foreach (var neighbour in GetNeighboursCoords(x, y))
                    {
                        try
                        {
                            TryOpenCell(neighbour[0], neighbour[1]);
                        }
                        catch (ArgumentOutOfRangeException) { }
                    }
                }
            }
            catch (InvalidOperationException)
            {
                return false;
            }

            return true;
        }

        private bool CheckFieldFuseState()
        {
            return Cells.Cast<ICell>().All(cell => !(cell is EmptyCell) || cell.IsOpened);
        }
    }
}