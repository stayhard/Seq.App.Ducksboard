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
            HelpText = "Whether or not the value should be regarded as a delta, that is, incrementing the existing value in Ducksboard. Do not process old events while updating with delta values.")]
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

            object data;
            var timestamp = (int)Math.Round((evt.TimestampUtc - new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc)).TotalSeconds);
            if (IsDelta)
            {
                data = new
                {
                    delta = value,
                    timestamp
                };
            }
            else
            {
                data = new
                {
                    value,
                    timestamp
                };
            }

            await DucksboardUtil.SendData(ApiKey, DataLabel, data);
        }
    }
}
