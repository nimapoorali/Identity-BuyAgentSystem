using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NP.Common
{
    public static class PersianDate
    {
        /// <summary>
        /// Convert Miladi DateTime to Shamsi Date
        /// </summary>
        /// <param name="dt"></param>
        /// <returns></returns>
        public static string ToPersianDate(this DateTime dt)
        {
            PersianCalendar pc = new();
            int year = pc.GetYear(dt);
            int month = pc.GetMonth(dt);
            int day = pc.GetDayOfMonth(dt);

            return year.ToString("0000") + "/" + month.ToString("00") + "/" + day.ToString("00");
        }

        /// <summary>
        /// Convert Miladi DateTime to Shamsi Date
        /// </summary>
        /// <param name="dt"></param>
        /// <returns></returns>
        public static string? ToPersianDate(this DateTime? dt)
        {
            if (dt is null)
                return null;

            return dt.Value.ToPersianDate();
        }

        /// <summary>
        /// Convert Miladi DateTime to Shamsi date & time
        /// </summary>
        /// <param name="dt"></param>
        /// <param name="seperator">Seperator between date and time</param>
        /// <returns></returns>
        public static string ToPersianDateTime(this DateTime dt, string seperator)
        {
            PersianCalendar pc = new();
            int year = pc.GetYear(dt);
            int month = pc.GetMonth(dt);
            int day = pc.GetDayOfMonth(dt);

            return year.ToString("0000") + "/" + month.ToString("00") + "/" + day.ToString("00") + seperator + dt.ToString("HH:mm:ss");
        }

        /// <summary>
        /// Convert Miladi DateTime to Shamsi date & time
        /// </summary>
        /// <param name="dt"></param>
        /// <returns></returns>
        public static string ToPersianDateTime(this DateTime dt)
        {
            return ToPersianDateTime(dt, " ");
        }

        /// <summary>
        /// Convert Miladi DateTime to Shamsi date & time
        /// </summary>
        /// <param name="dt"></param>
        /// <returns></returns>
        public static string? ToPersianDateTime(this DateTime? dt)
        {
            if (dt is null)
                return null;

            return dt.Value.ToPersianDateTime();
        }

        /// <summary>
        /// Convert Miladi DateTime to Shamsi short date & time
        /// </summary>
        /// <param name="dt"></param>
        /// <param name="seperator">Seperator between date and time</param>
        /// <returns></returns>
        public static string ToPersianShortDateTime(this DateTime dt, string seperator)
        {
            PersianCalendar pc = new();
            int year = pc.GetYear(dt);
            int month = pc.GetMonth(dt);
            int day = pc.GetDayOfMonth(dt);

            return year.ToString("0000") + "/" + month.ToString("00") + "/" + day.ToString("00") + seperator + dt.ToString("HH:mm");
        }

        /// <summary>
        /// Convert Miladi DateTime to Shamsi short date & time
        /// </summary>
        /// <param name="dt"></param>
        /// <returns></returns>
        public static string ToPersianShortDateTime(this DateTime dt)
        {
            return ToPersianShortDateTime(dt, " ");
        }

        /// <summary>
        /// Convert Miladi DateTime to Shamsi short date & time
        /// </summary>
        /// <param name="dt"></param>
        /// <returns></returns>
        public static string? ToPersianShortDateTime(this DateTime? dt)
        {
            if (dt is null)
                return null;

            return dt.Value.ToPersianShortDateTime();
        }

        /// <summary>
        /// Convert Shamsi Date to Miladi DateTime
        /// </summary>
        /// <param name="dt"></param>
        /// <returns></returns>
        public static DateTime? ToDate(this string persianDate)
        {
            if (string.IsNullOrEmpty(persianDate))
            {
                return null;
            }
            else
            {
                string[] sepPersDt = persianDate.Split('/', '-');
                DateTime dt = DateTime.MinValue;
                if (sepPersDt.Length == 3)
                {
                    if (sepPersDt[0].Length == 4)
                    {
                        PersianCalendar pc = new();
                        dt = pc.ToDateTime(Convert.ToInt32(sepPersDt[0]),
                            Convert.ToInt32(sepPersDt[1]),
                            Convert.ToInt32(sepPersDt[2]),
                            0,
                            0,
                            0,
                            0);
                    }
                    else if (sepPersDt[2].Length == 4)
                    {
                        PersianCalendar pc = new();
                        dt = pc.ToDateTime(Convert.ToInt32(sepPersDt[2]),
                            Convert.ToInt32(sepPersDt[1]),
                            Convert.ToInt32(sepPersDt[0]),
                            0,
                            0,
                            0,
                            0);
                    }
                }
                return dt;
            }
        }

        /// <summary>
        /// Convert Shamsi date and time to Miladi DateTime
        /// </summary>
        /// <param name="dt"></param>
        /// <returns></returns>
        public static DateTime? ToDateTime(this string persianDateTime)
        {
            if (string.IsNullOrEmpty(persianDateTime))
            {
                return null;
            }
            else
            {
                persianDateTime = persianDateTime.Replace("  ", " ");
                string[] sepPersDt = persianDateTime.Split(' ');
                DateTime dt = DateTime.MinValue;
                if (sepPersDt.Length == 2)
                {
                    DateTime? dtTemp;
                    if (sepPersDt[0].Length == 10)
                    {
                        dtTemp = sepPersDt[0].ToDate();
                        if (dtTemp != null)
                        {
                            string[] sepTime = sepPersDt[1].Split(':');
                            int hour = 0, min = 0, sec = 0;
                            if (sepTime.Length == 2)
                            {
                                hour = Convert.ToInt32(sepTime[0]);
                                min = Convert.ToInt32(sepTime[1]);
                            }
                            else if (sepTime.Length == 3)
                            {
                                hour = Convert.ToInt32(sepTime[0]);
                                min = Convert.ToInt32(sepTime[1]);
                                sec = Convert.ToInt32(sepTime[2]);
                            }

                            dt = new DateTime(dtTemp.Value.Year, dtTemp.Value.Month, dtTemp.Value.Day, hour, min, sec);
                        }
                    }
                    else if (sepPersDt[1].Length == 10)
                    {
                        dtTemp = sepPersDt[1].ToDate();
                        if (dtTemp != null)
                        {
                            string[] sepTime = sepPersDt[0].Split(':');
                            int hour = 0, min = 0, sec = 0;
                            if (sepTime.Length == 2)
                            {
                                hour = Convert.ToInt32(sepTime[0]);
                                min = Convert.ToInt32(sepTime[1]);
                            }
                            else if (sepTime.Length == 3)
                            {
                                hour = Convert.ToInt32(sepTime[0]);
                                min = Convert.ToInt32(sepTime[1]);
                                sec = Convert.ToInt32(sepTime[2]);
                            }

                            dt = new DateTime(dtTemp.Value.Year, dtTemp.Value.Month, dtTemp.Value.Day, hour, min, sec);
                        }
                    }
                }
                return dt;
            }
        }

        /// <summary>
        /// Get Persian day of week
        /// </summary>
        /// <param name="dt"></param>
        /// <returns></returns>
        public static string GetPersianDayOfWeek(this DateTime dt)
        {
            string dayOfWeek = "";
            switch (dt.DayOfWeek)
            {
                case DayOfWeek.Friday:
                    dayOfWeek = "جمعه";
                    break;
                case DayOfWeek.Monday:
                    dayOfWeek = "دوشنبه";
                    break;
                case DayOfWeek.Saturday:
                    dayOfWeek = "شنبه";
                    break;
                case DayOfWeek.Sunday:
                    dayOfWeek = "يك شنبه";
                    break;
                case DayOfWeek.Thursday:
                    dayOfWeek = "پنج شنبه";
                    break;
                case DayOfWeek.Tuesday:
                    dayOfWeek = "سه شنبه";
                    break;
                case DayOfWeek.Wednesday:
                    dayOfWeek = "چهارشنبه";
                    break;
            }
            return dayOfWeek;
        }

        /// <summary>
        /// Get Shamsi year
        /// </summary>
        /// <param name="dt"></param>
        /// <returns></returns>
        public static int GetPersianYear(this DateTime dt)
        {
            PersianCalendar pc = new();
            return pc.GetYear(dt);
        }

        /// <summary>
        /// Get Shamsi month
        /// </summary>
        /// <param name="dt"></param>
        /// <returns></returns>
        public static int GetPersianMonth(this DateTime dt)
        {
            PersianCalendar pc = new();
            return pc.GetMonth(dt);
        }

        /// <summary>
        /// Get Shamsi day of month
        /// </summary>
        /// <param name="dt"></param>
        /// <returns></returns>
        public static int GetPersianDayOfMonth(this DateTime dt)
        {
            PersianCalendar pc = new();
            return pc.GetDayOfMonth(dt);
        }

        /// <summary>
        /// Get Persian remained time from one datetime to another
        /// </summary>
        /// <param name="dt1"></param>
        /// <param name="dt2"></param>
        /// <returns></returns>
        public static string GetRemainedTimeWithSeconds(DateTime dt1, DateTime dt2)
        {
            TimeSpan span = (dt2 - dt1);
            return String.Format("{0} روز, {1} ساعت, {2} دقیقه, {3} ثانیه",
                                span.Days, span.Hours, span.Minutes, span.Seconds);
        }

        /// <summary>
        /// Get Persian remained time from one datetime to another
        /// </summary>
        /// <param name="dt1"></param>
        /// <param name="dt2"></param>
        /// <returns></returns>
        public static string GetRemainedTime(DateTime dt1, DateTime dt2)
        {
            TimeSpan span = (dt2 - dt1);
            if (span.Days != 0)
            {
                return String.Format("{0} روز, {1} ساعت, {2} دقیقه", span.Days, span.Hours, span.Minutes);
            }
            else
            {
                return String.Format("{0} ساعت, {1} دقیقه", span.Hours, span.Minutes);
            }
        }

        public static DateTime GetNextBussinessDate(DateTime startDate, int businessDays)
        {
            DateTime dtEnd = startDate;
            int businessDaysCalculated = 0;
            while ((businessDays > 0 && businessDaysCalculated < businessDays) || (businessDays < 0 && businessDaysCalculated > businessDays))
            {
                dtEnd = dtEnd.AddDays(businessDays > 0 ? 1 : businessDays < 0 ? -1 : 0);

                if (dtEnd.DayOfWeek != DayOfWeek.Thursday && dtEnd.DayOfWeek != DayOfWeek.Friday)
                {
                    if (businessDays > 0)
                    {
                        businessDaysCalculated++;

                    }
                    else
                    {
                        businessDaysCalculated--;
                    }
                }
            }

            return dtEnd;
        }
    }
}
