using System;
using System.Collections.Generic;
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

    public class PlayerAction
    {
        public string ImageName { get; set; }
        public Actions LinkedAction { get; set; }

        public string ActionInfo { get; set; }

        public string ClickFunction { get; set; }
    }
}
