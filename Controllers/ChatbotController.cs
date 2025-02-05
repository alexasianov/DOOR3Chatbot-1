using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualBasic;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net;
using System;
using System.Net.Http;
using System.Numerics;
using System.Reflection.Metadata;
using System.Text;
using System.Text.Json;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using static System.Net.Mime.MediaTypeNames;
using static System.Net.WebRequestMethods;
using static System.Runtime.InteropServices.JavaScript.JSType;
using System.Xml.Linq;

[Route("api/chatbot")]
[ApiController]
public class ChatbotController : ControllerBase
{
    private readonly IHttpClientFactory _httpClientFactory;

    public ChatbotController(IHttpClientFactory httpClientFactory)
    {
        _httpClientFactory = httpClientFactory;
    }

    [HttpPost("ask")]
    public async Task<IActionResult> AskChatbot([FromBody] ChatRequest request)
    {
        string openAiApiKey = "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"; // Replace with your actual API Key
        string modelName = "gpt-4-turbo"; // Change if needed

        var requestBody = new
        {
            model = modelName,
            messages = new[]
            {
                new { role = "system", content = "Your name is Adam, and you  are a senior technology and digital consultant at DOOR3 (https://www.door3.com). Your job is to help direct potential clients who are interested in having DOOR3 perform user experience design, software development, or digital strategy projects. You introduce yourself when first saying hello and state your purpose. Then, you want to ask them to describe the nature of the project they are considering.If the user does not volunteer the business purpose(s) of the software project, pelase ask them to express it. Based on this information, provide them with at least 3 ways that a company like DOOR3 can help them, and ask them which is the closest to what they are looking for. If they say they are looking for a job, please direct them to visit https://www.door3.com/careers/ and politely terminate the conversation. If on the other hand they engage constructively and provide a bit of information about what they are trying to accomplish, please encourage them, tell them we have a well - oiled process for delivering solutions like they one they are requesting, and ask them for their full name, email address, and(optionally) their phone number. Then, let them know that you will think a bit about what you learned from them and get back to them with next steps. If the user engages you in topics which are not related to DOOR3's services, politely let them know that you can only discuss business. Throughout the entire interaction, be polite, professional, but also a bit informal, perhaps with a touch of humor when it is clearly appropriate.   If the person does not respond in kind, resume the rest of the conversation with no attempt at humor." },
                new { role = "user", content = request.Message }
            }
        };

        var content = new StringContent(JsonSerializer.Serialize(requestBody), Encoding.UTF8, "application/json");
        /*
         * using (var _httpClient = _httpClientFactory.CreateClient())
        {
            _httpClient.DefaultRequestHeaders.Authorization = 
                new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", openAiApiKey);
        }
        */
        var _httpClient = _httpClientFactory.CreateClient();
        _httpClient.DefaultRequestHeaders.Authorization =
                new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", openAiApiKey);
        if (string.IsNullOrWhiteSpace(openAiApiKey))
        {
            return BadRequest("OpenAI API key is missing.");
        }
        var response = await _httpClient.PostAsync("https://api.openai.com/v1/chat/completions", content);
        var responseString = await response.Content.ReadAsStringAsync();

        return Ok(responseString);
    }
}

public class ChatRequest
{
    public string Message { get; set; }
}
