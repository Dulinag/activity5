
using System;
using System.Threading.Tasks;
using System.Net.Http;
using Newtonsoft.Json;
using System.Collections.Generic;

namespace WebAPIClient
{
    class Pokemon
    {
        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("id")]
        public int Id { get; set; }

        [JsonProperty("weight")]
        public int Weight { get; set; }

        [JsonProperty("height")]
        public int Height { get; set; }

        public List<Types> Types { get; set; }
    }

    public class Type
    {
        [JsonProperty("name")]
        public string Name { get; set; }
    }

    public class Types
    {
        [JsonProperty("type")]
        public Type Type { get; set; }
    }

    class Program
    {
        private static readonly HttpClient client = new HttpClient();

        static async Task Main(string[] args)
        {
            await ProcessRepos();
        }

        private static async Task ProcessRepos()
        {
            while (true)
            {
                Console.WriteLine("Enter Pokemon name. Press enter without writing a name to quit the program");
                var pokemonName = Console.ReadLine();
                if (string.IsNullOrEmpty(pokemonName))
                {
                    break;
                }

                var result = await client.GetAsync("https://pokeapi.co/api/v2/pokemon/" + pokemonName.ToLower());
                var resultRead = await result.Content.ReadAsStringAsync();

                try
                {
                    var pokemon = JsonConvert.DeserializeObject<Pokemon>(resultRead);

                    Console.WriteLine("___");
                    Console.WriteLine("Pokemon #" + pokemon.Id);
                    Console.WriteLine("Name: " + pokemon.Name);
                    Console.WriteLine("Weight: " + pokemon.Weight + "lb");
                    Console.WriteLine("Height: " + pokemon.Height + "ft");
                    Console.WriteLine("Type(s):");
                    pokemon.Types.ForEach(t => Console.Write(" " + t.Type.Name));
                    Console.WriteLine("\n---");
                }
                catch (Exception)
                {
                    Console.WriteLine("Error: not a valid name");
                }
            }
        }
    }
}
