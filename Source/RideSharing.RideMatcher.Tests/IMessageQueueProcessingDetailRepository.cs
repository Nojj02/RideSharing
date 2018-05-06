namespace RideSharing.RideMatcher.Tests
{
    public interface IMessageQueueProcessingDetailRepository
    {
        MessageQueueProcessingDetail Get(string queueName);
    }
}