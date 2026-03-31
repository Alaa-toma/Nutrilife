using Google.Apis.Auth.OAuth2;
using Google.Apis.Auth.OAuth2.Flows;
using Google.Apis.Auth.OAuth2.Responses;
using Google.Apis.Calendar.v3;
using Google.Apis.Calendar.v3.Data;
using Google.Apis.Services;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nutrilife.LogicLayer.Service
{
    public class MeetingLinkCreation : IMeetingLinkCreation
    {

        private readonly CalendarService _calendarService;
        private readonly string _timeZone;

        public MeetingLinkCreation(IConfiguration config)
        {
            var clientSecrets = new ClientSecrets
            {
                ClientId = config["Google:ClientId"]!,
                ClientSecret = config["Google:ClientSecret"]!
            };

            var token = new TokenResponse
            {
                RefreshToken = config["Google:RefreshToken"]!
            };

            var credential = new UserCredential(
                new GoogleAuthorizationCodeFlow(
                    new GoogleAuthorizationCodeFlow.Initializer
                    {
                        ClientSecrets = clientSecrets,
                        Scopes = new[] { CalendarService.Scope.Calendar }
                    }),
                "user",
                token
            );

            _calendarService = new CalendarService(new BaseClientService.Initializer
            {
                HttpClientInitializer = credential,
                ApplicationName = "NutriLife"
            });
        }

        public async Task<string> CreateMeetingLinkAsync(string title, DateTimeOffset start, DateTimeOffset end)
        {
            // Validate inputs
            if (string.IsNullOrWhiteSpace(title))
                throw new ArgumentException("Title cannot be empty.", nameof(title));

            if (start >= end)
                throw new ArgumentException("Start time must be before end time.");

            // Build the event
            var startUtc = start.ToUniversalTime();
            var endUtc = end.ToUniversalTime();

            var newEvent =  new Event
            {
                Summary = title,
                Start = new EventDateTime
                {
                    DateTimeRaw = startUtc.ToString("yyyy-MM-ddTHH:mm:ssZ"),
                    TimeZone = _timeZone
                },
                End = new EventDateTime
                {
                    DateTimeRaw = endUtc.ToString("yyyy-MM-ddTHH:mm:ssZ"),
                    TimeZone = _timeZone
                },
                ConferenceData = new ConferenceData
                {
                    CreateRequest = new CreateConferenceRequest
                    {
                        RequestId = Guid.NewGuid().ToString(),
                        ConferenceSolutionKey = new ConferenceSolutionKey
                        {
                            Type = "hangoutsMeet"
                        }
                    }
                }
            };

            var insertRequest = _calendarService.Events.Insert(newEvent, "primary");
            insertRequest.ConferenceDataVersion = 1;

            Event createdEvent = await insertRequest.ExecuteAsync();

            // Extract Meet link
            string? meetLink = createdEvent.ConferenceData
                ?.EntryPoints
                ?.FirstOrDefault(e => e.EntryPointType == "video")
                ?.Uri;

            if (string.IsNullOrEmpty(meetLink))
                throw new Exception("Meeting created but no Meet link was returned.");

            return meetLink; // https://meet.google.com/abc-defg-hij
        }



    }
}
