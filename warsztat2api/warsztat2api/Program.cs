using System;
using Newtonsoft.Json;

namespace warsztat2api
{
    internal class Program
    {
        public static HttpClient client = new HttpClient();
        static async Task Main(string[] args)
        {
            while (true)
            {
                Console.WriteLine($@"Jaką opcję chcesz wybrać?
1. Żart o Chucku Norissie
2. Kurs walut NBP
3. Pokemon
4. Zakończ");
                int answer = int.Parse(Console.ReadLine());
                while (answer < 1 || answer > 4)
                {
                    Console.WriteLine("Błędne dane. Proszę podać liczbę całkowitą od 1 do 4.");
                }

                switch (answer)
                {
                    case 1:
                        await ChuckNorrisJoke();
                        break;
                    case 2:
                        await NBPExchangeRates();
                        break;
                    case 3:
                        await PokemonMenu();
                        break;
                    default:
                        Console.WriteLine("Coś poszło nie tak.");
                        break;
                }
            }

           
        }

        static async Task ChuckNorrisJoke()
        {
            string apiUrl = "https://api.chucknorris.io/jokes/random";
            try
            {
                HttpResponseMessage response = await client.GetAsync(apiUrl);
                if (response.IsSuccessStatusCode)
                {
                    string apiResponse = await response.Content.ReadAsStringAsync();
                    var norrisJokeFormJson = JsonConvert.DeserializeObject<ChuckNorrisJoke>(apiResponse);
                    Console.WriteLine(norrisJokeFormJson.Value);
                }
                else
                {
                    Console.WriteLine($"Błąd API: {response.StatusCode}");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Wystąpił błąd: {ex.Message}");
            }
            
        }

        static async Task NBPExchangeRates()
        {
            string apiUrl = "http://api.nbp.pl/api/exchangerates/tables/A?format=json";
            try
            {
                HttpResponseMessage response = await client.GetAsync(apiUrl);
                if (response.IsSuccessStatusCode)
                {
                    string apiResponse = await response.Content.ReadAsStringAsync();
                    List<ExchangeRatesTable> data = JsonConvert.DeserializeObject<List<ExchangeRatesTable>>(apiResponse);

                    foreach (var rate in data[0].Rates)
                    {
                        Console.WriteLine($"{rate.Code} : {rate.Mid} - {rate.Currency}");
                    }
                }
                else
                {
                    Console.WriteLine($"Błąd API: {response.StatusCode}");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Wystąpił błąd: {ex.Message}");
            }
        }

        static async Task PokemonMenu()
        {
            while (true)
            {
                Console.WriteLine($@"Pokemon menu:
1. Lista Pokemon
2. Pokemon szczegóły
3. Powrót do głównego menu.");
                int answer;
                if (!int.TryParse(Console.ReadLine(), out answer))
                {
                    Console.WriteLine("Błędne dane. Proszę podać liczbę całkowitą.");
                }
                else if (answer < 0 || answer > 3)

                {
                    Console.WriteLine("Błędny wybór. Proszę podać liczbę od 1 do 3.");

                }
                else
                {
                    switch (answer)
                    {
                        case 1:
                            break;
                        case 2:
                            break;
                        case 3:
                            return;
                    }
                }

            }
        }

        static async Task ListPokemon()
        {
            string apiUrl = "https://pokeapi.co/api/v2/pokemon/";
            int offset = 0;
            int limit = 20; 
            string requestUrl = $"{apiUrl}?offset={offset}&limit={limit}";
            
            try
            {
                HttpResponseMessage response = await client.GetAsync(apiUrl);
                if (response.IsSuccessStatusCode)
                {
                    string apiResponse = await response.Content.ReadAsStringAsync();
                    var pokemonFormJson = JsonConvert.DeserializeObject<PokemonList>(apiResponse);
                    Console.WriteLine("Lista pokemonów:");
                    foreach (var pokemon in pokemonFormJson.Results)
                    {
                        Console.WriteLine($"ID: {GetPokemonId(pokemon.Url)}, Name: {pokemon.Name}");
                    }
                }
                else
                {
                    Console.WriteLine($"Błąd API: {response.StatusCode}");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Wystąpił błąd: {ex.Message}");
            }
        }

        static int GetPokemonId(string url)
        {
            string[] urlParts = url.Trim('/').Split('/');
            int id;
            if (urlParts.Length >= 2 && int.TryParse(urlParts[urlParts.Length - 1], out id))
            {
                return id;
            }
            else
            {
                return -1;
            }
            
        }
    }
}
