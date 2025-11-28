using System;
using SQLite;

namespace MauiAppPrevisaoDoTempo.Models
{
    public class HistoricoConsulta
    {
        int _id;
        int _usuarioId;
        string _cidade = default!;
        DateTime _dataConsulta;
        string _resultadoPrevisao = default!;

        public HistoricoConsulta() { }

        [PrimaryKey, AutoIncrement]
        public int Id
        {
            get => _id;
            set => _id = value;
        }

        [Indexed]
        public int UsuarioId
        {
            get => _usuarioId;
            set => _usuarioId = value;
        }

        public string Cidade
        {
            get => _cidade;
            set
            {
                if (string.IsNullOrWhiteSpace(value))
                    throw new Exception("A cidade não pode ser vazia.");
                _cidade = value;
            }
        }

        public DateTime DataConsulta
        {
            get => _dataConsulta;
            set => _dataConsulta = value;
        }

        public string ResultadoPrevisao
        {
            get => _resultadoPrevisao;
            set
            {
                if (string.IsNullOrWhiteSpace(value))
                    throw new Exception("O resultado da previsão não pode ser vazio.");
                _resultadoPrevisao = value;
            }
        }

        public string Validar()
        {

            try
            {
                var _ = Cidade;
                var __ = ResultadoPrevisao;

                if (UsuarioId <= 0)
                    return "ID do Usuário é inválido.";

                return string.Empty; 
            }
            catch (Exception ex)
            {
                return ex.Message; 
            }
        }
    }
}