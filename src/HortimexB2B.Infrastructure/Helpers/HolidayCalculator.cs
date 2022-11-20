using System;
using System.Collections.Generic;
using System.Text;

namespace HortimexB2B.Infrastructure.Helpers
{
    public static class HolidayCalculator
    {
        public static bool IsHoliday(DateTime day)
        {
            var easter = Easter(day.Year);
            var easterMonday = easter.AddDays(1);

            if (day.DayOfWeek == DayOfWeek.Saturday || day.DayOfWeek == DayOfWeek.Sunday) return true; //Weekend

            if (day.Month == 1 && day.Day == 1) return true; // Nowy Rok
            if (day.Month == 1 && day.Day == 6) return true; // Trzech Króli
            if (day.Month == easterMonday.Month && day.Day == easterMonday.Day) return true; // Poniedziałek Wielkanocny
            if (day.Month == 5 && day.Day == 1) return true; // 1 maja
            if (day.Month == 5 && day.Day == 3) return true; // 3 maja
            if (day.Month == easter.AddDays(60).Month && day.Day == easterMonday.AddDays(60).Day) return true; // Boże ciało
            if (day.Month == 8 && day.Day == 15) return true; // Wniebowzięcie Najświętszej Marii Panny, Święto Wojska Polskiego
            if (day.Month == 11 && day.Day == 1) return true; // Dzień Wszystkich Świętych
            if (day.Month == 11 && day.Day == 11) return true; // Dzień Niepodległości 
            if (day.Month == 12 && day.Day == 25) return true; // Boże Narodzenie
            if (day.Month == 12 && day.Day == 26) return true; // Boże Narodzenie

            return false;
        }

        private static DateTime Easter(int year)
        {
            int day = 0;
            int month = 0;

            int g = year % 19;
            int c = year / 100;
            int h = (c - (int)(c / 4) - (int)((8 * c + 13) / 25) + 19 * g + 15) % 30;
            int i = h - (int)(h / 28) * (1 - (int)(h / 28) * (int)(29 / (h + 1)) * (int)((21 - g) / 11));

            day = i - ((year + (int)(year / 4) + i + 2 - c + (int)(c / 4)) % 7) + 28;
            month = 3;

            if (day > 31)
            {
                month++;
                day -= 31;
            }

            return new DateTime(year, month, day);
        }
    }
}
