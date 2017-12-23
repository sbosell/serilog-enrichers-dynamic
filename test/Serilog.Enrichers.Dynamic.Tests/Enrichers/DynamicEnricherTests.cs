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
                .Enrich.WithDynamic(() =>
                {
                    return "value";
                }, "MyProperty")
                .WriteTo.Sink(new DelegatingSink(e => evt = e))
                .CreateLogger();

            log.Information(@"Some sample log.");

            Assert.NotNull(evt);
            Assert.Equal("value", (string)evt.Properties["MyProperty"].LiteralValue());
        }

        [Fact]
        public void DynamicEnricherIsAppliedWithUnNamedProperty()
        {
            LogEvent evt = null;
            var log = new LoggerConfiguration()
                .Enrich.WithDynamic(() =>
                {
                    return "value";
                })
                .WriteTo.Sink(new DelegatingSink(e => evt = e))
                .CreateLogger();

            log.Information(@"Some sample log.");

            Assert.NotNull(evt);

            Assert.Equal("value", (string)evt.Properties["DynamicProperty"].LiteralValue());
        }

        [Fact]
        public void DynamicEnricherIsAppliedWithMulptileDynamicProperties()
        {
            LogEvent evt = null;
            var log = new LoggerConfiguration()
                .Enrich.WithDynamic(() =>
                {
                    return "value";
                })
                .Enrich.WithDynamic(() =>
                {
                    return "value2";
                }, "MyProperty2")
                .WriteTo.Sink(new DelegatingSink(e => evt = e))
                .CreateLogger();

            log.Information(@"Some sample log.");

            Assert.NotNull(evt);

            Assert.Equal("value", (string)evt.Properties["DynamicProperty"].LiteralValue());
            Assert.Equal("value2", (string)evt.Properties["MyProperty2"].LiteralValue());
        }

        [Fact]
        public void DynamicEnricherIgnoreNull()
        {
            LogEvent evt = null;
            var log = new LoggerConfiguration()
                .Enrich.WithDynamic(() =>
                {
                    return null;
                }, logNullValue: false)
                
                .WriteTo.Sink(new DelegatingSink(e => evt = e))
                .CreateLogger();

            log.Information(@"Some sample log.");

            Assert.NotNull(evt);
            Assert.False( evt.Properties.ContainsKey("DynamicProperty"));
           }
        [Fact]
        public void DynamicEnricheLogNull()
        {
            LogEvent evt = null;
            var log = new LoggerConfiguration()
                .Enrich.WithDynamic(() =>
                {
                    return null;
                }, logNullValue: true)

                .WriteTo.Sink(new DelegatingSink(e => evt = e))
                .CreateLogger();

            log.Information(@"Some sample log.");

            Assert.NotNull(evt);
            Assert.Null(evt.Properties["DynamicProperty"].LiteralValue());
        }
    }

  
}
