using System;
using System.ComponentModel;
using System.Net.Http;
using Newtonsoft.Json;
using Xamarin.Forms;

namespace DemoCommentary
{
    [DesignTimeVisible(true)]
    public partial class MainPage : ContentPage
    {
        public MainPage()
        {
            InitializeComponent();
        }

        public async void GetResult(object sender, EventArgs e)
        {
            if (String.IsNullOrWhiteSpace(Texto.Text))
                return;

            using (var client = new HttpClient())
            {
                client.Timeout = new TimeSpan(0, 0, 0, 30);

                var request = new HttpRequestMessage
                {
                    RequestUri = new Uri("http://192.168.0.5:5000/api/commentary/" + Texto.Text),
                    Method = HttpMethod.Get
                };

                var result = await client.SendAsync(request);

                if (!result.IsSuccessStatusCode)
                    throw new Exception(result.Content.ReadAsStringAsync().Result);

                var json = result.Content.ReadAsStringAsync().Result;
                var data = JsonConvert.DeserializeObject<PredictionResult>(json);

                
                if (data.Predicition)
                    Emoji.Source = "happy.png";
                else
                    Emoji.Source = "sad.png";

                LblResult.Text = $"Predicition:{data.Predicition}\nScore:{data.Score}\nProbability: {data.Probability}";

            }
        }
    }
}
