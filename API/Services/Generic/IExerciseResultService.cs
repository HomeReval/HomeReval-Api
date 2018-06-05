using API.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.Services
{
    public interface IExerciseResultService : IService
    {

        void AddByPlanningID(ExerciseResult exerciseResult, long user_ID, long exercisePlanning_ID);

    }
}
