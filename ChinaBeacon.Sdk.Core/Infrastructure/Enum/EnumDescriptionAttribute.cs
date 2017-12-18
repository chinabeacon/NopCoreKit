/**********************************************************************************************************************
 * 描述：
 *      枚举项附加信息特性（Attribute）类。
 * 
 * 变更历史：
 *      作者：李长皓  时间：2016/9/11 18:47:07  新建
 * 
 *********************************************************************************************************************/

using System;

namespace ChinaBeacon.Sdk.Core.Infrastructure.Enum
{
	/// <summary>
	/// 枚举项附加信息特性（Attribute）类。
	/// </summary>
	[AttributeUsage(AttributeTargets.Field, AllowMultiple = true)]
	public class EnumDescriptionAttribute : System.Attribute
	{
		#region 公共属性

		/// <summary>
		/// 获取附加信息关键字。
		/// </summary>
		/// <value><see cref="String"/></value>
		public string Key { get; private set; }

		/// <summary>
		/// 获取附加信息。
		/// </summary>
		/// <value><see cref="String"/></value>
		public string Description { get; private set; }

		#endregion

		#region 构造方法

		/// <summary>
		/// 新建默认枚举项附加信息特性。
		/// 即 Key 值为“Default”的附加信息。
		/// </summary>
		/// <param name="description">枚举项附加信息字符串。</param>
		public EnumDescriptionAttribute(string description)
			: this(string.Empty, description)
		{
		}

		/// <summary>
		/// 新建枚举项多样描述特性（Attribute）
		/// </summary>
		/// <param name="key">描述关键字。</param>
		/// <param name="description">枚举项描述信息。</param>
		public EnumDescriptionAttribute(string key, string description)
		{
			this.Key = key;
			this.Description = description;
		}

		#endregion
	}
}