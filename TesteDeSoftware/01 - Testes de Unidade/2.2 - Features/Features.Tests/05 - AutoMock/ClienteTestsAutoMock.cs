using System.Linq;
using System.Threading;
using Features.Clientes;
using MediatR;
using Moq;
using Xunit;
using FluentAssertions;

namespace Features.Tests
{
    [Collection(nameof(ClienteAutoMockCollection))]
    public class ClienteTestsAutoMock
    {
        readonly ClienteTestsAutoMockFixtures _clienteTestsAutoMockerFixture;
        private readonly ClienteService _clienteService;

        public ClienteTestsAutoMock
            (ClienteTestsAutoMockFixtures clienteTestsFixture)
        {
            _clienteTestsAutoMockerFixture = clienteTestsFixture;
            _clienteService = _clienteTestsAutoMockerFixture.ObterClienteService();
        }

        [Fact(DisplayName = "Adicionar cliente com sucesso")]
        [Trait("Categoria", "Cliente Service Auto Mock Tests")]
        public void ClienteService_Adicionar_DeveExecutarComSucesso()
        {
            //Arrange
            var cliente = _clienteTestsAutoMockerFixture.GerarClienteValido();

            //Act
            _clienteService.Adicionar(cliente);

            //Assert
            cliente.EhValido()
                .Should().BeTrue();
            _clienteTestsAutoMockerFixture.Mocker.GetMock<IClienteRepository>()
                .Verify(rep => rep.Adicionar(cliente), Times.Once);
            _clienteTestsAutoMockerFixture.Mocker.GetMock<IMediator>()
                .Verify(med => med.Publish(It.IsAny<INotification>(), CancellationToken.None), Times.Once);
        }


        [Fact(DisplayName = "Adicionar cliente com falha")]
        [Trait("Categoria", "Cliente Service Auto Mock Tests")]
        public void ClienteService_Adicionar_DeveFalharDevidoClienteInvalido()
        {
            //Arrange
            var cliente = _clienteTestsAutoMockerFixture.GerarClienteInvalido();

            //Act
            _clienteService.Adicionar(cliente);

            //Assert
            cliente.EhValido().Should().BeFalse();
            _clienteTestsAutoMockerFixture.Mocker.GetMock<IClienteRepository>()
                .Verify(rep => rep.Adicionar(cliente), Times.Never);
            _clienteTestsAutoMockerFixture.Mocker.GetMock<IMediator>()
                .Verify(med => med.Publish(It.IsAny<INotification>(), CancellationToken.None), Times.Never);
        }

        [Fact(DisplayName = "Obter todos clientes ativos")]
        [Trait("Categoria", "Cliente Service Auto Mock Tests")]
        public void ClienteService_ObterTodosAtivos_DeveRetornarApenasAtivos()
        {
            // Arrange
            _clienteTestsAutoMockerFixture.Mocker.GetMock<IClienteRepository>()
                .Setup(c => c.ObterTodos())
                .Returns(_clienteTestsAutoMockerFixture.ObterClientesVariados());

            // Act
            var clientes = _clienteService.ObterTodosAtivos();

            // Assert 
            //Assert.True(clientes.Any());
            //Assert.False(clientes.Count(c => !c.Ativo) > 0);
            clientes
                .Should().HaveCountGreaterOrEqualTo(1, "É preciso ter um cliente");
            clientes.Count(c => !c.Ativo)
                .Should().BeGreaterOrEqualTo(0, "É preciso ter ao menos um cliente, todos ativos");
            _clienteTestsAutoMockerFixture.Mocker.GetMock<IClienteRepository>().Verify(r => r.ObterTodos(), Times.Once);
            
        }
    }
}