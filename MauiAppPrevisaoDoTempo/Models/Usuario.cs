using System;
using SQLite;

namespace MauiAppPrevisaoDoTempo.Models
{
    public class Usuario
    {
        int _id;
        string _nome = default!;
        DateTime _dataNascimento = default!;
        string _email = default!;
        string _senha = default!;

        public Usuario() { }

        [PrimaryKey, AutoIncrement]
        public int Id
        {
            get => _id;
            set => _id = value;
        }

        public string Nome
        {
            get => _nome;
            set
            {
                if (string.IsNullOrWhiteSpace(value))
                    throw new Exception("Por favor, preencha o nome.");
                _nome = value;
            }
        }

        public DateTime DataNascimento
        {
            get => _dataNascimento;
            set
            {
                if (value == DateTime.MinValue)
                    throw new Exception("Por favor, informe a data de nascimento.");

                if (value > DateTime.Now)
                    throw new Exception("Data de nascimento inválida (não pode ser futura).");

                _dataNascimento = value;
            }
        }

        [Unique]
        public string Email
        {
            get => _email;
            set
            {
                if (string.IsNullOrWhiteSpace(value))
                    throw new Exception("Por favor, preencha o email.");

                if (!value.Contains("@") || !value.Contains("."))
                    throw new Exception("Email inválido.");

                _email = value;
            }
        }

        public string Senha
        {
            get => _senha;
            set
            {
                if (string.IsNullOrWhiteSpace(value))
                    throw new Exception("Por favor, preencha a senha.");

                if (value.Length < 4)
                    throw new Exception("Senha deve ter no mínimo 4 caracteres.");

                _senha = value;
            }
        }

        public string Validar()
        {
            try
            {
                var _ = Nome;
                var __ = DataNascimento;
                var ___ = Email;
                var ____ = Senha;
                return "";
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }
    }
}