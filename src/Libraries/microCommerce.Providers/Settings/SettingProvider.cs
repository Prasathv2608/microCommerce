using microCommerce.Caching;
using microCommerce.Common;
using microCommerce.Dapper;
using microCommerce.Domain.Settings;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

namespace microCommerce.Providers.Settings
{
    public class SettingProvider : ISettingProvider
    {
        private readonly IDataContext _dataContext;
        private readonly ICacheManager _cacheManager;

        public SettingProvider(IDataContext dataContext,
            ICacheManager cacheManager)
        {
            _dataContext = dataContext;
            _cacheManager = cacheManager;
        }

        [Serializable]
        public class SettingForCaching
        {
            public int Id { get; set; }
            public string Name { get; set; }
            public string Value { get; set; }
        }

        protected virtual IDictionary<string, IList<SettingForCaching>> GetAllSettingsCached()
        {
            //cache
            var settings = _cacheManager.Get(ProviderCacheKey.SETTINGS_ALL_KEY, () =>
            {
                var allSettings = _dataContext.QueryProcedure<Setting>("SELECT * FROM Setting").ToList();

                var dictionary = new Dictionary<string, IList<SettingForCaching>>();
                foreach (var s in allSettings)
                {
                    var resourceName = s.Name.ToLowerInvariant();
                    var settingForCaching = new SettingForCaching
                    {
                        Id = s.Id,
                        Name = s.Name,
                        Value = s.Value
                    };

                    if (!dictionary.ContainsKey(resourceName))
                    {
                        //first setting
                        dictionary.Add(resourceName, new List<SettingForCaching>
                         {
                            settingForCaching
                         });
                    }
                    else//already added
                        dictionary[resourceName].Add(settingForCaching);
                }

                return dictionary;
            });

            return settings;
        }

        protected virtual void InsertSetting(Setting setting, bool clearCache)
        {
            if (setting == null)
                throw new ArgumentNullException(nameof(setting));

            _dataContext.Insert(setting);

            //cache
            if (clearCache)
                _cacheManager.RemoveByPattern(ProviderCacheKey.SETTINGS_PATTERN_KEY);
        }

        protected virtual void UpdateSetting(Setting setting, bool clearCache)
        {
            if (setting == null)
                throw new ArgumentNullException(nameof(setting));

            _dataContext.Update(setting);

            //cache
            if (clearCache)
                _cacheManager.RemoveByPattern(ProviderCacheKey.SETTINGS_PATTERN_KEY);
        }

        protected virtual void DeleteSetting(Setting setting, bool clearCache)
        {
            if (setting == null)
                throw new ArgumentNullException(nameof(setting));

            _dataContext.Delete(setting);

            //cache
            if (clearCache)
                _cacheManager.RemoveByPattern(ProviderCacheKey.SETTINGS_PATTERN_KEY);
        }

        protected virtual void DeleteSettings(IList<Setting> settings)
        {
            if (settings == null)
                throw new ArgumentNullException(nameof(settings));

            _dataContext.DeleteBulk(settings);

            //cache
            _cacheManager.RemoveByPattern(ProviderCacheKey.SETTINGS_PATTERN_KEY);
        }

        public virtual Setting GetSettingById(int Id)
        {
            if (Id == 0)
                return null;

            return _dataContext.Find<Setting>(Id);
        }

        public virtual Setting GetSetting(string name)
        {
            if (string.IsNullOrEmpty(name))
                return null;

            return _dataContext.First<Setting>("SELECT * FROM Setting WHERE Name = @name ", new { name });
        }

        public virtual T GetSettingByKey<T>(string key, T defaultValue = default(T))
        {
            if (string.IsNullOrEmpty(key))
                return defaultValue;

            var settings = GetAllSettingsCached();
            key = key.Trim().ToLowerInvariant();
            if (settings.ContainsKey(key))
            {
                var setting = settings[key].FirstOrDefault();
                if (setting != null)
                    return CommonHelper.To<T>(setting.Value);
            }

            return defaultValue;
        }

        public virtual T GetSetting<T>() where T : ISettings, new()
        {
            var setting = GetSetting(typeof(T));

            return (T)setting;
        }

        public virtual ISettings GetSetting(Type type)
        {
            var settings = Activator.CreateInstance(type);

            foreach (var prop in type.GetProperties())
            {
                // get properties we can read and write to
                if (!prop.CanRead || !prop.CanWrite)
                    continue;

                var key = type.Name + "." + prop.Name;

                var setting = GetSettingByKey<string>(key);
                if (setting == null)
                    continue;

                if (!TypeDescriptor.GetConverter(prop.PropertyType).CanConvertFrom(typeof(string)))
                    continue;

                if (!TypeDescriptor.GetConverter(prop.PropertyType).IsValid(setting))
                    continue;

                var value = TypeDescriptor.GetConverter(prop.PropertyType).ConvertFromInvariantString(setting);

                //set property
                prop.SetValue(settings, value, null);
            }

            return settings as ISettings;
        }

        public virtual IList<Setting> GetAllSettings()
        {
            return _dataContext.QueryProcedure<Setting>("SELECT * FROM Setting").ToList();
        }

        public virtual void SaveSetting<T>(string key, T value, bool clearCache = true)
        {
            if (key == null)
                throw new ArgumentNullException(nameof(key));

            key = key.Trim().ToLowerInvariant();
            var valueStr = TypeDescriptor.GetConverter(typeof(T)).ConvertToInvariantString(value);

            var allSettings = GetAllSettingsCached();
            var settingForCaching = allSettings.ContainsKey(key) ? allSettings[key].FirstOrDefault() : null;
            if (settingForCaching != null)
            {
                //update
                var setting = GetSettingById(settingForCaching.Id);
                setting.Value = valueStr;
                UpdateSetting(setting, clearCache);
            }
            else
            {
                //insert
                InsertSetting(new Setting
                {
                    Name = key,
                    Value = valueStr
                }, clearCache);
            }
        }

        public virtual void SaveSetting<T>(T setting) where T : ISettings, new()
        {
            foreach (var prop in typeof(T).GetProperties())
            {
                // get properties we can read and write to
                if (!prop.CanRead || !prop.CanWrite)
                    continue;

                if (!TypeDescriptor.GetConverter(prop.PropertyType).CanConvertFrom(typeof(string)))
                    continue;

                var key = typeof(T).Name + "." + prop.Name;
                //Duck typing is not supported in C#. That's why we're using dynamic type
                dynamic value = prop.GetValue(setting, null);
                if (value != null)
                    SaveSetting(key, value, false);
                else
                    SaveSetting(key, "", false);
            }
        }

        public virtual void SaveSetting<T, TPropType>(T settings, Expression<Func<T, TPropType>> keySelector, bool clearCache = true) where T : ISettings, new()
        {
            var member = keySelector.Body as MemberExpression;
            if (member == null)
            {
                throw new ArgumentException(string.Format(
                    "Expression '{0}' refers to a method, not a property.",
                    keySelector));
            }

            var propInfo = member.Member as PropertyInfo;
            if (propInfo == null)
            {
                throw new ArgumentException(string.Format(
                       "Expression '{0}' refers to a field, not a property.",
                       keySelector));
            }

            var key = settings.GetSettingKey(keySelector);
            //Duck typing is not supported in C#. That's why we're using dynamic type
            dynamic value = propInfo.GetValue(settings, null);
            if (value != null)
                SaveSetting(key, value, clearCache);
            else
                SaveSetting(key, "", clearCache);
        }

        public virtual void RemoveSetting<T>(T setting) where T : ISettings, new()
        {
            var settingsToDelete = new List<Setting>();
            var allSettings = GetAllSettings();
            foreach (var prop in typeof(T).GetProperties())
            {
                var key = typeof(T).Name + "." + prop.Name;
                settingsToDelete.AddRange(allSettings.Where(x => x.Name.Equals(key, StringComparison.InvariantCultureIgnoreCase)));
            }

            DeleteSettings(settingsToDelete);
        }

        public virtual void RemoveSetting<T, TPropType>(T settings, Expression<Func<T, TPropType>> keySelector) where T : ISettings, new()
        {
            var key = settings.GetSettingKey(keySelector);
            key = key.Trim().ToLowerInvariant();

            var allSettings = GetAllSettingsCached();
            var settingForCaching = allSettings.ContainsKey(key) ? allSettings[key].FirstOrDefault() : null;
            if (settingForCaching != null)
            {
                //delete
                var setting = GetSettingById(settingForCaching.Id);
                DeleteSetting(setting, true);
            }
        }
    }
}