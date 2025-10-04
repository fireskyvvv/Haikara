using System;

namespace Haikara.Editor.Localization
{
    internal readonly struct LocalizableText
    {
        private readonly Locale _locale;
        private readonly string _en;
        private readonly string _ja;

        public LocalizableText(string en, string ja)
        {
            _en = en;
            _ja = ja;

            _locale = UnityEngine.Application.systemLanguage == UnityEngine.SystemLanguage.Japanese
                ? Locale.Ja
                : Locale.En;
        }

        public static implicit operator string(LocalizableText localizableText)
        {
            var locale = localizableText._locale;
            return locale switch
            {
                Locale.En => localizableText._en,
                Locale.Ja => localizableText._ja,
                _ => throw new ArgumentOutOfRangeException(nameof(locale), locale, null)
            };
        }
    }
}