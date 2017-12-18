using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using ChinaBeacon.Sdk.Core.Data;
using ChinaBeacon.Sdk.Core.Enum;

namespace ChinaBeacon.Sdk.Core.Tools
{
    public static class BasicDataConversion
    {
        public static string BirthdayToAge(this DateTime? birthday)
        {
            if (!birthday.HasValue)
            {
                return string.Empty;
            }
            var age = DateTime.Now.Year - birthday.Value.Year;
            if (DateTime.Now.Month < birthday.Value.Month || (DateTime.Now.Month == birthday.Value.Month && DateTime.Now.Day < birthday.Value.Day))
            {
                age--;
            }
            return age.ToString();
        }

        /// <summary>
        /// DataTime2Long
        /// </summary>
        /// <param name="dt"></param>
        /// <returns></returns>
        public static long ConvertDataTimeLong(this DateTime dt)
        {
            DateTime dtStart = TimeZone.CurrentTimeZone.ToLocalTime(new DateTime(1970, 1, 1));
            TimeSpan toNow = dt.Subtract(dtStart);
            long timeStamp = toNow.Ticks;
            timeStamp = long.Parse(timeStamp.ToString().Substring(0, timeStamp.ToString().Length - 4));
            timeStamp = timeStamp > 0 ? timeStamp : 0;
            return timeStamp;
        }
        /// <summary>
        /// DataTime2Long
        /// </summary>
        /// <param name="dt"></param>
        /// <returns></returns>
        public static long ConvertDataTimeLong(this DateTime? dt)
        {
            if (!dt.HasValue)
            {
                return 0;
            }

            return ConvertDataTimeLong(dt.Value);
        }

        /// <summary>
        /// Long2DataTime
        /// </summary>
        /// <param name="d"></param>
        /// <returns></returns>
        public static DateTime ConvertLongDateTime(this long d)
        {
            DateTime dtStart = TimeZone.CurrentTimeZone.ToLocalTime(new DateTime(1970, 1, 1));
            long lTime = long.Parse(d + "0000");
            TimeSpan toNow = new TimeSpan(lTime);
            DateTime dtResult = dtStart.Add(toNow);
            return dtResult;
        }

        /// <summary>
        /// Long2NullableDataTime
        /// </summary>
        /// <param name="d"></param>
        /// <returns></returns>
        public static DateTime? ConvertLongToNullableDateTime(this long d)
        {
            if (d <= 0) return null;
            DateTime dtStart = TimeZone.CurrentTimeZone.ToLocalTime(new DateTime(1970, 1, 1));
            long lTime = long.Parse(d + "0000");
            TimeSpan toNow = new TimeSpan(lTime);
            DateTime dtResult = dtStart.Add(toNow);
            return dtResult;
        }

        /// <summary>
        /// 转换为年月的日期字符串 例:2017-03
        /// </summary>
        /// <param name="dt">日期时间</param>
        /// <returns></returns>
        public static string ToDateYearAndMonthString(this DateTime dt)
        {
            return dt.ToString("yyyy-MM");
        }
        /// <summary>
        /// 转换为年月的日期字符串 例:2017-03
        /// </summary>
        /// <param name="dt">日期时间</param>
        /// <returns></returns>
        public static string ToDateYearAndMonthString(this DateTime? dt)
        {
            return dt.HasValue ? dt.Value.ToDateYearAndMonthString() : string.Empty;
        }
        /// <summary>
        /// 转换为日期字符串 例:2017-03-03
        /// </summary>
        /// <param name="dt">日期时间</param>
        /// <returns></returns>
        public static string ToDateString(this DateTime dt)
        {
            return dt.ToString("yyyy-MM-dd");
        }
        /// <summary>
        /// 转换为日期字符串 例:2017-03-03
        /// </summary>
        /// <param name="dt">日期时间</param>
        /// <returns></returns>
        public static string ToDateString(this DateTime? dt)
        {
            return dt.HasValue ? dt.Value.ToDateString() : string.Empty;
        }

        public static DateTime GetFirstDayOfCurrentYear()
        {
            return Convert.ToDateTime(DateTime.Now.Year + "-01-01");
        }

        /// <summary>
        /// bool?类型的参数，手机端方便传递int值，接口需要转化成bool?。对应值为0：null,全选；1为true;2为false;
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static bool? IntToNullableBool(this int value)
        {
            if (value == 1) return true;
            if (value == 2) return false;
            if (value == 0) return null;
            throw new ArgumentException($"{value}不能转换成nullableBool值");
        }

        /// <summary>
        /// bool型转成int 0：null,全选；1为true;2为false;
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static int BoolToInt(this bool? value)
        {
            if (value == true) return 1;
            if (value == false) return 2;
            if (!value.HasValue) return 0;
            throw new ArgumentException($"{value}不能转换成int值");
        }

        /// <summary>
        /// pc端查询页面传回来的为string，需要转成datetime?,
        /// </summary>
        /// <param name="dateTimeString"></param>
        /// <returns></returns>
        public static DateTime? StringToNullableDateTime(this string dateTimeString)
        {
            if (string.IsNullOrEmpty(dateTimeString)) return null;
            return StringToDateTime(dateTimeString);
        }

        /// <summary>
        /// pc端查询页面传回来的为string，需要转成datetime
        /// </summary>
        /// <param name="dateTimeString"></param>
        /// <returns></returns>
        public static DateTime StringToDateTime(this string dateTimeString)
        {
            if (string.IsNullOrEmpty(dateTimeString)) throw new ArgumentException($"{dateTimeString}参数不能为空");
            return Convert.ToDateTime(dateTimeString);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="datetime"></param>
        /// <returns></returns>
        public static string NullableDateTimeToShortDateString(this DateTime? datetime)
        {
            if (datetime == null) return string.Empty;
            return Convert.ToDateTime(datetime).ToShortDateString();
        }

        #region 货币数字与大写金额的转换

        public static string CmycurD(this string num)
        {
            decimal dRebate2;
            if (decimal.TryParse(num, out dRebate2))
            {
                return dRebate2.CmycurD();
            }
            return string.Empty;
        }

        /// <summary>
        /// 转换人民币大小金额
        /// </summary>
        /// <param name="num">金额</param>
        /// <returns>返回大写形式</returns>
        public static string CmycurD(this decimal num)
        {
            string str1 = "零壹贰叁肆伍陆柒捌玖";            //0-9所对应的汉字
            string str2 = "万仟佰拾亿仟佰拾万仟佰拾元角分"; //数字位所对应的汉字
            string str3 = "";    //从原num值中取出的值
            string str4 = "";    //数字的字符串形式
            string str5 = "";  //人民币大写金额形式
            int i;    //循环变量
            int j;    //num的值乘以100的字符串长度
            string ch1 = "";    //数字的汉语读法
            string ch2 = "";    //数字位的汉字读法
            int nzero = 0;  //用来计算连续的零值是几个
            int temp;            //从原num值中取出的值

            num = Math.Round(Math.Abs(num), 2);    //将num取绝对值并四舍五入取2位小数
            str4 = ((long)(num * 100)).ToString();        //将num乘100并转换成字符串形式
            j = str4.Length;      //找出最高位
            if (j > 15) { return "溢出"; }
            str2 = str2.Substring(15 - j);   //取出对应位数的str2的值。如：200.55,j为5所以str2=佰拾元角分

            //循环取出每一位需要转换的值
            for (i = 0; i < j; i++)
            {
                str3 = str4.Substring(i, 1);          //取出需转换的某一位的值
                temp = Convert.ToInt32(str3);      //转换为数字
                if (i != (j - 3) && i != (j - 7) && i != (j - 11) && i != (j - 15))
                {
                    //当所取位数不为元、万、亿、万亿上的数字时
                    if (str3 == "0")
                    {
                        ch1 = "";
                        ch2 = "";
                        nzero = nzero + 1;
                    }
                    else
                    {
                        if (str3 != "0" && nzero != 0)
                        {
                            ch1 = "零" + str1.Substring(temp * 1, 1);
                            ch2 = str2.Substring(i, 1);
                            nzero = 0;
                        }
                        else
                        {
                            ch1 = str1.Substring(temp * 1, 1);
                            ch2 = str2.Substring(i, 1);
                            nzero = 0;
                        }
                    }
                }
                else
                {
                    //该位是万亿，亿，万，元位等关键位
                    if (str3 != "0" && nzero != 0)
                    {
                        ch1 = "零" + str1.Substring(temp * 1, 1);
                        ch2 = str2.Substring(i, 1);
                        nzero = 0;
                    }
                    else
                    {
                        if (str3 != "0" && nzero == 0)
                        {
                            ch1 = str1.Substring(temp * 1, 1);
                            ch2 = str2.Substring(i, 1);
                            nzero = 0;
                        }
                        else
                        {
                            if (str3 == "0" && nzero >= 3)
                            {
                                ch1 = "";
                                ch2 = "";
                                nzero = nzero + 1;
                            }
                            else
                            {
                                if (j >= 11)
                                {
                                    ch1 = "";
                                    nzero = nzero + 1;
                                }
                                else
                                {
                                    ch1 = "";
                                    ch2 = str2.Substring(i, 1);
                                    nzero = nzero + 1;
                                }
                            }
                        }
                    }
                }
                if (i == (j - 11) || i == (j - 3))
                {
                    //如果该位是亿位或元位，则必须写上
                    ch2 = str2.Substring(i, 1);
                }
                str5 = str5 + ch1 + ch2;

                //                if (i == j - 1 && str3 == "0")
                //                {
                //                    //最后一位（分）为0时，加上“整”
                //                    str5 = str5 + '整';
                //                }
            }
            if (num == 0)
            {
                str5 = "零";
            }
            return str5;
        }


        public static int ChangeNum(this string str)
        {
            var num = 0;
            if (str.Equals("一"))
            {
                return 1;
            }
            else if (str.Equals("二"))
            {
                return 2;
            }
            else if (str.Equals("三"))
            {
                return 3;
            }
            else if (str.Equals("四"))
            {
                return 4;
            }
            else if (str.Equals("五"))
            {
                return 5;
            }
            else if (str.Equals("六"))
            {
                return 6;
            }
            else if (str.Equals("七"))
            {
                return 7;
            }
            else if (str.Equals("八"))
            {
                return 8;
            }
            else if (str.Equals("九"))
            {
                return 9;
            }
            else if (str.Equals("十"))
            {
                return 10;
            }
            else if (str.Equals("十一"))
            {
                return 11;
            }
            else if (str.Equals("十二"))
            {
                return 12;
            }
            return num;
        }

        #endregion

        #region string数组、int数组，guid数组与用特殊符号连接的字符串之前的转换
        /// <summary>
        /// 将string数组连接成字符串
        /// </summary>
        /// <param name="codes"></param>
        /// <param name="separator">连接符号</param>
        /// <returns>"1,2,3"</returns>
        public static string JoinWithSeparator(this List<string> codes, char separator = ',')
        {
            return string.Join(separator.ToString(), codes);
        }

        /// <summary>
        /// 将Guid数组连接成字符串
        /// </summary>
        /// <param name="codes">[1,2,3]</param>
        /// <param name="separator">连接符号</param>
        /// <returns>"1,2,3"</returns>
        public static string JoinGuidList2String(this List<Guid> codes, char separator = ',')
        {
            return string.Join(separator.ToString(), codes);
        }

        /// <summary>
        /// 将字符串拆成数组
        /// </summary>
        /// <param name="codesString">"1,2,3"</param>
        /// <param name="separator">连接符号</param>
        /// <returns>[1,2,3]</returns>
        public static List<Guid> SplitCodesString2GuidList(this string codesString, char separator = ',')
        {
            var codeStrings = codesString.Split(separator).ToList();
            var list = new List<Guid>();
            foreach (var codeString in codeStrings)
            {
                Guid codeGuid;
                if (Guid.TryParse(codeString, out codeGuid))
                {
                    list.Add(codeGuid);
                }
                else
                {
                    LogHelper.Write("", LogMessageEnum.Error);
                }
            }
            return list;
        }

        /// <summary>
        /// 将数组连接成字符串
        /// </summary>
        /// <param name="codes">[1,2,3]</param>
        /// <param name="separator">连接符号</param>
        /// <returns>"1,2,3"</returns>
        public static string JoinCodeList2CodesString(this List<int> codes, char separator = ',')
        {
            return string.Join(separator.ToString(), codes);
        }

        /// <summary>
        /// 将字符串拆成数组
        /// </summary>
        /// <param name="codesString">"1,2,3"</param>
        /// <param name="separator">连接符号</param>
        /// <returns>[1,2,3]</returns>
        public static List<int> SplitCodesString2List(this string codesString, char separator = ',')
        {
            var codeStrings = codesString.Split(separator).ToList();
            var intList = new List<int>();
            foreach (var codeString in codeStrings)
            {
                int codeInt;
                if (int.TryParse(codeString, out codeInt))
                {
                    intList.Add(codeInt);
                }
                else
                {
                    LogHelper.Write("多选拼成的字符串混入了非数字", LogMessageEnum.Error);
                }
            }
            return intList;
        }
        #endregion

        #region  string 与guid的转换
        public static Guid ToGuid(this string guidString)
        {
            if (string.IsNullOrEmpty(guidString)) return Guid.Empty;
            Guid codeGuid;
            if (Guid.TryParse(guidString, out codeGuid))
            {
                return codeGuid;
            }
            else
            {
                throw new Exception($"{guidString}不是合格的guid字符串");
            }
        }
        #endregion
    }
}
