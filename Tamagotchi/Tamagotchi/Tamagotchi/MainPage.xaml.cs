using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using System.Timers;

namespace Tamagotchi
{
    public partial class MainPage : ContentPage
    {
        ObservableCollection<PlayerAction> playeractions = new ObservableCollection<PlayerAction>();
        public ObservableCollection<PlayerAction> PlayerActions { get { return playeractions; } }

        public Creature SharkPuppy { get; set; }

        private Timer creatureStatusReduce = null;
        private int statusReductions = 0;

        private IDataStore<Creature> creatureDataStore = DependencyService.Get<IDataStore<Creature>>();

        public MainPage()
        {
            BindingContext = this;

            InitializeComponent();

            AddPlayerActions();
        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();

            SharkPuppy = await creatureDataStore.ReadItem();
            if (SharkPuppy == null)
            {
                SharkPuppy = new Creature
                {
                    name = "Sharky",
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

            //The timer that increases the values of hunger, thirst, etc overtime.
            creatureStatusReduce = new Timer
            {
                Interval = 12000000, //every 20 minutes
                AutoReset = true
            }; 
            creatureStatusReduce.Elapsed += increaseValues;
        }

        private void AddPlayerActions()
        {
            //The player actions are used for displaying in the CarouselView.

            //ActionInfo = Header description.
            //LinkedAction = The enum related to the action.
            //ActionTitle = The name of the stat it will affect.

            playeractions.Add(new PlayerAction { ImageName = "Shark_pup_food.png", LinkedAction = Actions.FEED, ActionInfo = "Feeding time!", ClickFunction = "NavigateToFeedPage", ActionTitle = "Hunger"});
            playeractions.Add(new PlayerAction { ImageName = "Shark_pup_drink.png", LinkedAction = Actions.DRINK, ActionInfo = "Do sharks really need to drink?", ClickFunction = "NavigateToDrinkPage", ActionTitle = "Thirst"});
            playeractions.Add(new PlayerAction { ImageName = "Shark_pup_attention.png", LinkedAction = Actions.ATTENTION, ActionInfo = "Give the little shark some attention!", ClickFunction = "NavigateToAttentionPage", ActionTitle = "Boredom"});
            playeractions.Add(new PlayerAction { ImageName = "Shark_pup_friends.png", LinkedAction = Actions.FRIENDS, ActionInfo = "Look for some friends for the little shark.", ClickFunction = "NavigateToFriendsPage", ActionTitle = "Loneliness" });
            playeractions.Add(new PlayerAction { ImageName = "Shark_pup_alonetime.png", LinkedAction = Actions.ALONETIME, ActionInfo = "Leave the little shark alone for a while.", ClickFunction = "NavigateToAlonetimePage", ActionTitle = "Stimulated" });
            playeractions.Add(new PlayerAction { ImageName = "Shark_pup_sleep.png", LinkedAction = Actions.SLEEP, ActionInfo = "Everyone needs a nap!", ClickFunction = "NavigateToSleepPage", ActionTitle = "Tiredness"});

            carView_Actions.ItemsSource = PlayerActions;
        }

        private void increaseValues(object o, ElapsedEventArgs e)
        {
            if(statusReductions > 0)
            {
                SharkPuppy.hunger += 0.01f;
                SharkPuppy.thirst += 0.01f;
                SharkPuppy.boredom += 0.01f;
                SharkPuppy.loneliness += 0.01f;
                SharkPuppy.stimulated += 0.01f;
                SharkPuppy.tired += 0.01f;

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
            //Every playeraction in the CarouselView has this btn clicked function assigned to it. The correct function gets selected here based on the CarouselView index.
            PlayerAction curAction = playeractions[carView_Actions.Position];

            switch (curAction.LinkedAction)
            {
                case Actions.FEED:
                    Navigation.PushAsync(new FeedingMinigame());
                    break;
                case Actions.DRINK:
                    Navigation.PushAsync(new DrinkingMinigame());
                    break;
                case Actions.ATTENTION:
                    Navigation.PushAsync(new AttentionMinigame());
                    break;
                case Actions.FRIENDS:
                    Navigation.PushAsync(new PlaygroundWindow());
                    break;
                case Actions.ALONETIME:
                    Navigation.PushAsync(new StimulatedMinigame());
                    break;
                case Actions.SLEEP:
                    Navigation.PushAsync(new SleepingMinigame());
                    break;
                default:
                    Console.WriteLine("Player action not defined");
                    break;
            }

            UpdatePlayerActionValues();
        }

        private void UpdatePlayerActionValues()
        {
            //Update for the percentage values in the CarouselView overview.
            foreach (PlayerAction item in playeractions)
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
    }
}
