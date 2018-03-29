using System;
using System.Data.Common;

namespace RideSharing.RideApi.Model
{
    public abstract class Entity
    {
        protected Entity(Guid id)
        {
            Id = id;
        }
        
        public Guid Id { get; }
    }
}