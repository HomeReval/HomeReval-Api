using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.Services
{
    public interface IExercisePlanningService : IService
    {

        object GetByWeek(long ID, int weeknumber);
        object GetByID(long user_ID, long exercisePlanning_ID);
    }
}
