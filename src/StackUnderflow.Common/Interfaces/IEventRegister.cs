namespace StackUnderflow.Common.Interfaces
{
    public interface IEventRegister
    {
        void RegisterEvent<T>(object newEvent)
            where T : class;
    }
}
