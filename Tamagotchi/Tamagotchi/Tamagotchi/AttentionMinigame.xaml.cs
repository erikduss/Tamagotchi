using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Tamagotchi
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class AttentionMinigame : ContentPage
    {
        private List<Image> clouds = new List<Image>();

        private int maxAmountOfClouds = 10;

        private Random rnd = new Random();

        private float speed = 0;

        private Timer fallingTimer = null;
        private bool timerRunning = false;
        private int timerCalled = 0;


        private float score = 0;

        public AttentionMinigame()
        {
            InitializeComponent();

            setupClouds();
        }

        private void setupClouds()
        {
            for (int i = 0; i < maxAmountOfClouds; i++)
            {
                clouds.Add(new Image
                {
                    Source = "cloud_1.png",
                    BackgroundColor = Color.Transparent,
                    IsEnabled = false
                });
            }

            DrawClouds();
        }

        private void DrawClouds()
        {
            foreach(Image img in clouds)
            {
                int xValue = rnd.Next(0,4);
                int yValue = rnd.Next(0,4);

                minigameGrid.Children.Add(img, xValue, yValue);
            }

            ImageButton creatureBtn = new ImageButton
            {
                Source = "Shark_pup_balloon.png",
                BackgroundColor = Color.Transparent,
            };
            creatureBtn.Clicked += btn_Creature_Clicked;

            minigameGrid.Children.Add(creatureBtn, 2, 2);
        }

        private void btn_Creature_Clicked(object sender, EventArgs e)
        {
            if (timerRunning)
            {
                speed += 25f;
                UpdateCloudsPositions();
                CalculateScoreIncease();
                UpdateTitle();
            }
        }

        private void UpdateCloudsPositions()
        {
            foreach (Image img in clouds)
            {
                img.TranslateTo(0, speed, 500);
            }
        }

        private void btn_Start_Clicked(object sender, EventArgs e)
        {
            if (!timerRunning)
            {
                fallingTimer = new Timer
                {
                    Interval = 250,
                    AutoReset = true
                };
                fallingTimer.Elapsed += ApplyGravity;
                timerRunning = true;
                lbl_score.FontSize = 21;
            }
        }

        private void CalculateScoreIncease()
        {
            //The player is giving too much speed or too little speed. (bad)
            if (speed > 250 || speed < -100)
            {
                if(minigameGrid.BackgroundColor != Color.LightBlue)
                {
                    minigameGrid.BackgroundColor = Color.LightBlue;
                }
                score-=1.5f;
            }
            else
            {
                if (minigameGrid.BackgroundColor != Color.SkyBlue)
                {
                    minigameGrid.BackgroundColor = Color.SkyBlue;
                }
                score +=0.25f;
            }
        }

        private void ApplyGravity(object o, ElapsedEventArgs e)
        {
            if (timerRunning)
            {
                speed -= 10;
                UpdateCloudsPositions();
            }

            timerCalled++;

            if (timerCalled == 80)
            {
                timerRunning = false;
                fallingTimer = null;
                CalculateScoreIncease();
                UpdateTitle();
                SaveData();
            }
            else if (timerRunning)
            {
                CalculateScoreIncease();
                UpdateTitle();
            }
        }
        private void UpdateTitle()
        {
            int scoreInPercentage = (int)Math.Ceiling(score);
            Device.BeginInvokeOnMainThread(() =>
            {
                lbl_score.Text = "Boredom decrease earned: " + scoreInPercentage + "%";
            });
        }

        private void SaveData()
        {
            var creatureDataStore = DependencyService.Get<IDataStore<Creature>>();

            Creature sharkPup = creatureDataStore.ReadItem().Result;

            float newBoredomValue = sharkPup.boredom - (score/100);
            if (newBoredomValue < 0) newBoredomValue = 0;

            int fixedValueInt = (int)Math.Ceiling(newBoredomValue * 100);
            float fixedValue = ((float)fixedValueInt) / 100;

            sharkPup.boredom = fixedValue;

            if (creatureDataStore.UpdateItem(sharkPup).Result)
            {
                Device.BeginInvokeOnMainThread(() =>
                {
                    int scoreInPercentage = (int)Math.Ceiling(score);
                    lbl_score.FontSize = 18;
                    lbl_score.Text = "Minigame complete, returning to the main page in 5 seconds. Your boredom is reduces by " + scoreInPercentage + "%";
                });
                fallingTimer = new Timer
                {
                    Interval = 5000,
                    AutoReset = false
                };
                fallingTimer.Elapsed += ReturnToMainPage;
            }
            else
            {
                throw new Exception();
            }
        }

        private void ReturnToMainPage(object o, ElapsedEventArgs e)
        {
            Device.BeginInvokeOnMainThread(async () => await Navigation.PopAsync());
        }
    }
}