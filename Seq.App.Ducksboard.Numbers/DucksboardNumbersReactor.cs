using System;
using Seq.App.Ducksboard;
using Seq.App.Ducksboard.Numbers;
using Seq.Apps;
using Seq.Apps.LogEvents;

namespace Seq.App.HipChat
{
    [SeqApp("Ducksboard Numbers",
    Description = "Sends numeric data to Ducksboard.")]
    public class DucksboardNumbers : Reactor, ISubscribeTo<LogEventData>
    {

        [SeqAppSetting(
            DisplayName = "API Key")]
        public string ApiKey { get; set; }

        [SeqAppSetting(
            DisplayName = "Data Source Label",
            HelpText = "Get this from widget preferences.")]
        public string DataLabel { get; set; }

        [SeqAppSetting(
            DisplayName = "Value as delta",
            HelpText = "Whether or not the value should be regarded as a delta, that is, incrementing the existing value in Ducksboard.",
            IsOptional = true)]
        public bool IsDelta { get; set; }

        [SeqAppSetting(
            HelpText = "A static integer value or an event property.",
            IsOptional = false)]
        public string Value { get; set; }

        public async void On(Event<LogEventData> evt)
        {
            var value = ValueGetter.GetValue(Log, Value, evt.Data);
            if (value == null)
            {
                return;
            }

            object data = null;
            if (IsDelta)
            {
                data = new
                {
                    delta = value
                };
            }
            else
            {
                data = new
                {
                    value
                };
            }

            await DucksboardUtil.SendData(ApiKey, DataLabel, data);
        }
    }
}
