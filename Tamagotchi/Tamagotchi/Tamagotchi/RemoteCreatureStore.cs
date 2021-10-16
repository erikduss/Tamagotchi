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

                    Preferences.Set("MyCreatureID", postedCreature.id);

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

        public async Task<bool> DeleteItem(Creature item)
        {
            int creatureID = Preferences.Get("MyCreatureID", 0);
            if (creatureID == 0)
            {
                return false;
            }

            try
            {
                var response = await client.GetAsync("https://tamagotchi.hku.nl/api/Creatures/" + creatureID);

                if (response.IsSuccessStatusCode)
                {
                    var successDelete = await client.DeleteAsync("https://tamagotchi.hku.nl/api/Creatures/" + creatureID);

                    return successDelete.IsSuccessStatusCode;
                }
                else
                {
                    return false;
                }
            }
            catch (HttpRequestException e)
            {
                return false;
            }
        }

        public async Task<Creature> ReadItem()
        {
            int creatureID = Preferences.Get("MyCreatureID", 0);
            if(creatureID == 0)
            {
                Preferences.Set("MyCreatureID", 10); //My creature is already in the database, I dont want to create duplicates while testing.
                return null;
            }

            try
            {
                var response = await client.GetAsync("https://tamagotchi.hku.nl/api/Creatures/" + creatureID);

                if (response.IsSuccessStatusCode)
                {
                    string postedCreatureAsText = await response.Content.ReadAsStringAsync();

                    Creature postedCreature = JsonConvert.DeserializeObject<Creature>(postedCreatureAsText);

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

        public async Task<bool> UpdateItem(Creature item)
        {
            string creatureAsText = JsonConvert.SerializeObject(item);

            int creatureID = Preferences.Get("MyCreatureID", 0);
            if (creatureID == 0)
            {
                return false;
            }

            try
            {
                var getResponse = await client.GetAsync("https://tamagotchi.hku.nl/api/Creatures/" + creatureID);

                if (getResponse.IsSuccessStatusCode)
                {
                    var response = await client.PutAsync("https://tamagotchi.hku.nl/api/Creatures/" + creatureID, new StringContent(creatureAsText, Encoding.UTF8, "application/json"));

                    if (response.IsSuccessStatusCode)
                    {
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                }
                else
                {
                    return false;
                }
            }
            catch (HttpRequestException e)
            {
                return false;
            }
        }
    }
}
