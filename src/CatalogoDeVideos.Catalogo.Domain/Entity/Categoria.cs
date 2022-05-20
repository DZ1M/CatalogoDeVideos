using CatalogoDeVideos.Catalogo.Domain.Exceptions;
using CatalogoDeVideos.Catalogo.Domain.SeedWork;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CatalogoDeVideos.Catalogo.Domain.Entity
{
    public class Categoria : AggregateRoot
    {
        public string Nome { get; private set; }
        public string Descricao { get; private set; }
        public DateTime DataCriacao { get; private set; }
        public bool Ativo { get; private set; }
        public Categoria(string nome, string descricao, bool ativo = true) : base()
        {
            Nome = nome;
            Descricao = descricao;
            DataCriacao = DateTime.Now;
            Ativo = ativo;
            Validate();
        }
        protected Categoria() { }

        private void Validate()
        {
            if (string.IsNullOrWhiteSpace(Nome))
                throw new EntityValidationException($"{nameof(Nome)} should not be empty or null");

            if (Nome.Length < 3)
                throw new EntityValidationException($"{nameof(Nome)} should be at leats 3 characteres long");

            if (Nome.Length > 255)
                throw new EntityValidationException($"{nameof(Nome)} should be at less or equal 255 characteres long");

            if (Descricao is null)
                throw new EntityValidationException($"{nameof(Descricao)} should not be null");

            if (Descricao.Length > 10_000)
                throw new EntityValidationException($"{nameof(Descricao)} should be at less or equal 10.000 characteres long");
        }

        public void Activate()
        {
            Ativo = true;
            Validate();
        }

        public void Update(string nome, string descricao = null)
        {
            Nome = nome;
            Descricao = descricao ?? Descricao;
            Validate();
        }
    }
}
