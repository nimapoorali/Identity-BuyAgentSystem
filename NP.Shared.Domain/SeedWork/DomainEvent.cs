using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NP.Shared.Domain.Models.SeedWork
{
    public static class DomainEvent
    {
        public static Func<IMediator>? Mediator { get; set; }
        public static async Task Raise<T>(T args) where T : INotification
        {
            var mediator = Mediator!.Invoke();
            await mediator.Publish<T>(args);
        }
    }
}
