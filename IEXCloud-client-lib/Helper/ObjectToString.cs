using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Text;

namespace IEXCloudClient.NetCore.Helper
{
    public static class ObjectToString  {
        public static string ToTableString(this object input){
            var properties = new Dictionary<string, string>();
            foreach (PropertyInfo propertyInfo in input.GetType().GetProperties())
            {
                if (propertyInfo.CanRead)
                {
                    object firstValue = propertyInfo.GetValue(input, null);
                    string valueStr;
                    if(firstValue == null){
                        valueStr = "<null>";
                    } else if(firstValue is IEnumerable list) {
                        valueStr = string.Join(", ", list);
                    }else {
                        valueStr = firstValue.ToString();
                    }
                    properties[propertyInfo.Name] = valueStr;
                }
            }
            var colOneLength = properties.Keys.Max(x=>x.Length) + 2;
            var lines = new string[properties.Count];
            for(var i = 0; i < properties.Keys.Count; i++){
                lines[i] = string.Format("{0,-"+colOneLength+"}{1}", 
                    properties.Keys.ElementAt(i)+":", 
                    properties[properties.Keys.ElementAt(i)]
                );
            }
            return string.Join("\n", lines);
        }
    }
}