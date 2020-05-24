using System;

namespace Focus.Core.Common.Abstract
{
    public interface IAuditable
    {
        DateTime CreatedAt { get; set; }
        DateTime? ChangedAt { get; set; }
        string? ChangedBy { get; set; }
    }
}