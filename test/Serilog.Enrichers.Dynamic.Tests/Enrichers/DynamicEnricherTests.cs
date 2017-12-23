using Serilog.Events;
using Serilog.Tests.Support;
using System;
using System.Diagnostics;
using Xunit;

namespace Serilog.Tests.Enrichers
{
    public class DynamicEnricherTests
    {
        [Fact]
        public void DynamicEnricherIsAppliedWithNamedProperty()
        {
            LogEvent evt = null;
            var log = new LoggerConfiguration()
                .Enrich.WithDynamicProperty("MyProperty", () =>
                {
                    return "value";
                })
                .WriteTo.Sink(new DelegatingSink(e => evt = e))
                .CreateLogger();

            log.Information(@"Some sample log.");

            Assert.NotNull(evt);
            Assert.Equal("value", (string)evt.Properties["MyProperty"].LiteralValue());
        }

       
        [Fact]
        public void DynamicEnricherIsAppliedWithMulptileDynamicProperties()
        {
            LogEvent evt = null;
            var log = new LoggerConfiguration()
                .Enrich.WithDynamicProperty("Prop1", () =>
                {
                    return "value";
                })
                .Enrich.WithDynamicProperty("Prop2", () =>
                {
                    return "value2";
                })
                .WriteTo.Sink(new DelegatingSink(e => evt = e))
                .CreateLogger();

            log.Information(@"Some sample log.");

            Assert.NotNull(evt);

            Assert.Equal("value", (string)evt.Properties["Prop1"].LiteralValue());
            Assert.Equal("value2", (string)evt.Properties["Prop2"].LiteralValue());
        }

        [Fact]
        public void DynamicEnricherIgnoreNull()
        {
            LogEvent evt = null;
            var log = new LoggerConfiguration()
                .Enrich.WithDynamicProperty("MyProp", () =>
                {
                    return null;
                }, enrichWhenNullOrEmptyString: false)
                
                .WriteTo.Sink(new DelegatingSink(e => evt = e))
                .CreateLogger();

            log.Information(@"Some sample log.");

            Assert.NotNull(evt);
            Assert.False( evt.Properties.ContainsKey("MyProp"));
           }
        [Fact]
        public void DynamicEnricheLogNull()
        {
            LogEvent evt = null;
            var log = new LoggerConfiguration()
                .Enrich.WithDynamicProperty("MyProp", () =>
                {
                    return null;
                }, enrichWhenNullOrEmptyString: true)

                .WriteTo.Sink(new DelegatingSink(e => evt = e))
                .CreateLogger();

            log.Information(@"Some sample log.");

            Assert.NotNull(evt);
            Assert.Null(evt.Properties["MyProp"].LiteralValue());
        }

        [Fact]
        public void DynamicEnricheLogEmptyString()
        {
            LogEvent evt = null;
            var log = new LoggerConfiguration()
                .Enrich.WithDynamicProperty("MyProp", () =>
                {
                    return string.Empty;
                }, enrichWhenNullOrEmptyString: true)

                .WriteTo.Sink(new DelegatingSink(e => evt = e))
                .CreateLogger();

            log.Information(@"Some sample log.");

            Assert.NotNull(evt);
            Assert.Empty((string)evt.Properties["MyProp"].LiteralValue());
        }

        [Fact]
        public void DynamicEnricheLogIfErrorOrAbove()
        {
            LogEvent evt = null;
            var log = new LoggerConfiguration()
                .Enrich.WithDynamicProperty("MyProp", () =>
                {
                    return "value";
                }, minimumLevel: LogEventLevel.Error)

                .WriteTo.Sink(new DelegatingSink(e => evt = e))
                .CreateLogger();

            log.Fatal(@"Some sample log.");

            Assert.NotNull(evt);
            Assert.Equal((string)evt.Properties["MyProp"].LiteralValue(), "value");
        }

        [Fact]
        public void DynamicEnricheDontLogIfNotFatal()
        {
            LogEvent evt = null;
            var log = new LoggerConfiguration()
                .Enrich.WithDynamicProperty("MyProp",() =>
                {
                    return "value";
                },  minimumLevel: LogEventLevel.Fatal)

                .WriteTo.Sink(new DelegatingSink(e => evt = e))
                .CreateLogger();

            log.Information(@"Some sample log.");

            Assert.NotNull(evt);
            Assert.False(evt.Properties.ContainsKey("MyProp"));
        }

        
    }

  
}
