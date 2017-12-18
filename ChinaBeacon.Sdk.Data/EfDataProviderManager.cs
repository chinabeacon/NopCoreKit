using System;
using ChinaBeacon.Sdk.Core;
using ChinaBeacon.Sdk.Core.Data;

namespace ChinaBeacon.Sdk.Data
{
    public partial class EfDataProviderManager : BaseDataProviderManager
    {
        public EfDataProviderManager(DataSettings settings) : base(settings)
        {
        }

        public override IDataProvider LoadDataProvider()
        {

            var providerName = Settings.DataProvider;
            if (String.IsNullOrWhiteSpace(providerName))
                throw new LawEnforcementException("Data Settings doesn't contain a providerName");

            switch (providerName.ToLowerInvariant())
            {
                case "sqlserver":
                    return new SqlServerDataProvider();
//                case "mysql":
//                    return new MySqlDataProvider();
                //case "sqlce":
                //    return new SqlCeDataProvider();
                default:
                    throw new LawEnforcementException(string.Format("Not supported dataprovider name: {0}", providerName));
            }
        }

    }
}
