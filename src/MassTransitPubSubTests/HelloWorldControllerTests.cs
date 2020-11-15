using Castle.MicroKernel.Registration;
using Castle.Windsor;
using MassTransit;
using MassTransit.Testing;
using System;
using MassTransitPubSubSender;
using Xunit;

namespace MassTransitWebAppPubSub.Controllers
{
    public class HelloWorldControllerTests
  {
    [Fact]
    public async void GivenUserName_WhenHelloIsCalled_ThenSendHelloText()
    {
      var container = new WindsorContainer()
          .Register(Component.For<InMemoryTestHarness>().UsingFactoryMethod(kernel =>
          {
            var testHarness = new InMemoryTestHarness() { TestTimeout = TimeSpan.FromSeconds(5) };
            var busRegistrationContext = kernel.Resolve<IBusRegistrationContext>();
            testHarness.OnConfigureInMemoryBus += configurator => configurator.ConfigureEndpoints(busRegistrationContext);
            return testHarness;
          }).LifestyleSingleton())
          .AddMassTransit(x =>
          {
            x.AddBus(context => context.GetRequiredService<InMemoryTestHarness>().BusControl);
          });

      var harness = container.Resolve<InMemoryTestHarness>();
      await harness.Start();

      var sendEndpointProvider = container.Resolve<ISendEndpointProvider>();

      var helloController = new HelloWorldController(sendEndpointProvider);
      try
      {
        var response = helloController.Hello("Henri");
        Assert.True(await harness.Sent.Any<HelloWorldModel>(x => x.Context.Message.UserName.Equals("Henri") && x.Context.Message.HelloText.Equals("hello henri")));
      }
      finally
      {
        await harness.Stop();
      }
    }
  }
}
