using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;

namespace NP_hw_6
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        HttpClient client;
        string body;
        HttpResponseMessage response;

        DispatcherTimer timer = new DispatcherTimer();
        DispatcherTimer timer2 = new DispatcherTimer();
        public MainWindow()
        {
            InitializeComponent();

            tb_date.Text = DateTime.Now.ToString("dd.MM.yyyy");

            timer.Interval = new TimeSpan(0, 0, 1);
            timer.Tick += NowTime;
            timer.Start();

            //timer2.Interval = new TimeSpan(0, 0, 10);
            //timer2.Tick += Refresh;
            //timer2.Start();

            getWeather();
        }

        private async void getWeather()
        {
            using (client = new HttpClient())
            {
                try
                {
                    response = await client.GetAsync("https://www.gismeteo.ru/weather-moscow-4368/");
                    body = await response.Content.ReadAsStringAsync();
                    string text = body;
                   

                    Regex weather_pattern = new Regex("Солнечно|Пасмурно|Дождь|Малооблачно|Облачно");
                    Regex temperature_pattern = new Regex("temperatureAir: \\[^-? [0 - 9]$");

                    Match weather_match = weather_pattern.Match(text);
                    Match temperature_match = temperature_pattern.Match(text);
                    

                    tb_temperature.Text = temperature_match.ToString();
                    tb_wetherCondition.Text = weather_match.ToString();

                    if (tb_wetherCondition.Text == "Солнечно")
                    {
                        wether_condition_icon.Source = new BitmapImage(new Uri("sunny.png", UriKind.Relative));
                    }
                    else
                         if (tb_wetherCondition.Text == "Пасмурно")
                    {
                        wether_condition_icon.Source = new BitmapImage(new Uri("overcast.png", UriKind.Relative));
                    }
                    else
                         if (tb_wetherCondition.Text == "Дождь")
                    {
                        wether_condition_icon.Source = new BitmapImage(new Uri("rain.png", UriKind.Relative));
                    }
                    else
                         if (tb_wetherCondition.Text == "Облачно" || tb_wetherCondition.Text == "Малооблачно")
                    {
                        wether_condition_icon.Source = new BitmapImage(new Uri("cloudy.png", UriKind.Relative));
                    }

                }
                catch (HttpRequestException exception)
                {
                    Console.WriteLine(exception);

                }
            }
        }

        private void NowTime(object sender, EventArgs e)
        {
            tb_time.Text = DateTime.Now.ToString("HH:mm:ss");
        }

        private void Refresh(object sender, EventArgs e)
        {
            /*response = await client.GetAsync("https://www.gismeteo.ru/weather-moscow-4368/");
            body = await response.Content.ReadAsStringAsync();*/

            //Task.Run(() => getWeather());
            
        }
    }
}
