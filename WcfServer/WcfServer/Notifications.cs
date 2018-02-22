using Microsoft.Azure.NotificationHubs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace WcfServer
{
    public class Notifications
    {
        public static Notifications Instance = new Notifications();
        private const string HubName = "easyNotificationHub";
        private const string HubFullConnectionString = "Endpoint=sb://easy2m.servicebus.windows.net/;SharedAccessKeyName=DefaultFullSharedAccessSignature;SharedAccessKey=ETy0vIq5rMRf7OwmJ5zTZEN3XgtsMwZv5zTG3JqHZ3E=";
        public NotificationHubClient Hub { get; set; }
        
        private Notifications()
        {
            Hub = NotificationHubClient.CreateClientFromConnectionString(HubFullConnectionString, HubName);
        }

        public async Task<bool> Notify(string message , List<long> usersId)
        {
            string tagExpression = "";
            for (int index = 0 ; index < usersId.Count; index++)
            {
                if(index == usersId.Count - 1)
                    tagExpression += usersId[index];
                else
                    tagExpression += usersId[index] + " || ";
            }

            string[] tags = new String[1];
            tags[0] = tagExpression;

            var notif = "{ \"data\" : {\"message\":\"" + message + "\"}}";
            NotificationOutcome outcome = null;
            outcome = await Hub.SendGcmNativeNotificationAsync(notif, tags);

            if (outcome != null)
            {
                if (!((outcome.State == NotificationOutcomeState.Abandoned) ||
                    (outcome.State == NotificationOutcomeState.Unknown)))
                {
                    return true;
                }
            }

            return false;
        }
    }
}