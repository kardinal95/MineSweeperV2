using System;

namespace MineSweeper.Core.Models.Cell
{
    public class FusedCell : ICell
    {
        public bool IsOpened { get; private set; }
        public bool IsFlagged { get; private set; }

        public void Open()
        {
            if (IsOpened || IsFlagged)
            {
                throw new InvalidOperationException("Cannot apply with current cell state!");
            }

            IsOpened = true;
        }

        public void SwitchFlag()
        {
            if (IsOpened)
            {
                throw new InvalidOperationException("Cannot apply with current cell state!");
            }

            IsFlagged = !IsFlagged;
        }
    }
}