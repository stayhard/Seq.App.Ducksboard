using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Seq.Apps.LogEvents;
using Serilog;

namespace Seq.App.Ducksboard.Numbers
{
    public static class ValueGetter
    {

        public static int? GetValue(ILogger logger, string value, LogEventData eventData)
        {
            int result;
            if (!int.TryParse(value, out result))
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
                            result = Convert.ToInt32(p);
                        }
                        catch (FormatException)
                        {
                            logger.Warning("Unable to send value to Ducksboard. Could not parse event property {EventPropertyKey} with value {Value} to integer.", value, eventData.Properties[value]);
                            return null;
                        }
                    }
                }
                else
                {
                    logger.Warning("Unable to send value to Ducksboard. Could not parse {Value}. Expected an integer or an event property key.");
                    return null;
                }
            }
            return result;
        }

    }
}
