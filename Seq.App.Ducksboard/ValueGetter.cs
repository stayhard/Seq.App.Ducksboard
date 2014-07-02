using System;
using System.Globalization;
using Seq.Apps.LogEvents;
using Serilog;

namespace Seq.App.Ducksboard
{
    /// <summary>
    /// Service for fetching values from events
    /// </summary>
    public static class ValueGetter
    {
        /// <summary>
        /// Gets a decimal based on a static decimal as a string, or a pointer to a property of given event.
        /// </summary>
        /// <param name="logger">Logging instance for logging programs.</param>
        /// <param name="value">The value to parse</param>
        /// <param name="eventData">The event to fetch the value from should the "value" point to an event property.</param>
        /// <returns></returns>
        public static decimal? GetDecimal(ILogger logger, string value, LogEventData eventData)
        {
            decimal result;
            if (!decimal.TryParse(value, NumberStyles.Float, CultureInfo.GetCultureInfo("en-us"), out result))
            {
                if (eventData.Properties.ContainsKey(value))
                {
                    var p = eventData.Properties[value];
                    if (p is TimeSpan)
                    {
                        result = ((TimeSpan)p).Milliseconds;
                    }
                    else
                    {
                        try
                        {
                            result = Convert.ToDecimal(p);
                        }
                        catch (FormatException)
                        {
                            logger.Warning("Unable to send value to Ducksboard. Could not parse event property {EventPropertyKey} with value {Value} to decimal.", value, eventData.Properties[value]);
                            return null;
                        }
                    }
                }
                else
                {
                    logger.Warning("Unable to send value to Ducksboard. Could not parse {Value}. Expected an decimal or an event property key.");
                    return null;
                }
            }
            return result;
        }

        /// <summary>
        /// Gets a string based on a static string, or a pointer to a property of given event.
        /// </summary>
        /// <param name="logger">Logging instance for logging programs.</param>
        /// <param name="value">The value to parse</param>
        /// <param name="eventData">The event to fetch the value from should the "value" point to an event property.</param>
        /// <returns></returns>
        public static string GetString(ILogger logger, string value, LogEventData eventData)
        {
            if (ReferenceEquals(value, null)) return null;

            if (eventData.Properties != null && eventData.Properties.ContainsKey(value))
            {
                return eventData.Properties[value].ToString();
            }
            return value;
        }
    }
}
