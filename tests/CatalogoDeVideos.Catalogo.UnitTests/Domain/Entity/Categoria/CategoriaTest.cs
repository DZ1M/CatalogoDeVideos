using CatalogoDeVideos.Catalogo.Domain.Exceptions;
using FluentAssertions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DomainEntity = CatalogoDeVideos.Catalogo.Domain.Entity;
namespace CatalogoDeVideos.Catalogo.UnitTests.Domain.Entity.Categoria
{
    public class CategoriaTest
    {
        [Fact(DisplayName = nameof(Instantiate))]
        [Trait("Domain", "Categoria - Aggregates")]
        public void Instantiate()
        {
            var obj = new
            {
                Nome = "Categoria A",
                Descricao = "Descrição da Categoria",
            };

            var dataAntes = DateTime.Now;
            var categoria = new DomainEntity.Categoria(obj.Nome, obj.Descricao);
            var dataDepois = DateTime.Now;

            categoria.Should().NotBeNull();
            categoria.Nome.Should().Be(obj.Nome);
            categoria.Descricao.Should().Be(obj.Descricao);
            categoria.Id.Should().NotBe(default(Guid));
            categoria.DataCriacao.Should().NotBe(default(DateTime));
            (categoria.DataCriacao > dataAntes).Should().BeTrue();
            (categoria.DataCriacao < dataDepois).Should().BeTrue();
            (categoria.Ativo).Should().BeTrue();
            //Assert.NotNull(categoria);
            //Assert.Equal(obj.Nome, categoria.Nome);
            //Assert.Equal(obj.Descricao, categoria.Descricao);
            //Assert.NotEqual(default(Guid), categoria.Id);
            //Assert.NotEqual(default(DateTime), categoria.DataCriacao);
            //Assert.True(categoria.DataCriacao > dataAntes);
            //Assert.True(categoria.DataCriacao < dataDepois);
            //Assert.True(categoria.Ativo);
        }

        [Theory(DisplayName = nameof(InstantiateAtivo))]
        [Trait("Domain", "Categoria - Aggregates")]
        [InlineData(true)]
        [InlineData(false)]
        public void InstantiateAtivo(bool ativo)
        {
            var obj = new
            {
                Nome = "Categoria A",
                Descricao = "Descrição da Categoria",
            };

            var dataAntes = DateTime.Now;
            var categoria = new DomainEntity.Categoria(obj.Nome, obj.Descricao, ativo);
            var dataDepois = DateTime.Now;

            Assert.NotNull(categoria);
            Assert.Equal(obj.Nome, categoria.Nome);
            Assert.Equal(obj.Descricao, categoria.Descricao);
            Assert.NotEqual(default(Guid), categoria.Id);
            Assert.NotEqual(default(DateTime), categoria.DataCriacao);
            Assert.True(categoria.DataCriacao > dataAntes);
            Assert.True(categoria.DataCriacao < dataDepois);
            Assert.Equal(ativo, categoria.Ativo);
        }

        [Theory(DisplayName = nameof(InstantiateErroWhenNameIsEmpty))]
        [Trait("Domain", "Categoria - Aggregates")]
        [InlineData("")]
        [InlineData(null)]
        [InlineData(" ")]
        public void InstantiateErroWhenNameIsEmpty(string? name)
        {
            Action action = () => new DomainEntity.Categoria(name!, "Descrição");

            var exception = Assert.Throws<EntityValidationException>(action);
            Assert.Equal("Nome should not be empty or null", exception.Message);
        }

        [Fact(DisplayName = nameof(InstantiateErroWhenDescricaoIsNull))]
        [Trait("Domain", "Categoria - Aggregates")]
        public void InstantiateErroWhenDescricaoIsNull()
        {
            Action action = () => new DomainEntity.Categoria("Nome", null);

            var exception = Assert.Throws<EntityValidationException>(action);
            Assert.Equal("Descricao should not be null", exception.Message);
        }

        [Theory(DisplayName = nameof(InstantiateErroWhenNameLessThan3Caractertes))]
        [Trait("Domain", "Categoria - Aggregates")]
        [InlineData("ab")]
        [InlineData("a")]
        public void InstantiateErroWhenNameLessThan3Caractertes(string invalidName)
        {
            Action action = () => new DomainEntity.Categoria(invalidName, "Categoria OK");

            var exception = Assert.Throws<EntityValidationException>(action);

            Assert.Equal("Nome should be at leats 3 characteres long", exception.Message);
        }

        [Fact(DisplayName = nameof(InstantiateErroWhenNameIsGreaterThan255Caractertes))]
        [Trait("Domain", "Categoria - Aggregates")]
        public void InstantiateErroWhenNameIsGreaterThan255Caractertes()
        {
            var invalidName = String.Join(null, Enumerable.Range(1, 256).Select(_ => "a").ToArray());
            Action action = () => new DomainEntity.Categoria(invalidName, "Categoria OK");

            var exception = Assert.Throws<EntityValidationException>(action);

            Assert.Equal("Nome should be at less or equal 255 characteres long", exception.Message);
        }

        [Fact(DisplayName = nameof(InstantiateErroWhenDescriptionIsGreaterThan255Caractertes))]
        [Trait("Domain", "Categoria - Aggregates")]
        public void InstantiateErroWhenDescriptionIsGreaterThan255Caractertes()
        {
            var invalidDescription = String.Join(null, Enumerable.Range(1, 10_001).Select(_ => "a").ToArray());
            Action action = () => new DomainEntity.Categoria("Categoria", invalidDescription!);

            var exception = Assert.Throws<EntityValidationException>(action);

            Assert.Equal("Descricao should be at less or equal 10.000 characteres long", exception.Message);
        }

        [Fact(DisplayName = nameof(Activate))]
        [Trait("Domain", "Categoria - Aggregates")]
        public void Activate()
        {
            var obj = new
            {
                Nome = "Categoria A",
                Descricao = "Descrição da Categoria",
            };

            var categoria = new DomainEntity.Categoria(obj.Nome, obj.Descricao, false);
            categoria.Activate();

            Assert.True(categoria.Ativo);
        }
        [Fact(DisplayName = nameof(Update))]
        [Trait("Domain", "Categoria - Aggregates")]
        public void Update()
        {
            var categoria = new DomainEntity.Categoria("Nome", "Descricao");
            var novosValores = new { Nome = "Novo Nome", Descricao = "Nova descrição" };

            categoria.Update(novosValores.Nome, novosValores.Descricao);

            Assert.Equal(novosValores.Nome, categoria.Nome);
            Assert.Equal(novosValores.Descricao, categoria.Descricao);
        }

        [Fact(DisplayName = nameof(UpdateApenasNome))]
        [Trait("Domain", "Categoria - Aggregates")]
        public void UpdateApenasNome()
        {
            var categoria = new DomainEntity.Categoria("Nome", "Descricao");
            var novosValores = new { Nome = "Novo Nome" };
            var descricaoAtual = categoria.Descricao;

            categoria.Update(novosValores.Nome);

            Assert.Equal(novosValores.Nome, categoria.Nome);
            Assert.Equal(descricaoAtual, categoria.Descricao);
        }

        [Theory(DisplayName = nameof(UpdateErroWhenNameLessThan3Caractertes))]
        [Trait("Domain", "Categoria - Aggregates")]
        [InlineData("ab")]
        [InlineData("a")]
        public void UpdateErroWhenNameLessThan3Caractertes(string invalidName)
        {
            var categoria = new DomainEntity.Categoria("Nome", "Descricao");

            Action action = () => categoria.Update(invalidName!);

            var exception = Assert.Throws<EntityValidationException>(action);

            Assert.Equal("Nome should be at leats 3 characteres long", exception.Message);
        }

        [Fact(DisplayName = nameof(UpdateErroWhenNameIsGreaterThan255Caractertes))]
        [Trait("Domain", "Categoria - Aggregates")]
        public void UpdateErroWhenNameIsGreaterThan255Caractertes()
        {
            var categoria = new DomainEntity.Categoria("Nome", "Descricao");
            var invalidName = String.Join(null, Enumerable.Range(1, 256).Select(_ => "a").ToArray());
            
            Action action = () => categoria.Update(invalidName!);

            var exception = Assert.Throws<EntityValidationException>(action);

            Assert.Equal("Nome should be at less or equal 255 characteres long", exception.Message);
        }

        [Fact(DisplayName = nameof(UpdateErroWhenDescriptionIsGreaterThan255Caractertes))]
        [Trait("Domain", "Categoria - Aggregates")]
        public void UpdateErroWhenDescriptionIsGreaterThan255Caractertes()
        {
            var categoria = new DomainEntity.Categoria("Nome", "Descricao");
            var invalidDescription = String.Join(null, Enumerable.Range(1, 10_001).Select(_ => "a").ToArray());
            Action action = () => categoria.Update("Categoria", invalidDescription!);

            var exception = Assert.Throws<EntityValidationException>(action);

            Assert.Equal("Descricao should be at less or equal 10.000 characteres long", exception.Message);
        }

    }
}
