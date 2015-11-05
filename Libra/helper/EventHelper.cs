using System;
using System.Reflection;

namespace Libra.helper
{
    public class EventHelper
    {
        /// <summary>
        /// 移除所有事件监听
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="c"></param>
        /// <param name="name"></param>
        public static void RemoveEvent<T>(T c, string name)
        {
            Delegate[] invokeList = GetObjectEventList(c, name);
            if (invokeList != null)
            {
                var eventInfo = typeof(T).GetEvent(name);
                foreach (Delegate del in invokeList)
                {
                    eventInfo.RemoveEventHandler(c, del);
                }
            }
        }

        ///  <summary>     
        /// 获取对象事件 zgke@sina.com qq:116149     
        ///  </summary>     
        ///  <param name="p_Object">对象 </param>     
        ///  <param name="p_EventName">事件名 </param>     
        ///  <returns>委托列 </returns>     
        private static Delegate[] GetObjectEventList(object p_Object, string p_EventName)
        {
            FieldInfo _Field = p_Object.GetType().GetField(p_EventName, BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Public | BindingFlags.Static);
            if (_Field != null)
            {
                object _FieldValue = _Field.GetValue(p_Object);
                if (_FieldValue != null && _FieldValue is Delegate)
                {
                    Delegate _ObjectDelegate = (Delegate)_FieldValue;
                    return _ObjectDelegate.GetInvocationList();
                }
            }
            return null;
        }
    }
}
