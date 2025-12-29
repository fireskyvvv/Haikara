using System.Collections.Generic;
using System.Xml.Linq;
using SourceGenerator.Utils;

namespace SourceGenerator.CodeTemplates.ViewTemplates;

internal partial class ViewTemplate
{
    private static string OnDetachViewInternalCodeString(
        GenerationSource generationSource,
        List<ElementPropertyFieldInfo> elementPropertyFieldInfoList,
        List<TemplatePropertyFieldInfo> templatePropertyFieldInfoList,
        List<(XElement element, int index)> engineElements
    )
    {
        var codeString =
            $$"""

                      protected {{generationSource.ReturnType("void")}} OnDetachViewInternal(DetachFromPanelEvent detachFromPanelEvent)
                      {
                      
              """;

        foreach (var callbackInfo in GetInputActionCallbackInfo(generationSource))
        {
            codeString +=
                $$"""

                              {{DefinedStrings.InputSystemExtensionNamespace}}.InputCallbackManager.UnregisterInputAction(
                                  actionNameOrId: {{callbackInfo.actionNameOrId}},
                                  throwIfNotFound: {{callbackInfo.throwIfNotFound}},
                                  inputType: {{callbackInfo.inputTypeString}},
                                  callback: {{callbackInfo.methodName}}
                              );
                  """;
        }

        codeString +=
            $$"""
              
                          base.OnDetachViewInternal(detachFromPanelEvent);
                      }
              """;

        return codeString;
    }
}