using API.Models;
using API.Models.Form;
using API.Services.Security;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace API.Services
{
    public class ExerciseService : IExerciseService
    {

        private readonly IServiceScopeFactory _scopeFactory;
        private readonly IJwtHandler _jwtHandler;
        private readonly IUserGroupService _roleService;

        public ExerciseService(IServiceScopeFactory scopeFactory, IJwtHandler jwtHandler, IUserGroupService roleService)
        {
            _scopeFactory = scopeFactory;
            _jwtHandler = jwtHandler;
            _roleService = roleService;
        }

        public object Get(long Id)
        {
            throw new NotImplementedException();
        }

        public object GetByUserID(long user_ID)
            => GetExerciseByUserID(user_ID);

        public object GetByIDAndUserID(long exercise_ID, long user_ID)
            => GetExerciseByIDAndUserID(exercise_ID, user_ID);

        public void Add(object o)
        {
            throw new NotImplementedException();
        }

        public object AddWithReturn(object o)
            => AddExercise((Exercise)o);

        public void Delete(object o)
        {
            throw new NotImplementedException();
        }
      
        public void Update(object o)
        {
            throw new NotImplementedException();
        }

        public byte[] Compress (string recording)
        {
            return Convert.FromBase64String(recording);
        }

        public string Compress(byte[] recording)
        {
            return Convert.ToBase64String(recording);
        }

        private object AddExercise(Exercise exercise)
        {
            using (var scope = _scopeFactory.CreateScope())
            {
                var dbContext = scope.ServiceProvider.GetRequiredService<Context>();
                dbContext.Exercises.Add(exercise);
                dbContext.SaveChanges();

                exercise.Recording = null;

                return exercise;
            }
        }

        private ClientExercise GetExerciseByIDAndUserID(long exercise_ID, long user_ID)
        {
            using (var scope = _scopeFactory.CreateScope())
            {
                var dbContext = scope.ServiceProvider.GetRequiredService<Context>();
                var exercises = (from ue in dbContext.UserExercises
                                 join e in dbContext.Exercises on ue.Exercise_ID equals e.ID
                                 where ue.User_ID == user_ID && e.ID == exercise_ID
                                 select new ClientExercise
                                 {
                                     ID = e.ID,
                                     Name = e.Name,
                                     Recording = Compress(e.Recording),
                                     Description = e.Description
                                 }).ToList();
                if (!exercises.Any())
                {
                    throw new Exception("No exercises were found for user: " + user_ID);
                }
                return exercises.First();
            }
        }

        private List<Exercise> GetExerciseByUserID(long user_ID)
        {
            using (var scope = _scopeFactory.CreateScope())
            {
                var dbContext = scope.ServiceProvider.GetRequiredService<Context>();
                var exercises = (from ue in dbContext.UserExercises
                                 join e in dbContext.Exercises on ue.Exercise_ID equals e.ID
                                 where ue.User_ID == user_ID
                                 select new Exercise
                                 {
                                     ID = e.ID,
                                     Name = e.Name,
                                     Description = e.Description
                                 }).ToList();
                if (!exercises.Any())
                {
                    throw new Exception("No exercises were found for user: " + user_ID);
                }
                return exercises;
            }
        }
    }
}