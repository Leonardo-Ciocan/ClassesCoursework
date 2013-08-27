using System;
using System.Collections.Generic;
using Windows.UI;

namespace TimetableApp
{
    /// <summary>
    /// Represents a timetable , which includes
    /// </summary>
    public class StudentDataBundle
    {
        public string Name;
        public string Description;
        //public AcademyBundle AcademyInfo=new AcademyBundle();
        public List<Course> Courses = new List<Course>();


        /// <summary>
        /// This method returns the total number of instances in the timetable
        /// </summary>
        /// <returns></returns>
        public int GetTotalInstances()
        {
            int total=0;
            foreach (Course model in Courses)
            {
                foreach (Instance instance in model.Instances)
                {
                    total++;
                }
            }
            return total;
        }

        /// <summary>
        /// This method returns whether the bundle timetable has a weekend or not.
        /// </summary>
        /// <returns></returns>
        public bool hasWeekend()
        {
            bool result = false;
            foreach (Course model in this.Courses)
            {
                foreach (Instance instance in model.Instances)
                {
                    if (instance.Day > 4) return true;
                }
            }
            return result;
        }
    }


    public class DateSpan
    {
        public bool AcademyClosed;
        public DateTime StartDate;
        public int Duration;//in days;
        public string Description;
    }

    public class AcademyBundle
    {
        public string Name="";
        public string Location="";
        public string Phone="";
        public string Website = "";
        public Color Accent;
        public List<DateSpan> Events = new List<DateSpan>();
        
    }
}


