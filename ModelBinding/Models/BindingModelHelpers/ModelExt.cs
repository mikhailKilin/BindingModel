using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace ModelBinding.Models.BindingModelHelpers
{
    public static class ModelExt
    {
        private static readonly JsonSerializerSettings Settings = new JsonSerializerSettings
        {
            NullValueHandling = NullValueHandling.Ignore,
            //DefaultValueHandling = DefaultValueHandling.Ignore
        };

        public static readonly string QuotesReplaceFlag = "123MFS321";

        private static string Serialize(object value)
        {
            var result = JsonConvert.SerializeObject(value, Settings);
            return result.Replace("\"" + QuotesReplaceFlag, "").Replace(QuotesReplaceFlag + "\"", "");
        }

        /// <summary>
        /// Преобразует объект в JSON строку
        /// </summary>
        /// <param name="item">Экземпляр для сериализации</param>
        /// <param name="replacements">Словарь замен - если в полученной строке что-то надо заменить по шаблону</param>
        /// <returns></returns>
        public static string ToJsonString(this IJsonStringify item, Dictionary<string, string> replacements)
        {
            var serializedObject = Serialize(item);

            return replacements == null
                ? serializedObject
                : replacements.Aggregate(serializedObject,
                    (current, keyValuePair) => current.Replace(keyValuePair.Key, keyValuePair.Value));
        }

        public static string ToJsonString(this object dictionary)
        {
            var serializedObject = Serialize(dictionary);
            return serializedObject;
        }

        public static string ToJsonString<T>(this List<T> list)
        {
            var serializedObject = Serialize(list);
            return serializedObject;
        }

        public static ChoiceObject ToChoiceObj(this object value)
        {
            if (value == null) return null;
            var jObj = value as JObject;
            if (jObj == null) return null;
            var choiceObj = jObj.ToObject<ChoiceObject>();
            if (choiceObj == null || choiceObj.Id == Guid.Empty) return null;
            return choiceObj;
        }

        public static IEnumerable<ChoiceObject> ToChoiceObjList(this object value)
        {
            if (value == null) return new List<ChoiceObject>();
            var jObj = value as JArray;
            return jObj == null ? new List<ChoiceObject>() : jObj.Select(item => item.ToChoiceObj()).Where(obj => obj != null).ToList();
        }

        public static int? ToInt32(this object value)
        {
            int val;
            return Int32.TryParse((value ?? "").ToString(), out val) ? val : (int?)null;
        }

        public static bool ToBoolean(this object value)
        {
            bool val;
            return Boolean.TryParse((value ?? "").ToString(), out val) ? val : false;
        }

        public static double? ToDouble(this object value)
        {
            double val;
            return Double.TryParse((value ?? "").ToString(), out val) ? val : (double?)null;
        }

        public static Guid ToGuid(this object value)
        {
            Guid g;
            return Guid.TryParse((value ?? "").ToString(), out g) ? g : Guid.Empty;
        }

        public static DateTime ToDate(this object value)
        {
            DateTime d;
            return DateTime.TryParse((value ?? "").ToString(), out d) ? d : DateTime.MinValue;
        }
    }
    public class ChoiceObject
    {
        public Guid Id { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public bool Checked { get; set; }
        public string Type { get; set; }
        public bool Selectable { get; set; }
    }
}
