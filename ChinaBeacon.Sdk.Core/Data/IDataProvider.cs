using System.Data.Common;

namespace ChinaBeacon.Sdk.Core.Data
{
    /// <summary>
    /// Data provider interface
    /// </summary>
    public interface IDataProvider
    {
        /// <summary>
        /// A value indicating whether this data provider supports stored procedures
        /// </summary>
        bool StoredProceduredSupported { get; }

//        /// <summary>
//        /// Gets a support database parameter object (used by stored procedures)
//        /// </summary>
//        /// <returns>Parameter</returns>
//        DbParameter GetParameter();

        /// <summary>
        /// Maximum length of the data for HASHBYTES functions
        /// returns 0 if HASHBYTES function is not supported
        /// </summary>
        /// <returns>Length of the data for HASHBYTES functions</returns>
        int SupportedLengthOfBinaryHash();
    }
}
