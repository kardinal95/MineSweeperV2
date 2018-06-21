using System;
using Android.App;
using Android.OS;
using Android.Widget;
using MineSweeper.Core;
using MineSweeper.Core.Models.Cell;
using MineSweeper.Core.Models.Field;

namespace MineSweeperV2
{
    [Activity(Label = "@string/app_name", Theme = "@style/AppTheme")]
    public class GameActivity : Activity
    {
        private IField mineField;
        private GameState state = GameState.Idle;
        private Chronometer timer;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            SetContentView(Resource.Layout.activity_game);

            if (Intent.HasExtra("Level"))
            {
                Enum.TryParse<FieldType>(Intent.GetStringExtra("Level"), out var ftype);
                mineField = FieldFactory.GetPredefinedField(ftype);
            }
            else
            {
                var raw = Intent.GetStringExtra("Field").Split('-');
                mineField =
                    FieldFactory.GetCustomField(int.Parse(raw[0]), int.Parse(raw[1]),
                                                int.Parse(raw[2]));
            }

            timer = FindViewById<Chronometer>(Resource.Id.gameTimer);

            var field = FindViewById<GridView>(Resource.Id.cellField);
            field.SetNumColumns(mineField.Width);
            field.Adapter = new CellAdapter(this, mineField);
            field.ItemClick += delegate(object sender, AdapterView.ItemClickEventArgs args)
            {
                if (state == GameState.Idle)
                {
                    timer.Start();
                    state = GameState.Running;
                }
                else if (state != GameState.Running)
                {
                    return;
                }

                TryOpenCell(sender, args);
                ValidateGameState();
                field.InvalidateViews();
                if (state != GameState.Running)
                {
                    ShowEndDialog();
                }
            };
            field.ItemLongClick += delegate(object sender, AdapterView.ItemLongClickEventArgs args)
            {
                if (state == GameState.Idle)
                {
                    timer.Start();
                    state = GameState.Running;
                }
                else if (state != GameState.Running)
                {
                    return;
                }

                TryFlagCell(sender, args);
                ValidateGameState();
                field.InvalidateViews();

                if (state != GameState.Running)
                {
                    ShowEndDialog();
                }
            };
        }

        private void ValidateGameState()
        {
            if (state != GameState.Running || !mineField.IsDefused)
            {
                return;
            }

            timer.Stop();
            state = GameState.Win;
        }

        private void ShowEndDialog()
        {
            var text = "";
            switch (state)
            {
                case GameState.Win:
                    text = $"Congratulations! You've completed it in {timer.Text}";
                    break;
                case GameState.Lose:
                    text = "Unfortunately you have lost...";
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }

            var alert = new AlertDialog.Builder(this);
            alert.SetTitle("Game end");
            alert.SetMessage(text);
            alert.SetNeutralButton("Return", (senderAlert, args) => { Finish(); });

            Dialog dialog = alert.Create();
            dialog.Show();
        }

        private void TryOpenCell(object sender, AdapterView.ItemClickEventArgs args)
        {
            var cell = mineField.GetCell(args.Position);

            if (cell.IsFlagged || cell.IsOpened)
            {
                return;
            }

            var coords = mineField.ConvertCoords(args.Position);
            mineField.TryOpenCell(coords[0], coords[1]);

            if (!(cell is FusedCell))
            {
                return;
            }

            timer.Stop();
            state = GameState.Lose;
        }

        private void TryFlagCell(object sender, AdapterView.ItemLongClickEventArgs args)
        {
            var cell = mineField.GetCell(args.Position);

            if (!cell.IsOpened)
            {
                cell.SwitchFlag();
            }
        }
    }
}