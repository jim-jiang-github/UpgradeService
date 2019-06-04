using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace Share.Pipe
{
    public static class FastSerialize
    {
        #region TypeHelper
        private static readonly string[] ListTypeStrs = { "List`1", "HashSet`1", "IList`1", "ISet`1", "ICollection`1", "IEnumerable`1" };
        private static readonly string[] DicTypeStrs = { "Dictionary`2", "IDictionary`2" };
        private static readonly ConcurrentDictionary<string, TypeInfo> InstanceCache = new ConcurrentDictionary<string, TypeInfo>();
        /// <summary>
        /// 添加或获取实例
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        private static TypeInfo GetOrAddInstance(Type type, string methodName = "Add")
        {
            if (type.IsInterface)
            {
                throw new Exception("服务方法中不能包含接口内容！");
            }

            if (type.IsClass)
            {
                var fullName = type.FullName + methodName;

                TypeInfo typeInfo = InstanceCache.GetOrAdd(fullName, (v) =>
                {
                    Type[] argsTypes = null;

                    if (type.IsGenericType)
                    {
                        argsTypes = type.GetGenericArguments();
                        type = type.GetGenericTypeDefinition().MakeGenericType(argsTypes);
                    }

                    var mi = type.GetMethod(methodName);

                    return new TypeInfo()
                    {
                        Type = type,
                        MethodInfo = mi,
                        ArgTypes = argsTypes
                    };
                });
                typeInfo.Instance = Activator.CreateInstance(type);
                return typeInfo;
            }
            return null;
        }
        #endregion
        private static Type stringType = typeof(string);
        #region Serialize

        /// <summary>
        /// len+data
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        public static byte[] Serialize(object param)
        {
            List<byte> datas = new List<byte>();

            var len = 0;

            byte[] data = null;

            if (param == null)
            {
                len = 0;
            }
            else
            {
                if (param is string)
                {
                    data = Encoding.UTF8.GetBytes((string)param);
                }
                else if (param is byte)
                {
                    data = new byte[] { (byte)param };
                }
                else if (param is bool)
                {
                    data = BitConverter.GetBytes((bool)param);
                }
                else if (param is short)
                {
                    data = BitConverter.GetBytes((short)param);
                }
                else if (param is int)
                {
                    data = BitConverter.GetBytes((int)param);
                }
                else if (param is long)
                {
                    data = BitConverter.GetBytes((long)param);
                }
                else if (param is float)
                {
                    data = BitConverter.GetBytes((float)param);
                }
                else if (param is double)
                {
                    data = BitConverter.GetBytes((double)param);
                }
                else if (param is DateTime)
                {
                    var str = "wl" + ((DateTime)param).Ticks;
                    data = Encoding.UTF8.GetBytes(str);
                }
                else if (param is Enum)
                {
                    var enumValType = Enum.GetUnderlyingType(param.GetType());

                    if (enumValType == typeof(byte))
                    {
                        data = new byte[] { (byte)param };
                    }
                    else if (enumValType == typeof(short))
                    {
                        data = BitConverter.GetBytes((Int16)param);
                    }
                    else if (enumValType == typeof(int))
                    {
                        data = BitConverter.GetBytes((Int32)param);
                    }
                    else
                    {
                        data = BitConverter.GetBytes((Int64)param);
                    }
                }
                else if (param is byte[])
                {
                    data = (byte[])param;
                }
                else
                {
                    var type = param.GetType();

                    if (type.IsGenericType || type.IsArray)
                    {
                        if (DicTypeStrs.Contains(type.Name))
                            data = SerializeDic((System.Collections.IDictionary)param);
                        else if (ListTypeStrs.Contains(type.Name) || type.IsArray)
                            data = SerializeList((System.Collections.IEnumerable)param);
                        else
                            data = SerializeClass(param, type);
                    }
                    else if (type.IsClass)
                    {
                        data = SerializeClass(param, type);
                    }

                }
                if (data != null)
                    len = data.Length;
            }
            datas.AddRange(BitConverter.GetBytes(len));
            if (len > 0)
            {
                datas.AddRange(data);
            }
            return datas.ToArray();
        }

        private static byte[] SerializeClass(object obj, Type type)
        {
            if (obj == null) return null;

            List<byte> datas = new List<byte>();

            var len = 0;

            byte[] data = null;

            var ps = type.GetProperties();

            if (ps != null && ps.Length > 0)
            {
                List<object> clist = new List<object>();

                foreach (var p in ps)
                {
                    clist.Add(p.GetValue(obj, null));
                    //clist.Add(FastInvoke.Getter(type, obj, p));
                }
                data = Serialize(clist.ToArray());

                len = data.Length;
            }
            if (len > 0)
            {
                return data;
            }
            return null;
        }

        private static byte[] SerializeList(System.Collections.IEnumerable param)
        {
            if (param != null)
            {
                List<byte> slist = new List<byte>();

                var itemtype = param.AsQueryable().ElementType;

                foreach (var item in param)
                {
                    if (itemtype.IsClass && itemtype != stringType)
                    {
                        var ps = itemtype.GetProperties();

                        if (ps != null && ps.Length > 0)
                        {
                            List<object> clist = new List<object>();
                            foreach (var p in ps)
                            {
                                clist.Add(p.GetValue(item, null));
                            }
                            var clen = 0;
                            var cdata = Serialize(clist.ToArray());
                            if (cdata != null)
                            {
                                clen = cdata.Length;
                            }
                            slist.AddRange(BitConverter.GetBytes(clen));
                            slist.AddRange(cdata);
                        }
                    }
                    else
                    {
                        var clen = 0;
                        var cdata = Serialize(item);
                        if (cdata != null)
                        {
                            clen = cdata.Length;
                        }
                        slist.AddRange(BitConverter.GetBytes(clen));
                        slist.AddRange(cdata);
                    }
                }
                if (slist.Count > 0)
                {
                    return slist.ToArray();
                }
            }
            return null;
        }

        private static byte[] SerializeDic(System.Collections.IDictionary param)
        {
            if (param != null && param.Count > 0)
            {
                List<byte> list = new List<byte>();
                foreach (var item in param)
                {
                    var type = item.GetType();
                    var ps = type.GetProperties();
                    if (ps != null && ps.Length > 0)
                    {
                        List<object> clist = new List<object>();
                        foreach (var p in ps)
                        {
                            clist.Add(p.GetValue(item, null));
                        }
                        var clen = 0;

                        var cdata = Serialize(clist.ToArray());

                        if (cdata != null)
                        {
                            clen = cdata.Length;
                        }

                        if (clen > 0)
                        {
                            list.AddRange(cdata);
                        }
                    }
                }
                return list.ToArray();
            }
            return null;
        }

        /// <summary>
        /// len+data
        /// </summary>
        /// <param name="params"></param>
        /// <returns></returns>
        public static byte[] Serialize(params object[] @params)
        {
            List<byte> datas = new List<byte>();

            if (@params != null)
            {
                foreach (var param in @params)
                {
                    datas.AddRange(Serialize(param));
                }
            }

            return datas.Count == 0 ? null : datas.ToArray();
        }

        #endregion

        #region Deserialize

        /// <summary>
        /// 反序列化
        /// </summary>
        /// <param name="types"></param>
        /// <param name="datas"></param>
        /// <returns></returns>
        public static object[] Deserialize(Type[] types, byte[] datas)
        {
            List<object> list = new List<object>();

            int offset = 0;

            for (int i = 0; i < types.Length; i++)
            {
                list.Add(Deserialize(types[i], datas, ref offset));
            }
            return list.ToArray();
        }

        /// <summary>
        /// 反序列化
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="data"></param>
        /// <returns></returns>
        public static T Deserialize<T>(byte[] data)
        {
            int offset = 0;
            return (T)Deserialize(typeof(T), data, ref offset);
        }

        /// <summary>
        /// 反序列化
        /// </summary>
        /// <param name="type"></param>
        /// <param name="datas"></param>
        /// <param name="offset"></param>
        /// <returns></returns>
        private static object Deserialize(Type type, byte[] datas, ref int offset)
        {
            dynamic obj = null;

            var len = 0;

            byte[] data = null;

            len = BitConverter.ToInt32(datas, offset);
            offset += 4;
            if (len > 0)
            {
                data = new byte[len];
                Buffer.BlockCopy(datas, offset, data, 0, len);
                offset += len;

                if (type == stringType)
                {
                    obj = Encoding.UTF8.GetString(data);
                }
                else if (type == typeof(byte) || type == typeof(byte?))
                {
                    obj = (data[0]);
                }
                else if (type == typeof(bool) || type == typeof(bool?))
                {
                    obj = (BitConverter.ToBoolean(data, 0));
                }
                else if (type == typeof(short) || type == typeof(short?))
                {
                    obj = (BitConverter.ToInt16(data, 0));
                }
                else if (type == typeof(int) || type == typeof(int?))
                {
                    obj = (BitConverter.ToInt32(data, 0));
                }
                else if (type == typeof(long) || type == typeof(long?))
                {
                    obj = (BitConverter.ToInt64(data, 0));
                }
                else if (type == typeof(float) || type == typeof(float?))
                {
                    obj = (BitConverter.ToSingle(data, 0));
                }
                else if (type == typeof(double) || type == typeof(double?))
                {
                    obj = (BitConverter.ToDouble(data, 0));
                }
                else if (type == typeof(decimal) || type == typeof(decimal?))
                {
                    obj = (BitConverter.ToDouble(data, 0));
                }
                else if (type == typeof(DateTime) || type == typeof(DateTime?))
                {
                    var dstr = Encoding.UTF8.GetString(data);
                    var ticks = long.Parse(dstr.Substring(2));
                    obj = (new DateTime(ticks));
                }
                else if (type.BaseType == typeof(Enum))
                {
                    var numType = Enum.GetUnderlyingType(type);

                    if (numType == typeof(byte) || type == typeof(byte?))
                    {
                        obj = Enum.ToObject(type, data[0]);
                    }
                    else if (numType == typeof(short) || type == typeof(short?))
                    {
                        obj = Enum.ToObject(type, BitConverter.ToInt16(data, 0));
                    }
                    else if (numType == typeof(int) || type == typeof(int?))
                    {
                        obj = Enum.ToObject(type, BitConverter.ToInt32(data, 0));
                    }
                    else
                    {
                        obj = Enum.ToObject(type, BitConverter.ToInt64(data, 0));
                    }
                }
                else if (type == typeof(byte[]))
                {
                    obj = (byte[])data;
                }
                else if (type.IsGenericType)
                {
                    if (ListTypeStrs.Contains(type.Name))
                    {
                        obj = DeserializeList(type, data);
                    }
                    else if (DicTypeStrs.Contains(type.Name))
                    {
                        obj = DeserializeDic(type, data);
                    }
                    else
                    {
                        obj = DeserializeClass(type, data);
                    }
                }
                else if (type.IsClass)
                {
                    obj = DeserializeClass(type, data);
                }
                else if (type.IsArray)
                {
                    obj = DeserializeArray(type, data);
                }
                else
                {
                    throw new Exception("SAEASerialize.Deserialize 未定义的类型：" + type.ToString());
                }

            }
            return obj;
        }

        private static object DeserializeClass(Type type, byte[] datas)
        {
            var tinfo = GetOrAddInstance(type);

            var instance = tinfo.Instance;

            var ts = new List<Type>();

            var ps = type.GetProperties();

            foreach (var p in ps)
            {
                ts.Add(p.PropertyType);
            }
            var vas = Deserialize(ts.ToArray(), datas);
            for (int j = 0; j < ps.Length; j++)
            {
                if (!ps[j].CanWrite) { continue; }
                try
                {
                    if (!ps[j].PropertyType.IsGenericType)
                    {
                        ps[j].SetValue(instance, vas[j], null);
                    }
                    else
                    {
                        Type genericTypeDefinition = ps[j].PropertyType.GetGenericTypeDefinition();
                        if (genericTypeDefinition == typeof(Nullable<>))
                        {
                            try
                            {
                                ps[j].SetValue(instance, Convert.ChangeType(vas[j], Nullable.GetUnderlyingType(ps[j].PropertyType)), null);
                            }
                            catch
                            {
                                ps[j].SetValue(instance, vas[j], null);
                            }
                        }
                        else
                        {
                            ps[j].SetValue(instance, vas[j], null);
                        }
                    }
                }
                catch (Exception ex)
                {
                    throw new Exception("SAEASerialize.Deserialize 未定义的类型：" + type.ToString());
                }
            }

            return instance;
        }

        private static object DeserializeList(Type type, byte[] datas)
        {
            var info = GetOrAddInstance(type);

            var instance = info.Instance;

            if (info.ArgTypes[0].IsClass && info.ArgTypes[0] != stringType)
            {
                //子项内容
                var slen = 0;
                var soffset = 0;
                while (soffset < datas.Length)
                {
                    slen = BitConverter.ToInt32(datas, soffset);
                    if (slen > 0)
                    {
                        var sobj = Deserialize(info.ArgTypes[0], datas, ref soffset);
                        if (sobj != null)
                            info.MethodInfo.Invoke(instance, new object[] { sobj });

                    }
                    else
                    {
                        info.MethodInfo.Invoke(instance, null);
                    }
                }
                return instance;
            }
            else
            {
                //子项内容
                var slen = 0;
                var soffset = 0;
                while (soffset < datas.Length)
                {
                    var len = BitConverter.ToInt32(datas, soffset);
                    var data = new byte[len];
                    Buffer.BlockCopy(datas, soffset + 4, data, 0, len);
                    soffset += 4;
                    slen = BitConverter.ToInt32(datas, soffset);
                    if (slen > 0)
                    {
                        var sobj = Deserialize(info.ArgTypes[0], datas, ref soffset);
                        if (sobj != null)
                            info.MethodInfo.Invoke(instance, new object[] { sobj });
                    }
                    else
                    {
                        info.MethodInfo.Invoke(instance, null);
                    }
                }
                return instance;
            }

        }

        private static object DeserializeArray(Type type, byte[] datas)
        {
            var obj = DeserializeList(type, datas);

            if (obj == null) return null;

            var list = (obj as List<object>);

            return list.ToArray();
        }

        private static object DeserializeDic(Type type, byte[] datas)
        {
            var tinfo = GetOrAddInstance(type);

            var instance = tinfo.Instance;

            //子项内容
            var slen = 0;

            var soffset = 0;

            int m = 1;

            object key = null;
            object val = null;

            while (soffset < datas.Length)
            {
                slen = BitConverter.ToInt32(datas, soffset);
                var sdata = new byte[slen + 4];
                Buffer.BlockCopy(datas, soffset, sdata, 0, slen + 4);
                soffset += slen + 4;
                if (m % 2 == 1)
                {
                    object v = null;
                    if (slen > 0)
                    {
                        int lloffset = 0;
                        var sobj = Deserialize(tinfo.ArgTypes[0], sdata, ref lloffset);
                        if (sobj != null)
                            v = sobj;
                    }
                    key = v;
                }
                else
                {
                    object v = null;
                    if (slen > 0)
                    {
                        int lloffset = 0;
                        var sobj = Deserialize(tinfo.ArgTypes[1], sdata, ref lloffset);
                        if (sobj != null)
                            v = sobj;
                    }
                    val = v;
                    tinfo.MethodInfo.Invoke(instance, new object[] { key, val });
                }
                m++;
            }
            return instance;
        }

        #endregion

        private class TypeInfo
        {
            public Type Type { get; set; }

            public Object Instance { get; set; }

            public Type[] ArgTypes { get; set; }

            public MethodInfo MethodInfo { get; set; }
        }
    }
}
