using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Data.Entity.Core;
using System.Diagnostics;
using System.IO.Pipelines;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using TAP.Models;

namespace TAP.Data
{
    public class ClientRepository : IClient
    {
        private readonly TAPContext _context;
        public ClientRepository(TAPContext context)
        {
            _context = context;
        }

        public void CreateClient(Client client)
        {
            if(client==null)
            {
                throw new ArgumentNullException(nameof(client));
            }
            _context.Client.Add(client);
        }

        public void DeleteClient(Client client)
        {
            if (client == null)
            {
                throw new ArgumentNullException(nameof(client));
            }
            _context.Client.Remove(client);
        }

        public ErrorDetails ErrorDescription(HttpStatusCode code)
        {
           var _httpStatusCode = (int)code;

           if(_httpStatusCode == 400)
            {
                return new ErrorDetails()
                {
                    StatusCode = 400,
                    Message = "Bad request"
                };
                
            }
            else if(_httpStatusCode == 401)
            {
                return new ErrorDetails()
                {
                    StatusCode = 401,
                    Message = "You are unauthorized user. You have no access. Please contact your authorization authority."
                };
            }
           else if(_httpStatusCode == 403)
            {
                return new ErrorDetails()
                {
                    StatusCode = 403,
                    Message = "Forbidden"
                };
            }
           else if(_httpStatusCode == 404)
            {
                return new ErrorDetails()
                {
                    StatusCode = 404,
                    Message = "Not found"
                };
            }
           else
            {
                return new ErrorDetails()
                {
                    StatusCode = 500,
                    Message = "Internal Server error. Something went wrong"
                };
            }

        }

        public IEnumerable<Client> GetAllClients()
        {
           
            return _context.Client.ToList();
        }


        public Client GetClientDataById(int id)
        {
            return _context.Client.Where(clientId => clientId.Id == id).FirstOrDefault();
        }

        public bool SaveChanges()
        {
            try
            {
               return (_context.SaveChanges()>=0);
            }
            catch (UpdateException update)
            {
                Debug.WriteLine(update.InnerException);
                return false;

            }
            catch (System.Data.Entity.Infrastructure.DbUpdateException excp)
            {
                Debug.WriteLine(excp.InnerException);
                return false;
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.InnerException);
                return false;
            }

        }

        public void UpdateClient(Client client)
        {
            var new_client = _context.Client.Find(client.Id);
            new_client.Name = client.Name;
            new_client.CreatedAt = client.CreatedAt;
            _context.Entry(new_client).State = Microsoft.EntityFrameworkCore.EntityState.Modified;

            try
            {
                _context.SaveChanges();
            }

            catch (UpdateException update)
            {
                Debug.WriteLine(update.InnerException);

            }
            catch (System.Data.Entity.Infrastructure.DbUpdateException excp)
            {
                Debug.WriteLine(excp.InnerException);
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.InnerException);
            }

        }
    }
}
