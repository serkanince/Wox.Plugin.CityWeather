using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Wox.Plugin.CityWeather
{
    public class Main : IPlugin
    {
        public void Init(PluginInitContext context)
        {
            //throw new NotImplementedException();
        }

        public List<Result> Query(Query query)
        {
            List<Result> results = new List<Result>();

            if (query.ActionParameters.Count > 0)
            {
                string keyword = query.ActionParameters[0];
                var weather = GetWeather(keyword.ToLower());

                results.Add(new Result()
                {
                    Title = keyword.First().ToString().ToUpper() + keyword.Substring(1) + " Weather",
                    SubTitle = "Current : " + weather.main.temp + " C",
                    IcoPath = "Images\\app.png",  //relative path to your plugin directory
                    Action = e =>
                    {
                        // after user select the item

                        // return false to tell Wox don't hide query window, otherwise Wox will hide it automatically
                        return false;
                    }
                });
            }

            return results;
        }

        public WeatherClass GetWeather(string cityName)
        {
            using (var client = new System.Net.WebClient())
            {
                client.Encoding = System.Text.Encoding.UTF8;
                var responseString = client.DownloadString("http://api.openweathermap.org/data/2.5/weather?q=" + cityName + "&units=metric&appid=82722c4d90b0642d3c487afb1167e9ca");

                var hava = JsonConvert.DeserializeObject<WeatherClass>(responseString);
                hava.weather[0].icon = @"http://openweathermap.org/img/w/" + hava.weather[0].icon + ".png";
                return hava;
            }
        }

    }
}
