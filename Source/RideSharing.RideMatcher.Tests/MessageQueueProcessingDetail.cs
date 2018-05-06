namespace RideSharing.RideMatcher.Tests
{
    public class MessageQueueProcessingDetail
    {
        public MessageQueueProcessingDetail(
            string queueName,
            int lastMessageNumber)
        {
            QueueName = queueName;
            LastMessageNumber = lastMessageNumber;
        }

        public string QueueName { get; }

        public int LastMessageNumber { get; }
    }
}