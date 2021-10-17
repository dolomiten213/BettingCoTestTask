using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BettinCoTestTask
{
    class WeatherDay
    {
        public DateTime Date { get; set; }

        public int MaxTemperature { get; set; }
        public int AverageTemperature { get; set; }
        public int MinTemperature { get; set; }

        public string WindDirection { get; set; }
        public int AverageWindSpeed { get; set; }
        public int MaxWindSpeed { get; set; }

        public int MaxPressure { get; set; }
        public int MinPressure { get; set; }

        public double Precipitation { get; set; }
        public int RelativeHumidiry { get; set; }

        public byte UltravioletIndex { get; set; }

        public static bool operator ==(WeatherDay obj1, WeatherDay obj2)
        {
            if (obj1 is null || obj2 is null) return false;

            if (obj1.MaxTemperature != obj2.MaxTemperature) return false;
            if (obj1.AverageTemperature != obj2.AverageTemperature) return false;
            if (obj1.MinTemperature != obj2.MinTemperature) return false;

            if (obj1.WindDirection != obj2.WindDirection) return false;
            if (obj1.AverageWindSpeed != obj2.AverageWindSpeed) return false;
            if (obj1.MaxWindSpeed != obj2.MaxWindSpeed) return false;

            if (obj1.MaxPressure != obj2.MaxPressure) return false;
            if (obj1.MinPressure != obj2.MinPressure) return false;

            if (obj1.Precipitation != obj2.Precipitation) return false;
            if (obj1.RelativeHumidiry != obj2.RelativeHumidiry) return false;
            if (obj1.UltravioletIndex != obj2.UltravioletIndex) return false;

            return true;
        }
        public static bool operator !=(WeatherDay obj1, WeatherDay obj2)
        {
            return !(obj1 == obj2);
        }

        public override string ToString()
        {
            return $"Max Temp: {MaxTemperature}\n" +
                $"Average Temp: {AverageTemperature}\n" +
                $"Min Temp: {MinTemperature}\n" +
                $"Wind Direction: {WindDirection}\n" +
                $"Max Wind Speed: {MaxWindSpeed}\n" +
                $"Average Wind Speed: {AverageWindSpeed}\n" +
                $"Max Pressure: {MaxPressure}\n" +
                $"Min Pressure: {MinPressure}\n" +
                $"Precipitation: {Precipitation}\n" +
                $"RelativeHumidiry: {RelativeHumidiry}\n" +
                $"Ultraviolet Index: {UltravioletIndex}\n" +
                $"=========================================\n";

        }
    }
}
