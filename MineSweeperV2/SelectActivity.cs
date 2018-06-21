using System;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Support.V7.App;
using Android.Widget;
using MineSweeper.Core;
using MineSweeper.Core.Models.Field;
using AlertDialog = Android.Support.V7.App.AlertDialog;

namespace MineSweeperV2
{
    [Activity(Label = "@string/app_name", Theme = "@style/AppTheme")]
    public class SelectActivity : AppCompatActivity
    {
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            // Set our view from the "main" layout resource
            SetContentView(Resource.Layout.activity_select);

            var inputW = FindViewById<EditText>(Resource.Id.input_width);
            var inputH = FindViewById<EditText>(Resource.Id.input_height);
            var inputM = FindViewById<EditText>(Resource.Id.input_mines);

            var buttonCreate = FindViewById<Button>(Resource.Id.custom_create);
            var buttonCancel = FindViewById<Button>(Resource.Id.custom_cancel);

            buttonCreate.Click += delegate
            {
                if (!(int.TryParse(inputW.Text, out var width) &&
                      int.TryParse(inputH.Text, out var height) &&
                      int.TryParse(inputM.Text, out var mines)))
                {
                    AlertIncomplete("Please fill all the fields");
                    return;
                }

                IField field = null;
                try
                {
                    field = FieldFactory.GetCustomField(width, height, mines);
                }
                catch (ArgumentException e)
                {
                    AlertIncomplete(e.Message);
                    return;
                }
                var intent = new Intent(this, typeof(GameActivity));
                intent.PutExtra("Field", $"{width}-{height}-{mines}");
                StartActivity(intent);
            };
            buttonCancel.Click += delegate { Finish(); };
        }

        private void AlertIncomplete(string message)
        {
            var alert = new AlertDialog.Builder(this);
            alert.SetTitle("Incorrect operation");
            alert.SetMessage(message);
            alert.SetNeutralButton("OK", (senderAlert, args) => { alert.Dispose(); });

            Dialog dialog = alert.Create();
            dialog.Show();
        }
    }
}