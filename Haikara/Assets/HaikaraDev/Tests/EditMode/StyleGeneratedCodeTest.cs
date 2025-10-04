using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Xml.Linq;
using HaikaraDev.Tests.Common.Editor.View;
using HaikaraDev.Tests.Common.Editor;
using NUnit.Framework;
using UnityEditor;
using Style_TestStyle = HaikaraDev.Tests.Common.Editor.Style.TestStyle;
using TestStyle = HaikaraDev.Tests.Common.Editor.Style.TestStyle;

namespace HaikaraDev.Tests.EditMode
{
    public class StyleGeneratedCodeTest
    {
        private readonly Dictionary<string, string> _expectedFieldNameToClassNames = new()
        {
            { "UnityLabel", "unity-label" },
            { "UnityButton", "unity-button" },
            { "UnityToggleCheckmark", "unity-toggle__checkmark" },
            { "UnityBaseTextFieldInput", "unity-base-text-field__input" }
        };

        [Test]
        public void Style_Guid_AfterSourceGeneration_ContainsAllAndOnlyUxmlElements()
        {
            var expectedGuid = AssetDatabase.AssetPathToGUID(TestFilesUtil.TestStyleFileAssetPath);

            var view = new Style_TestStyle();
            var actualGuid = view.GetGuid();

            Assert.AreEqual(expectedGuid, actualGuid, "GetGuid() method did not return the correct GUID.");
        }

        [Test]
        public void Style_UsedClassNames_AfterSourceGeneration_IsCorrect()
        {
            var usedClassNamesType = TestFilesUtil.TestStyleType.GetNestedType("UsedClassNames");
            Assert.IsNotNull(usedClassNamesType, "UsedClassNames nested class should exist.");

            var actualFields = usedClassNamesType
                .GetFields(BindingFlags.Public | BindingFlags.Static)
                .ToDictionary(
                    fieldInfo => fieldInfo.Name,
                    fieldInfo => fieldInfo.GetValue(null) as string
                );

            Assert.AreEqual(_expectedFieldNameToClassNames.Count, actualFields.Count,
                "The number of generated fields should match the expected count.");

            foreach (var expected in _expectedFieldNameToClassNames)
            {
                Assert.IsTrue(actualFields.ContainsKey(expected.Key), $"Field '{expected.Key}' should exist.");

                var actualValue = actualFields[expected.Key];
                Assert.AreEqual(expected.Value, actualValue, $"The value of field '{expected.Key}' should be correct.");
            }
        }
    }
}