using System.Threading;
using Features.Clientes;
using MediatR;
using Moq;
using Moq.AutoMock;
using Xunit;

namespace Features.Tests
{
    [Collection(nameof(ClienteCollection))]
    public class ClienteServiceTests
    {
        private readonly ClienteTestsFixture _clienteTestsFixture;

        public ClienteServiceTests(ClienteTestsFixture clienteTestsFixture)
        {
            _clienteTestsFixture = clienteTestsFixture;
        }

        [Fact(DisplayName = "Adicionar cliente com sucesso")]
        [Trait("Categoria", "Cliente Service Mock Tests")]
        public void ClienteService_Adicionar_DeveExecutarComSucesso()
        {
            //Arrange
            var cliente = _clienteTestsFixture.GerarClienteValido();
            var clienteRepo = new Mock<IClienteRepository>();
            var mediatr = new Mock<IMediator>();
            var clienteService = new ClienteService(clienteRepo.Object, mediatr.Object);

            //Act
            clienteService.Adicionar(cliente);

            //Assert
            Assert.True(cliente.EhValido());
            clienteRepo.Verify(rep => rep.Adicionar(cliente), Times.Once);
            mediatr.Verify(med => med.Publish(It.IsAny<INotification>(),CancellationToken.None), Times.Once);
        }


        [Fact(DisplayName = "Adicionar cliente com falha")]
        [Trait("Categoria", "Cliente Service Mock Tests")]
        public void ClienteService_Adicionar_DeveFalharDevidoClienteInvalido()
        {
            //Arrange
            var cliente = _clienteTestsFixture.GerarClienteInvalido();
            var clienteRepo = new Mock<IClienteRepository>();
            var mediatr = new Mock<IMediator>();
            var clienteService = new ClienteService(clienteRepo.Object, mediatr.Object);

            //Act
            clienteService.Adicionar(cliente);

            //Assert
            Assert.False(cliente.EhValido());
            clienteRepo.Verify(rep => rep.Adicionar(cliente), Times.Never);
            mediatr.Verify(med => med.Publish(It.IsAny<INotification>(), CancellationToken.None), Times.Never);
        }
    }
}