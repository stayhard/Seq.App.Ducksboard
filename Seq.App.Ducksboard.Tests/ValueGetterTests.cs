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
        #region GetDecimal
        [Fact]
        public void GetDecimal_IntegerAsString_ReturnDecimal()
        {
            Assert.Equal(123m, ValueGetter.GetDecimal(new LoggerConfiguration().CreateLogger(), "123", new LogEventData()));
        }

        [Fact]
        public void GetDecimal_DecimalAsString_ReturnDecimal()
        {
            Assert.Equal(123.123m, ValueGetter.GetDecimal(new LoggerConfiguration().CreateLogger(), "123.123", new LogEventData()));
        }

        [Fact]
        public void GetDecimal_EventPropertyTimeSpan_ReturnMilliseconds()
        {
            Assert.Equal(123m, ValueGetter.GetDecimal(new LoggerConfiguration().CreateLogger(), "Elapsed", new LogEventData
            {
                Properties = new Dictionary<string, object>
            {
                {"Elapsed", new TimeSpan(0, 0, 0, 0, 123)}
            }}));
        }

        [Fact]
        public void GetDecimal_EventPropertyInteger_ReturnDecimal()
        {
            Assert.Equal(123m, ValueGetter.GetDecimal(new LoggerConfiguration().CreateLogger(), "SomeProp", new LogEventData
            {
                Properties = new Dictionary<string, object>
            {
                {"SomeProp", 123}
            }
            }));
        }

        [Fact]
        public void GetDecimal_EventPropertyDecimal_ReturnDecimal()
        {
            Assert.Equal(123.123m, ValueGetter.GetDecimal(new LoggerConfiguration().CreateLogger(), "SomeProp", new LogEventData
            {
                Properties = new Dictionary<string, object>
            {
                {"SomeProp", 123.123m}
            }
            }));
        }

        [Fact]
        public void GetDecimal_EventPropertyDouble_ReturnDecimal()
        {
            Assert.Equal(123.123m, ValueGetter.GetDecimal(new LoggerConfiguration().CreateLogger(), "SomeProp", new LogEventData
            {
                Properties = new Dictionary<string, object>
            {
                {"SomeProp", 123.123d}
            }
            }));
        }

        [Fact]
        public void GetDecimal_EventPropertyString_ReturnNull()
        {
            Assert.Null(ValueGetter.GetDecimal(new LoggerConfiguration().CreateLogger(), "SomeProp", new LogEventData
            {
                Properties = new Dictionary<string, object>
            {
                {"SomeProp", "SomeValue"}
            }
            }));
        }

        [Fact]
        public void GetDecimal_EventPropertyDoesNotExist_ReturnNull()
        {
            Assert.Null(ValueGetter.GetDecimal(new LoggerConfiguration().CreateLogger(), "SomeProp", new LogEventData
            {
                Properties = new Dictionary<string, object>
            {
                {"SomeOtherProp", "SomeValue"}
            }
            }));
        }
        #endregion

        #region GetString
        [Fact]
        public void GetString_String_ReturnString()
        {
            Assert.Equal("123", ValueGetter.GetString(new LoggerConfiguration().CreateLogger(), "123", new LogEventData()));
        }

        [Fact]
        public void GetString_EventPropertyInteger_ReturnString()
        {
            Assert.Equal("123", ValueGetter.GetString(new LoggerConfiguration().CreateLogger(), "SomeProp", new LogEventData
            {
                Properties = new Dictionary<string, object>
            {
                {"SomeProp", 123}
            }
            }));
        }

        [Fact]
        public void GetString_EventPropertyString_ReturnString()
        {
            Assert.Equal("123", ValueGetter.GetString(new LoggerConfiguration().CreateLogger(), "SomeProp", new LogEventData
            {
                Properties = new Dictionary<string, object>
            {
                {"SomeProp", "123"}
            }
            }));
        }

        [Fact]
        public void GetString_Null_ReturnNull()
        {
            Assert.Null(ValueGetter.GetString(new LoggerConfiguration().CreateLogger(), null, new LogEventData()));
        }
        #endregion
    }
}
