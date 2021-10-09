using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace Tamagotchi
{
    public enum Actions
    {
        FEED,
        DRINK,
        ATTENTION,
        FRIENDS,
        ALONETIME,
        SLEEP
    }

    public class PlayerAction : INotifyPropertyChanged
    {
        public string ImageName { get; set; }
        public Actions LinkedAction { get; set; }

        public string ActionInfo { get; set; }

        public string ClickFunction { get; set; }

        public string ActionTitle { get; set; }

        public string ActionValue { get; set; }

        public event PropertyChangedEventHandler PropertyChanged;
    }
}
