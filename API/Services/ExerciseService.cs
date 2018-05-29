using API.Models;
using API.Services.Security;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
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

        public object GetByUserID(long User_ID)
            => GetExerciseByUserID(User_ID);


        public void Add(object o)
        {        
            var exercise = (Exercise)o;
            AddExercise(exercise);         
        }

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

        private void AddExercise(Exercise exercise)
        {
            using (var scope = _scopeFactory.CreateScope())
            {
                var dbContext = scope.ServiceProvider.GetRequiredService<Context>();
                dbContext.Exercises.Add(exercise);
                dbContext.SaveChanges();
            }
        }

        private List<Exercise> GetExerciseByUserID(long User_ID)
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
        public object GetByWeek(long id, int weeknumber)
         => GetExercisesByWeek(id, weeknumber);

        private List<ExerciseSession> GetExercisesByWeek(long id, int weeknumber)
        {
            using (var scope = _scopeFactory.CreateScope())
            {
                var dbContext = scope.ServiceProvider.GetRequiredService<Context>();

                var exercises = dbContext.ExerciseSessions
                .Include(e => e.ExercisePlanning)
                    .ThenInclude(e => e.UserExercise)
                        .ThenInclude(e => e.Exercise)
                .Where(e =>
                   GetIso8601WeekOfYear(e.Date) == weeknumber
                ).ToList();

                var exercisePlannings = dbContext.ExercisePlannings
                    .Include(e => e.UserExercise)
                        .ThenInclude(e => e.Exercise)
                    .Include(e => e.ExerciseSessions)
                        .ThenInclude(e => e.)



                //var exercises = (from ue in dbContext.UserExercises
                //                 join e in dbContext.Exercises on ue.Exercise_ID equals e.ID
                //                 join ep in dbContext.ExercisePlannings on ue.ID equals ep.UserExercise_ID
                //                 where ue.User_ID == id //&& weekNum == (ep.Date.DayOfYear / 7)
                //                 select new Exercise
                //                 {
                //                     ID = e.ID,
                //                     Name = e.Name,
                //                     Description = e.Description
                //                 }).ToList();
                if (!exercises.Any())
                {
                    throw new Exception("No exercises were found for user: " + id);
                }
                return exercises;
            }
        }


        // https://stackoverflow.com/questions/11154673/get-the-correct-week-number-of-a-given-date
        // This presumes that weeks start with Monday.
        // Week 1 is the 1st week of the year with a Thursday in it.
        private static int GetIso8601WeekOfYear(DateTime time)
        {
            // Seriously cheat.  If its Monday, Tuesday or Wednesday, then it'll 
            // be the same week# as whatever Thursday, Friday or Saturday are,
            // and we always get those right
            DayOfWeek day = CultureInfo.InvariantCulture.Calendar.GetDayOfWeek(time);
            if (day >= DayOfWeek.Monday && day <= DayOfWeek.Wednesday)
            {
                time = time.AddDays(3);
            }

            // Return the week of our adjusted day
            return CultureInfo.InvariantCulture.Calendar.GetWeekOfYear(time, CalendarWeekRule.FirstFourDayWeek, DayOfWeek.Monday);
        }

    }
}