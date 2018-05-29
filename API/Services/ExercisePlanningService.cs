using API.Models;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;

namespace API.Services
{
    public class ExercisePlanningService : IExercisePlanningService
    {

        private readonly IServiceScopeFactory _scopeFactory;

        public ExercisePlanningService(IServiceScopeFactory scopeFactory)
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

        public object GetByWeek(long id, int weeknumber)
             => GetExercisesByWeek(id, weeknumber);

        private List<dynamic> GetExercisesByWeek(long id, int weeknumber)
        {
            using (var scope = _scopeFactory.CreateScope())
            {
                var dbContext = scope.ServiceProvider.GetRequiredService<Context>();
                var exercisePlannings = dbContext.ExercisePlannings
                    .Where(e => (Helper.GetIso8601WeekOfYear(e.StartDate) == weeknumber || Helper.GetIso8601WeekOfYear(e.EndDate) == weeknumber) && e.UserExercise.User_ID == id)
                    .Select(e => new
                    {
                        e.ID,
                        e.StartDate,
                        e.EndDate,
                        e.Description,
                        e.Amount,
                        Exercise = new
                        {
                            e.UserExercise.Exercise.ID,
                            e.UserExercise.Exercise.Name,
                            e.UserExercise.Exercise.Description
                        },
                        ExerciseSessions = e.ExerciseSessions.Select(es => new
                        {
                            es.ID,
                            es.Date,
                            es.IsComplete,
                            ExerciseResult = (es.ExerciseResult == null ? null : new
                            {
                                es.ExerciseResult.ID,
                                es.ExerciseResult.Duration,
                                es.ExerciseResult.Score
                            }
                            )
                        })
                        .ToList<dynamic>()
                    }).ToList<dynamic>();

                //var exercisePlannings = (from ue in dbContext.UserExercises
                //                 join e in dbContext.Exercises on ue.Exercise_ID equals e.ID
                //                 join ep in dbContext.ExercisePlannings on ue.ID equals ep.UserExercise_ID
                //                 join es in dbContext.ExerciseSessions on ep.ID equals es.ExercisePlanning_ID
                //                 join er in dbContext.ExerciseResults on es.ID equals er.ExerciseSession_ID
                //                 where ue.User_ID == id //&& weekNum == (ep.Date.DayOfYear / 7)
                //                 select new
                //                 {
                //                     ep.ID,
                //                     ep.StartDate,
                //                     ep.EndDate,
                //                     ep.Description,
                //                     ep.Amount,
                //                     ExerciseSessions = ep.ExerciseSessions.Select(qes => new
                //                     {
                //                         qes.ID,
                //                         qes.Date,
                //                         qes.IsComplete,
                //                         ExerciseResult = new
                //                         {

                //                             es.ExerciseResult.ID


                //                         }
                //                     })



                //                 }).ToList<dynamic>();




                if (!exercisePlannings.Any())
                {
                    throw new Exception("No exercises plannings were found for user: " + id + ", in week: " + weeknumber);
                }
                return exercisePlannings;
            }
        }
    }
}