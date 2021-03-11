using MediatR;
using Application.Common.DTOs;

namespace Application.Common.Events
{
    public class WorldCreatedNotification : INotification
    {
        public readonly EventBusPayload eventBusPayload;
        public readonly string action;
        public WorldCreatedNotification(EventBusPayload eventBusPayload, string action)
        {
            this.eventBusPayload = eventBusPayload;
            this.action = action;
        }
    }
}