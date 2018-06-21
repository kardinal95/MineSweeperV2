using MineSweeper.Core.Models.Cell;

namespace MineSweeper.Core.Models.Field
{
    public interface IField
    {
        int Width { get; }
        int Height { get; }
        int Mines { get; }
        bool IsDefused { get; }

        ICell[,] Cells { get; }

        bool TryOpenCell(int x, int y);
    }
}