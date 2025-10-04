using System;
using HaikaraDev.Tests.Common.Editor.Style;
using HaikaraDev.Tests.Common.Editor.View;
using UnityEditor;
using UnityEngine.UIElements;

namespace HaikaraDev.Tests.Common.Editor
{
    public static class TestFilesUtil
    {
        public static Type TestViewType => typeof(TestView);
        public static Type TestStyleType => typeof(TestStyle);
        public const string TestViewFileAssetPath = "Assets/HaikaraDev/Tests/Common/Editor/View/TestView.uxml";
        public const string TestStyleFileAssetPath = "Assets/HaikaraDev/Tests/Common/Editor/Style/TestStyle.uss";

        public const string PanelSettingsFileAssetPath =
            "Assets/HaikaraDev/Tests/Common/Editor/HaikaraTestPanelSettings.asset";

        public static VisualTreeAsset TestViewVisualTreeAsset
        {
            get
            {
                var asset = AssetDatabase.LoadAssetAtPath<VisualTreeAsset>(TestViewFileAssetPath);
                if (asset == null)
                {
                    throw new Exception($"TestView.uxml not found.");
                }

                return asset;
            }
        }

        public static StyleSheet TestStyleSheetAsset
        {
            get
            {
                var asset = AssetDatabase.LoadAssetAtPath<StyleSheet>(TestStyleFileAssetPath);
                if (asset == null)
                {
                    throw new Exception("TestStyle.uss not found");
                }

                return asset;
            }
        }
    }
}