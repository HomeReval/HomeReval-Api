﻿using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;

namespace API.Services
{
    public static class Helper
    {

        // https://stackoverflow.com/questions/11154673/get-the-correct-week-number-of-a-given-date
        // This presumes that weeks start with Monday.
        // Week 1 is the 1st week of the year with a Thursday in it.
        public static int GetIso8601WeekOfYear(DateTime time)
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

        public static List<DateTime> GetDateTimes(DateTime startDate, DateTime endDate)
        {
            var dates = new List<DateTime>();

            for (var dt = startDate; dt <= endDate; dt = dt.AddDays(1))
            {
                dates.Add(dt);
            }

            return dates;
        }

        public static bool AreDatesInWeekNumber(DateTime startDate, DateTime endDate, int weeknumber)
        {
            var dates = GetDateTimes(startDate, endDate);
            foreach (DateTime date in dates)
            {
                var week = GetIso8601WeekOfYear(date);
                if (week == weeknumber)
                {
                    return true;
                }
            }
            return false;
        }

        public static byte[] Compress(string recording)
        {
            return Convert.FromBase64String(recording);
        }

        public static string Compress(byte[] recording)
        {
            return Convert.ToBase64String(recording);
        }


    }
}
