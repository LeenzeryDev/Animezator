using Avalonia.Controls;
using Avalonia.Interactivity;
using System.Collections.Generic;
using System.Net.Http;
using System.Net;
using System.Text.Json;
using NetCoreAudio;

namespace Animezator
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }
        private void Animezator(object sender, RoutedEventArgs e)
        {
            result.Text = word.Text?.Replace("л", "р").Replace("Л","Р");
        }
        private async void tts(object sender, RoutedEventArgs e)
        {
            if (result.Text != null) { 
                var values = new Dictionary<string, string>
                {
                    { "key", "ec902c61cd7e1ba83a20e750255d49fc" },
                    { "voice", "ru-RU010" },
                    { "text", result.Text },
                    { "pitch", "1.0" },
                    { "rate", "1.0" },
                    { "format", "mp3" },
                    { "volume", "1.0" },
                    { "hertz", "44100" }
                };
                var data = new FormUrlEncodedContent(values);
                var url = "https://texttospeech.ru/api";
                using var client = new HttpClient();
                var response = await client.PostAsync(url, data);
                string resp_result = response.Content.ReadAsStringAsync().Result;
                TTS_Result tts = JsonSerializer.Deserialize<TTS_Result>(resp_result);
                var webclient = new WebClient();
                webclient.DownloadFile(tts.file, "tts.mp3");
                var player = new Player();
                player.Play("tts.mp3").Wait();
            }
         }
     }
}
public class TTS_Result
{
    public string file { get; set; }
    public string status { get; set; }
}