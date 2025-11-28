using MauiAppPrevisaoDoTempo.Views;
using Microsoft.Maui.Controls;

namespace MauiAppPrevisaoDoTempo
{
    public partial class AppShell : Shell
    {
        public AppShell()
        {
            InitializeComponent();

            Routing.RegisterRoute(nameof(CadastroUsuario), typeof(CadastroUsuario));

            Routing.RegisterRoute(nameof(PrevisaoTempo), typeof(PrevisaoTempo));

            Routing.RegisterRoute(nameof(HitoricoConsultas), typeof(HitoricoConsultas));
        }
    }
}