using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;

namespace Neather;

public partial class MainWindow : Window
{
    private const string API_KEY = "YOUR_API"; // Replaced real key with placeholder
    private readonly HttpClient _http = new();
    private readonly List<WeatherCard> _cards = new();
    private bool _loadedOnce;

    public MainWindow()
    {
        InitializeComponent();
        CreateCards();
        CityBox.KeyDown += (s,e)=> { if(e.Key==Key.Enter) _ = Search(); };
        CityBox.PlaceholderText("Enter city name...");
        MouseLeftButtonDown += (_, __) => { try { DragMove(); } catch { } };
    }

    private void CreateCards()
    {
        _cards.Clear();
        CardsHost.Children.Clear();
        var titles = new[]{"Temperature","Condition","Humidity","Wind Speed","Pressure"};
        for(int i=0;i<titles.Length;i++)
        {
            var c = new WeatherCard(titles[i]);
            var bottomGap = 24;
            c.Margin = i==0 ? new Thickness(0,8,0,bottomGap) : new Thickness(0,0,0,bottomGap);
            _cards.Add(c);
            CardsHost.Children.Add(c);
            if(_loadedOnce) c.AnimateIn();
        }
        _loadedOnce = true;
    }

    private async Task Search()
    {
        if(string.IsNullOrWhiteSpace(CityBox.Text)) return;
        try
        {
            SearchBtn.IsEnabled = false;
            var url = $"https://api.openweathermap.org/data/2.5/weather?q={CityBox.Text}&appid={API_KEY}&units=metric";
            var resp = await _http.GetAsync(url);
            var json = await resp.Content.ReadAsStringAsync();
            if(!resp.IsSuccessStatusCode) { MessageBox.Show("City not found", "Neather", MessageBoxButton.OK, MessageBoxImage.Information); return; }
            var data = JsonSerializer.Deserialize<WeatherResponse>(json, new JsonSerializerOptions{PropertyNameCaseInsensitive=true});
            if(data?.Main==null || data.Weather==null || data.Weather.Length==0) { MessageBox.Show("Invalid response", "Neather", MessageBoxButton.OK, MessageBoxImage.Warning); return; }
            UpdateCards(data);
        }
        catch(Exception ex) { MessageBox.Show(ex.Message, "Neather", MessageBoxButton.OK, MessageBoxImage.Error); }
        finally { SearchBtn.IsEnabled = true; }
    }

    private void UpdateCards(WeatherResponse w)
    {
        foreach(var card in _cards) card.Reset();
        _cards[0].SetPrimary($"{Math.Round(w.Main.Temp)}°C");
        _cards[1].SetPrimary(Cap(w.Weather[0].Description));
        _cards[2].SetPrimary($"{w.Main.Humidity}%");
        _cards[3].SetPrimary($"{w.Wind.Speed} m/s");
        _cards[4].SetPrimary($"{w.Main.Pressure} hPa");
        foreach(var c in _cards) c.AnimateIn();
    }

    private string Cap(string s) => string.IsNullOrWhiteSpace(s) ? s : char.ToUpper(s[0]) + s[1..];

    private void SearchBtn_Click(object sender, RoutedEventArgs e) => _ = Search();
    private void CityBox_TextChanged(object sender, TextChangedEventArgs e) {}
    private void Minimize_Click(object sender, RoutedEventArgs e) => WindowState = WindowState.Minimized;
    private void Close_Click(object sender, RoutedEventArgs e) => Close();
}

public record WeatherResponse(WeatherMain Main, WeatherInfo[] Weather, WeatherWind Wind);
public record WeatherMain(double Temp, int Humidity, int Pressure);
public record WeatherInfo(string Description, string Icon);
public record WeatherWind(double Speed);

public class WeatherCard : Border
{
    private readonly TextBlock _title;
    private readonly TextBlock _primary;

    public WeatherCard(string title)
    {
        Style = (Style)Application.Current.FindResource("Card");
        Opacity = 0;
        RenderTransform = new TranslateTransform(0,12);
        var stack = new StackPanel();
        _title = new TextBlock{ Text=title, FontSize=13, FontWeight=FontWeights.SemiBold, Foreground=(Brush)Application.Current.FindResource("ForegroundMuted") };
        _primary = new TextBlock{ Text="--", Style=(Style)Application.Current.FindResource("Heading"), Margin=new Thickness(0,6,0,0)};
        stack.Children.Add(_title);
        stack.Children.Add(_primary);
        Child = stack;
    }

    public void SetPrimary(string text) => _primary.Text = text;
    public void Reset() => _primary.Text = "--";

    public void AnimateIn()
    {
        if(RenderTransform is not TranslateTransform tt)
        {
            tt = new TranslateTransform(0,12);
            RenderTransform = tt;
        }
        var sb = new Storyboard();
        var fade = new DoubleAnimation(0,1,TimeSpan.FromMilliseconds(320)) { EasingFunction = new QuadraticEase{EasingMode=EasingMode.EaseOut}};
        var slide = new DoubleAnimation(12,0,TimeSpan.FromMilliseconds(320)) { EasingFunction = new QuadraticEase{EasingMode=EasingMode.EaseOut}};
        Storyboard.SetTarget(fade,this);
        Storyboard.SetTargetProperty(fade,new PropertyPath("Opacity"));
        Storyboard.SetTarget(slide,tt);
        Storyboard.SetTargetProperty(slide,new PropertyPath(TranslateTransform.YProperty));
        sb.Children.Add(fade);
        sb.Children.Add(slide);
        sb.Begin();
    }
}

public static class TextBoxExtensions
{
    public static void PlaceholderText(this TextBox box, string text)
    {
        var normal = box.Foreground;
        var placeholder = new SolidColorBrush(Color.FromRgb(120,120,130));
        box.Tag = false;
        void SetPlaceholder()
        {
            if(string.IsNullOrEmpty(box.Text))
            {
                box.Tag = true;
                box.Text = text;
                box.Foreground = placeholder;
                box.CaretIndex = 0;
            }
        }
        void RemovePlaceholder()
        {
            if((bool)box.Tag)
            {
                box.Tag = false;
                box.Text = string.Empty;
                box.Foreground = normal;
            }
        }
        box.GotFocus += (_,__) => RemovePlaceholder();
        box.LostFocus += (_,__) => SetPlaceholder();
        box.TextChanged += (_,__) => { if(!(bool)box.Tag && string.IsNullOrEmpty(box.Text) && !box.IsFocused) SetPlaceholder(); };
        SetPlaceholder();
    }
}
