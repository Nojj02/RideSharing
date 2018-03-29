using System;
using System.Collections.Generic;
using System.Linq;
using RideSharing.CommonApi.Model;

namespace RideSharing.RideApi.Model
{
    public class Ride : Entity
    {
        private readonly List<RideEvent> _events = new List<RideEvent>();
        
        private readonly List<RideEvent> _newEvents = new List<RideEvent>();

        public Ride(Guid id, string pickupPoint, string destination)
            : base(id)
        {
            this.Apply(
                new RideRequestedEvent(
                    pickupPoint : pickupPoint,
                    destination : destination
                    ),
                isNew: true);
        }

        public Ride(Guid id, IReadOnlyList<RideEvent> rideEvents)
            : base(id)
        {
            foreach (var rideEvent in rideEvents)
            {
                switch (rideEvent)
                {
                    case RideRequestedEvent e:
                    {
                        Apply(e, isNew: false);
                        break;
                    }
                    case RideAcceptedEvent e:
                    {
                        Apply(e, isNew: false);
                        break;
                    }
                    default:
                        throw new ApplicationException();
                }
            }
        }

        public IReadOnlyList<RideEvent> Events => _events;

        public IReadOnlyList<RideEvent> NewEvents => _newEvents;
        
        public RideState State { get; private set; }

        public string PickupPoint { get; private set; }

        public string Destination { get; private set; }

        public void Accept()
        {
            Apply(
                new RideAcceptedEvent(),
                isNew: true);
        }

        private void Apply(RideRequestedEvent e, bool isNew)
        {
            State = RideState.Requested;
            PickupPoint = e.PickupPoint;
            Destination = e.Destination;

            AddEvent(e, isNew);
        }

        private void Apply(RideAcceptedEvent e, bool isNew)
        {
            State = RideState.Accepted;
            
            AddEvent(e, isNew);
        }

        private void AddEvent(RideEvent e, bool isNew)
        {
            e.Id = this.Id;
            e.Version = this.GetLastVersionNumber() + 1;
            
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
    }

    public class RideRequestedEvent : RideEvent
    {
        public RideRequestedEvent(
            string pickupPoint,
            string destination)
        {
            PickupPoint = pickupPoint;
            Destination = destination;
        }
        
        public string PickupPoint { get; }

        public string Destination { get; }
    }
    
    public class RideAcceptedEvent : RideEvent
    {
    }

    public class RideEvent
    {
        public Guid Id { get; set; }
        
        public int Version { get; set; }
    }
}