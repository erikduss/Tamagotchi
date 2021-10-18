using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Tamagotchi
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class DrinkingMinigame : ContentPage
    {
        private float imageScale = 1;
        private float maxImageScale = 1.5f;

        private int scoreInPercentage = 0;

        private bool timerRunning = false;

        private Timer countdownTimer = null;
        private int timesReduced = 0;


        public DrinkingMinigame()
        {
            InitializeComponent();
        }
        private void btn_Drink_Clicked(object sender, EventArgs e)
        {
            if (timerRunning)
            {
                if (imageScale < maxImageScale)
                {
                    imageScale += 0.01f;
                }
                else imageScale = 1.5f;
                
                img_SharkPup.ScaleTo(imageScale, 250);
                UpdateTitle();
            }
        }
        private void btn_Start_Clicked(object sender, EventArgs e)
        {
            if (!timerRunning)
            {
                countdownTimer = new Timer(reduceScore, null, 0, 250);
                timerRunning = true;
                UpdateTitle();
                lbl_score.FontSize = 21;
            }
        }

        private void SaveData()
        {
            var creatureDataStore = DependencyService.Get<IDataStore<Creature>>();

            Creature sharkPup = creatureDataStore.ReadItem().Result;

            float newThirstValue = sharkPup.thirst - (imageScale - 1);
            if (newThirstValue < 0) newThirstValue = 0;

            float fixedValue = (float)Math.Ceiling(newThirstValue * 100);

            sharkPup.thirst = fixedValue;

            if (creatureDataStore.UpdateItem(sharkPup).Result)
            {
                Device.BeginInvokeOnMainThread(() =>
                {
                    lbl_score.FontSize = 18;
                    lbl_score.Text = "Minigame complete, returning to the main page in 5 seconds. Your thirst is reduces by " + scoreInPercentage + "%";
                });
                countdownTimer = new Timer(ReturnToMainPage, null, 5000, 5000);
            }
            else
            {
                throw new Exception();
            }
        }

        private void ReturnToMainPage(object o)
        {
            Device.BeginInvokeOnMainThread(async () => await Navigation.PopAsync());
        }

        private void reduceScore(object o)
        {
            if(imageScale > 1 && timerRunning)
            {
                imageScale -= 0.01f;
                img_SharkPup.ScaleTo(imageScale, 250);
            }

            timesReduced++;

            if (timesReduced == 80)
            {
                timerRunning = false;
                UpdateTitle();
                countdownTimer = null;
                SaveData();
            }
            else if (timerRunning)
            {
                UpdateTitle();
            }
        }
        private int ConvertFloatToPercentage(float floatValue)
        {
            return (int)(floatValue * 100);
        }

        private void UpdateTitle()
        {
            scoreInPercentage = ConvertFloatToPercentage(imageScale-1);
            Device.BeginInvokeOnMainThread(() =>
            {
                lbl_score.Text = "Thirst decrease earned: " + scoreInPercentage + "%";
            });
        }

    }
}