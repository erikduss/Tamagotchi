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
    public partial class SleepingMinigame : ContentPage
    {
        private float score = 0;
        private Timer minigameTimer = null;

        private int timesClicked = 0;

        public SleepingMinigame()
        {
            InitializeComponent();
        }
        private void btn_Sleep_Clicked(object sender, EventArgs e)
        {
            if (timesClicked < 10)
            {
                timesClicked++;
                score += 2.5f;
                UpdateTitle();

                if(timesClicked == 10)
                {
                    SaveData();
                }
            }
        }

        private async void SaveData()
        {
            var creatureDataStore = DependencyService.Get<IDataStore<Creature>>();

            Creature sharkPup = await creatureDataStore.ReadItem();

            float newTiredValue = sharkPup.tired - (score / 100);
            if (newTiredValue < 0) newTiredValue = 0;

            int fixedValueInt = (int)Math.Ceiling(newTiredValue * 100);
            float fixedValue = ((float)fixedValueInt) / 100;

            sharkPup.tired = fixedValue;

            bool updateResult = await creatureDataStore.UpdateItem(sharkPup);

            if (updateResult)
            {
                Device.BeginInvokeOnMainThread(() =>
                {
                    int scoreInPercentage = (int)Math.Ceiling(score);
                    lbl_score.FontSize = 18;
                    lbl_score.Text = "Minigame complete, returning to the main page in 5 seconds. Your Tired is reduces by " + scoreInPercentage + "%";
                });
                minigameTimer = new Timer
                {
                    Interval = 5000,
                    AutoReset = false
                };
                minigameTimer.Elapsed += ReturnToMainPage;
                minigameTimer.Start();
            }
            else
            {
                throw new Exception();
            }
        }

        private void UpdateTitle()
        {
            int scoreInPercentage = (int)Math.Ceiling(score);
            Device.BeginInvokeOnMainThread(() =>
            {
                lbl_score.FontSize = 21;
                lbl_score.Text = "Tired decrease earned: " + scoreInPercentage + "%";
            });
        }
        private void ReturnToMainPage(object o, ElapsedEventArgs e)
        {
            Device.BeginInvokeOnMainThread(async () => await Navigation.PopAsync());
        }
    }
}