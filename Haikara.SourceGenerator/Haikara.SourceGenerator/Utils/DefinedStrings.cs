namespace SourceGenerator.Utils;

public static class DefinedStrings
{
    public const string RuntimeNamespace = "Haikara.Runtime";

    public const string CatalogNamespace = $"{RuntimeNamespace}.Catalog";
    public const string UiAttributeName = $"{RuntimeNamespace}.HaikaraUIAttribute";
    public const string AssetReferenceMode = $"{RuntimeNamespace}.AssetReferenceMode";


    private const string RuntimeViewNamespace = RuntimeNamespace + ".View";
    public const string ViewTypeName = $"{RuntimeViewNamespace}.HaikaraViewBase";
    public const string ViewInterfaceName = $"{RuntimeViewNamespace}.IHaikaraView";
    private const string ElementNameInfoTypeName = $"{RuntimeViewNamespace}.ElementNameInfo";
    public const string ElementFindTypeTypeName = $"{ElementNameInfoTypeName}.ElementFindType";
    public const string ViewInstallerBaseTypeName = $"{RuntimeViewNamespace}.ViewInstallerBase";
    public const string ViewProviderTypeName = $"{RuntimeViewNamespace}.ViewProvider";

    private const string RuntimeStyleNamespace = RuntimeNamespace + ".Style";
    public const string StyleInterfaceName = $"{RuntimeStyleNamespace}.IHaikaraStyle";
    public const string StyleBaseTypeName = $"{RuntimeStyleNamespace}.HaikaraStyleBase";

    public const string UiElementsNamespace = "UnityEngine.UIElements";
    public const string UnityEventBaseTypeName = $"{UiElementsNamespace}.EventBase";

    public const string BindableNamespace = $"{RuntimeNamespace}.Bindable";
    public const string BindablePropertyInterfaceName = $"{BindableNamespace}.IBindableProperty";
    public const string ManipulatorElementPropertyDisplayString = $"{BindableNamespace}.ManipulatorProperty<T>";
    public const string ClickCommandAttributeTypeName = $"{BindableNamespace}.ClickCommandAttribute";

    public const string TemplatePropertyInterfaceName = $"{BindableNamespace}.ITemplateProperty";
}