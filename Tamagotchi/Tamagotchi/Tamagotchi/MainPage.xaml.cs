using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace Tamagotchi
{
    public partial class MainPage : ContentPage
    {
        ObservableCollection<PlayerAction> playeractions = new ObservableCollection<PlayerAction>();
        public ObservableCollection<PlayerAction> PlayerActions { get { return playeractions; } }

        public Creature SharkPuppy { get; set; } = new Creature
        {
            HungerValue = 0.5f,
            ThirstValue = 0.5f,
            AloneTimeValue = 0.5f,
            AttentionValue = 0.5f,
            FriendsNeededValue = 0.5f,
            TiredValue = 0.5f
        };

        public MainPage()
        {
            BindingContext = this;

            InitializeComponent();

            AddPlayerActions();
            UpdatePlayerActionValues();
        }

        private void AddPlayerActions()
        {
            playeractions.Add(new PlayerAction { ImageName = "Shark_pup_food.png", LinkedAction = Actions.FEED, ActionInfo = "Feeding time!", ClickFunction = "NavigateToFeedPage", ActionTitle = "Hunger"});
            playeractions.Add(new PlayerAction { ImageName = "Shark_pup_drink.png", LinkedAction = Actions.DRINK, ActionInfo = "Do sharks really need to drink?", ClickFunction = "NavigateToDrinkPage", ActionTitle = "Thirst"});
            playeractions.Add(new PlayerAction { ImageName = "Shark_pup_attention.png", LinkedAction = Actions.ATTENTION, ActionInfo = "Give the little shark some attention!", ClickFunction = "NavigateToAttentionPage", ActionTitle = "Attention"});
            playeractions.Add(new PlayerAction { ImageName = "Shark_pup_friends.png", LinkedAction = Actions.FRIENDS, ActionInfo = "Look for some friends for the little shark.", ClickFunction = "NavigateToFriendsPage", ActionTitle = "Lonelyness"});
            playeractions.Add(new PlayerAction { ImageName = "Shark_pup_alonetime.png", LinkedAction = Actions.ALONETIME, ActionInfo = "Leave the little shark alone for a while.", ClickFunction = "NavigateToAlonetimePage", ActionTitle = "Stress"});
            playeractions.Add(new PlayerAction { ImageName = "Shark_pup_sleep.png", LinkedAction = Actions.SLEEP, ActionInfo = "Everyone needs a nap!", ClickFunction = "NavigateToSleepPage", ActionTitle = "Tiredness"});

            carView_Actions.ItemsSource = PlayerActions;
        }

        private int ConvertFloatToPercentage(float floatValue)
        {
            return (int)(floatValue * 100);
        }

        private void btn_Clicked(object sender, EventArgs e)
        {
            try
            {
                PlayerAction curAction = (PlayerAction)carView_Actions.CurrentItem;

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
                }
            }
            catch
            {
                Console.WriteLine("Player action not defined");
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
                        item.ActionValue = ConvertFloatToPercentage(SharkPuppy.HungerValue) + "%";
                        break;
                    case Actions.DRINK:
                        item.ActionValue = ConvertFloatToPercentage(SharkPuppy.ThirstValue) + "%";
                        break;
                    case Actions.ATTENTION:
                        item.ActionValue = ConvertFloatToPercentage(SharkPuppy.AttentionValue) + "%";
                        break;
                    case Actions.FRIENDS:
                        item.ActionValue = ConvertFloatToPercentage(SharkPuppy.FriendsNeededValue) + "%";
                        break;
                    case Actions.ALONETIME:
                        item.ActionValue = ConvertFloatToPercentage(SharkPuppy.AloneTimeValue) + "%";
                        break;
                    case Actions.SLEEP:
                        item.ActionValue = ConvertFloatToPercentage(SharkPuppy.TiredValue) + "%";
                        break;
                    default:
                        Console.WriteLine("Undefined player action");
                        break;
                }
            }
        }

        private void NavigateToFeedPage()
        {
            SharkPuppy.HungerValue += 0.1f;
            Console.WriteLine("Navigate to feed");
        }
        private void NavigateToDrinkPage()
        {
            SharkPuppy.ThirstValue += 0.1f;
            Console.WriteLine("Navigate to drink");
        }
        private void NavigateToAttentionPage()
        {
            SharkPuppy.AttentionValue += 0.1f;
            Console.WriteLine("Navigate to attention");
        }
        private void NavigateToFriendsPage()
        {
            SharkPuppy.FriendsNeededValue += 0.1f;
            Console.WriteLine("Navigate to friends");
        }
        private void NavigateToAlonetimePage()
        {
            SharkPuppy.AloneTimeValue += 0.1f;
            Console.WriteLine("Navigate to alone time");
        }
        private void NavigateToSleepPage()
        {
            SharkPuppy.TiredValue += 0.1f;
            Console.WriteLine("Navigate to sleep");
        }
    }
}
