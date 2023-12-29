using Newtonsoft.Json;
using System.Diagnostics;

namespace WeatherJson;

class Program
{
    public static readonly string _desktopPath = Environment.GetFolderPath(Environment.SpecialFolder.DesktopDirectory);
    public static readonly string _fileName = "weather.json";
    public static readonly string _path = $"{_desktopPath}/{_fileName}";

    static void Main()
    {
        Start();
    }

    static void Start()
    {
        bool isWorking = true;
        Console.WriteLine("ПОГОДА");
        if (!File.Exists(_path))
        {
            var file = File.Create(_path);
            file.Dispose();
        }
        
        while (isWorking)
        {
            ShowCommand();
            Console.Write("Ваша команда: ");
            string inputCommand = Console.ReadLine();
            switch (int.Parse(inputCommand))
            {
                case 1:
                    Console.Write("Введите данные.\nГород: ");
                    string city = Console.ReadLine();
                    Console.Write("Дата: ");
                    string inputDate = Console.ReadLine();
                    try
                    {
                        Forecast.IsValidDate(inputDate);

                        Console.Write("Температура: ");
                        int temp = int.Parse(Console.ReadLine());
                        var forecast = new Forecast(city, inputDate, temp);
                        CreateForecast(forecast);
                        Console.WriteLine("----------------\nУспешно!\n----------------");
                        break;
                    }
                    catch (ArgumentOutOfRangeException)
                    {
                        Console.WriteLine("----------------\nНеверная дата!\n----------------");
                        break;
                    }
                    
                case 2:
                    List<Forecast> allForecasts = ReadJson(_path);
                    if (allForecasts.Count == 0) Console.WriteLine("----------------\nСписок пуст!\n----------------");
                    else
                    {
                        Console.WriteLine("----------------");
                        for (int i = 0; i < allForecasts.Count; i++)
                        {
                            //Console.WriteLine($"----------------\n{allForecasts[i]}\n----------------");
                            Console.WriteLine($"{allForecasts[i]}\n----------------");
                        }
                    }
                    break;
                case 3:
                    Console.WriteLine("----------------\nПока!\n----------------");
                    isWorking = false;
                    break;
                default:
                    Console.WriteLine("Такой команды не существует!");
                    break;
            }
        }
    }
    static void ShowCommand()
    {
        Console.WriteLine("1) Добавить прогноз\n2) Вывести все прогнозы\n3) Выход");
    }

    //Создаёт и записывает новый прогноз в json БД
    private static void CreateForecast(Forecast forecast)
    {
        Forecast.IsValidDate(forecast.Date);

        List<Forecast> allForecasts = ReadJson(_path);
        allForecasts.Add(forecast);
        string json = JsonConvert.SerializeObject(allForecasts);

        File.WriteAllTextAsync(_path, json);
    }

    //Возвращает все прогнозы из json БД
    private static List<Forecast> ReadJson(string path)
    {
        string forecasts = File.ReadAllText(path);
        
        List<Forecast> allForecasts = JsonConvert.DeserializeObject<List<Forecast>>(forecasts);

        return allForecasts ?? new List<Forecast>();
    }
}
