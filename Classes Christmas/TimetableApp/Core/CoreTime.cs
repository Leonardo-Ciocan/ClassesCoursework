using System;

namespace TimetableApp
{
    /// <summary>
    /// Used to deal with time operations.
    /// </summary>
    public static class CoreTime
    {
        /// <summary>
        /// Check for the earliest time for space setting the StudentDataBundle view.
        /// </summary>
        /// <param name="tb"></param>
        /// <returns>Earliest Time</returns>
        public static Time GetEarliest(StudentDataBundle tb)
        {
            Time earliest = Time.MaximumTime;
            foreach (Course t in tb.Courses)
            {
                foreach (Instance instance in t.Instances)
                {
                    if (instance.StartTime <= earliest) earliest = instance.StartTime;
                }
            }
            return earliest;
        }

        /// <summary>
        /// Check for the latest time for space setting the StudentDataBundle view.
        /// </summary>
        /// <param name="tb"></param>
        /// <returns>Latest time</returns>
        public static Time GetLatest(StudentDataBundle tb)
        {
            Time latest = Time.MinimumTime;
            foreach (Course t in tb.Courses)
            {
                foreach (Instance instance in t.Instances)
                {
                    if (instance.EndTime >= latest) latest = instance.EndTime;
                }
            }
            return latest;
            
        }

        /// <summary>
        /// This method checks whether a proposed instace is overlapping another
        /// </summary>
        /// <param name="instance">The instance we are checking the overlap with</param>
        /// <param name="table">The StudentDataBundle in which we check for overlaps</param>
        /// <returns></returns>
        public static TimeOverlapData isOverlapping(Instance instance, StudentDataBundle table)
        {
            foreach (Course model in table.Courses)
            {
                foreach (Instance current in model.Instances)
                {
                    if (instance.Day == current.Day)
                    {
                        if (instance != current && !model.Instances.Contains(instance) && !(current.EndTime == instance.EndTime || current.StartTime == instance.StartTime) )
                        {
                            if (instance.StartTime == current.StartTime) return new TimeOverlapData { isOverlapping = true, OverlappingEventName = model.Name };
                            if (instance.StartTime < current.StartTime)
                            {
                                if (instance.EndTime > current.StartTime)
                                {
                                    return new TimeOverlapData { isOverlapping = true, OverlappingEventName = model.Name };
                                }
                            }
                            if (instance.StartTime >= current.StartTime && instance.EndTime <= current.EndTime)
                            {
                                return new TimeOverlapData { isOverlapping = true, OverlappingEventName = model.Name };
                            }
                        }
                    }
                }
            }
            return new TimeOverlapData{isOverlapping=false};
        }

        /// <summary>
        /// Formats a string into a easily readable format
        /// </summary>
        /// <param name="date">The date to format</param>
        /// <returns>The formatted string</returns>
        public static string FormatTime(DateTime date)
        {
            string final="";
            final += Constants.Months[date.Month - 1];
            final += " ";
            if (date.Day.ToString().EndsWith("1"))
            {
                final += date.Day.ToString() + "st";
            }
            else if (date.Day.ToString().EndsWith("2"))
            {
                final += date.Day.ToString() + "nd";
            }
            else if (date.Day.ToString().EndsWith("3"))
            {
                final += date.Day.ToString() + "rd";
            }
            else 
            {
                final += date.Day.ToString() + "th";
            }
            final += "   " + date.Year.ToString();
            return final;
        }
    }

    /// <summary>
    /// A structure that holds the result of an overlapping check
    /// </summary>
    public struct TimeOverlapData
    {
        public bool isOverlapping;
        public string OverlappingEventName; //this is null if no overlap was found
    }

    /// <summary>
    /// Class that represents a time span.
    /// </summary>
    public class Time
    {
        public int Minutes;
        public int Hours;

        //standard comparasion operators
        public static bool operator >=(Time t1, Time t2)
        {
            return (t1.TotalMinutes >= t2.TotalMinutes);
        }
        public static bool operator <=(Time t1, Time t2)
        {
            return (t1.TotalMinutes <= t2.TotalMinutes);
        }

        public static bool operator >(Time t1, Time t2)
        {
            return (t1.TotalMinutes > t2.TotalMinutes);
        }
        public static bool operator <(Time t1, Time t2)
        {
            return (t1.TotalMinutes < t2.TotalMinutes);
        }

        public static Time operator +(Time t1, Time t2)
        {
            return new Time { Hours = t1.Hours + t2.Hours, Minutes = t1.Minutes + t2.Minutes };
        }

        public static Time operator -(Time t1, Time t2)
        {
            return new Time { Hours = t1.Hours - t2.Hours, Minutes = t1.Minutes - t2.Minutes };
        }

        public static bool operator ==(Time t1 , Time t2)
        {
            return t1.Hours == t2.Hours && t1.Minutes == t2.Minutes;
        }

        public static bool operator !=(Time t1, Time t2)
        {
            return t1.Hours != t2.Hours || t1.Minutes != t2.Minutes;
        }

        public int TotalMinutes
        {
            get { return Hours * 60 + Minutes; }
        }

        //from 00:00 to 23:59
        public static Time MinimumTime = new Time { Hours = 0, Minutes = 0 };
        public static Time MaximumTime = new Time { Hours = 23, Minutes = 59 };

        //Useful for formatting , returns in hh:mm format
        public override string ToString()
        {
            string result="";
            result += Hours.ToString();
            result += ":" + Minutes.ToString();
            return result;
        }
    }
}
