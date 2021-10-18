using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace Tamagotchi
{
    public partial class MainPage : ContentPage
    {
        ObservableCollection<PlayerAction> playeractions = new ObservableCollection<PlayerAction>();
        public ObservableCollection<PlayerAction> PlayerActions { get { return playeractions; } }

        public Creature SharkPuppy { get; set; }

        private Timer creatureStatusReduce = null;
        private int statusReductions = 0;

        public MainPage()
        {
            var creatureDataStore = DependencyService.Get<IDataStore<Creature>>();

            BindingContext = this;

            InitializeComponent();

            AddPlayerActions();
        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();

            var creatureDataStore = DependencyService.Get<IDataStore<Creature>>();
            SharkPuppy = await creatureDataStore.ReadItem();
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
                //I don't want to create multiple entries in the database while testing.
                //await creatureDataStore.CreateItem(SharkPuppy);
            }
            UpdatePlayerActionValues();

            statusReductions = 0;

            creatureStatusReduce = new Timer(increaseValues, null, 0, 600000); //every 10 minutes
        }

        private void AddPlayerActions()
        {
            playeractions.Add(new PlayerAction { ImageName = "Shark_pup_food.png", LinkedAction = Actions.FEED, ActionInfo = "Feeding time!", ClickFunction = "NavigateToFeedPage", ActionTitle = "Hunger"});
            playeractions.Add(new PlayerAction { ImageName = "Shark_pup_drink.png", LinkedAction = Actions.DRINK, ActionInfo = "Do sharks really need to drink?", ClickFunction = "NavigateToDrinkPage", ActionTitle = "Thirst"});
            playeractions.Add(new PlayerAction { ImageName = "Shark_pup_attention.png", LinkedAction = Actions.ATTENTION, ActionInfo = "Give the little shark some attention!", ClickFunction = "NavigateToAttentionPage", ActionTitle = "Boredom"});
            playeractions.Add(new PlayerAction { ImageName = "Shark_pup_friends.png", LinkedAction = Actions.FRIENDS, ActionInfo = "Look for some friends for the little shark.", ClickFunction = "NavigateToFriendsPage", ActionTitle = "Loneliness" });
            playeractions.Add(new PlayerAction { ImageName = "Shark_pup_alonetime.png", LinkedAction = Actions.ALONETIME, ActionInfo = "Leave the little shark alone for a while.", ClickFunction = "NavigateToAlonetimePage", ActionTitle = "Stimulated" });
            playeractions.Add(new PlayerAction { ImageName = "Shark_pup_sleep.png", LinkedAction = Actions.SLEEP, ActionInfo = "Everyone needs a nap!", ClickFunction = "NavigateToSleepPage", ActionTitle = "Tiredness"});

            carView_Actions.ItemsSource = PlayerActions;
        }

        private void increaseValues(object o)
        {
            if(statusReductions > 0)
            {
                SharkPuppy.hunger += 0.01f;
                SharkPuppy.thirst += 0.01f;
                SharkPuppy.boredom += 0.01f;
                SharkPuppy.loneliness += 0.01f;
                SharkPuppy.stimulated += 0.01f;
                SharkPuppy.tired += 0.01f;

                Console.WriteLine("Increasing Values -> " + DateTime.Now);

                UpdatePlayerActionValues();
            }
            statusReductions++;
        }

        private int ConvertFloatToPercentage(float floatValue)
        {
            var number = Math.Ceiling(floatValue * 100);
            return (int)number;
        }

        private void btn_Clicked(object sender, EventArgs e)
        {
            PlayerAction curAction = playeractions[carView_Actions.Position];

            switch (curAction.LinkedAction)
            {
                case Actions.FEED:
                    NavigateToFeedPage();
                    break;
                case Actions.DRINK:
                    NavigateToDrinkPage();
                    break;
                case Actions.ATTENTION:
                    NavigateToAttentionPage();
                    break;
                case Actions.FRIENDS:
                    NavigateToFriendsPage();
                    break;
                case Actions.ALONETIME:
                    NavigateToAlonetimePage();
                    break;
                case Actions.SLEEP:
                    NavigateToSleepPage();
                    break;
                default:
                    Console.WriteLine("Player action not defined");
                    break;
            }

            UpdatePlayerActionValues();
        }

        private void UpdatePlayerActionValues()
        {
            foreach(PlayerAction item in playeractions)
            {
                switch (item.LinkedAction)
                {
                    case Actions.FEED:
                        item.ActionValue = ConvertFloatToPercentage(SharkPuppy.hunger) + "%";
                        break;
                    case Actions.DRINK:
                        item.ActionValue = ConvertFloatToPercentage(SharkPuppy.thirst) + "%";
                        break;
                    case Actions.ATTENTION:
                        item.ActionValue = ConvertFloatToPercentage(SharkPuppy.boredom) + "%";
                        break;
                    case Actions.FRIENDS:
                        item.ActionValue = ConvertFloatToPercentage(SharkPuppy.loneliness) + "%";
                        break;
                    case Actions.ALONETIME:
                        item.ActionValue = ConvertFloatToPercentage(SharkPuppy.stimulated) + "%";
                        break;
                    case Actions.SLEEP:
                        item.ActionValue = ConvertFloatToPercentage(SharkPuppy.tired) + "%";
                        break;
                    default:
                        Console.WriteLine("Undefined player action");
                        break;
                }
            }
        }

        private void NavigateToFeedPage()
        {
            SharkPuppy.hunger = SharkPuppy.hunger - 0.1f;
            /*if (SharkPuppy.HungerValue < 0) SharkPuppy.HungerValue = 0;
            else if (SharkPuppy.HungerValue > 1) SharkPuppy.HungerValue = 1;*/
            Console.WriteLine("Navigate to feed");
        }
        private void NavigateToDrinkPage()
        {
            Console.WriteLine("Navigate to drink");
            Navigation.PushAsync(new DrinkingMinigame());
        }
        private void NavigateToAttentionPage()
        {
            SharkPuppy.boredom = SharkPuppy.boredom - 0.1f;
            //if (SharkPuppy.AttentionValue < 0) SharkPuppy.AttentionValue = 0;
            //else if (SharkPuppy.AttentionValue > 1) SharkPuppy.AttentionValue = 1;
            Console.WriteLine("Navigate to attention");
        }
        private void NavigateToFriendsPage()
        {
            SharkPuppy.loneliness = SharkPuppy.loneliness - 0.1f;
            //if (SharkPuppy.FriendsNeededValue < 0) SharkPuppy.FriendsNeededValue = 0;
            //else if (SharkPuppy.FriendsNeededValue > 1) SharkPuppy.FriendsNeededValue = 1;
            Console.WriteLine("Navigate to friends");
        }
        private void NavigateToAlonetimePage()
        {
            SharkPuppy.stimulated = SharkPuppy.stimulated - 0.1f;
            //if (SharkPuppy.AloneTimeValue < 0) SharkPuppy.AloneTimeValue = 0;
            //else if (SharkPuppy.AloneTimeValue > 1) SharkPuppy.AloneTimeValue = 1;
            Console.WriteLine("Navigate to alone time");
        }
        private void NavigateToSleepPage()
        {
            SharkPuppy.tired = SharkPuppy.tired - 0.1f;
            //if (SharkPuppy.TiredValue < 0) SharkPuppy.TiredValue = 0;
            //else if (SharkPuppy.TiredValue > 1) SharkPuppy.TiredValue = 1;
            Console.WriteLine("Navigate to sleep");
        }
    }
}
