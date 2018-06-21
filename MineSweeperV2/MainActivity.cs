using Android.App;
using Android.Content;
using Android.OS;
using Android.Support.V7.App;
using Android.Widget;
using MineSweeper.Core;

namespace MineSweeperV2
{
    [Activity(Label = "@string/app_name", Theme = "@style/AppTheme", MainLauncher = true)]
    public class MainActivity : AppCompatActivity
    {
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            // Set our view from the "main" layout resource
            SetContentView(Resource.Layout.activity_main);

            var startEasyButton = FindViewById<Button>(Resource.Id.start_easy_b);
            startEasyButton.Click += (sender, e) =>
            {
                var intent = new Intent(this, typeof(GameActivity));
                intent.PutExtra("Level", FieldType.Easy.ToString());
                StartActivity(intent);
            };
            var startNormButton = FindViewById<Button>(Resource.Id.start_norm_b);
            startNormButton.Click += (sender, e) =>
            {
                var intent = new Intent(this, typeof(GameActivity));
                intent.PutExtra("Level", FieldType.Normal.ToString());
                StartActivity(intent);
            };
            var startHardButton = FindViewById<Button>(Resource.Id.start_hard_b);
            startHardButton.Click += (sender, e) =>
            {
                var intent = new Intent(this, typeof(GameActivity));
                intent.PutExtra("Level", FieldType.Hard.ToString());
                StartActivity(intent);
            };

            var startCustomButton = FindViewById<Button>(Resource.Id.custom_b_start);
            startCustomButton.Click += (sender, e) =>
            {
                var intent = new Intent(this, typeof(SelectActivity));
                StartActivity(intent);
            };
        }
    }
}