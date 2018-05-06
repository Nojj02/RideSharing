using System.Collections.Generic;
using System.Linq;

namespace RideSharing.RideMatcher.Tests
{
    public class InMemoryMessageQueueProcessingDetailRepository : IMessageQueueProcessingDetailRepository
    {
        private readonly IEnumerable<MessageQueueProcessingDetail> _messageQueueProcessingDetails;

        public InMemoryMessageQueueProcessingDetailRepository()
            : this(new List<MessageQueueProcessingDetail>())
        {
        }
        
        public InMemoryMessageQueueProcessingDetailRepository(
            IEnumerable<MessageQueueProcessingDetail> messageQueueProcessingDetails)
        {
            _messageQueueProcessingDetails = messageQueueProcessingDetails;
        }

        public MessageQueueProcessingDetail Get(string queueName)
        {
            return _messageQueueProcessingDetails.SingleOrDefault(x => x.QueueName == queueName);
        }
    }
}