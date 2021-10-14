using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Essentials;

namespace Tamagotchi
{
    class LocalCreatureStore : IDataStore<Creature>
    {
        public Task<bool> CreateItem(Creature item)
        {
            string creatureAsText = JsonConvert.SerializeObject(item);

            Preferences.Set("MyCreature",creatureAsText);

            return Task.FromResult(true);
        }

        public Task<bool> DeleteItem(Creature item)
        {
            Preferences.Remove("MyCreature");

            return Task.FromResult(true);
        }

        public Task<Creature> ReadItem()
        {
            string creatureAsText = Preferences.Get("MyCreature", "");

            Creature creatureFromText = JsonConvert.DeserializeObject<Creature>(creatureAsText);

            return Task.FromResult(creatureFromText);
        }

        public Task<bool> UpdateItem(Creature item)
        {
            if (Preferences.ContainsKey("MyCreature"))
            {
                string creatureAsText = JsonConvert.SerializeObject(item);

                Preferences.Set("MyCreature", creatureAsText);

                return Task.FromResult(true);
            }
            else return Task.FromResult(true);
        }
    }
}
