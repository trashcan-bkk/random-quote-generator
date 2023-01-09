using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace random_quote_generator.Controllers
{
    public class MainController
    {
        [HttpGet("/rq-generator/quote/random")]
        public async Task<ActionResult<Quote>> GetOneRandomQuote()
        {
            using(HttpClient client = new HttpClient())
            {
                var result = await client.GetAsync("https://type.fit/api/quotes");  

                // Endpoint returns text/plain, not JSON, so we'll grab the string...
                var quoteListString = await result.Content.ReadAsStringAsync();     

                // Use Newtonsoft to deserialize it into a list we can manipulate.
                var quoteList = Newtonsoft.Json.JsonConvert.DeserializeObject<IEnumerable<Quote>>(quoteListString);   

                // Now we have an enumeration of quotes, so we can return a random element somewhere between index 0 and the count of entries minus 1
                return quoteList.ElementAt(new Random().Next(0, quoteList.Count() - 1));  
            }
        }

        [HttpGet("/rq-generator/quote/random/{quantity}")]
        public async Task<IEnumerable<Quote>> GetRandomQuotesByQuantity(int quantity)
        {
            using(HttpClient client = new HttpClient())
            {
                var result = await client.GetAsync("https://type.fit/api/quotes");  
                var quoteListString = await result.Content.ReadAsStringAsync();     
                var quoteList = Newtonsoft.Json.JsonConvert.DeserializeObject<IEnumerable<Quote>>(quoteListString);  
               
                List<Quote> randomList = new List<Quote>();
                for (int i= 1; i <= quantity; i++) {
                    randomList.Add(quoteList.ElementAt(new Random().Next(0, quoteList.Count() - 1)));
                }
                return randomList;  
            }
        }

        [HttpGet("/rq-generator/quote/{id}")]
        public async Task<ActionResult<Quote>> GetQuoteById(int id)
        {
            using (HttpClient client = new HttpClient()){
                var result = await client.GetAsync("https://type.fit/api/quotes");
                var quoteListString = await result.Content.ReadAsStringAsync();
                var quoteList = Newtonsoft.Json.JsonConvert.DeserializeObject<IEnumerable<Quote>>(quoteListString);
                return quoteList.ElementAt(id);  
            }
        }

        [HttpGet("/rq-generator/quote/list")]
        public async Task<IEnumerable<Quote>> GetQuotesList()
        {
           using (HttpClient client = new HttpClient()){
                var result = await client.GetAsync("https://type.fit/api/quotes");
                var quoteListString = await result.Content.ReadAsStringAsync();
                var quoteList = Newtonsoft.Json.JsonConvert.DeserializeObject<IEnumerable<Quote>>(quoteListString);
                return quoteList;
            }
        }

        [HttpGet("/rq-generator/quote/list/{quantity}")]
        public async Task<IEnumerable<Quote>>  GetQuotesListBySpecificQuantity(int quantity)
        {
           using (HttpClient client = new HttpClient()){
                var result = await client.GetAsync("https://type.fit/api/quotes");
                var quoteListString = await result.Content.ReadAsStringAsync();
                var quoteList = Newtonsoft.Json.JsonConvert.DeserializeObject<IEnumerable<Quote>>(quoteListString);
                return quoteList.Take(quantity);
            }
        }
    }

    public class Quote 
    {
        public string Text { get; set; }
        public string Author { get; set; }
    }
}