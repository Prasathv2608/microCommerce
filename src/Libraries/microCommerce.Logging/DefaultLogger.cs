using BiletKesfet.Common;
using System;
using System.IO;

namespace microCommerce.Logging
{
    public class DefaultLogger : ILogger
    {
        #region Fields
        private readonly ICustomFileProvider _fileProvider;
        #endregion

        #region Ctor
        public DefaultLogger(ICustomFileProvider fileProvider)
        {
            _fileProvider = fileProvider;
        }
        #endregion

        #region Methods
        public virtual void Log(LogLevel logLevel,
            string shortMessage,
            string fullMessage = null,
            string ipAddress = null,
            string pageUrl = null,
            string referrerUrl = null)
        {
            if (string.IsNullOrEmpty(shortMessage))
                return;

            string logMessage = string.Format("{0} [{1}] {2}",
                DateTime.UtcNow.ToString("dd.MM.yyyy HH:mm:ss.fff zzz"),
                logLevel.ToString(),
                shortMessage);

            _fileProvider.CreateDirectory(_fileProvider.MapContentPath("logs"));
            using (StreamWriter sw = File.AppendText(_fileProvider.MapContentPath(string.Format("logs/{0:dd.MM.yyyy}.txt", DateTime.UtcNow))))
            {
                sw.WriteLine(logMessage);
            }
        }
        #endregion
    }
}