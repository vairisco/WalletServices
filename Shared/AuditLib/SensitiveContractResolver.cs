using System.Reflection;

namespace AuditLib
{
    public class SensitiveContractResolver
    {
        private readonly string[] props;

        public SensitiveContractResolver(params string[] prop)
        {
            props = prop;
        }
    }

    public class ReplaveValueProvider
    {
        readonly PropertyInfo _MemberInfo;
        readonly string[] _prop_rep;
        public ReplaveValueProvider(PropertyInfo memberInfo, string[] prop_rep)
        {
            _MemberInfo = memberInfo;
            _prop_rep = prop_rep;
        }
        //public object GetValue(object target)
        //{
        //    object result = _MemberInfo.GetValue(target);

        //    if (result != null && _MemberInfo.PropertyType == typeof(string))
        //    {
        //        var valueText = result.ToString();
        //        if (!string.IsNullOrEmpty(valueText) && valueText.Contains("{") && valueText.Contains("}"))
        //        {
        //            if (TryParseJSON(result.ToString(), out JObject jObj))
        //            {
        //                foreach (var p in _prop_rep)
        //                {
        //                    if (jObj.ContainsKey(p))
        //                        jObj[p] = "***";
        //                }
        //                result = jObj.ToString(Formatting.None);
        //                return result;
        //            }
        //        }
        //        if (_prop_rep.Contains(_MemberInfo.Name))
        //            result = "***";
        //    }
        //    else if (_MemberInfo.PropertyType == typeof(int) || _MemberInfo.PropertyType == typeof(long)) result = 0;
        //    return result;
        //}
        //private static bool TryParseJSON(string json, out JObject jObject)
        //{
        //    try
        //    {
        //        jObject = JObject.Parse(json);
        //        return true;
        //    }
        //    catch
        //    {
        //        jObject = null;
        //        return false;
        //    }
        //}
        public void SetValue(object target, object value)
        {
            _MemberInfo.SetValue(target, value);
        }
    }
}
