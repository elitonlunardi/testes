using System;
using Features.Clientes;
using Xunit;

namespace Features.Tests
{
    [CollectionDefinition(nameof(ClienteCollection))]
    public class ClienteCollection : ICollectionFixture<ClienteTestsFixture>
    {

    }

    public class ClienteTestsFixture : IDisposable
    {

        public Cliente GerarClienteValido()
        {
            var cliente = new Cliente(
                Guid.NewGuid(),
                "Éliton",
                "Lunardi",
                DateTime.Now.AddYears(-20),
                "el@el.com",
                true,
                DateTime.Now);
            return cliente;
        }

        public Cliente GerarClienteInvalido()
        {
            var cliente = new Cliente(
                Guid.NewGuid(),
                "",
                "",
                DateTime.Now,
                "el2el.com",
                true,
                DateTime.Now);
            return cliente;
        }

        public void Dispose()
        {

        }
    }
}