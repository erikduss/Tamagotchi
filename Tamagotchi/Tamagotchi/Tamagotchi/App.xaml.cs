using System;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Tamagotchi
{
    public partial class App : Application
    {
        public App()
        {
            DependencyService.RegisterSingleton<IDataStore<Creature>>(new RemoteCreatureStore());

            InitializeComponent();

            MainPage = new NavigationPage(new MainPage());
        }

        protected override void OnStart()
        {
            CalculateAndApplyAwayTime();
        }

        protected override void OnSleep()
        {
            var sleepTime = DateTime.Now;
            Preferences.Set("SleepTime", sleepTime);
        }

        protected override void OnResume()
        {
            CalculateAndApplyAwayTime();
        }

        private async void CalculateAndApplyAwayTime()
        {
            var sleepTime = Preferences.Get("SleepTime", DateTime.Now);
            var wakeTime = DateTime.Now;

            TimeSpan timeAsleep = wakeTime - sleepTime;

            var creatureDataStore = DependencyService.Get<IDataStore<Creature>>();

            Creature SharkPuppy = await creatureDataStore.ReadItem();
            if (SharkPuppy == null)
            {
                SharkPuppy = new Creature
                {
                    name = "Erik's Tamagotchi",
                    userName = "Erik",
                    hunger = 0.5f,
                    thirst = 0.5f,
                    boredom = 0.5f,
                    loneliness = 0.5f,
                    stimulated = 0.5f,
                    tired = 0.5f
                };
            }

            int increaseAmountOfTimes = (int)Math.Floor(timeAsleep.TotalSeconds / 1200);

            if (increaseAmountOfTimes > 0)
            {
                var increaseValue = 0.01f * increaseAmountOfTimes;

                SharkPuppy.hunger += increaseValue;
                SharkPuppy.thirst += increaseValue;
                SharkPuppy.boredom += increaseValue;
                SharkPuppy.loneliness += increaseValue;
                SharkPuppy.stimulated -= (increaseValue*3);
                SharkPuppy.tired += increaseValue;

                bool success = await creatureDataStore.UpdateItem(SharkPuppy);
            }
        }
    }
}
