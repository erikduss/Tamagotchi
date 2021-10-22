using Newtonsoft.Json;
using System;
using System.Net.Http;
using System.Threading.Tasks;
using System.Timers;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Tamagotchi
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class PlaygroundWindow : ContentPage
    {
        private HttpClient client = new HttpClient();

        private Timer minigameTimer = null;
        private bool timerRunning = false;

        private bool connectedToPlayground = false;

        private int amountOfOtherCreaturesActive = 0;

        float score = 0;

        public PlaygroundWindow()
        {
            InitializeComponent();
        }

        private void btn_Start_Clicked(object sender, EventArgs e)
        {
            if (!connectedToPlayground)
            {
                _ = ConnectToPlayground(Preferences.Get("MyCreatureID", 0));

                minigameTimer = new Timer
                {
                    Interval = 2000,
                    AutoReset = true
                };
                minigameTimer.Elapsed += ReduceLoneliness;
                minigameTimer.Start();

                timerRunning = true;
                lbl_score.FontSize = 18;
                Device.BeginInvokeOnMainThread(() =>
                {
                    btn_start.Text = "Stop";
                });
                _ = UpdateCurrentCreaturesAsync();
                UpdateTitle();
            }
            else if(timerRunning)//The start button became a stop button
            {
                StopPlaygroundTime();
            }
        }

        private async Task UpdateCurrentCreaturesAsync()
        {
            var creatures = await ReadItems();

            amountOfOtherCreaturesActive = creatures.Length - 1;
        }

        private void ReduceLoneliness(object o, ElapsedEventArgs e)
        {
            score += (0.0025f * amountOfOtherCreaturesActive);

            UpdateTitle();
        }

        public async Task<bool> ConnectToPlayground(int creatureID)
        {
            try
            {
                var response = await client.PostAsync("https://tamagotchi.hku.nl/api/Playground/" + creatureID, null);

                if (response.IsSuccessStatusCode)
                {
                    connectedToPlayground = true;

                    return true;
                }
                else
                {
                    var getResponse = await client.GetAsync("https://tamagotchi.hku.nl/api/Playground/" + creatureID);

                    if (getResponse.IsSuccessStatusCode)
                    {
                        connectedToPlayground = true;
                    }
                    return false;
                }
            }
            catch (HttpRequestException e)
            {
                return false;
            }
        }

        public async Task<PlaygroundCreature[]> ReadItems()
        {
            try
            {
                var response = await client.GetAsync("https://tamagotchi.hku.nl/api/Playground");

                if (response.IsSuccessStatusCode)
                {
                    string postedCreaturesAsText = await response.Content.ReadAsStringAsync();

                    PlaygroundCreature[] postedCreature = JsonConvert.DeserializeObject<PlaygroundCreature[]>(postedCreaturesAsText);

                    return postedCreature;
                }
                else
                {
                    return null;
                }
            }
            catch (HttpRequestException e)
            {
                return null;
            }
        }

        public async Task<bool> DisconnectFromPlayground()
        {
            int creatureID = Preferences.Get("MyCreatureID", 0);
            if (creatureID == 0)
            {
                return false;
            }

            try
            {
                var response = await client.GetAsync("https://tamagotchi.hku.nl/api/Playground/" + creatureID);

                if (response.IsSuccessStatusCode)
                {
                    var successDelete = await client.DeleteAsync("https://tamagotchi.hku.nl/api/Playground/" + creatureID);

                    if(successDelete.IsSuccessStatusCode) connectedToPlayground = false;

                    return successDelete.IsSuccessStatusCode;
                }
                else
                {
                    return false;
                }
            }
            catch (HttpRequestException e)
            {
                return false;
            }
        }

        private void StopPlaygroundTime()
        {
            if (timerRunning)
            {
                minigameTimer.Stop();
                timerRunning = false;
            }
            SaveData();
        }

        private async void SaveData()
        {
            var creatureDataStore = DependencyService.Get<IDataStore<Creature>>();

            Creature sharkPup = await creatureDataStore.ReadItem();

            float newLonelinessValue = sharkPup.loneliness - (score);
            if (newLonelinessValue < 0) newLonelinessValue = 0;

            int fixedValueInt = (int)Math.Ceiling(newLonelinessValue * 100);
            float fixedValue = ((float)fixedValueInt) / 100;

            sharkPup.loneliness = fixedValue;

            bool updateSuccess = await creatureDataStore.UpdateItem(sharkPup);

            if (updateSuccess)
            {
                _ = DisconnectFromPlayground();
                Device.BeginInvokeOnMainThread(() =>
                {
                    int scoreInPercentage = ConvertFloatToPercentage(score);
                    lbl_score.FontSize = 18;
                    lbl_score.Text = "Minigame complete, returning to the main page in 5 seconds. Your loneliness is reduces by " + scoreInPercentage + "%";
                });

                minigameTimer = new Timer
                {
                    Interval = 5000,
                    AutoReset = true
                };
                minigameTimer.Elapsed += ReturnToMainPage;
                minigameTimer.Start();
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

        private int ConvertFloatToPercentage(float floatValue)
        {
            var number = Math.Ceiling(floatValue * 100);
            return (int)number;
        }

        private void UpdateTitle()
        {
            int scoreInPercentage = ConvertFloatToPercentage(score);

            Device.BeginInvokeOnMainThread(() =>
            {
                lbl_score.Text = "Loneliness decrease earned: " + scoreInPercentage + "%! There are currently " + amountOfOtherCreaturesActive + " other creatures in the playground.";
            });
        }

        protected override void OnDisappearing()
        {
            Device.BeginInvokeOnMainThread(() =>
            {
                StopPlaygroundTime();
            });

            base.OnDisappearing();
        }
    }

    public class PlaygroundCreature
    {
        public int id { get; set; }
        public DateTime enterTime { get; set; }
        public Creature creature { get; set; }
    }

}