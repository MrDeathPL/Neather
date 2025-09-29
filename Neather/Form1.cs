using System.Net.Http;
using System.Text.Json;
using System.Drawing;
using System.Windows.Forms;
using System;
using System.Runtime.InteropServices;
using System.Threading.Tasks;

namespace Neather
{
    public partial class Form1 : Form
    {
        private const string API_KEY = "YOUR_API";
        private readonly HttpClient _httpClient = new HttpClient();
        private Panel searchPanel;

        public Form1()
        {
            InitializeComponent();
            this.Text = "Neather!";
            try
            {
                string iconPath = System.IO.Path.Combine(Application.StartupPath, "weather.ico");
                this.Icon = new Icon(iconPath);
            }
            catch { }
            ApplyModernStyle();
            this.Shown += Form1_Shown;
            if (textBoxCity != null)
                textBoxCity.KeyDown += textBoxCity_KeyDown;
        }

        private void textBoxCity_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                buttonSearch_Click(sender, EventArgs.Empty);
                e.Handled = true;
                e.SuppressKeyPress = true;
            }
        }

        private void Form1_Shown(object? sender, EventArgs e)
        {
            if (searchPanel != null)
            {
                searchPanel.Region = Region.FromHrgn(CreateRoundRectRgn(0, 0, searchPanel.Width, searchPanel.Height, 6, 6));
            }
            if (buttonSearch != null)
            {
                buttonSearch.Region = Region.FromHrgn(CreateRoundRectRgn(0, 0, buttonSearch.Width, buttonSearch.Height, 6, 6));
            }
        }

        private void ApplyModernStyle()
        {
            this.BackColor = Color.FromArgb(30, 34, 42);
            this.FormBorderStyle = FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.StartPosition = FormStartPosition.CenterScreen;
            this.ClientSize = new Size(500, 320);
            this.ClientSize = new Size(350, 320);

            searchPanel = new Panel();
            searchPanel.BackColor = Color.FromArgb(45, 50, 60);
            searchPanel.Location = new Point(25, 25);
            searchPanel.Size = new Size(440, 48);
            searchPanel.Location = new Point(15, 25);
            searchPanel.Size = new Size(320, 48);
            searchPanel.Padding = new Padding(8, 4, 8, 4);
            searchPanel.BorderStyle = BorderStyle.None;
            searchPanel.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            this.Controls.Add(searchPanel);

            int buttonWidth = 80;
            int buttonHeight = 32;
            int panelPadding = 8;
            int textBoxWidth = searchPanel.Width - buttonWidth - (panelPadding * 3);

            if (textBoxCity != null)
            {
                textBoxCity.Parent = searchPanel;
                textBoxCity.Location = new Point(panelPadding, panelPadding);
                textBoxCity.Size = new Size(textBoxWidth, buttonHeight);
                textBoxCity.PlaceholderText = "Enter city name...";
                textBoxCity.Font = new Font("Segoe UI", 12F);
                textBoxCity.BackColor = Color.FromArgb(45, 50, 60);
                textBoxCity.ForeColor = Color.White;
                textBoxCity.BorderStyle = BorderStyle.None;
                textBoxCity.Margin = new Padding(0);
            }

            if (buttonSearch != null)
            {
                buttonSearch.Parent = searchPanel;
                buttonSearch.Location = new Point(searchPanel.Width - buttonWidth - panelPadding, panelPadding);
                buttonSearch.Size = new Size(buttonWidth, buttonHeight);
                buttonSearch.Text = "Search";
                buttonSearch.Font = new Font("Segoe UI", 12F, FontStyle.Bold);
                buttonSearch.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
                buttonSearch.BackColor = Color.FromArgb(0, 120, 215);
                buttonSearch.ForeColor = Color.White;
                buttonSearch.FlatStyle = FlatStyle.Flat;
                buttonSearch.FlatAppearance.BorderSize = 0;
                buttonSearch.Cursor = Cursors.Hand;
                buttonSearch.Click -= buttonSearch_Click;
                buttonSearch.Click += buttonSearch_Click;
                buttonSearch.MouseEnter += (s, ev) => buttonSearch.BackColor = Color.FromArgb(0, 150, 255);
                buttonSearch.MouseLeave += (s, ev) => buttonSearch.BackColor = Color.FromArgb(0, 120, 215);
                buttonSearch.TextAlign = ContentAlignment.MiddleCenter;
                buttonSearch.Padding = new Padding(8, 0, 8, 0);
                buttonSearch.Padding = new Padding(0);
                buttonSearch.Image = null;
            }

            int weatherDataStartY = 90;
            int weatherDataSpacing = 35;

            if (labelTemperature != null)
            {
                labelTemperature.AutoSize = true;
                labelTemperature.Location = new Point(25, 80);
                labelTemperature.Location = new Point(15, weatherDataStartY);
                labelTemperature.Font = new Font("Segoe UI", 16F, FontStyle.Bold);
                labelTemperature.ForeColor = Color.White;
                labelTemperature.TextAlign = ContentAlignment.TopLeft;
            }

            if (labelDescription != null)
            {
                labelDescription.AutoSize = true;
                labelDescription.Location = new Point(25, 120);
                labelDescription.Location = new Point(15, weatherDataStartY + weatherDataSpacing);
                labelDescription.Font = new Font("Segoe UI", 12F);
                labelDescription.ForeColor = Color.FromArgb(200, 200, 200);
                labelDescription.TextAlign = ContentAlignment.TopLeft;
            }

            if (labelHumidity != null)
            {
                labelHumidity.AutoSize = true;
                labelHumidity.Location = new Point(25, 150);
                labelHumidity.Location = new Point(15, weatherDataStartY + weatherDataSpacing * 2);
                labelHumidity.Font = new Font("Segoe UI", 12F);
                labelHumidity.ForeColor = Color.FromArgb(200, 200, 200);
                labelHumidity.TextAlign = ContentAlignment.TopLeft;
            }

            if (pictureBoxWeather != null)
            {
                pictureBoxWeather.Location = new Point(350, 80);
                pictureBoxWeather.Location = new Point(250, weatherDataStartY);
                pictureBoxWeather.Size = new Size(64, 64);
                pictureBoxWeather.SizeMode = PictureBoxSizeMode.Zoom;
            }

            if (labelWind != null)
            {
                labelWind.AutoSize = true;
                labelWind.Location = new Point(25, 180);
                labelWind.Location = new Point(15, weatherDataStartY + weatherDataSpacing * 3);
                labelWind.Font = new Font("Segoe UI", 12F);
                labelWind.ForeColor = Color.FromArgb(200, 200, 200);
                labelWind.TextAlign = ContentAlignment.TopLeft;
            }

            if (labelPressure != null)
            {
                labelPressure.AutoSize = true;
                labelPressure.Location = new Point(25, 210);
                labelPressure.Location = new Point(15, weatherDataStartY + weatherDataSpacing * 4);
                labelPressure.Font = new Font("Segoe UI", 12F);
                labelPressure.ForeColor = Color.FromArgb(200, 200, 200);
            }
        }

        private async Task GetWeatherData(string city)
        {
            try
            {
                string url = $"https://api.openweathermap.org/data/2.5/weather?q={city}&appid={API_KEY}&units=metric";
                var response = await _httpClient.GetAsync(url);
                var responseContent = await response.Content.ReadAsStringAsync();

                if (!response.IsSuccessStatusCode)
                {
                    MessageBox.Show($"API Error: {response.StatusCode}\nDetails: {responseContent}", "API Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                var options = new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                };

                var weatherData = JsonSerializer.Deserialize<WeatherData>(responseContent, options);

                if (weatherData?.Main == null || weatherData.Weather == null || weatherData.Weather.Length == 0)
                {
                    MessageBox.Show("Could not parse weather data.", "Parse Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                int roundedTemp = (int)Math.Round(weatherData.Main.Temp);
                labelTemperature.Text = $"Temperature: {roundedTemp}°C";
                labelDescription.Text = $"Weather: {weatherData.Weather[0].Description}";
                labelHumidity.Text = $"Humidity: {weatherData.Main.Humidity}%";
                labelWind.Text = $"Wind: {weatherData.Wind.Speed} m/s";
                labelPressure.Text = $"Pressure: {weatherData.Main.Pressure} hPa";

                if (!string.IsNullOrEmpty(weatherData.Weather[0].Icon))
                {
                    string iconUrl = $"https://openweathermap.org/img/wn/{weatherData.Weather[0].Icon}@2x.png";
                    try
                    {
                        using (var iconStream = await _httpClient.GetStreamAsync(iconUrl))
                        {
                            pictureBoxWeather.Image = Image.FromStream(iconStream);
                        }
                    }
                    catch
                    {
                        pictureBoxWeather.Image = null;
                    }
                }
                else
                {
                    pictureBoxWeather.Image = null;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error Details:\n{ex.Message}\n\nStack Trace:\n{ex.StackTrace}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void buttonSearch_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrWhiteSpace(textBoxCity.Text))
            {
                _ = GetWeatherData(textBoxCity.Text);
            }
        }

        [DllImport("Gdi32.dll", EntryPoint = "CreateRoundRectRgn")]
        private static extern IntPtr CreateRoundRectRgn(int nLeftRect, int nTopRect, int nRightRect, int nBottomRect, int nWidthEllipse, int nHeightEllipse);
    }

    public class WeatherData
    {
        public MainData Main { get; set; }
        public Weather[] Weather { get; set; }
        public WindData Wind { get; set; }
    }

    public class MainData
    {
        public double Temp { get; set; }
        public int Humidity { get; set; }
        public int Pressure { get; set; }
        public double FeelsLike { get; set; }
    }

    public class Weather
    {
        public string Description { get; set; }
        public string Icon { get; set; }
    }

    public class WindData
    {
        public double Speed { get; set; }
    }
}
