using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BettinCoTestTask
{
    /// <summary>
    /// Класс для парсинга прогноза погоды на 10 дней с сайта Gismeteo.ru
    /// </summary>
    class GisMeteo10Days
    {
        List<WeatherDay> Days { get; init; }
        private readonly string htmlCode = String.Empty; 
        /// <summary>
        /// Парсинг происходит сразу в конструкторе
        /// </summary>
        /// <param name="htmlCode">
        /// html код страницы сайта Gismeteo
        /// </param>
        public GisMeteo10Days(string htmlCode)
        {
            this.htmlCode = htmlCode;
            Days = new List<WeatherDay>();

            (List<int> maxTemperatures, List<int> minTemperatures, List<int> avgTemperatures) = GetAllTemperatures();
            (List<int> maxWind, List<int> avgWind) = GetAllWinds();
            (List<int> maxPres, List<int> minPres) = GetAllPreasures();
            List<string> windDirections = GetWindDirections();
            List<double> precipitations = GetPrecipitations();
            List<int> relativeHumidiry = GetRelativeHumidiry();
            List<byte> ultravioletIndex = GetUltravioletIndex();

            for (int i = 0; i < 10; i++)
            {
                WeatherDay day = new WeatherDay
                {
                    MaxTemperature = maxTemperatures[i],
                    MinTemperature = minTemperatures[i],
                    AverageTemperature = avgTemperatures[i],
                    
                    MaxWindSpeed = maxWind[i],
                    AverageWindSpeed = avgWind[i],
                    WindDirection = windDirections[i],

                    MaxPressure = maxPres[i],
                    MinPressure = minPres[i],

                    Precipitation = precipitations[i],
                    RelativeHumidiry = relativeHumidiry[i],

                    UltravioletIndex = ultravioletIndex[i]

                };
                Days.Add(day);
            }
        }
        private (List<int>, List<int>, List<int>) GetAllTemperatures()
        {
            List<int> maxTemperatures = new List<int>();
            List<int> minTemperatures = new List<int>();
            List<int> avgTemperatures = new List<int>();

            string tempID = "<span class=\"unit unit_temperature_c\">";
            int startIndex = 0;
            for (int i = 0; i < 30; i++)
            {
                var curTemp = htmlCode.IndexOf(tempID, startIndex);
                startIndex = curTemp + tempID.Length;

                string res = htmlCode.Substring(curTemp + tempID.Length, 30);
                res = res.Substring(0, res.IndexOf('<'));
                if (res.Contains("&minus;"))
                {
                    res = res.Substring(7, res.Length - 7);
                    res = "-" + res;
                }
                if (i % 2 == 0 && i < 20) maxTemperatures.Add(int.Parse(res));
                if (i % 2 == 1 && i < 20) minTemperatures.Add(int.Parse(res));
                if (i >= 20) avgTemperatures.Add(int.Parse(res));
            }
            return (maxTemperatures, minTemperatures, avgTemperatures);
        }
        private (List<int>, List<int>) GetAllWinds()
        {
            List<int> avgWind = new List<int>();
            List<int> maxWind = new List<int>();
            string tempID = "<span class=\"unit unit_wind_m_s\">";
            int startIndex = 0;
            for (int i = 0; i < 21; i++)
            {
                var maxTemp = htmlCode.IndexOf(tempID, startIndex);
                startIndex = maxTemp + tempID.Length;

                string res = htmlCode.Substring(maxTemp + tempID.Length, 100);
                res = res.Substring(0, res.IndexOf('<'));
                if (i < 10) maxWind.Add(int.Parse(res));
                if (i == 10) continue;
                if (i > 10) avgWind.Add(int.Parse(res));
            }
            return (maxWind, avgWind);
        }
        private (List<int>, List<int>) GetAllPreasures()
        {

            List<int> maxPres = new List<int>();
            List<int> minPres = new List<int>();

            string pressureAnchor = "data-widget-fullname=\"Давление\"";
            string maxAnchor = "<div class=\'maxt\'>";
            string minAnchor = "<div class=\'mint\'>";
            string unitAnchor = "<span class=\"unit unit_pressure_mm_hg_atm\">";


            var pressureId = htmlCode.IndexOf(pressureAnchor, 0);
            var startIndex = pressureId + pressureAnchor.Length;

            for (int i = 0; i < 10; i++)
            {
                var maxtId = htmlCode.IndexOf(maxAnchor, startIndex);
                startIndex = maxtId + maxAnchor.Length;

                var unitId = htmlCode.IndexOf(unitAnchor, startIndex);
                startIndex = unitId + unitAnchor.Length;

                string res = htmlCode.Substring(startIndex, 100);
                res = res.Substring(0, res.IndexOf('<'));

                maxPres.Add(int.Parse(res));

                var mintId = htmlCode.IndexOf(minAnchor, startIndex);
                maxtId = htmlCode.IndexOf(maxAnchor, startIndex);

                if (maxtId < mintId)
                {
                    minPres.Add(int.Parse(res));
                } 
                else
                {
                    startIndex = mintId + maxAnchor.Length;
                    unitId = htmlCode.IndexOf(unitAnchor, startIndex);
                    startIndex = unitId + unitAnchor.Length;
                    res = htmlCode.Substring(startIndex, 100);
                    res = res.Substring(0, res.IndexOf('<'));
                    minPres.Add(int.Parse(res));
                }

            }
            //for (int i = 0; i < 21; i++)
            //{
            //    var maxTemp = htmlCode.IndexOf(tempID, startIndex);
            //    startIndex = maxTemp + tempID.Length;

            //    string res = htmlCode.Substring(maxTemp + tempID.Length, 100);
            //    res = res.Substring(0, res.IndexOf('<'));
            //    if (i == 0) continue;
            //    if (i % 2 == 1) maxPres.Add(int.Parse(res));
            //    if (i % 2 == 0) minPres.Add(int.Parse(res));
            //}
            return (maxPres, minPres);
        }
        private List<double> GetPrecipitations()
        {
            List<double> precipitations = new List<double>();
            string anchor = "<div class=\"w_prec__value\"";

            var startIndex = 0;

            for (int i = 0; i < 10; i++)
            {
                var index = htmlCode.IndexOf(anchor, startIndex);
                startIndex = index + anchor.Length;

                var bracketIndex = htmlCode.IndexOf('>', startIndex);
                startIndex = bracketIndex + 1;

                string res = htmlCode.Substring(startIndex, 100);
                res = res.Substring(0, res.IndexOf('<'));

                precipitations.Add(double.Parse(res));
            }
            return precipitations;
        }
        private List<int> GetRelativeHumidiry()
        {
            List<int> relativeHumidiry = new List<int>();

            string anchor = "widget__row widget__row_table widget__row_humidity";
            string anchor2 = "w-humidity widget__value w_humidity_type";

            var index = htmlCode.IndexOf(anchor, 0);
            var startIndex = index + anchor.Length;

            for (int i = 0; i < 10; i++)
            {
                var dataIndex = htmlCode.IndexOf(anchor2, startIndex);
                startIndex = dataIndex + anchor2.Length;

                var bracketIndex = htmlCode.IndexOf('>', startIndex);
                startIndex = bracketIndex + 1;

                string res = htmlCode.Substring(startIndex, 100);
                res = res.Substring(0, res.IndexOf('<'));

                relativeHumidiry.Add(int.Parse(res));
            }
            return relativeHumidiry;
        }
        private List<byte> GetUltravioletIndex()
        {
            List<byte> relativeHumidiry = new List<byte>();

            string anchor = "widget__row widget__row_table widget__row_uvb";
            string anchor2 = "w_uvb w_uvb__value_level";

            var index = htmlCode.IndexOf(anchor, 0);
            var startIndex = index + anchor.Length;

            for (int i = 0; i < 10; i++)
            {
                var dataIndex = htmlCode.IndexOf(anchor2, startIndex);
                startIndex = dataIndex + anchor2.Length;

                var bracketIndex = htmlCode.IndexOf('>', startIndex);
                startIndex = bracketIndex + 1;

                string res = htmlCode.Substring(startIndex, 100);
                res = res.Substring(0, res.IndexOf('<'));

                relativeHumidiry.Add(byte.Parse(res));
            }
            return relativeHumidiry;
        }
        private List<string> GetWindDirections()
        {
            List<string> result = new List<string>();
            string tempID = "<div class=\"w_wind__direction gray\">";
            int startIndex = 0;
            for (int i = 0; i < 10; i++)
            {
                var maxTemp = htmlCode.IndexOf(tempID, startIndex);
                startIndex = maxTemp + tempID.Length;

                string res = htmlCode.Substring(maxTemp + tempID.Length, 100);
                res = res.Substring(0, res.IndexOf('<'));
                result.Add(res.Trim());
            }
            return result;
        }

        public static bool operator == (GisMeteo10Days obj1, GisMeteo10Days obj2)
        {
            if (obj1 is null || obj2 is null) return false;
            if (obj1.Days.Count != obj2.Days.Count) return false;
            for (int i = 0; i < obj1.Days.Count; i++)
            {
                if (obj1.Days[i] != obj2.Days[i]) return false;            
            }
            return true;
        }
        public static bool operator != (GisMeteo10Days obj1, GisMeteo10Days obj2)
        {
            return !(obj1 == obj2);
        }

        public override string ToString()
        {
            StringBuilder result = new StringBuilder();
            foreach (var day in Days)
            {
                result.Append(day.ToString());
            }
            return result.ToString();
        }
    }
}
