using MauiAppPrevisaoDoTempo.Helpers;
using MauiAppPrevisaoDoTempo.Models;
using System;
using System.Collections.Generic;
using Microsoft.Maui.Controls;
using Microsoft.Maui.Storage;
using System.IO;

namespace MauiAppPrevisaoDoTempo.Views
{
    public partial class HitoricoConsultas : ContentPage
    {
        private SQLiteDatabaseHelpers _dbHelper;
        private int _usuarioId;

        public HitoricoConsultas()
        {
            InitializeComponent();

            var dbPath = Path.Combine(FileSystem.AppDataDirectory, "PrevisaoTempo.db3");
            _dbHelper = new SQLiteDatabaseHelpers(dbPath);

            _usuarioId = Preferences.Get("UsuarioId", 0);

            dtpDataInicio.Date = DateTime.Now.Date.AddDays(-7);
            dtpDataFim.Date = DateTime.Now.Date;
        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();
            if (_usuarioId > 0)
            {
                await LoadHistoricoAsync();
            }
        }

        private async Task LoadHistoricoAsync()
        {
            try
            {
                List<HistoricoConsulta> historico = await _dbHelper.GetHistoricoByUsuario(_usuarioId);
                cvHistorico.ItemsSource = historico;
            }
            catch (Exception ex)
            {
                await DisplayAlert("Erro", "Falha ao carregar o histórico: " + ex.Message, "OK");
            }
        }

        private async void OnAplicarFiltroClicked(object sender, EventArgs e)
        {
            if (_usuarioId <= 0)
            {
                await DisplayAlert("Erro", "Usuário não logado.", "OK");
                return;
            }

            try
            {
                string cidade = txtCidadeFiltro.Text;
                DateTime inicio = dtpDataInicio.Date;
                DateTime fim = dtpDataFim.Date.AddDays(1).AddSeconds(-1);

                List<HistoricoConsulta> resultados;

                if (!string.IsNullOrWhiteSpace(cidade))
                {
                    resultados = await _dbHelper.SearchHistorico(_usuarioId, cidade);
                }
                else
                {
                    resultados = await _dbHelper.FiltrarHistorico(_usuarioId, inicio, fim);
                }

                cvHistorico.ItemsSource = resultados;

                if (resultados.Count == 0)
                {
                    await DisplayAlert("Aviso", "Nenhum resultado encontrado para o filtro.", "OK");
                }
            }
            catch (Exception ex)
            {
                await DisplayAlert("Erro de Filtro", "Falha ao aplicar filtro: " + ex.Message, "OK");
            }
        }
    }
}