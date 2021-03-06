﻿using API.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.Services
{
    public interface IExerciseService : IService
    {
        object GetByUserID(long ID);
        object GetByIDAndUserID(long exercise_ID, long user_ID);
        object AddWithReturn(object o);
    }
}
