using MediatR;

namespace EventsApplication.Application.Events.Commands.DeleteEventImage
{
    public class DeleteEventImageCommand : IRequest
    {

        public Guid EventId { get; set; }

        public DeleteEventImageCommand(Guid EventId)
        {
            this.EventId = EventId;
        }
    }
    
}
