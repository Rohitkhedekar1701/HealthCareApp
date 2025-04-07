using OpenTokSDK;
using Microsoft.Extensions.Configuration;


namespace AppointmentService.Services
{
    public class OpenTokService
    {
        private readonly OpenTok _openTok;

        public OpenTokService(IConfiguration configuration)
        {

            var apiKeyStr = configuration["OpenTok:ApiKey"];
            var apiSecret = configuration["OpenTok:ApiSecret"];

            if (!int.TryParse(apiKeyStr, out var apiKey))
            {
                throw new Exception("OpenTok API key in configuration must be a valid integer.");
            }

            _openTok = new OpenTok(apiKey, apiSecret);
        }

        public (string sessionId, string token) CreateVideoRoom()
        {
            var session = _openTok.CreateSession();
            var token = _openTok.GenerateToken(session.Id);
            return (session.Id, token);
        }

        public string CreateSession()
        {
            var session = _openTok.CreateSession();
            return session.Id;
        }

        public string GenerateToken(string sessionId)
        {
            var expireTime = (DateTime.UtcNow.AddHours(2) - new DateTime(1970, 1, 1)).TotalSeconds;
            return _openTok.GenerateToken(sessionId, role: Role.PUBLISHER, expireTime: expireTime);
        }
    }
}
