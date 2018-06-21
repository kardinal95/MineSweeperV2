namespace MineSweeper.Core.Models.Cell
{
    public interface ICell
    {
        bool IsOpened { get; }
        bool IsFlagged { get; }

        void Open();
        void SwitchFlag();
    }
}