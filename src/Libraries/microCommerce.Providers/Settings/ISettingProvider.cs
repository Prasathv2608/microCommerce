using microCommerce.Domain.Settings;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace microCommerce.Providers.Settings
{
    public interface ISettingProvider
    {
        Setting GetSettingById(int Id);

        Setting GetSetting(string key);

        T GetSettingByKey<T>(string key, T defaultValue = default(T));

        T GetSetting<T>() where T : ISettings, new();

        IList<Setting> GetAllSettings();
        
        void SaveSetting<T>(T setting) where T : ISettings, new();

        void SaveSetting<T, TPropType>(T settings, Expression<Func<T, TPropType>> keySelector, bool clearCache = true) where T : ISettings, new();

        void RemoveSetting<T>(T setting) where T : ISettings, new();

        void RemoveSetting<T, TPropType>(T settings, Expression<Func<T, TPropType>> keySelector) where T : ISettings, new();
    }
}