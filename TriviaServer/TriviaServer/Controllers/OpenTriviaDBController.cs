using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using TriviaServer.Models;
using TriviaServer.Models.Repositories;

namespace TriviaServer.Controllers
{
    public class OpenTriviaDBController : ControllerBase
    {
        public async Task<IActionResult> RetrieveQuestions(int totalQuestions)
        {

            var uriBuilder = new UriBuilder("https://opentdb.com/api.php");
            uriBuilder.Query = "amount=" + totalQuestions.ToString();

            var connectionString = uriBuilder.ToString();

            var client = new HttpClient();
            client.BaseAddress = new Uri(connectionString);
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(
                new MediaTypeWithQualityHeaderValue("application/json"));

            var response = await client.GetAsync(client.BaseAddress);

            if(response.IsSuccessStatusCode)
            {
                QuestionsDTO questions = await response.Content.ReadAsAsync<QuestionsDTO>();

                return Ok(questions);
            }
            else
            {
                return BadRequest();
            }
        }
    }
}
