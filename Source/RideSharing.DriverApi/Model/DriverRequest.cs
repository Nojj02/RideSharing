using System;
using System.Collections.Generic;
using System.Linq;
using RideSharing.CommonApi.Model;

namespace RideSharing.DriverApi.Model
{
    public class DriverRequest : Entity
    {
        private readonly List<DriverRequestEvent> _events = new List<DriverRequestEvent>();
        
        private readonly List<DriverRequestEvent> _newEvents = new List<DriverRequestEvent>();
        
        public DriverRequest(Guid id, Guid rideId, string pickupPoint) 
            : base(id: id)
        {
            Apply(
                new DriverRequestCreatedEvent(
                    rideId: rideId,
                    pickupPoint: pickupPoint,
                    status: DriverRequestStatus.Pending), 
                true);
        }

        private void Apply(DriverRequestCreatedEvent e, bool isNew)
        {
            RideId = e.RideId;
            PickupPoint = e.PickupPoint;
            Status = e.Status;

            AddEvent(e, isNew);
        }

        private void AddEvent(DriverRequestCreatedEvent e, bool isNew)
        {
            e.Id = Id;
            e.Version = GetLastVersionNumber() + 1;
            
            _events.Add(e);
            if (isNew)
            {
                _newEvents.Add(e);
            }
        }

        private int GetLastVersionNumber()
        {
            return _events.Any()
                ? _events
                    .OrderByDescending(x => x.Version)
                    .First()
                    .Version
                : -1;
        }

        public Guid RideId { get; private set; }
        
        public string PickupPoint { get; private set; }
        
        public DriverRequestStatus Status { get; private set; }

        public IReadOnlyList<DriverRequestEvent> NewEvents => _newEvents;
    }
    
    public class DriverRequestCreatedEvent : DriverRequestEvent
    {
        public DriverRequestCreatedEvent(Guid rideId, string pickupPoint, DriverRequestStatus status)
        {
            RideId = rideId;
            PickupPoint = pickupPoint;
            Status = status;
        }
        
        public Guid RideId { get; }
        
        public string PickupPoint { get; }
        
        public DriverRequestStatus Status { get; }
    }

    public class DriverRequestEvent
    {
        public Guid Id { get; set; }
        
        public int Version { get; set; }
    }
}