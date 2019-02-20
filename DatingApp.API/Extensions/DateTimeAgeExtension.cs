using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DatingApp.API.Extensions
{
    public static class DateTimeAgeExtension
    {
        public static int CalculateAge(this DateTime dateTime)
        {
            var age = DateTime.Now.Year - dateTime.Year;
            //Checking if the user already had birthday this year. For example today is January 20th 2019, user was born on January 10th 2000. 
            //The age variable will contain value 19, but user will be 19 only on 20th, but now is 10th of January
            //so we are adding age amount of years to dateTime and checking if the b-day already passed. if not  we are subtracting 1 year;
            if (dateTime.AddYears(age) > DateTime.Today)
                age--;
            
            return age;
        }
    }
}
