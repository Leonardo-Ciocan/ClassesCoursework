using System.Collections.Generic;
using Windows.UI;

namespace TimetableApp
{
    /// <summary>
    /// This class represents a course
    /// </summary>
    public class Course
    {
        //default values
        public string Name="";
        public string Class="";
        public string Teacher="";
        public string Notes="";
        public Color TileColor = Color.FromArgb(255,0,104,255); //the default color is the color of the app icon/theme
        public List<Instance> Instances = new List<Instance>();
        public List<Module> Modules = new List<Module>();

       
    }

    /// <summary>
    /// This method is used to indicate the time and duration of each lesson. It is saved in the respective course.
    /// </summary>
    public class Instance
    {
        //setting default values
        public Time StartTime;
        public Time EndTime;
        public int Day;// 0...5
        public string EventName="";
       
    }


}
