using API.Models;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using Microsoft.EntityFrameworkCore;

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
            => AddExercisePlanning((ExercisePlanning)o);

        public void Delete(object o)
        {
            throw new System.NotImplementedException();
        }

        public object Get(long Id)
        {
            throw new System.NotImplementedException();
        }

        public object GetByID(long user_ID, long exercisePlanning_ID)
            => GetExercisePlanningsByIdWithResult(user_ID, exercisePlanning_ID);

        public object GetRemainingSessionsByDate(long user_ID, DateTime date)
            => GetExercisePlanningsWithRemainingSessionsByDate(user_ID, date);

        public void Update(object o)
        {
            throw new System.NotImplementedException();
        }

        public object GetByWeek(long id, int weeknumber)
             => GetExercisesByWeek(id, weeknumber);


        private List<dynamic> GetExercisePlanningsWithRemainingSessionsByDate(long user_ID, DateTime date)
        {
            using (var scope = _scopeFactory.CreateScope())
            {
                var dbContext = scope.ServiceProvider.GetRequiredService<Context>();
                var exercisePlannings = dbContext.ExercisePlannings
                    .Where(e => (e.UserExercise.User_ID == user_ID))
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
                        }).Where(session => (session.Date.Date == date.Date && session.IsComplete == false))
                        .ToList<dynamic>()
                    }).Where(ee => ee.ExerciseSessions.Any() == true)
                    .ToList<dynamic>();

                if (!exercisePlannings.Any())
                {
                    throw new Exception("No exercises sessions were found for user: " + user_ID + ", in on : " + date.Date);
                }
                return exercisePlannings;
            }
        }

        private List<dynamic> GetExercisePlanningsByIdWithResult(long user_ID, long exercisePlanning_ID)
        {
            using (var scope = _scopeFactory.CreateScope())
            {
                var dbContext = scope.ServiceProvider.GetRequiredService<Context>();
                var exercisePlannings = dbContext.ExercisePlannings
                    .Where(e => ( e.ID == exercisePlanning_ID && e.UserExercise.User_ID == user_ID  ))
                    .Select(e => new
                    {
                        e.ID,
                        ExerciseSessions = e.ExerciseSessions.Select(es => new
                        {
                            es.ID,
                            ExerciseResult = (es.ExerciseResult == null ? null : new
                            {
                                es.ExerciseResult.ID,
                                Recording = Helper.Compress(es.ExerciseResult.Result)
                            }
                            )
                        })
                        .ToList<dynamic>()
                    }).ToList<dynamic>();

                if (!exercisePlannings.Any())
                {
                    throw new Exception("No exercises plannings were found for user: " + user_ID + ", with planning ID: " + exercisePlanning_ID);
                }
                return exercisePlannings;
            }
        }

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

                if (!exercisePlannings.Any())
                {
                    throw new Exception("No exercises plannings were found for user: " + id + ", in week: " + weeknumber);
                }
                return exercisePlannings;
            }
        }

        private void AddExercisePlanning(ExercisePlanning exercisePlanning)
        {
            using (var scope = _scopeFactory.CreateScope())
            {
                var dbContext = scope.ServiceProvider.GetRequiredService<Context>();

                dbContext.Attach(exercisePlanning);
                dbContext.Entry(exercisePlanning).State = EntityState.Added;

                // Add ExerciseSessions
                foreach (var date in Helper.GetDateTimes(exercisePlanning.StartDate, exercisePlanning.EndDate))
                {
                    dbContext.ExerciseSessions.Add( new ExerciseSession()
                    {
                        Date = date,
                        IsComplete = false,
                        ExercisePlanning = exercisePlanning
                    });
                }

                dbContext.SaveChanges();
            }
        }
    }
}