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

        public MainPage()
        {
            BindingContext = this;

            InitializeComponent();

            AddPlayerActions();
        }

        private void AddPlayerActions()
        {
            playeractions.Add(new PlayerAction { ImageName = "Shark_pup_food.png", LinkedAction = Actions.FEED, ActionInfo = "Feeding time!", ClickFunction = "NavigateToFeedPage"});
            playeractions.Add(new PlayerAction { ImageName = "Shark_pup_drink.png", LinkedAction = Actions.DRINK, ActionInfo = "Do sharks really need to drink?", ClickFunction = "NavigateToDrinkPage" });
            playeractions.Add(new PlayerAction { ImageName = "Shark_pup_attention.png", LinkedAction = Actions.ATTENTION, ActionInfo = "Give the little shark some attention!", ClickFunction = "NavigateToAttentionPage" });
            playeractions.Add(new PlayerAction { ImageName = "Shark_pup_friends.png", LinkedAction = Actions.FRIENDS, ActionInfo = "Look for some friends for the little shark.", ClickFunction = "NavigateToFriendsPage" });
            playeractions.Add(new PlayerAction { ImageName = "Shark_pup_alonetime.png", LinkedAction = Actions.ALONETIME, ActionInfo = "Leave the little shark alone for a while.", ClickFunction = "NavigateToAlonetimePage" });
            playeractions.Add(new PlayerAction { ImageName = "Shark_pup_sleep.png", LinkedAction = Actions.SLEEP, ActionInfo = "Everyone needs a nap!", ClickFunction = "NavigateToSleepPage" });

            carView_Actions.ItemsSource = PlayerActions;
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
        }

        private void NavigateToFeedPage()
        {
            Console.WriteLine("Navigate to feed");
        }
        private void NavigateToDrinkPage()
        {
            Console.WriteLine("Navigate to drink");
        }
        private void NavigateToAttentionPage()
        {
            Console.WriteLine("Navigate to attention");
        }
        private void NavigateToFriendsPage()
        {
            Console.WriteLine("Navigate to friends");
        }
        private void NavigateToAlonetimePage()
        {
            Console.WriteLine("Navigate to alone time");
        }
        private void NavigateToSleepPage()
        {
            Console.WriteLine("Navigate to sleep");
        }
    }
}
