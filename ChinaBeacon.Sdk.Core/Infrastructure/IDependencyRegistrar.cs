using Autofac;
using ChinaBeacon.Sdk.Core.Configuration;

namespace ChinaBeacon.Sdk.Core.Infrastructure
{
    public interface IDependencyRegistrar
    {
        void Register(ContainerBuilder builder, ITypeFinder typeFinder, ChinaBeaconConfig config);

        int Order { get; }
    }
}
