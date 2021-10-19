using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Tamagotchi
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class StimulatedMinigame : ContentPage
    {
        public StimulatedMinigame()
        {
            InitializeComponent();
        }

        private void btn_Start_Clicked(object sender, EventArgs e)
        {
            var sleepTime = DateTime.Now;
            Preferences.Set("SleepTime", sleepTime);

            System.Diagnostics.Process.GetCurrentProcess().CloseMainWindow();
        }
    }
}