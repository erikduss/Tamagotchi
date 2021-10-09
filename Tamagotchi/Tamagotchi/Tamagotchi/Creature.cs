using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace Tamagotchi
{
    public class Creature : INotifyPropertyChanged
    {
        public float HungerValue { get; set; }
        public float ThirstValue { get; set; }

        public float AttentionValue { get; set; }

        public float TiredValue { get; set; }

        public float FriendsNeededValue { get; set; }

        public float AloneTimeValue { get; set; }

        public event PropertyChangedEventHandler PropertyChanged;
    }
}
