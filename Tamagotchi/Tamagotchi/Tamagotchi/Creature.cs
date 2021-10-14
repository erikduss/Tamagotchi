using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace Tamagotchi
{
    public class Creature : INotifyPropertyChanged
    {
        public int id { get; set; }

        public string name { get; set; }

        private float hungerValue;
        public float hunger 
        { 
            get { return hungerValue; }
            set 
            {
                if (value > 1) hungerValue = 1;
                else if (value < 0) hungerValue = 0;
                else
                {
                    hungerValue = value;
                }
            }
        }

        private float thirstValue;
        public float thirst
        {
            get { return thirstValue; }
            set
            {
                if (value > 1) thirstValue = 1;
                else if (value < 0) thirstValue = 0;
                else
                {
                    thirstValue = value;
                }
            }
        }

        private float attentionValue;
        public float AttentionValue
        {
            get { return attentionValue; }
            set
            {
                if (value > 1) attentionValue = 1;
                else if (value < 0) attentionValue = 0;
                else
                {
                    attentionValue = value;
                }
            }
        }

        private float tiredValue;
        public float TiredValue
        {
            get { return tiredValue; }
            set
            {
                if (value > 1) tiredValue = 1;
                else if (value < 0) tiredValue = 0;
                else
                {
                    tiredValue = value;
                }
            }
        }

        private float friendsNeededValue;
        public float FriendsNeededValue
        {
            get { return friendsNeededValue; }
            set
            {
                if (value > 1) friendsNeededValue = 1;
                else if (value < 0) friendsNeededValue = 0;
                else
                {
                    friendsNeededValue = value;
                }
            }
        }

        private float aloneTimeValue;
        public float stimulated
        {
            get { return aloneTimeValue; }
            set
            {
                if (value > 1) aloneTimeValue = 1;
                else if (value < 0) aloneTimeValue = 0;
                else
                {
                    aloneTimeValue = value;
                }
            }
        }


        public float CreatureStatus => (hungerValue + thirstValue + attentionValue + tiredValue + friendsNeededValue + aloneTimeValue) / 6;

        public string StatusText
        {
            get { return StatusTextCalc(); }
        }

        public string StatusTextCalc()
        {
            switch (CreatureStatus)
            {
                case >= 1.0f:
                    return "I'm feeling really terrible!";
                case > 0.75f:
                    return "I'm feeling quite bad";
                case > 0.5f:
                    return "I'm feeling a bit bad.";
                case > 0.25f:
                    return "I'm feeling alright";
                case > 0f:
                    return "I'm feeling good!";
                case  0f:
                    return "I'm feeling great!";
                default:
                    return "I don't know how I got here";
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
    }
}
