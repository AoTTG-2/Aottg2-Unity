using System;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;
using System.Linq;
using System.Collections;
using System.Globalization;

namespace Utility
{
    class BaseCSVObject
    {
        public virtual char Delimiter => ',';
        protected virtual char ParamDelimiter => ':';
        protected virtual char StructDelimiter => '/';
        protected virtual bool NamedParams => false;
        protected static Dictionary<Type, List<FieldInfo>> _fields = new Dictionary<Type, List<FieldInfo>>();

        public virtual string Serialize()
        {
            List<string> items = new List<string>();
            List<FieldInfo> fields = GetFields();
            for (int i = 0; i < fields.Count; i++)
            {
                string item = SerializeField(fields[i], this);
                items.Add(item);
            }
            return string.Join(Delimiter.ToString(), items.ToArray());
        }

        public virtual void Deserialize(string csv)
        {
            string[] items = csv.Split(Delimiter);
            List<FieldInfo> fields = GetFields();
            for (int i = 0; i < items.Length; i++)
                items[i] = items[i].Trim();
            if (NamedParams)
            {
                foreach (string item in items)
                {
                    string[] paramItems = item.Split(ParamDelimiter);
                    FieldInfo field = FindField(paramItems[0]);
                    if (field != null)
                        DeserializeField(field, this, paramItems[1]);
                }
            }
            else
            {
                for (int i = 0; i < fields.Count; i++)
                {
                    if (IsList(fields[i]))
                    {
                        Type t = fields[i].FieldType.GetGenericArguments()[0];
                        IList list = (IList)fields[i].GetValue(this);
                        list.Clear();
                        for (int j = i; j < items.Length; j++)
                        {
                            if (items[j] != string.Empty)
                                list.Add(DeserializeValue(t, items[j]));
                        }
                        
                        break;
                    }
                    else
                        DeserializeField(fields[i], this, items[i]);
                }
            }
        }

        public virtual void Copy(BaseCSVObject other)
        {
            Deserialize(other.Serialize());
        }

        protected virtual List<FieldInfo> GetFields()
        {
            Type type = GetType();
            if (!_fields.ContainsKey(type))
            {
                _fields.Add(type, type.GetFields().
                    OrderBy(t => Attribute.IsDefined(t, typeof(OrderAttribute)) ? ((OrderAttribute)t.GetCustomAttributes(typeof(OrderAttribute), true)[0]).Order: 0).
                    ToList());
            }
            return _fields[type];
        }

        protected virtual FieldInfo FindField(string name)
        {
            foreach (FieldInfo info in _fields[GetType()])
            {
                if (info.Name == name)
                    return info;
            }
            return null;
        }

        protected virtual bool IsList(FieldInfo field)
        {
            return field.FieldType.IsGenericType && field.FieldType.GetGenericTypeDefinition() == typeof(List<>);
        }

        protected virtual string SerializeField(FieldInfo info, object instance)
        {
            string str = string.Empty;
            if (NamedParams)
                str = info.Name + ParamDelimiter;
            if (IsList(info))
            {
                List<string> list = new List<string>();
                Type t = info.FieldType.GetGenericArguments()[0];
                foreach (object obj in (IList)info.GetValue(instance))
                    list.Add(SerializeValue(t, obj));
                if (list.Count > 0)
                    str += string.Join(Delimiter.ToString(), list.ToArray());
            }
            else
                str += SerializeValue(info.FieldType, info.GetValue(instance));
            return str;
        }

        protected virtual void DeserializeField(FieldInfo info, object instance, string value)
        {
            info.SetValue(instance, DeserializeValue(info.FieldType, value));
        }

        protected virtual string SerializeValue(Type t, object value)
        {
            if (t == typeof(string))
                return (string)value;
            else if (t == typeof(int) || t == typeof(float))
                return value.ToString();
            else if (t == typeof(bool))
                return Convert.ToInt32(value).ToString();
            else if (typeof(BaseCSVObject).IsAssignableFrom(t))
                return ((BaseCSVObject)value).Serialize();
            else if (t == typeof(Vector3))
            {
                Vector3 v = (Vector3)value;
                return string.Join(StructDelimiter.ToString(), new string[] { v.x.ToString(), v.y.ToString(), v.z.ToString() });
            }
            else if (t == typeof(Vector2))
            {
                Vector2 v = (Vector2)value;
                return string.Join(StructDelimiter.ToString(), new string[] { v.x.ToString(), v.y.ToString() });
            }
            else if (t == typeof(Color255))
            {
                Color255 color = (Color255)value;
                return string.Join(StructDelimiter.ToString(), new string[] { color.R.ToString(), color.G.ToString(), color.B.ToString(), color.A.ToString() });
            }
            return string.Empty;
        }

        protected virtual object DeserializeValue(Type t, string value)
        {
            if (t == typeof(string))
                return value;
            else if (t == typeof(int))
                return int.Parse(value);
            else if (t == typeof(float))
                return float.Parse(value);
            else if (t == typeof(bool))
                return Convert.ToBoolean(int.Parse(value));
            else if (typeof(BaseCSVObject).IsAssignableFrom(t))
            {
                BaseCSVObject item = (BaseCSVObject)Activator.CreateInstance(t);
                item.Deserialize(value);
                return item;
            }
            else if (t == typeof(Vector3))
            {
                string[] strArr = value.Split(StructDelimiter);
                Vector3 v = new Vector3(float.Parse(strArr[0]), 
                    float.Parse(strArr[1]), float.Parse(strArr[2]));
                return v;
            }
            else if (t == typeof(Vector2))
            {
                string[] strArr = value.Split(StructDelimiter);
                Vector2 v = new Vector2(float.Parse(strArr[0]), float.Parse(strArr[1]));
                return v;
            }
            else if (t == typeof(Color255))
            {
                string[] strArr = value.Split(StructDelimiter);
                Color255 color = new Color255(int.Parse(strArr[0]), int.Parse(strArr[1]), 
                    int.Parse(strArr[2]), int.Parse(strArr[3]));
                return color;
            }
            return null;
        }
    }
}
