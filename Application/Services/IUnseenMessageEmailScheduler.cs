namespace CMPNatural.Application.Services
{
    public interface IUnseenMessageEmailScheduler
    {
        void ScheduleChatMessageEmail(long messageId);
        void ScheduleCommonMessageEmail(long messageId);
    }
}
