using API.Models;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Linq;

namespace API.Services
{
    public class ExerciseResultService : IExerciseResultService
    {

        private readonly IServiceScopeFactory _scopeFactory;

        public ExerciseResultService(IServiceScopeFactory scopeFactory)
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

        public void AddByPlanningID(ExerciseResult exerciseResult, long user_ID, long exercisePlanning_ID)
            => AddExerciseResultByPlanningID(exerciseResult, user_ID, exercisePlanning_ID);

        // Add an exerciseResult based on planning ID and today's date
        private void AddExerciseResultByPlanningID(ExerciseResult exerciseResult, long user_ID, long exercisePlanning_ID)
        {
            using (var scope = _scopeFactory.CreateScope())
            {
                var today = DateTime.UtcNow;
                var dbContext = scope.ServiceProvider.GetRequiredService<Context>();
                var exerciseSession = dbContext.ExerciseSessions
                    .Where(e => e.ExercisePlanning_ID == exercisePlanning_ID && e.Date.Date == today.Date && e.ExercisePlanning.UserExercise.User_ID == user_ID).First();

                exerciseSession.IsComplete = true;
                dbContext.Update(exerciseSession);

                exerciseResult.ExerciseSession = exerciseSession;
                dbContext.Add(exerciseResult);

                dbContext.SaveChanges();
            }
        }
    }
}