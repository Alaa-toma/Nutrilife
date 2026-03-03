using Nutrilife.DataAccessLayer.DTO.Request;
using Nutrilife.DataAccessLayer.DTO.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nutrilife.LogicLayer.Service
{
    public interface IClientService
    {
        Task<List<ClientResponse>> GetAllClients();
        Task<ClientResponse> CreateClient(ClientRequest request);
    }
}
