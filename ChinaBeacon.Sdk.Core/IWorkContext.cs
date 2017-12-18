namespace ChinaBeacon.Sdk.Core
{
    /// <summary>
    /// Work context
    /// </summary>
    public interface IWorkContext
    {
//        /// <summary>
//        /// 获取或设置当前客户
//        /// </summary>
//        Customer CurrentCustomer { get; set; }

        /// <summary>
        /// 取得或设定当前用户是否为admin
        /// </summary>
        bool IsAdmin { get; set; }
        bool IsAuthenticated { get; }

        /// <summary>
        /// 获得IP地址
        /// </summary>
        /// <returns></returns>
        string GetIpAddress();
    }
}
