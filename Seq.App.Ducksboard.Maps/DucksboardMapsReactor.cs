using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Seq.Apps;
using Seq.Apps.LogEvents;

namespace Seq.App.Ducksboard.Maps
{
    [SeqApp("Ducksboard Maps",
    Description = "Sends geographical data to Ducksboard.")]
    public class DucksboardMapsReactor : DucksboardBaseReactor, ISubscribeTo<LogEventData>
    {
        [SeqAppSetting(
            HelpText = "A static decimal (use dot as decimal point) value or an event property.",
            IsOptional = true)]
        public string Latitude { get; set; }

        [SeqAppSetting(
            HelpText = "A static decimal (use dot as decimal point) value or an event property.",
            IsOptional = true)]
        public string Longitude { get; set; }

        [SeqAppSetting(
            HelpText = "A static IP address or an event property containing an IP address. If this is specified, it will be used instead of Latitude and Longitude fields.",
            IsOptional = true)]
        public string IpAddress { get; set; }

        [SeqAppSetting(
            HelpText = "If set, the size of points on the map are scaled based on this value.",
            IsOptional = true)]
        public string Value { get; set; }

        [SeqAppSetting(
            DisplayName = "Size (px)",
            HelpText = "If set, will override value and set the size of points on the map to this value.",
            IsOptional = true)]
        public string Size { get; set; }

        [SeqAppSetting(
            HelpText = "Color can be sent in hex (#fff or #ffffff) or rgb (rgb(0, 70, 255)).",
            IsOptional = true)]
        public string Color { get; set; }

        [SeqAppSetting(
            HelpText = "Will show up as a tooltip on the point.",
            IsOptional = true)]
        public string Information { get; set; }

        public async void On(Event<LogEventData> evt)
        {
            if (string.IsNullOrWhiteSpace(Latitude) &&
                string.IsNullOrWhiteSpace(Longitude) &&
                string.IsNullOrWhiteSpace(IpAddress))
            {
                Log.Warning("Ducksboard: Could not send geographical data. Neither Latitude, Longitude or an IP address was specified in app instance settings.");
                return;
            }

            // Verify numeric values
            decimal? latitude = null;
            if (!string.IsNullOrWhiteSpace(Latitude) && (latitude = ValueGetter.GetDecimal(Log, Latitude, evt.Data)) == null) return;
            decimal? longitude = null;
            if (!string.IsNullOrWhiteSpace(Longitude) && (longitude = ValueGetter.GetDecimal(Log, Longitude, evt.Data)) == null) return;
            decimal? size = null;
            if (!string.IsNullOrWhiteSpace(Size) && (size = ValueGetter.GetDecimal(Log, Size, evt.Data)) == null) return;
            decimal? value = null;
            if (!string.IsNullOrWhiteSpace(Value) && (value = ValueGetter.GetDecimal(Log, Value, evt.Data)) == null) return;

            var data = new Dictionary<string, object>();

            if (!string.IsNullOrWhiteSpace(IpAddress))
            {
                data.Add("ip", ValueGetter.GetString(Log, IpAddress, evt.Data));
            }
            else
            {
                data.Add("latitude", latitude);
                data.Add("longitude", longitude);
            }

            if (size != null)
            {
                data.Add("size", Math.Round((decimal)size));
            }
            else if (value != null)
            {
                data.Add("value", value);
            }

            if (!string.IsNullOrWhiteSpace(Color))
            {
                data.Add("color", ValueGetter.GetString(Log, Color, evt.Data));
            }

            if (!string.IsNullOrWhiteSpace(Information))
            {
                data.Add("information", ValueGetter.GetString(Log, Information, evt.Data));
            }

            await DucksboardUtil.SendData(ApiKey, DataLabel, data);
        }
    }
}
