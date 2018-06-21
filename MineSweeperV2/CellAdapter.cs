using System.Collections.Generic;
using Android.Content;
using Android.Views;
using Android.Widget;
using Java.Lang;
using MineSweeper.Core.Models.Cell;
using MineSweeper.Core.Models.Field;

namespace MineSweeperV2
{
    class CellAdapter : BaseAdapter
    {
        private readonly Context context;
        private readonly IField field;

        public CellAdapter(Context context, IField field)
        {
            this.context = context;
            this.field = field;
        }

        public override Object GetItem(int position)
        {
            return position;
        }

        public override long GetItemId(int position)
        {
            return position;
        }

        public override View GetView(int position, View convertView, ViewGroup parent)
        {
            ImageView imageView;

            if (convertView == null)
            {
                // if it's not recycled, initialize some attributes
                imageView = new ImageView(context);
                imageView.SetMinimumHeight(50);
                imageView.SetMinimumWidth(50);
                imageView.SetAdjustViewBounds(true);
                imageView.SetScaleType(ImageView.ScaleType.FitXy);
                imageView.SetPadding(0, 0, 0, 0);
            }
            else
            {
                imageView = (ImageView) convertView;
            }

            var cell = field.GetCell(position);
            if (cell.IsFlagged)
            {
                imageView.SetImageResource(thumbs["cell_flagged"]);
            }
            else if (!cell.IsOpened)
            {
                imageView.SetImageResource(thumbs["cell_closed"]);
            }
            else if (cell is FusedCell)
            {
                imageView.SetImageResource(thumbs["cell_bomb"]);
            }
            else
            {
                var count = ((EmptyCell) cell).NeighbourMineCount;
                imageView.SetImageResource(count == 0 ? thumbs["cell_opened"]
                                               : thumbs[$"count_{count}"]);
            }

            return imageView;
        }

        private readonly Dictionary<string, int> thumbs = new Dictionary<string, int>
        {
            {"cell_closed", Resource.Drawable.cell_closed},
            {"cell_flagged", Resource.Drawable.cell_flagged},
            {"cell_opened", Resource.Drawable.cell_opened},
            {"cell_bomb", Resource.Drawable.cell_bomb},
            {"count_1", Resource.Drawable.count_1},
            {"count_2", Resource.Drawable.count_2},
            {"count_3", Resource.Drawable.count_3},
            {"count_4", Resource.Drawable.count_4},
            {"count_5", Resource.Drawable.count_5},
            {"count_6", Resource.Drawable.count_6},
            {"count_7", Resource.Drawable.count_7},
            {"count_8", Resource.Drawable.count_8},
            {"count_9", Resource.Drawable.count_9}
        };

        //Fill in cound here, currently 0
        public override int Count => field.Width * field.Height;
    }

    class CellAdapterViewHolder : Object
    {
        //Your adapter views to re-use
        //public TextView Title { get; set; }
    }
}