using System;
using Seq.App.Ducksboard;
using Seq.Apps;
using Seq.Apps.LogEvents;

namespace Seq.App.HipChat
{
    [SeqApp("Ducksboard Numbers",
    Description = "Sends numeric data to Ducksboard.")]
    public class DucksboardNumbers : DucksboardBaseReactor, ISubscribeTo<LogEventData>
    {
        [SeqAppSetting(
            DisplayName = "Value as delta",
            HelpText = "Whether or not the value should be regarded as a delta, that is, incrementing the existing value in Ducksboard. Do not process old events while updating with delta values.")]
        public bool IsDelta { get; set; }

        [SeqAppSetting(
            HelpText = "A static decimal (use dot as decimal point) value or an event property.",
            IsOptional = false)]
        public string Value { get; set; }
        
        public async void On(Event<LogEventData> evt)
        {
            var value = ValueGetter.GetDecimal(Log, Value, evt.Data);
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
                    delta = value
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
