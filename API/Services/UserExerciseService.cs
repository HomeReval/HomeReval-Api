using API.Models;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Linq;

namespace API.Services
{
    public class UserExerciseService : IUserExerciseService
    {

        private readonly IServiceScopeFactory _scopeFactory;

        public UserExerciseService(IServiceScopeFactory scopeFactory)
        {
            _scopeFactory = scopeFactory;
        }

        public void Add(object o)
        {
            throw new System.NotImplementedException();
        }

        public void Add(long user_ID, long exercise_ID)
            => AddUserExercise(user_ID, exercise_ID);

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

        public UserExercise Get(long user_ID, long exercise_ID)
            => GetUserExercise(user_ID, exercise_ID);

        private UserExercise GetUserExercise(long user_ID, long exercise_ID)
        {

            using (var scope = _scopeFactory.CreateScope())
            {
                var dbContext = scope.ServiceProvider.GetRequiredService<Context>();
                var userExercise = dbContext.UserExercises
                    .SingleOrDefault(x => x.User_ID == user_ID && x.Exercise_ID == exercise_ID);

                if (userExercise == null)
                {
                    throw new Exception("User: " + user_ID + ", does not have exercise: " + exercise_ID);
                }
                return userExercise;

            }

        }

        private void AddUserExercise(long user_ID, long exercise_ID)
        {
            using (var scope = _scopeFactory.CreateScope())
            {
                var dbContext = scope.ServiceProvider.GetRequiredService<Context>();

                UserExercise userExercise = new UserExercise
                {
                    Exercise_ID = exercise_ID,
                    User_ID = user_ID
                };

                dbContext.UserExercises.Add(userExercise);
                dbContext.SaveChanges();
            }
        }

    }
}