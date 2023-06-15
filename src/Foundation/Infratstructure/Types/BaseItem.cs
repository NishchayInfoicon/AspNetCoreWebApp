using System.Runtime.Serialization;

namespace Practice.Foundation.Infrastructure.Types
{
    [Serializable]
    [DataContract]
    public class BaseItem
    {
        public BaseItem()
        {

        }

        public BaseItem(string className, bool IsTranslated = false)
        {
            ClassName = className;
            Id = Guid.NewGuid().ToString();
            PIMId = Id;
        }

        [DataMember(Order = 1)]
        public string Name { get; set; }

        [DataMember(Order = 2)]
        public string Id { get; set; }

        [DataMember(Order = 3)]
        public string ClassName { get; set; }

        [DataMember(Order = 4)]
        public string PIMId { get; set; }

        [DataMember(Order = 5)]
        public Dictionary<string, string> LanguageFields { get; set; } = new Dictionary<string, string>();

        [DataMember(Order = 6)]
        public Dictionary<string, string> SharedFields { get; set; } = new Dictionary<string, string>();

        [DataMember(Order = 7)]
        public bool IsTranslated { get; set; }

        #region Entity Helpers

        public string DisplayName
        {
            get
            {
                return GetDisplayName();
            }
            set
            {
                SetLanguageField("displayname", value);
            }
        }
        public string DisplayTitle
        {
            get
            {
                return GetDisplayTitle();
            }
            set
            {
                SetLanguageField("displaytitle", value);
            }
        }

        public string GetDisplayName(string culture = "en")
        {
            string displayName = string.Empty;
            displayName = GetLanguageField("displayname", culture);
            return displayName;
        }

        public string GetDisplayTitle(string culture = "en")
        {
            string displayTitle = string.Empty;
            displayTitle = GetLanguageField("displaytitle", culture);
            return displayTitle;
        }

        #region Remover for entities

        public void RemoveSharedField(string fieldName)
        {
            if (string.IsNullOrEmpty(fieldName))
            {
                return;
            }
            fieldName = fieldName.ToLower();
            if (SharedFields.ContainsKey(fieldName))
            {
                SharedFields.Remove(fieldName);
            }
            return;
        }

        public void RemoveLanguageField(string fieldName, string culture = "en")
        {
            if (string.IsNullOrEmpty(fieldName))
            {
                return;
            }
            string fieldKey = $"{fieldName.ToLower()}_{culture.ToLower()}";
            if (LanguageFields.ContainsKey(fieldKey))
            {
                LanguageFields.Remove(fieldKey);
            }
            return;
        }

        #endregion

        #region Setter for entities

        public void SetLanguageField(string fieldName, string fieldValue, string culture = "en")
        {
            if (string.IsNullOrEmpty(fieldName) || string.IsNullOrEmpty(fieldValue))
            {
                return;
            }
            string fieldKey = $"{fieldName.ToLower()}_{culture.ToLower()}";
            if (LanguageFields.ContainsKey(fieldKey))
            {
                RemoveLanguageField(fieldName);
            }
            LanguageFields.Add(fieldKey, fieldValue.ToString());
        }

        public void SetSharedField(string fieldName, string fieldValue)
        {
            if (string.IsNullOrEmpty(fieldName) || string.IsNullOrEmpty(fieldValue))
            {
                return;
            }
            fieldName = $"{fieldName.ToLower()}";
            if (SharedFields.ContainsKey(fieldName))
            {
                RemoveSharedField(fieldName);
            }
            SharedFields.Add(fieldName, fieldValue.ToString());
        }

        #endregion

        #region Getter for entities

        public T GetSharedField<T>(string fieldName) where T : new()
        {
            if (string.IsNullOrEmpty(fieldName))
            {
                return default(T);
            }
            fieldName = fieldName.ToLower();
            if (SharedFields.ContainsKey(fieldName))
            {
                SharedFields.TryGetValue(fieldName, out string fieldValue);
                if (!string.IsNullOrEmpty(fieldValue))
                {
                    return (T)Convert.ChangeType(fieldValue, typeof(T));
                }
            }
            return default(T);
        }

        public string GetLanguageField(string fieldName, string culture = "en")
        {
            if (string.IsNullOrEmpty(fieldName))
            {
                return string.Empty;
            }
            string fieldKey = $"{fieldName.ToLower()}_{culture.ToLower()}";
            string fieldValue = string.Empty;
            if (LanguageFields.ContainsKey(fieldKey))
            {
                LanguageFields.TryGetValue(fieldKey, out fieldValue);
                if (!string.IsNullOrEmpty(fieldValue))
                {
                    return fieldValue;
                }
            }
            return fieldValue;
        }

        #endregion



        #endregion
    }
}
