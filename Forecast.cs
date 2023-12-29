using System.Globalization;

namespace WeatherJson
{
    //Моделька Прогноза
    public class Forecast
    {
        public string City { get; set; }
        public string Date { get; set; }
        public int Temperature { get; set; }
        public Weather Weather { get; private set; }

        public Forecast(string city, string data, int temperature)
        {
            City = city;
            Date = data;
            Temperature = temperature;
            Weather = ConvertTempToWeather(temperature);
        }
        //Конвертирует string в DateTime
        private DateTime GetDateTime()
        {
            return DateTime.ParseExact(Date, "dd.MM.yyyy", CultureInfo.InvariantCulture);
        }
        //Проверяет введённую дату на валидность
        public static bool IsValidDate(string date)
        {
            DateTime result;
            List<string> list = new List<string>();

            list = date.Split('.').ToList();
            int day = int.Parse(list[0]);
            int month = int.Parse(list[1]);
            int year = int.Parse(list[2]);

            try
            {
                result = new DateTime(year, month, day);

                if(result.Year > DateTime.Now.Year)
                    throw new ArgumentOutOfRangeException("Date is not correct (The date can't be the future)");

                else if (result.Month > DateTime.Now.Month && result.Year >= DateTime.Now.Year)
                    throw new ArgumentOutOfRangeException("Date is not correct (The date can't be the future)");

                else if (result.Day > DateTime.Now.Month && result.Month > DateTime.Now.Month && result.Year >= DateTime.Now.Year)
                    throw new ArgumentOutOfRangeException("Date is not correct (The date can't be the future)");

                return true;
            }
            catch (ArgumentOutOfRangeException)
            {
                if(day < 0 || month < 0 || year < 0)
                    throw new ArgumentOutOfRangeException("Date is not correct (The date can't be the negative)");

                else
                    throw new ArgumentOutOfRangeException("Date is not correct (The date can't be the future)");
            }
        }
        //Переводит температуру в Weather (enum) (в строковый формат)
        public static Weather ConvertTempToWeather(int temp)
        {
            if (temp <= 0) return Weather.Cold;
            else if (temp > 0 && temp <= 15) return Weather.Cool;
            else if (temp > 15 && temp < 25) return Weather.Warm;
            else return Weather.Hot;
        }
        public override string ToString()
        {
            return $"Город: {City}\nДата: {Date}\nТемпература: {Temperature}\nПогода: {Weather}";
        }
    }
}
