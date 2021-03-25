using MediatR;
using Application.Common.DTOs;

namespace Application.Common.Events
{
    public class WorldReceived : INotification
    {
        public readonly EventBusPayload eventBusPayload;
        public readonly string action;
        public WorldReceived(EventBusPayload eventBusPayload, string action)
        {
            this.eventBusPayload = eventBusPayload;
            this.action = action;
        }
    }
}