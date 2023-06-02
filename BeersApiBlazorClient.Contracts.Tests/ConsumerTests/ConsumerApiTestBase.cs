using System.Diagnostics;
using System.Runtime.CompilerServices;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using PactNet;

namespace BeersApiBlazorClient.Contracts.Tests.ConsumerTests;

public class ConsumerApiTestBase
{
    private readonly Module _consumer;
    private readonly Module _provider;
    private readonly string _apiName;
    private readonly PactConfig _config;

    protected IPactBuilderV3 Pact { get; private set; }

    public ConsumerApiTestBase(Module consumer, Module provider, string apiName)
    {
        _consumer = consumer;
        _provider = provider;
        _apiName = apiName;
        string str = Path.Combine("..", "..", "..", "..", "..", "pacts", _provider.ToString());
        _config = new PactConfig
        {
            PactDir = str,
            LogLevel = Debugger.IsAttached ? PactLogLevel.Trace : PactLogLevel.Error,
            DefaultJsonSettings = new JsonSerializerSettings
            {
                ContractResolver = (IContractResolver)new CamelCasePropertyNamesContractResolver()
            }
        };
    }

    [SetUp]
    public void BeforeEach()
    {
        DefaultInterpolatedStringHandler interpolatedStringHandler = new DefaultInterpolatedStringHandler(2, 3);
        interpolatedStringHandler.AppendFormatted(_apiName);
        interpolatedStringHandler.AppendLiteral("-");
        interpolatedStringHandler.AppendFormatted<ContractType>(ContractType.Api);
        interpolatedStringHandler.AppendLiteral("-");
        interpolatedStringHandler.AppendFormatted<Module>(_consumer);
        Pact = PactNet.Pact.V3(interpolatedStringHandler.ToStringAndClear(), _provider.ToString(), _config).WithHttpInteractions();
    }
}