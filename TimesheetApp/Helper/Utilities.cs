using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace TimesheetApp.Helper
{
    public class Utilities
    {
        public static JObject ParseJsonFileToObject(string filePath)
        {
            using (var reader = new StreamReader(filePath))
            {
                var json = reader.ReadToEnd();
                var jobj = JObject.Parse(json);
                return jobj;
            }
        }

        public static string GetWorkingDir()
        {
            var workingDir = AppDomain.CurrentDomain.BaseDirectory;
            return Directory.GetParent(workingDir).FullName.Substring(0, workingDir.IndexOf("bin\\"));
        }

        public static string GetMD5(string str)
        {
            MD5 md5 = new MD5CryptoServiceProvider();
            byte[] fromData = Encoding.UTF8.GetBytes(str);
            byte[] targetData = md5.ComputeHash(fromData);
            string byte2String = null;

            for (int i = 0; i < targetData.Length; i++)
            {
                byte2String += targetData[i].ToString("x2");

            }
            return byte2String;
        }

        public static string RandomString(int length)
        {
            Random random = new Random();
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            return new string(Enumerable.Repeat(chars, length)
              .Select(s => s[random.Next(s.Length)]).ToArray());
        }

        public static DateTime GetMonday(DateTime time)
        {
            if (time.DayOfWeek != DayOfWeek.Monday)
            {
                var monday = time.Subtract(new TimeSpan((int)time.DayOfWeek - 1, 0, 0, 0));
                if (monday.Day > time.Day)
                {
                    return time.Subtract(new TimeSpan((int)time.DayOfWeek + 6, 0, 0, 0));
                }

                return monday;
            }

            return time;
        }

        public static IEnumerable<DateTime> GetDaysOfCurrentWeek()
        {
            var daysOfCurrentWeek = new List<DateTime>();
            var currentDate = DateTime.Today;
            var monday = GetMonday(currentDate);

            for (var i = 0; i < 7; i++)
            {
                daysOfCurrentWeek.Add(monday);
                monday = monday.AddDays(1);
            }

            return daysOfCurrentWeek;
        }

        public static IEnumerable<string> GetDaysName()
        {
            var names = new List<string> { "Monday", "Tuesday", "Wednesday", "Thursday", "Friday", "Saturday", "Sunday" };

            return names;
        }

        public static IEnumerable<string> GetSimpleDate(IEnumerable<DateTime> dates)
        {
            var simpleDate = new List<string>();
            for(var i = 0; i < dates.Count(); i++)
            {
                var day = dates.ElementAt(i).Day;
                var month = dates.ElementAt(i).Month;
                
                var dayString = day < 10 ? ("0" + day.ToString()) : day.ToString();
                var monthString = month < 10 ? ("0" + month.ToString()) : month.ToString();

                var date = string.Format("{0}/{1}", monthString, dayString);
                simpleDate.Add(date);
            }

            return simpleDate;
        }
    }
}
