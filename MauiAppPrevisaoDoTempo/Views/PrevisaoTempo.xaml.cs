using MauiAppPrevisaoDoTempo.Helpers;
using MauiAppPrevisaoDoTempo.Models;
using Microsoft.Maui.Controls;
using Microsoft.Maui.Storage;
using System.Text.Json;
using System.Net.Http;
using System.IO;
using System;
using System.Threading.Tasks;

namespace MauiAppPrevisaoDoTempo.Views
{
    public partial class PrevisaoTempo : ContentPage
    {
        private const string ApiKey = "f5cbdfad25b5ca4fe1779f3869627fbd";
        private const string BaseUrl = "http://api.openweathermap.org/data/2.5/weather";

        private readonly HttpClient _httpClient;
        private readonly SQLiteDatabaseHelpers _dbHelper;

        public PrevisaoTempo()
        {
            InitializeComponent();

            _httpClient = new HttpClient();

            var dbPath = Path.Combine(FileSystem.AppDataDirectory, "PrevisaoTempo.db3");
            _dbHelper = new SQLiteDatabaseHelpers(dbPath);

            activityIndicator.IsVisible = false;
            borderResultado.IsVisible = false;
        }

        private async void OnBuscarClicked(object sender, EventArgs e)
        {
            string cidade = txtCidade.Text?.Trim();

            if (string.IsNullOrWhiteSpace(cidade))
            {
                lblMensagem.Text = "Por favor, digite o nome de uma cidade.";
                return;
            }

            lblMensagem.Text = string.Empty;
            borderResultado.IsVisible = false;
            activityIndicator.IsVisible = true;
            activityIndicator.IsRunning = true;

            try
            {
                string url = $"{BaseUrl}?q={cidade}&appid={ApiKey}&units=metric&lang=pt_br";
                HttpResponseMessage response = await _httpClient.GetAsync(url);

                if (response.IsSuccessStatusCode)
                {
                    string? json = await response.Content.ReadAsStringAsync();

                    if (string.IsNullOrEmpty(json))
                    {
                        lblMensagem.Text = "Erro: Resposta da API vazia.";
                        return;
                    }

                    WeatherApiResult? resultado = JsonSerializer.Deserialize<WeatherApiResult>(
                        json,
                        new JsonSerializerOptions { PropertyNameCaseInsensitive = true }
                    );

                    if (resultado != null)
                    {
                        DisplayResult(resultado);

                        await SaveToHistorico(resultado, json);
                    }
                    else
                    {
                        lblMensagem.Text = "Não foi possível processar a resposta da API.";
                    }
                }
                else
                {
                    lblMensagem.Text = $"Erro na busca. Código: {(int)response.StatusCode}. Verifique o nome da cidade ou a API Key.";
                }
            }
            catch (Exception ex)
            {
                lblMensagem.Text = $"Erro de conexão: {ex.Message}. Verifique sua API Key ou internet.";
            }
            finally
            {
                activityIndicator.IsRunning = false;
                activityIndicator.IsVisible = false;
            }
        }

        private void DisplayResult(WeatherApiResult resultado)
        {
            string descricaoClima = resultado.Weather.Count > 0
                                  ? resultado.Weather[0].Description
                                  : "Clima não detalhado";

            lblCidadeResultado.Text = resultado.CityName;
            lblTemperatura.Text = $"{resultado.Main.Temp:F1}°C";
            lblDescricao.Text = descricaoClima.ToUpper();
            lblSensacaoTermica.Text = $"{resultado.Main.FeelsLike:F1}°C";
            lblUmidade.Text = $"{resultado.Main.Humidity}%";
            lblMinMax.Text = $"{resultado.Main.TempMin:F1}°C / {resultado.Main.TempMax:F1}°C";

            borderResultado.IsVisible = true;
        }

        private async Task SaveToHistorico(WeatherApiResult resultado, string rawJson)
        {
            int usuarioId = Preferences.Get("UsuarioId", 0);

            if (usuarioId <= 0)
            {
                return;
            }

            string resumoPrevisao = $"Temp: {resultado.Main.Temp:F1}°C. {resultado.Weather[0].Description.ToUpper()}.";

            var historico = new HistoricoConsulta
            {
                UsuarioId = usuarioId,
                Cidade = resultado.CityName,
                DataConsulta = DateTime.Now,
                ResultadoPrevisao = resumoPrevisao
            };

            try
            {
                string validacao = historico.Validar();
                if (string.IsNullOrEmpty(validacao))
                {
                    await _dbHelper.InsertHistorico(historico);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Erro ao salvar histórico: {ex.Message}");
            }
        }

        private async void OnVerHistoricoClicked(object sender, EventArgs e)
        {
            await Shell.Current.GoToAsync(nameof(HitoricoConsultas));
        }
    }
}