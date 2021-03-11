using Application.Common.DTOs;
using System.Threading.Tasks;

namespace Application.Common.Interfaces
{
    public interface IPublishEvent
    {
        Task PublishEvent(object payload, string routingKey);
    }
}