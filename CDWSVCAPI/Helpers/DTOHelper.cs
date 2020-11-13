using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace CDWSVCAPI.Helpers
{
    public class DtoHelper<T> where T : new()
    {
        private static readonly Dictionary<Type, Dictionary<string, PropertyInfo>> _memberReflectionCache = new Dictionary<Type, Dictionary<string, PropertyInfo>>();

        private static readonly Dictionary<Type, FieldInfo[]> _fieldReflectionCache = new Dictionary<Type, FieldInfo[]>();

        private static readonly Dictionary<Type, PropertyInfo[]> _propertyReflectionCache = new Dictionary<Type, PropertyInfo[]>();

        /// <summary>
        /// Use a reflection based mapping to map between properties and fields
        /// </summary>
        public static T Map(object source)
        {
            if (source == null)
            {
                return default(T);
            }

            var fields = GetFieldsetInfo(typeof(T));
            var props = GetPropertysetInfo(typeof(T));
            var target = new T();

            foreach (MemberInfo field in fields.Union<MemberInfo>(props))
            {
                try
                {
                    MemberInfo sourceProperty = GetMemberInfo(source.GetType(), field.Name);
                    if (sourceProperty == null) continue;
                    var info = sourceProperty as FieldInfo;
                    if (info != null && field is FieldInfo) ((FieldInfo)field).SetValue(target, info.GetValue(source));
                    else ((PropertyInfo)field).SetValue(target, ((PropertyInfo)sourceProperty).GetValue(source));
                }
                catch (InvalidCastException)
                {
                }
                catch (TargetParameterCountException)
                {
                    return target;
                }
            }

            return target;
        }

        // <summary>
        /// Use a reflection based mapping to map between properties and fields
        /// </summary>
        public static T Map(object source, T target)
        {
            if (source == null)
            {
                return default(T);
            }

            var fields = GetFieldsetInfo(typeof(T));
            var props = GetPropertysetInfo(typeof(T));

            foreach (MemberInfo field in fields.Union<MemberInfo>(props))
            {
                try
                {
                    MemberInfo sourceProperty = GetMemberInfo(source.GetType(), field.Name);
                    if (sourceProperty == null) continue;
                    var info = sourceProperty as FieldInfo;
                    if (info != null && field is FieldInfo) ((FieldInfo)field).SetValue(target, info.GetValue(source));
                    else ((PropertyInfo)field).SetValue(target, ((PropertyInfo)sourceProperty).GetValue(source));
                }
                catch (InvalidCastException)
                {
                }
                catch (TargetParameterCountException)
                {
                    return target;
                }
            }

            return target;
        }

        public static T MapExpando(object source)
        {
            if (source == null)
                return default(T);

            dynamic target = Map(source); // reg map to start

            dynamic src = source;

            var sourceFields = src as IEnumerable<KeyValuePair<string, object>>;

            sourceFields = sourceFields ?? src.GetProperties(false);
            if (sourceFields == null) return target; // source not dynamic at all so just reg map.

            foreach (var field in sourceFields)
            {
                MemberInfo targetProperty = GetMemberInfo(target.GetType(), field.Key);
                if (targetProperty == null) // expando/dynamic map
                    target[field.Key] = field.Value;
                else if (field.Value != null) // static field map
                {
                    var info = targetProperty as FieldInfo;
                    if (info != null) info.SetValue(target, Convert.ChangeType(field.Value, info.FieldType));
                    else
                    {
                        var propertyInfo = (PropertyInfo)targetProperty;
                        if (propertyInfo.PropertyType.IsEnum)
                            propertyInfo.SetValue(target, Enum.Parse(propertyInfo.PropertyType, field.Value.ToString()));
                        else
                            propertyInfo.SetValue(target, Convert.ChangeType(field.Value, propertyInfo.PropertyType));
                    }
                }
            }
            return target;
        }

        public static T MapExpando(object source, T target)
        {
            if (source == null)
                return default(T);

            dynamic tgt = Map(source, target); // reg map to start

            dynamic src = source;

            var sourceFields = src as IEnumerable<KeyValuePair<string, object>>;

            sourceFields = sourceFields ?? src.GetProperties(false);
            if (sourceFields == null) return tgt; // source not dynamic at all so just reg map.

            foreach (var field in sourceFields)
            {
                MemberInfo targetProperty = GetMemberInfo(tgt.GetType(), field.Key);
                if (targetProperty == null) // expando/dynamic map
                    tgt[field.Key] = field.Value;
                else if (field.Value != null) // static field map
                {
                    var info = targetProperty as FieldInfo;
                    if (info != null) info.SetValue(tgt, Convert.ChangeType(field.Value, info.FieldType));
                    else
                    {
                        var propertyInfo = (PropertyInfo)targetProperty;
                        if (propertyInfo.PropertyType.IsEnum)
                            propertyInfo.SetValue(tgt, Enum.Parse(propertyInfo.PropertyType, field.Value.ToString()));
                        else
                            propertyInfo.SetValue(tgt, Convert.ChangeType(field.Value, propertyInfo.PropertyType));
                    }
                }
            }
            return tgt;
        }

        /// <summary>
        /// Finds a property in the reflection cache
        /// </summary>
        private static MemberInfo GetMemberInfo(Type t, string name)
        {
            if (_memberReflectionCache.ContainsKey(t))
            {
                if (_memberReflectionCache[t].ContainsKey(name))
                {
                    return _memberReflectionCache[t][name];
                }

                _memberReflectionCache[t].Add(name, t.GetProperty(name, BindingFlags.Public | BindingFlags.Instance));
                return _memberReflectionCache[t][name];
            }

            Dictionary<string, PropertyInfo> dictionary = new Dictionary<string, PropertyInfo>();
            _memberReflectionCache.Add(t, dictionary);
            dictionary.Add(name, t.GetProperty(name, BindingFlags.Public | BindingFlags.Instance));

            return _memberReflectionCache[t][name];
        }

        /// <summary>
        /// Finds a collection of field information from the reflection cache
        /// </summary>
        private static FieldInfo[] GetFieldsetInfo(Type t)
        {
            if (_fieldReflectionCache.ContainsKey(t))
            {
                return _fieldReflectionCache[t];
            }

            var fields = t.GetFields(BindingFlags.Public | BindingFlags.Instance);

            _fieldReflectionCache.Add(t, fields.ToArray());

            return _fieldReflectionCache[t];
        }

        private static PropertyInfo[] GetPropertysetInfo(Type t)
        {
            if (_propertyReflectionCache.ContainsKey(t))
            {
                return _propertyReflectionCache[t];
            }

            var props = t.GetProperties(BindingFlags.Public | BindingFlags.Instance);

            _propertyReflectionCache.Add(t, props.ToArray());

            return _propertyReflectionCache[t];
        }
    }
}