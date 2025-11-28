using MauiAppPrevisaoDoTempo.Helpers;
using MauiAppPrevisaoDoTempo.Models;
using System;
using Microsoft.Maui.Controls;

namespace MauiAppPrevisaoDoTempo.Views
{
    public partial class CadastroUsuario : ContentPage
    {
        public CadastroUsuario()
        {
            InitializeComponent();
        }

        private async void OnCadastrarClicked(object sender, EventArgs e)
        {
            try
            {
                Usuario novoUsuario = new Usuario
                {
                    Nome = txtNome.Text,
                    DataNascimento = dtpDataNascimento.Date,
                    Email = txtEmail.Text,
                    Senha = txtSenha.Text
                };


                string erroValidacao = novoUsuario.Validar();
                if (!string.IsNullOrEmpty(erroValidacao))
                {
                    lblMensagem.Text = $"Erro de Validação: {erroValidacao}";
                    return;
                }

                var dbHelper = new SQLiteDatabaseHelpers(
                    Path.Combine(FileSystem.AppDataDirectory, "PrevisaoTempo.db3"));

                int resultado = await dbHelper.InsertUsuario(novoUsuario);

                if (resultado > 0)
                {
                    await DisplayAlert("Sucesso", "Usuário cadastrado com sucesso!", "OK");
                    await Shell.Current.GoToAsync($"//{nameof(LoginPage)}"); 
                }
                else
                {
                    lblMensagem.Text = "Falha ao cadastrar o usuário (Verifique se o email já existe se implementar Unique).";
                }
            }
            catch (Exception ex)
            {
                lblMensagem.Text = $"Erro: {ex.Message}";
            }
        }
    }
}