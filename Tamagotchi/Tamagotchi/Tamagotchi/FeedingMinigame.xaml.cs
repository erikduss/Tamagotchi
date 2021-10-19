using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using System.Timers;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Tamagotchi
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class FeedingMinigame : ContentPage
    {
        private List<ImageButton> food = new List<ImageButton>();

        private List<Vector2> occupiedSpaces = new List<Vector2>();

        private int maxAmountOfFood = 3;

        private Random rnd = new Random();

        private Timer minigameTimer = null;
        private bool timerRunning = false;
        private int timerCalled = 0;

        private float score = 0;

        public FeedingMinigame()
        {
            InitializeComponent();
            setupFood();
        }


        private void setupFood()
        {
            for (int i = 0; i < maxAmountOfFood; i++)
            {
                food.Add(new ImageButton
                {
                    Source = "food.png",
                    BackgroundColor = Color.Transparent,
                    IsEnabled = true
                });
            }
            DrawFood();
        }

        private void DrawFood()
        {
            foreach (ImageButton img in food)
            {
                img.Clicked += btn_Food_Clicked;

                Vector2 thisPos = NewValidFoodLocation();
                occupiedSpaces.Add(new Vector2(thisPos.X, thisPos.Y));

                minigameGrid.Children.Add(img, (int)thisPos.X, (int)thisPos.Y);
            }
        }

        private void btn_Food_Clicked(object sender, EventArgs e)
        {
            if (timerRunning)
            {
                score++;
                int index = minigameGrid.Children.IndexOf(sender as ImageButton);

                Vector2 oldVectorValue = new Vector2(Grid.GetColumn((ImageButton)sender), Grid.GetRow((ImageButton)sender));

                Vector2 newVectorValue = NewValidFoodLocation();

                int xpos = (int)newVectorValue.X;
                int ypos = (int)newVectorValue.Y;

                minigameGrid.Children[index].SetValue(Grid.ColumnProperty, xpos);
                minigameGrid.Children[index].SetValue(Grid.RowProperty, ypos);

                occupiedSpaces.Add(newVectorValue);
                occupiedSpaces.Remove(oldVectorValue);

                UpdateTitle();
            }
        }

        private void btn_Start_Clicked(object sender, EventArgs e)
        {
            if (!timerRunning)
            {
                minigameTimer = new Timer
                {
                    Interval = 20000,
                    AutoReset = false
                };
                minigameTimer.Elapsed += EndMinigame;
                minigameTimer.Start();

                timerRunning = true;
                lbl_score.FontSize = 21;
            }
        }

        private Vector2 NewValidFoodLocation()
        {
            int xValue = rnd.Next(0, 4);
            int yValue = rnd.Next(0, 4);

            Vector2 newVectorValue = new Vector2(xValue, yValue);
            if (occupiedSpaces.Contains(newVectorValue))
            {
                do
                {
                    xValue = rnd.Next(0, 4);
                    yValue = rnd.Next(0, 4);

                    newVectorValue = new Vector2(xValue, yValue);
                }
                while (occupiedSpaces.Contains(newVectorValue));
            }

            return newVectorValue;
        }

        private void EndMinigame(object o, ElapsedEventArgs e)
        {
            timerRunning = false;
            minigameTimer = null;
            Console.WriteLine(score);
            UpdateTitle();
            SaveData();
        }

        private void SaveData()
        {
            var creatureDataStore = DependencyService.Get<IDataStore<Creature>>();

            Creature sharkPup = creatureDataStore.ReadItem().Result;

            score = score * .5f;

            float newHungerValue = sharkPup.hunger - (score / 100);
            if (newHungerValue < 0) newHungerValue = 0;

            int fixedValueInt = (int)Math.Ceiling(newHungerValue * 100);
            float fixedValue = ((float)fixedValueInt) / 100;

            sharkPup.hunger = fixedValue;

            if (creatureDataStore.UpdateItem(sharkPup).Result)
            {
                Device.BeginInvokeOnMainThread(() =>
                {
                    int scoreInPercentage = (int)Math.Ceiling(score);
                    lbl_score.FontSize = 18;
                    lbl_score.Text = "Minigame complete, returning to the main page in 5 seconds. Your hunger is reduces by " + scoreInPercentage + "%";
                });
                minigameTimer = new Timer
                {
                    Interval = 5000,
                    AutoReset = false
                };
                minigameTimer.Elapsed += ReturnToMainPage;
            }
            else
            {
                throw new Exception();
            }
        }

        private void UpdateTitle()
        {
            int scoreInPercentage = (int)Math.Ceiling(score/2);
            Device.BeginInvokeOnMainThread(() =>
            {
                lbl_score.Text = "Hunger decrease earned: " + scoreInPercentage + "%";
            });
        }
        private void ReturnToMainPage(object o, ElapsedEventArgs e)
        {
            Device.BeginInvokeOnMainThread(async () => await Navigation.PopAsync());
        }
    }
}