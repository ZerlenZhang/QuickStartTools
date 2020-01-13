using System;

namespace ReadyGamerOne.Utility
{
    public class TypeUtil
    {
        /// <summary>
        /// 获取Type的特性
        /// </summary>
        /// <param name="type"></param>
        /// <param name="inherit"></param>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static T GetAttribute<T>(Type type, bool inherit = true)
            where T : System.Attribute
        {
            foreach (var attribute in type.GetCustomAttributes(inherit))
            {
                if (attribute is T)
                    return (T)attribute;
            }

            return null;
        }
    }
}