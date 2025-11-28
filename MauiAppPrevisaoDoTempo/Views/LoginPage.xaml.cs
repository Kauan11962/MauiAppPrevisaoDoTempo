using MauiAppPrevisaoDoTempo.Helpers;
using MauiAppPrevisaoDoTempo.Models;
using System;
using Microsoft.Maui.Controls;
using System.IO; 

namespace MauiAppPrevisaoDoTempo.Views
{
    public partial class LoginPage : ContentPage
    {
        public LoginPage()
        {
            InitializeComponent();
        }

        private async void OnLoginClicked(object sender, EventArgs e)
        {
            try
            {
                string email = txtEmail.Text;
                string senha = txtSenha.Text;

                if (string.IsNullOrWhiteSpace(email) || string.IsNullOrWhiteSpace(senha))
                {
                    lblMensagem.Text = "Por favor, preencha E-mail e Senha.";
                    return;
                }

                var dbPath = Path.Combine(FileSystem.AppDataDirectory, "PrevisaoTempo.db3");
                var dbHelper = new SQLiteDatabaseHelpers(dbPath);

                Usuario? usuarioLogado = await dbHelper.Login(email, senha);

                if (usuarioLogado != null)
                {
                    Preferences.Set("UsuarioId", usuarioLogado.Id);
                    Preferences.Set("UsuarioNome", usuarioLogado.Nome);

                    await DisplayAlert("Sucesso", $"Bem-vindo(a), {usuarioLogado.Nome}!", "OK");

                    await Shell.Current.GoToAsync($"//{nameof(PrevisaoTempo)}");
                }
                else
                {
                    lblMensagem.Text = "E-mail ou Senha incorretos.";
                }
            }
            catch (Exception ex)
            {
                lblMensagem.Text = $"Erro de Login: {ex.Message}";
            }
        }

        private async void OnCadastrarClicked(object sender, EventArgs e)
        {
            await Shell.Current.GoToAsync(nameof(CadastroUsuario));
        }
    }
}