namespace QueuingSystem
{
    public enum SystemType
    {
        WithRejects,
        WithQueueLimited,
        WithQueueUnlimited,

        WithRejectsOneChannel,
        WithRejectsSeveralChannels,
        
        WithQueueLimitedOneChannel,
        WithQueueLimitedSeveralChannel,
        
        WithQueueUnlimitedOneChannel,
        WithQueueUnlimitedSeveralChannel,
    }
}