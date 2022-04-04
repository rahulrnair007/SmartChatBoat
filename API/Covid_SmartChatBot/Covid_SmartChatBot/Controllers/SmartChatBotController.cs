using Google.Apis.Auth.OAuth2;
using Google.Apis.Dialogflow.v2;
using Google.Apis.Dialogflow.v2.Data;
using Google.Apis.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace Covid_SmartChatBot.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class SmartChatBotController : ControllerBase
    {
        private const string ProjectId = "covidchatbot-fapm";
        private const string CredId = "covidchatbot-fapm-e9b31f08a8f2.json";
        private readonly GoogleCredential _googleCredential;
        private readonly ILogger<SmartChatBotController> _logger;

        /// <summary>
        /// Initializes a new instance of the <see cref="SmartChatBotController"/> class.
        /// </summary>
        /// <param name="logger">The logger.</param>
        public SmartChatBotController(ILogger<SmartChatBotController> logger)
        {
            _logger = logger;
            _googleCredential = GoogleCredential.FromFile(CredId);
        }

        /// <summary>
        /// Gets the specified user data.
        /// </summary>
        /// <param name="userData">The user data.</param>
        /// <returns></returns>
        [Route("GetIntents")]
        [HttpPost]
        public IActionResult Get([FromBody]UserData userData)
        {
            var scopedCreds = _googleCredential.CreateScoped(DialogflowService.Scope.CloudPlatform);
            var SessionId = userData.sessionId;
            var response = new DialogflowService(new BaseClientService.Initializer
            {
                HttpClientInitializer = scopedCreds,
                ApplicationName = ProjectId
            }).Projects.Agent.Sessions.DetectIntent(
                        new GoogleCloudDialogflowV2DetectIntentRequest
                        {
                            QueryInput = new GoogleCloudDialogflowV2QueryInput
                            {
                                Text = new GoogleCloudDialogflowV2TextInput
                                {
                                    Text = userData.queryData,
                                    LanguageCode = "en-US"
                                }
                            }
                        },
                        $"projects/{ProjectId}/agent/sessions/{SessionId}")
                        .Execute();
            return Ok(response.QueryResult);
        }

    }
}
