﻿using System.Data.Common;

namespace ChinaBeacon.Sdk.Data
{
    public abstract class BaseEfDataProvider : IEfDataProvider
    {
//        /// <summary>
//        /// Get connection factory
//        /// </summary>
//        /// <returns>Connection factory</returns>
//        public abstract IDbConnectionFactory GetConnectionFactory();
//
//        /// <summary>
//        /// Initialize connection factory
//        /// </summary>
//        public void InitConnectionFactory()
//        {
//            Database.DefaultConnectionFactory = GetConnectionFactory();
//        }

        /// <summary>
        /// Set database initializer
        /// </summary>
        //public abstract void SetDatabaseInitializer();

        ///// <summary>
        ///// Initialize database
        ///// </summary>
        //public virtual void InitDatabase()
        //{
        //    InitConnectionFactory();
        //    SetDatabaseInitializer();
        //}

        /// <summary>
        /// A value indicating whether this data provider supports stored procedures
        /// </summary>
        public abstract bool StoredProceduredSupported { get; }

//        /// <summary>
//        /// Gets a support database parameter object (used by stored procedures)
//        /// </summary>
//        /// <returns>Parameter</returns>
//        public abstract DbParameter GetParameter();

        /// <summary>
        /// Maximum length of the data for HASHBYTES functions
        /// returns 0 if HASHBYTES function is not supported
        /// </summary>
        /// <returns>Length of the data for HASHBYTES functions</returns>
        public abstract int SupportedLengthOfBinaryHash();
    }
}
