using System;
using MediatR;

namespace Focus.Core.Common.Messages.Events
{
    public class NewDay : INotification
    {
        public DateTime Date => DateTime.Today.ToUniversalTime();
    }
}