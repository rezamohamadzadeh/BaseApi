using System.Net.Http;
using System.Threading.Tasks;
using BaseApi.Models;
using Microsoft.AspNetCore.Mvc;




namespace BaseApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ConvertCurrencyController : ControllerBase
    {
        private readonly IHttpClientFactory _clientFactory;

        public ConvertCurrencyController(IHttpClientFactory clientFactory)
        {
            _clientFactory = clientFactory;
        }

        /// <summary>
        /// Change price to new currency by base USD 
        /// </summary>
        /// <param name="currency"></param>
        /// <param name="amount"></param>
        /// <returns></returns>

        [HttpGet]
        public async Task<IActionResult> Get(string currency,double amount)
        {
            var httpClient = _clientFactory.CreateClient();

            HttpResponseMessage result =
                 await httpClient
                .GetAsync($"https://api.currencylayer.com/convert?access_key=e090f48dbae0748c3a200fa7d14f6c0d&from=USD&to={currency}&amount={amount}");

            if(result.IsSuccessStatusCode)
            {
                var data = await result.Content.ReadAsAsync<CurrencyDto>();
                return Ok(data);
            }

            return BadRequest();
        }

    }
}
