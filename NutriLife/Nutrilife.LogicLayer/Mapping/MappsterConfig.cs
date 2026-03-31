using Mapster;
using Nutrilife.DataAccessLayer.DTO.Response;
using Nutrilife.DataAccessLayer.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nutrilife.LogicLayer.Mapping
{
    public static class MappsterConfig
    {
        public static void MappsterConfigRegister()
        {
            TypeAdapterConfig<Client, ClientResponse>.NewConfig().Map(dest => dest.Client_id , source=> source.Id);
        }
    }
}
