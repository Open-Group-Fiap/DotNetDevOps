
using CIDA.Api;
using Microsoft.AspNetCore.Mvc.Testing;

namespace CRC.Tests;
[CollectionDefinition("Test Collection")]
public class TestCollection : ICollectionFixture<WebApplicationFactory<Program>>
{
    
}