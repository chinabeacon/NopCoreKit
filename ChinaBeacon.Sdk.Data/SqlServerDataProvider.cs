using System.Data.Common;

namespace ChinaBeacon.Sdk.Data
{
    public class SqlServerDataProvider : BaseEfDataProvider
    {
//        /// <summary>
//        /// Get connection factory
//        /// </summary>
//        /// <returns>Connection factory</returns>
//        public override IDbConnectionFactory GetConnectionFactory()
//        {
//            return new SqlConnectionFactory();
//        }

        /// <summary>
        /// A value indicating whether this data provider supports stored procedures
        /// </summary>
        public override bool StoredProceduredSupported
        {
            get { return true; }
        }

//        /// <summary>
//        /// Gets a support database parameter object (used by stored procedures)
//        /// </summary>
//        /// <returns>Parameter</returns>
//        public override DbParameter GetParameter()
//        {
//            return new SqlParameter();
//        }

        /// <summary>
        /// Maximum length of the data for HASHBYTES functions
        /// returns 0 if HASHBYTES function is not supported
        /// </summary>
        /// <returns>Length of the data for HASHBYTES functions</returns>
        public override int SupportedLengthOfBinaryHash()
        {
            return 8000; //for SQL Server 2008 and above HASHBYTES function has a limit of 8000 characters.
        }
    }
}
