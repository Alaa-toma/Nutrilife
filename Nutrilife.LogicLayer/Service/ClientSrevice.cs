using Mapster;
using Nutrilife.DataAccessLayer.DTO.Request;
using Nutrilife.DataAccessLayer.DTO.Response;
using Nutrilife.DataAccessLayer.Models;
using Nutrilife.DataAccessLayer.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nutrilife.LogicLayer.Service
{
    public class ClientSrevice : IClientService
    {
        private readonly IClientRepository _IClientRepository;
        public ClientSrevice(IClientRepository IclientRepository)
        {
            _IClientRepository = IclientRepository;
        }

      public async  Task<ClientResponse> CreateClient(ClientRequest request)
        {
            var client = request.Adapt<Client>(); // نحول الصيغة الى client
            await _IClientRepository.createAsync(client); // التخزين وفحص الشروط
            return client.Adapt<ClientResponse>(); // نرجع رد بصيغة ريسبونس
        }

        public async Task<List<ClientResponse>> GetAllClients()
        {
            var clients = await _IClientRepository.GetAllAsync(); // احضار البيانات
            return clients.Adapt<List<ClientResponse>>();  // ارجاع البيانات على شكل ليست من ريسبونس
        }
    }
}
