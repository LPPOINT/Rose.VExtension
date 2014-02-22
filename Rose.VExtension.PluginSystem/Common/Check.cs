using System;
using System.Runtime.CompilerServices;
using NLog;

namespace Rose.VExtension.PluginSystem.Common
{
    public static class Check
    {
        private static bool useLogging = true;
        public static bool UseLogging
        {
            get { return useLogging; }
            set { useLogging = value; }
        }

        private static void Log(Exception exception, string member = null)
        {
            LogManager.GetCurrentClassLogger().ErrorException(string.Format("Валидация объекта провалена. Имя валидатора: {0}", member), exception);
        }

        public static void ThrowException(Exception exception, [CallerMemberName]string member = null)
        {
            if (UseLogging)
            {
                Log(exception, member ?? string.Empty);
            }
            throw exception;
        }

        public static void NotNull(object obj, string name)
        {
            if (obj == null)
            {
                ThrowException(string.IsNullOrWhiteSpace(name)
                    ? new ArgumentNullException()
                    : new ArgumentNullException(name));
            }
        }
        public static void NotNull(object obj)
        {
            NotNull(obj, string.Empty);
        }
        public static void NotEmpty(string str, string name)
        {
            if (string.IsNullOrEmpty(str))
            {
                ThrowException(new ArgumentException("Строка пуста", name));
            }
        }
        public static void NotEmpty(string str)
        {
            NotEmpty(str, string.Empty);
        }
        public static void NotNullOrWhiteSpace(string str, string name)
        {
            if (string.IsNullOrWhiteSpace(str))
            {
                ThrowException(new ArgumentException("Строка пуста", name));
            }
        }
        public static void NotNullOrWhiteSpace(string str)
        {
            NotNullOrWhiteSpace(str, string.Empty);
        }
        public static void IsInRange(string str, int minLen, int maxLen, string name)
        {
            if (str.Length < minLen || str.Length > maxLen)
            {
                ThrowException(new ArgumentOutOfRangeException(name, str, "Строка не входит в задданые границы"));
            }
        }


    }
}
