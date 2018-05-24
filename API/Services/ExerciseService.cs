using API.Models;
using API.Services.Security;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.Services
{
    public class ExerciseService : IExerciseService
    {

        private readonly IServiceScopeFactory _scopeFactory;
        private readonly IJwtHandler _jwtHandler;

        public ExerciseService(IServiceScopeFactory scopeFactory, IJwtHandler jwtHandler)
        {
            _scopeFactory = scopeFactory;
            _jwtHandler = jwtHandler;
        }


        public object Get(string token)
            => GetExercise(_jwtHandler.GetUserID(token));


        public void Add(string token, object o)
        {
            throw new NotImplementedException();
        }

        public void Delete(string token, object o)
        {
            throw new NotImplementedException();
        }
      
        public void Update(string token, object o)
        {
            throw new NotImplementedException();
        }

        private List<Exercise> GetExercise(long User_ID)
        {
            using (var scope = _scopeFactory.CreateScope())
            {
                var dbContext = scope.ServiceProvider.GetRequiredService<Context>();
                var exercises = (from ue in dbContext.UserExercises
                                 join e in dbContext.Exercises on ue.Exercise_ID equals e.ID
                                 where ue.ID == User_ID
                                 select new Exercise
                                 {
                                     ID = e.ID,
                                     Name = e.Name,
                                     Description = e.Description
                                 }).ToList();
                if (!exercises.Any())
                {
                    throw new Exception("No exercises were found for user: " + User_ID);
                }
                return exercises;
            }

        }
    }
}
