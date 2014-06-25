using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Seq.Apps.LogEvents;
using Serilog;
using Xunit;

namespace Seq.App.Ducksboard.Numbers.Tests
{
    public class ValueGetterTests
    {

        [Fact]
        public void GetValue_IntegerAsString_ReturnInteger()
        {
            Assert.Equal(123, ValueGetter.GetValue(new LoggerConfiguration().CreateLogger(), "123", new LogEventData()));
        }

        [Fact]
        public void GetValue_EventPropertyTimeSpan_ReturnMilliseconds()
        {
            Assert.Equal(123, ValueGetter.GetValue(new LoggerConfiguration().CreateLogger(), "Elapsed", new LogEventData
            {
                Properties = new Dictionary<string, object>
            {
                {"Elapsed", new TimeSpan(0, 0, 0, 0, 123)}
            }}));
        }

        [Fact]
        public void GetValue_EventPropertyString_ReturnNull()
        {
            Assert.Null(ValueGetter.GetValue(new LoggerConfiguration().CreateLogger(), "SomeProp", new LogEventData
            {
                Properties = new Dictionary<string, object>
            {
                {"SomeProp", "SomeValue"}
            }
            }));
        }

        [Fact]
        public void GetValue_EventPropertyDoesNotExist_ReturnNull()
        {
            Assert.Null(ValueGetter.GetValue(new LoggerConfiguration().CreateLogger(), "SomeProp", new LogEventData
            {
                Properties = new Dictionary<string, object>
            {
                {"SomeOtherProp", "SomeValue"}
            }
            }));
        }

    }
}
