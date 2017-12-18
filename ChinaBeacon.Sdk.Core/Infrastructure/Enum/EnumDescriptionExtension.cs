/**********************************************************************************************************************
 * 描述：
 *      枚举项附加信息特性（Attribute）类。
 * 
 * 变更历史：
 *      作者：李长皓  时间：2016/9/11 18:56:39	新建
 * 
 *********************************************************************************************************************/

using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Reflection;

namespace ChinaBeacon.Sdk.Core.Infrastructure.Enum
{
    public static class EnumDescriptionExtension
    {
        #region 扩展方法

        /// <summary>
        /// 获取当前枚举项的默认描述字符串。
        /// </summary>
        /// <param name="enumItem">当前枚举项。</param>
        /// <returns>默认描述字符串。</returns>
        public static string GetDescriptionEx(this System.Enum enumItem)
        {
            return GetDescription(enumItem);
        }

        /// <summary>
        /// 根据指定关键字获取当前枚举项的描述字符串。
        /// </summary>
        /// <param name="enumItem">当前枚举项。</param>
        /// <param name="key">描述关键字。</param>
        /// <returns>描述字符串。</returns>
        public static string GetDescriptionEx(this System.Enum enumItem, string key)
        {
            return GetDescription(enumItem, key);
        }

        /// <summary>
        /// 获取指定枚举项的整数值。
        /// </summary>
        /// <param name="enumItem">枚举项。</param>
        /// <returns>对应的整数值。</returns>
        public static int GetEnumValueEx(this System.Enum enumItem)
        {
            return GetEnumValue(enumItem);
        }

        #endregion

        #region Enum转换为SelectList
        /// <summary>
        /// Enum转换为SelectList。
        /// </summary>
        /// <typeparam name="TEnum">枚举类型。</typeparam>
        /// <returns>System.Web.Mvc.SelectList 类的实例中的选定项的列表。</returns>
        public static List<GeneralSelectItemModel> ToSelectList<TEnum>()
        {
            return TransformToSelectList<TEnum>(string.Empty);
        }
        /// <summary>
        /// Enum转换为SelectList。
        /// </summary>
        /// <typeparam name="TEnum">枚举类型。</typeparam>
        /// <returns>System.Web.Mvc.SelectList 类的实例中的选定项的列表。</returns>
        public static List<GeneralSelectItemModel> ToSelectList<TEnum>(string key)
        {
            return TransformToSelectList<TEnum>(key);
        }

        /// <summary>
        /// Enum转换为SelectList。
        /// </summary>
        /// <typeparam name="TEnum">枚举类型。</typeparam>
        /// <param name="key">枚举附加键值。</param>
        /// <returns>System.Web.Mvc.SelectList 类的实例中的选定项的列表。</returns>
        private static List<GeneralSelectItemModel> TransformToSelectList<TEnum>(string key)
        {
            IEnumerable<Triple<string, int, string>> actual =
                EnumDescriptionExtension.GetDescriptions(typeof(TEnum), key);
            return actual.Select(actualEle => new GeneralSelectItemModel()
            {
                Description = actualEle.Third,
                Code = actualEle.Second
            }).OrderBy(m => m.Code).ToList();
        }
        #endregion

        #region 获取枚举值

        /// <summary>
        /// 根据给定枚举项，获取枚举值。
        /// </summary>
        /// <param name="enumItem">枚举项。</param>
        /// <returns>枚举值。</returns>
        public static int GetEnumValue(System.Enum enumItem)
        {
            return int.Parse(System.Enum.Format(enumItem.GetType(), enumItem, "D"), CultureInfo.InvariantCulture);
        }

        /// <summary>
        /// 根据给定枚举项描述，获取枚举值。
        /// </summary>
        /// <param name="description">枚举项描述。</param>
        /// <returns>枚举值。</returns>
        public static int GetEnumValue<TEnum>(string description) where TEnum : struct
        {
            return GetEnumItemByDescription(typeof(TEnum), string.Empty, description).GetEnumValueEx();
        }
        #endregion

        #region 获取符合附加信息关键字的所有枚举项附加信息字符串

        /// <summary>
        /// 获取符合附加信息关键字的所有枚举项附加信息字符串。
        /// </summary>
        /// <param name="enumType">枚举类型。</param>
        /// <param name="key">附加信息关键字。</param>
        /// <returns>
        /// 包含枚举项名称、枚举项值及相对应枚举项附加信息的 <see cref="Triple{T1,T2,T3}"/> 结构集合。
        /// 第一个对象存储枚举项名称；第二个对象存储枚举项值；第三个对象存储枚举项附加信息。
        /// </returns>
        public static IEnumerable<Triple<String, Int32, String>> GetDescriptions(Type enumType, string key)
        {
            FieldInfo[] fis = enumType.GetFields(BindingFlags.Public | BindingFlags.Static);

            var triples =
                from fi in fis
                let enumItemValue = Convert.ToInt32(System.Enum.Parse(enumType, fi.Name, true), CultureInfo.InvariantCulture)
                let attrs = fi.GetCustomAttributes(typeof(EnumDescriptionAttribute), false)
                from EnumDescriptionAttribute attr in attrs
                where attr.Key == key
                select new Triple<String, Int32, String>(fi.Name, enumItemValue, attr.Description);

            return triples;
        }

        /// <summary>
        /// 根据给定枚举项，获取所有的附加信息。
        /// </summary>
        /// <param name="enumItem">枚举项。</param>
        /// <returns>
        /// 包含枚举项附加信息关键字、枚举项值及相对应枚举项附加信息的 <see cref="Triple{T1, T2, T3}"/> 结构集合。
        /// 第一个对象存储枚举项附加信息关键字；第二个对象存储枚举项值；第三个对象存储枚举项附加信息。
        /// </returns>
        /// <exception cref="InvalidOperationException">
        /// 当给定枚举类型中的枚举项未定义附加信息时引发异常。
        /// 当给定枚举类型中的枚举项上使用重复关键字定义了附加信息时引发异常。
        /// </exception>
        public static IEnumerable<Triple<String, Int32, String>> GetDescriptions(System.Enum enumItem)
        {
            Type enumType = enumItem.GetType();
            FieldInfo fi = enumType.GetField(enumItem.ToString());
            int enumItemValue = Convert.ToInt32(
                System.Enum.Parse(enumType, enumItem.ToString()), CultureInfo.InvariantCulture);

            var triples =
                from EnumDescriptionAttribute attr in fi.GetCustomAttributes(typeof(EnumDescriptionAttribute), false)
                select new Triple<String, Int32, String>(attr.Key, enumItemValue, attr.Description);

            return triples;
        }

        /// <summary>
        /// 根据给定枚举项名称（值）及附加信息关键字，获取附加信息字符串。
        /// </summary>
        /// <param name="enumType">枚举类型。</param>
        /// <param name="value">枚举项名称（值）。</param>
        /// <param name="key">附加信息关键字。</param>
        /// <returns>返回附加信息字符串。</returns>
        public static string GetDescription(Type enumType, string value, string key)
        {
            System.Enum enumObj = (System.Enum)System.Enum.Parse(enumType, value, true);

            IEnumerable<Triple<String, Int32, String>> triples = GetDescriptions(enumObj);

            string description = (from triple in triples
                                  where triple.First == key
                                  select triple.Third).FirstOrDefault();

            return description;
        }

        /// <summary>
        /// 根据给定枚举项名称（值）获取默认的附加信息字符串。
        /// </summary>
        /// <param name="enumType">枚举类型。</param>
        /// <param name="value">枚举项名称（值）。</param>
        /// <returns>附加信息字符串。</returns>
        public static string GetDescription(Type enumType, string value)
        {
            return GetDescription(enumType, value, String.Empty);
        }
        public static string GetDescription(Type enumType, int value)
        {
            return GetDescription(enumType, value.ToString(), String.Empty);
        }
        /// <summary>
        /// 根据给定枚举项及附加信息关键字，获取附加信息字符串。
        /// </summary>
        /// <param name="enumItem">枚举项。</param>
        /// <param name="key">附加信息关键字。</param>
        /// <returns>附加信息字符串。</returns>
        public static string GetDescription(System.Enum enumItem, string key)
        {
            return GetDescription(enumItem.GetType(), enumItem.ToString(), key);
        }

        /// <summary>
        /// 根据给定枚举项，获取默认的附加信息字符串。
        /// </summary>
        /// <param name="enumItem">枚举项。</param>
        /// <returns>附加信息字符串。</returns>
        public static string GetDescription(System.Enum enumItem)
        {
            return GetDescription(enumItem.GetType(), enumItem.ToString());
        }


        public static string GetValueByText(Type aa, string str)
        {
            if (string.IsNullOrEmpty(str))
            {
                return "";
            }
            else
            {
                return System.Enum.Format(aa, System.Enum.Parse(aa, str), "d");
            }


        }
        #endregion

        #region 根据指定的默认枚举项附加信息获取对应的枚举项

        /// <summary>
        /// 根据指定的默认枚举项附加信息获取对应的枚举项。
        /// </summary>
        /// <typeparam name="TEnum">枚举类型。</typeparam>
        /// <param name="description">附加信息字符串。</param>
        /// <returns>定义指定默认枚举项附加信息的枚举项。</returns>
        public static TEnum GetEnumItemByDescription<TEnum>(string description) where TEnum : struct
        {
            return GetEnumItemByDescription<TEnum>(String.Empty, description);
        }

        /// <summary>
        /// 根据指定的枚举项附加信息获取对应的枚举项。
        /// </summary>
        /// <typeparam name="TEnum">枚举类型。</typeparam>
        /// <param name="key">附加信息关键字。</param>
        /// <param name="description">附加信息字符串。</param>
        /// <returns>定义指定枚举项附加信息的枚举项。</returns>
        public static TEnum GetEnumItemByDescription<TEnum>(string key, string description) where TEnum : struct
        {
            return (TEnum)(object)GetEnumItemByDescription(typeof(TEnum), key, description);
        }

        /// <summary>
        /// 根据指定的枚举项附加信息获取对应的枚举项。
        /// </summary>
        /// <param name="enumType">枚举类型。</param>
        /// <param name="key">附加信息关键字。</param>
        /// <param name="description">附加信息字符串。</param>
        /// <returns>定义指定枚举项附加信息的枚举项。</returns>
        public static System.Enum GetEnumItemByDescription(Type enumType, string key, string description)
        {

            IEnumerable<Triple<String, Int32, String>> descriptions =
                GetDescriptions(enumType, key);

            Triple<String, Int32, String> tmpTriple = descriptions.FirstOrDefault(triple => triple.Third == description);

            return GetEnumItem(
                enumType, tmpTriple.Second.ToString(CultureInfo.InvariantCulture));
        }

        /// <summary>
        /// 根据给定枚举值获取枚举项。
        /// </summary>
        /// <param name="enumType">枚举类型。</param>
        /// <param name="value">枚举名称或枚举值。</param>
        /// <returns>枚举项。</returns>
        public static System.Enum GetEnumItem(Type enumType, string value)
        {

            return (System.Enum)System.Enum.Parse(enumType, value, true);
        }

        #endregion
    }
}
