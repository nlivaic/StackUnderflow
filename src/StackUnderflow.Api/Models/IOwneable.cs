using System;

namespace StackUnderflow.Api.Models
{
    public interface IOwneable
    {
        public Guid UserId { get; set; }
    }
}