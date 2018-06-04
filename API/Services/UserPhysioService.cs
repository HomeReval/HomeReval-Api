using API.Models;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Linq;

namespace API.Services
{
    public class UserPhysioService : IUserPhysioService
    {
        private readonly IServiceScopeFactory _scopeFactory;

        public UserPhysioService (IServiceScopeFactory scopeFactory)
        {
            _scopeFactory = scopeFactory;
        }


        public void Add(object o)
        {
            throw new System.NotImplementedException();
        }

        public void Delete(object o)
        {
            throw new System.NotImplementedException();
        }

        public object Get(long Id)
        {
            throw new System.NotImplementedException();
        }

        public void Update(object o)
        {
            throw new System.NotImplementedException();
        }

        public void IsMemberOfPhysio(long physio_ID, long user_ID)
        {
            GetUserPhysio(user_ID, physio_ID);           
        }

        private UserPhysio GetUserPhysio(long user_ID, long physio_ID)
        {
            using (var scope = _scopeFactory.CreateScope())
            {
                var dbContext = scope.ServiceProvider.GetRequiredService<Context>();
                var userPhysio = dbContext.UserPhysios
                    .SingleOrDefault(x => x.Physio_ID == physio_ID && x.User_ID == user_ID);

                if (userPhysio == null)
                {
                    throw new Exception("User: " + user_ID + ", is not a member of Physio: " + physio_ID);
                }
                return userPhysio;

            }
        }
    }
}