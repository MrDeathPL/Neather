namespace Neather
{
    partial class Form1
    {
        private System.ComponentModel.IContainer components = null;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            textBoxCity = new TextBox();
            buttonSearch = new Button();
            labelTemperature = new Label();
            labelDescription = new Label();
            labelHumidity = new Label();
            pictureBoxWeather = new PictureBox();
            labelWind = new Label();
            labelPressure = new Label();

            textBoxCity.Location = new Point(12, 12);
            textBoxCity.Size = new Size(200, 23);
            textBoxCity.PlaceholderText = "Enter city name...";

            buttonSearch.Location = new Point(218, 12);
            buttonSearch.Size = new Size(75, 23);
            buttonSearch.Text = "Search";
            buttonSearch.Click += buttonSearch_Click;

            labelTemperature.AutoSize = true;
            labelTemperature.Location = new Point(12, 50);
            labelTemperature.Size = new Size(200, 23);

            labelDescription.AutoSize = true;
            labelDescription.Location = new Point(12, 80);
            labelDescription.Size = new Size(200, 23);

            labelHumidity.AutoSize = true;
            labelHumidity.Location = new Point(12, 110);
            labelHumidity.Size = new Size(200, 23);

            pictureBoxWeather.Location = new Point(230, 50);
            pictureBoxWeather.Size = new Size(64, 64);
            pictureBoxWeather.SizeMode = PictureBoxSizeMode.Zoom;

            labelWind.AutoSize = true;
            labelWind.Location = new Point(12, 140);
            labelWind.Size = new Size(200, 23);

            labelPressure.AutoSize = true;
            labelPressure.Location = new Point(12, 170);
            labelPressure.Size = new Size(200, 23);

            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(314, 240);
            Controls.AddRange(new Control[] { textBoxCity, buttonSearch, labelTemperature, labelDescription, labelHumidity, pictureBoxWeather, labelWind, labelPressure });
            Name = "Form1";
            Text = "Weather App";
            Icon = new System.Drawing.Icon(System.IO.Path.Combine(Application.StartupPath, "weather.ico"));
            ResumeLayout(false);
            PerformLayout();
        }

        private TextBox textBoxCity;
        private Button buttonSearch;
        private Label labelTemperature;
        private Label labelDescription;
        private Label labelHumidity;
        private PictureBox pictureBoxWeather;
        private Label labelWind;
        private Label labelPressure;
    }
}
