using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Essentials;

namespace Tamagotchi
{
    public class RemoteCreatureStore : IDataStore<Creature>
    {
        private HttpClient client = new HttpClient();

        public async Task<bool> CreateItem(Creature item)
        {
            string creatureAsText = JsonConvert.SerializeObject(item);
            try
            {
                var response = await client.PostAsync("https://tamagotchi.hku.nl/api/Creatures", new StringContent(creatureAsText, Encoding.UTF8, "application/json"));

                if (response.IsSuccessStatusCode)
                {
                    string postedCreatureAsText = await response.Content.ReadAsStringAsync();

                    Creature postedCreature = JsonConvert.DeserializeObject<Creature>(postedCreatureAsText);

                    Preferences.Set("MyCreatureId", postedCreature.id);

                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch(HttpRequestException e)
            {
                return false;
            }
        }

        public bool DeleteItem(Creature item)
        {
            throw new NotImplementedException();
        }

        public async Task<Creature> ReadItem()
        {
            int creatureID = Preferences.Get("MyCreatureID", 0);
            if(creatureID == 0)
            {
                return null;
            }

            try
            {
                var response = await client.GetAsync("https://tamagotchi.hku.nl/api/Creatures/" + creatureID);

                if (response.IsSuccessStatusCode)
                {
                    string postedCreatureAsText = await response.Content.ReadAsStringAsync();

                    Creature postedCreature = JsonConvert.DeserializeObject<Creature>(postedCreatureAsText);

                    //Preferences.Set("MyCreatureId", postedCreature.ID);

                    return postedCreature;
                }
                else
                {
                    return null;
                }
            }
            catch(HttpRequestException e)
            {
                return null;
            }
        }

        public bool UpdateItem(Creature item)
        {
            throw new NotImplementedException();
        }
    }
}
