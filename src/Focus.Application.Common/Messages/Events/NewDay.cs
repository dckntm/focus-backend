using System;
using MediatR;

namespace Focus.Application.Common.Messages.Events
{
    public class NewDay : INotification
    {
        public DateTime Date => DateTime.Today.ToUniversalTime();
    }
}