/**********************************************************************************************************************
 * 描述：
 *      log4net日志帮助累
 * 
 * 变更历史：
 *      作者：李长皓  时间：2016/9/19 9:26:01	新建
 * 
 *********************************************************************************************************************/

using System;
using System.IO;
using ChinaBeacon.Sdk.Core.Enum;
using log4net;
using log4net.Config;
using log4net.Repository;

namespace ChinaBeacon.Sdk.Core.Data
{

    public class LogHelper
    {
        public static ILoggerRepository Repository { get; set; }
        private static ILog loggerInfo;
        private static ILog loggerError;

        //        /// <summary>
        //        /// 初始化日志系统
        //        /// 在系统运行开始初始化
        //        /// </summary>
        public static void Init()
        {
            Repository = LogManager.CreateRepository("NETCoreRepository");
            XmlConfigurator.Configure(Repository, new FileInfo("Config/Log4net.config"));
            loggerInfo = LogManager.GetLogger(Repository.Name, "loginfo");
            loggerError = LogManager.GetLogger(Repository.Name, "logerror");
        }

        /// <summary>
        /// 写入日志
        /// </summary>
        /// <param name="message">日志信息</param>
        /// <param name="messageType">日志类型</param>
        public static void Write(string message, LogMessageEnum messageType)
        {
            DoLog(message, messageType, null, Type.GetType("System.Object"));
        }

        /// <summary>
        /// 写入日志
        /// </summary>
        /// <param name="message">日志信息</param>
        /// <param name="messageType">日志类型</param>
        /// <param name="type"></param>
        public static void Write(string message, LogMessageEnum messageType, Type type)
        {
            DoLog(message, messageType, null, type);
        }

        /// <summary>
        /// 写入日志
        /// </summary>
        /// <param name="message">日志信息</param>
        /// <param name="messageType">日志类型</param>
        /// <param name="ex">异常</param>
        public static void Write(string message, LogMessageEnum messageType, Exception ex)
        {
            DoLog(message, messageType, ex, Type.GetType("System.Object"));
        }

        /// <summary>
        /// 写入日志
        /// </summary>
        /// <param name="message">日志信息</param>
        /// <param name="messageType">日志类型</param>
        /// <param name="ex">异常</param>
        /// <param name="type"></param>
        private static void Write(string message, LogMessageEnum messageType, Exception ex,
                                 Type type)
        {
            DoLog(message, messageType, ex, type);
        }
        /// <summary>
        /// 保存日志
        /// </summary>
        /// <param name="message">日志信息</param>
        /// <param name="messageType">日志类型</param>
        /// <param name="ex">异常</param>
        /// <param name="type">日志类型</param>
        private static void DoLog(string message, LogMessageEnum messageType, Exception ex,
                                  Type type)
        {
            switch (messageType)
            {
                case LogMessageEnum.Debug:
                    loggerInfo.Debug(message, ex);
                    break;
                case LogMessageEnum.Info:
                    loggerInfo.Info(message, ex);
                    break;
                case LogMessageEnum.Warn:
                    loggerInfo.Warn(message, ex);
                    break;
                case LogMessageEnum.Error:
                    loggerError.Error(message, ex);
                    break;
                case LogMessageEnum.Fatal:
                    loggerError.Fatal(message, ex);
                    break;
            }
        }
        /// <summary>
        /// 关闭log4net
        /// </summary>
        public static void ShutDown()
        {
            LogManager.Shutdown();
        }
    }
}
