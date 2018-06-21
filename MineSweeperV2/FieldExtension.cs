using MineSweeper.Core.Models.Cell;
using MineSweeper.Core.Models.Field;

namespace MineSweeperV2
{
    static class FieldExtension
    {
        public static ICell GetCell(this IField field, int x, int y)
        {
            return field.Cells[x, y];
        }

        public static ICell GetCell(this IField field, int num)
        {
            var converted = field.ConvertCoords(num);
            return field.GetCell(converted[0], converted[1]);
        }

        public static int[] ConvertCoords(this IField field, int num)
        {
            return new[] {num % field.Width, num / field.Width};
        }
    }
}