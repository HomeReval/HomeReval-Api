using API.Models;
using API.Services.Security;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace API.Services
{
    public class ExerciseService : IExerciseService
    {

        private readonly IServiceScopeFactory _scopeFactory;
        private readonly IJwtHandler _jwtHandler;
        private readonly IUserService _userService;

        public ExerciseService(IServiceScopeFactory scopeFactory, IJwtHandler jwtHandler, IUserService userService)
        {
            _scopeFactory = scopeFactory;
            _jwtHandler = jwtHandler;
            _userService = userService;
        }


        public object Get(string token)
            => GetExercise(_jwtHandler.GetUserID(token));


        public void Add(string token, object o)
        {
            var user = _userService.GetUser(_jwtHandler.GetUserID(token));

            if (user.UserGroup.ID != Models.Type.Manager && user.UserGroup.ID != Models.Type.Administrator)
            {
                throw new Exception("UserGroup: " + user.UserGroup.Type + " , does not have the rights to add a new exercise");
            }

            var exercise = (Exercise)o;
            AddExercise(exercise);         
        }

        public void Delete(string token, object o)
        {
            throw new NotImplementedException();
        }
      
        public void Update(string token, object o)
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

        private void AddExercise(Exercise exercise)
        {
            using (var scope = _scopeFactory.CreateScope())
            {
                var dbContext = scope.ServiceProvider.GetRequiredService<Context>();
                dbContext.Exercises.Add(exercise);
                dbContext.SaveChanges();
            }
        }

        private List<Exercise> GetExercise(long User_ID)
        {
            using (var scope = _scopeFactory.CreateScope())
            {
                var dbContext = scope.ServiceProvider.GetRequiredService<Context>();
                var exercises = (from ue in dbContext.UserExercises
                                 join e in dbContext.Exercises on ue.Exercise_ID equals e.ID
                                 where ue.User_ID == User_ID
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