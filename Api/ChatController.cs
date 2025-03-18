using System.Net;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using BlazorApp.Shared;

namespace Api
{
    public class ChatController
    {
        private readonly ILogger _logger;
        private static List<ChatMessage> messages = new List<ChatMessage>();

        public ChatController(ILoggerFactory loggerFactory)
        {
            _logger = loggerFactory.CreateLogger<ChatController>();
        }

        [Function("GetChatMessages")]
        public HttpResponseData GetMessages([HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "Chat")] HttpRequestData req)
        {
            var response = req.CreateResponse(HttpStatusCode.OK);
            response.WriteAsJsonAsync(messages.OrderBy(m => m.Timestamp));
            return response;
        }

        [Function("PostChatMessage")]
        public HttpResponseData PostMessage([HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = "Chat")] HttpRequestData req)
        {
            var message = req.ReadFromJsonAsync<ChatMessage>().Result;
            message.Timestamp = DateTime.Now;
            messages.Add(message);

            var response = req.CreateResponse(HttpStatusCode.OK);
            return response;
        }
    }
}