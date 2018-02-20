using Microsoft.Azure.NotificationHubs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp1
{
    class Program
    {
        private const string HubName = "easyNotificationHub";
        private const string HubFullConnectionString = "Endpoint=sb://easy2m.servicebus.windows.net/;SharedAccessKeyName=DefaultFullSharedAccessSignature;SharedAccessKey=ETy0vIq5rMRf7OwmJ5zTZEN3XgtsMwZv5zTG3JqHZ3E=";

        static void Main(string[] args)
        {

            xAsync().Wait();
        }

        private static async Task xAsync()
        {
            NotificationHubClient Hub = NotificationHubClient.CreateClientFromConnectionString(HubFullConnectionString, HubName);
            string tagExpression = "1";

            NotificationOutcome outcome = null;
            string message = "hello";
            var notif = "{ \"data\" : {\"message\":\"" + message + "\"}}";
            outcome = await Hub.SendGcmNativeNotificationAsync(notif, tagExpression);

            if (outcome != null)
            {
                if (!((outcome.State == NotificationOutcomeState.Abandoned) ||
                    (outcome.State == NotificationOutcomeState.Unknown)))
                {
                    return;
                }
            }

            return;
        }
    }
}
