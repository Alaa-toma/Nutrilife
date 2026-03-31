using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Google.Apis.Auth.OAuth2;
using Google.Apis.Calendar.v3;
using Google.Apis.Calendar.v3.Data;
using Google.Apis.Services;

namespace Nutrilife.LogicLayer.Service
{
    public interface IMeetingLinkCreation
    {
        Task<string> CreateMeetingLinkAsync(string title, DateTimeOffset start, DateTimeOffset end);
    }
}
