using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using Microsoft.VisualStudio.TestPlatform.UnitTestFramework;
using TimetableApp;

namespace Tests
{
    [TestClass]
    public class GeneralTests
    {
      


        [TestMethod]
        public void TimeTotalMinutes()
        {
            var time1 = new Time();
            var time2 = new Time();

            time1.Hours = 8;
            time1.Minutes = 30;

            time2.Hours = 8;
            time2.Minutes = 31;

            //Testing totals
            double actualTotal1 = time1.Hours * 60 + time1.Minutes;
            double actualTotal2 = time2.Hours * 60 + time2.Minutes;

            Assert.AreEqual(actualTotal1,time1.TotalMinutes);
            Assert.AreEqual(actualTotal2, time2.TotalMinutes);
        }

        Time timeA = new Time{Hours = 8 , Minutes = 9};
        Time timeB = new Time { Hours = 8, Minutes = 9 };

        [TestMethod]
        public void TimeBigger()
        {
            bool actual = false;

            Assert.AreEqual(actual,timeA > timeB);
        }

        [TestMethod]
        public void TimeSmaller()
        {
            bool actual = false;

            Assert.AreEqual(actual, timeA < timeB);
        }

        [TestMethod]
        public void TimeBiggerEqual()
        {
            bool actual = true;

            Assert.AreEqual(actual, timeA >= timeB);
        }

        [TestMethod]
        public void TimeSmallerEqual()
        {
            bool actual = true;

            Assert.AreEqual(actual, timeA <= timeB);
        }

        [TestMethod]
        public void TimeEqual()
        {
            Time a = new Time {Hours = 9, Minutes = 30};
            Time b = new Time {Hours = 9, Minutes = 30};
            Time c = new Time {Hours = 9, Minutes = 31};

            Assert.AreEqual(true, a==b);
            Assert.AreEqual(false , b == c);
        }




        [TestMethod]
        public void TimetableEarliest()
        {
            StudentDataBundle testTable = new StudentDataBundle();

            Instance A = new Instance {StartTime = new Time {Hours = 10, Minutes = 30}};
            Instance B = new Instance {StartTime = new Time {Hours = 12, Minutes = 30}};
            Course X = new Course();
            X.Instances.Add(A);
            X.Instances.Add(B);

            Instance C = new Instance { StartTime = new Time { Hours = 14, Minutes = 15 } };
            Instance D = new Instance { StartTime = new Time { Hours = 15, Minutes = 45 } };
            Course Y = new Course();
            Y.Instances.Add(C);
            Y.Instances.Add(D);


            testTable.Courses.Add(X);
            testTable.Courses.Add(Y);
            
            Time ActualLowest = new Time {Hours = 10, Minutes = 30};
            Assert.AreEqual(true , ActualLowest == CoreTime.GetEarliest(testTable));

        }


        [TestMethod]
        public void TimetableLatest()
        {
            StudentDataBundle testTable = new StudentDataBundle();

            Instance A = new Instance { EndTime = new Time { Hours = 10, Minutes = 30 } };
            Instance B = new Instance { EndTime = new Time { Hours = 12, Minutes = 30 } };
            Course X = new Course();
            X.Instances.Add(A);
            X.Instances.Add(B);

            Instance C = new Instance { EndTime = new Time { Hours = 14, Minutes = 15 } };
            Instance D = new Instance { EndTime = new Time { Hours = 15, Minutes = 45 } };
            Course Y = new Course();
            Y.Instances.Add(C);
            Y.Instances.Add(D);


            testTable.Courses.Add(X);
            testTable.Courses.Add(Y);

            Time tst = CoreTime.GetLatest(testTable);
            Debug.WriteLine(tst.Minutes);
            Time ActualHighest = new Time { Hours = 15, Minutes = 45 };
            Assert.AreEqual(true, ActualHighest  == CoreTime.GetLatest(testTable));

        }
    }
}
