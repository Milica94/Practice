using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using TAP.Models;

namespace TAP.Data
{
    public interface IClient
    {
        bool SaveChanges();
        IEnumerable<Client> GetAllClients();
        Client GetClientDataById(int id);
        void CreateClient(Client client);
        void UpdateClient(Client client);
        void DeleteClient(Client client);
        ErrorDetails ErrorDescription(HttpStatusCode code);
          
    }
}
