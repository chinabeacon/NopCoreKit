namespace ChinaBeacon.Sdk.Core.Data
{
    public interface IUnitOfWork
    {
        /// <summary>
        /// 提交事务
        /// </summary>
        /// <returns>受影响行数</returns>
        int Commit();
        //        /// <summary>
        //        /// 启动事务
        //        /// </summary>
        //        void BeginTransaction();
        //        /// <summary>
        //        /// 提交事务
        //        /// </summary>
        //        void Commit();
        //        /// <summary>
        //        /// 释放
        //        /// </summary>
        //        void Dispose();
    }
}
