namespace ChinaBeacon.Sdk.Core
{
    public interface IStartupTask
    {
        void Execute();

        int Order { get; }
    }
}
