using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
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
using DeviceTwins.DeviceSimulator.UI.ViewModel;
using System.Net.Http.Formatting;
using DeviceTwins.DeviceSimulator.UI.Models;

namespace DeviceTwins.DeviceSimulator.UI
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {

        static HttpClient _httpClient = new HttpClient();

        public MainWindow()
        {
            InitializeComponent();
            DataContext = new MainViewModel();

            _httpClient.BaseAddress = new Uri("http://localhost:8232/");
            _httpClient.DefaultRequestHeaders.Accept.Clear();
            _httpClient.DefaultRequestHeaders.Accept.Add(
                new MediaTypeWithQualityHeaderValue("application/json"));         
        }

        private async void Window_Loaded(object sender, RoutedEventArgs e)
        {
            await RegisterDevice();
        }

        private async void TemperatureChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            Color cool = Color.FromRgb(72,187,18);
            Color normal = Color.FromRgb(230,220,21);
            Color hot = Color.FromRgb(212,13,13);

            if (sldTemperature.Value <= 50)
                txtTemperature.Background = new SolidColorBrush(cool);

            if (sldTemperature.Value > 50 && sldTemperature.Value <= 79)
                txtTemperature.Background = new SolidColorBrush(normal);

            if (sldTemperature.Value > 79)
                txtTemperature.Background = new SolidColorBrush(hot);

            await SendTelemetry();
        }

        private async void HumidityChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            Color dry = Color.FromRgb(72, 187, 18);
            Color normal = Color.FromRgb(230, 220, 21);
            Color humid = Color.FromRgb(212, 13, 13);

            if (sldHumidity.Value <= 15)
                txtHumidity.Background = new SolidColorBrush(dry);

            if (sldHumidity.Value > 15 && sldHumidity.Value <= 70)
                txtHumidity.Background = new SolidColorBrush(normal);

            if (sldHumidity.Value > 71)
                txtHumidity.Background = new SolidColorBrush(humid);

            await SendTelemetry();
        }

        private async Task RegisterDevice()
        {
            var deviceId = Guid.Parse(((MainViewModel)(this.DataContext)).SerialNumber);

            var getDeviceResponse = await _httpClient.GetAsync($"api/devices/{deviceId}");

            if (getDeviceResponse.StatusCode == System.Net.HttpStatusCode.NotFound)
            {
                var device = new Device()
                {
                    Id = Guid.Parse(((MainViewModel)(this.DataContext)).SerialNumber),
                    RegisteredDate = DateTime.UtcNow,
                    Status = "Registered",
                    Type = "Thermostat"
                };

                HttpResponseMessage response = await _httpClient.PostAsJsonAsync("api/devices", device);
                response.EnsureSuccessStatusCode();
            }
        }

        private async Task SendTelemetry()
        {
            var deviceId = Guid.Parse(((MainViewModel)(this.DataContext)).SerialNumber);

            var telemetry = new Telemetry()
            {
                Timestamp = DateTime.UtcNow,
                Temperature = (float)sldTemperature.Value,
                Humidity = (float)sldHumidity.Value
            };

            HttpResponseMessage response = await _httpClient.PostAsJsonAsync($"api/telemetry/{deviceId}", telemetry);
            response.EnsureSuccessStatusCode();
        }
    }
}
