using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Essentials;

namespace Tamagotchi
{
    class LocalCreatureStore : IDataStore<Creature>
    {
        public bool CreateItem(Creature item)
        {
            string creatureAsText = JsonConvert.SerializeObject(item);

            Preferences.Set("MyCreature",creatureAsText);

            return true;
        }

        public bool DeleteItem(Creature item)
        {
            Preferences.Remove("MyCreature");

            return true;
        }

        public Creature ReadItem()
        {
            string creatureAsText = Preferences.Get("MyCreature", "");

            Creature creatureFromText = JsonConvert.DeserializeObject<Creature>(creatureAsText);

            return creatureFromText;
        }

        public bool UpdateItem(Creature item)
        {
            if (Preferences.ContainsKey("MyCreature"))
            {
                string creatureAsText = JsonConvert.SerializeObject(item);

                Preferences.Set("MyCreature", creatureAsText);

                return true;
            }
            else return false;
        }
    }
}
