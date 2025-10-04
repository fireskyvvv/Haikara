#if HAIKARA_IS_EXISTS_ADDRESSABLES
using System;
using Haikara.Editor.Localization;

namespace Haikara.Editor.AddressablesExtensions.AddressablesMaker
{
    internal static class HaikaraAddressablesMakerLocalizableTexts
    {
        public static readonly LocalizableText LabelAddressableGroupNames = new(
            ja: "Addressable Group名",
            en: "Name of Addressable Group"
        );

        public static readonly LocalizableText LabelRunBuildButton = new(
            ja: "実行",
            en: "Run"
        );

        public static readonly LocalizableText LabelWindowInfo = new(
            ja: $"UIアセット(.uxml,.uss)のAddressablesを作成するウィンドウです。\n" +
                $"登録されたUIアセットは<b>Addressables UI Loader</b>を使って呼び出すことができるようになります。\n" +
                $"詳細は<b><a href=\"{DocsUrl(Locale.Ja)}\">こちら</a></b>を参照してください。",
            en: $"This window creates Addressables for UI Assets(.uxml,.uss).\n" +
                $"Registered UI assets can be loaded using the <b>Addressables UI Loader</b>.\n" +
                $"For details, see <a href=\"{DocsUrl(Locale.En)}\">here</a>."
        );

        private static string DocsUrl(Locale locale)
        {
            return locale switch
            {
                Locale.En =>
                    $"https://fireskyvvv.github.io/haikara-docs/{locale.ToString().ToLower()}/guide/addressables-support/addressables-ui-loader.html",
                Locale.Ja =>
                    "https://fireskyvvv.github.io/haikara-docs/guide/addressables-support/addressables-ui-loader.html",
                _ => throw new ArgumentOutOfRangeException(nameof(locale), locale, null)
            };
        }
    }
}
#endif