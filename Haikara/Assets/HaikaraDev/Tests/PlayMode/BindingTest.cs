using System;
using System.Collections;
using Haikara.Runtime.View;
using HaikaraDev.Tests.Common;
using HaikaraDev.Tests.Common.Editor;
using HaikaraDev.Tests.Common.Editor.ViewModel;
using NUnit.Framework;
using UnityEditor;
using UnityEngine;
using UnityEngine.TestTools;
using UnityEngine.UIElements;
using TestView = HaikaraDev.Tests.Common.Editor.View.TestView;

namespace HaikaraDev.Tests.PlayMode
{
    public class BindingTest
    {
        [UnityTest]
        public IEnumerator DataBinding_WhenViewModelIsSet_ShouldUpdateUiAndViewModelCorrectly()
        {
            var gameObject = new GameObject();
            var uiDocument = gameObject.AddComponent<UIDocument>();
            var panelSettings = AssetDatabase.LoadAssetAtPath<PanelSettings>(TestFilesUtil.PanelSettingsFileAssetPath);
            uiDocument.panelSettings = panelSettings;

            ViewInstaller.Install();
            var layout = TestFilesUtil.TestViewVisualTreeAsset.Instantiate();
            var view = new TestView();

            var expectedLabel = Guid.NewGuid().ToString();
            var expectedSubLabel = Guid.NewGuid().ToString();
            var viewModel = new TestViewModel(
                label: expectedLabel,
                subViewModel: new TestSubViewModel(expectedSubLabel)
            );

            uiDocument.runInEditMode = true;
            uiDocument.rootVisualElement.Add(layout);
            view.LinkElements(layout);
            view.SetDataSource(viewModel);

            yield return null;

            // ToTarget Binding
            // Check if the Label text matches.
            var testViewLabel = layout.Q<Label>("test-view__label");
            Assert.AreEqual(expectedLabel, testViewLabel.text);

            // TwoWay Binding
            var testViewToggle = layout.Q<Toggle>("test-view__toggle");
            // Check if the Toggle's value matches the initial state.
            Assert.AreEqual(viewModel.Toggle, testViewToggle.value); // false

            // Check if the Toggle's value updates when the ViewModel is changed.
            viewModel.Toggle = !viewModel.Toggle;
            yield return null;
            Assert.AreEqual(viewModel.Toggle, testViewToggle.value); // true

            // Check if the ViewModel's value updates when the Toggle's value is changed from the UI.
            testViewToggle.value = !testViewToggle.value;
            yield return null;
            Assert.AreEqual(viewModel.Toggle, testViewToggle.value); // false

            // ToSource Binding
            var testViewTextField = layout.Q<TextField>("test-view__text-field");
            testViewTextField.value = Guid.NewGuid().ToString();
            yield return null;
            Assert.AreEqual(testViewTextField.value, viewModel.TextField);

            // ClickCommand
            var testViewButton = layout.Q<Button>("test-view__button");
            using (var e = new NavigationSubmitEvent())
            {
                e.target = testViewButton;
                testViewButton.SendEvent(e);
            }

            yield return null;
            Assert.AreEqual(1, viewModel.ClickCount);

            // Check if the SubView's Label text matches.
            var testSubViewLabel = layout.Q<Label>("test-sub__label");
            Assert.AreEqual(expectedSubLabel, testSubViewLabel.text);

            ViewProvider.Instance.UnregisterAll();
        }
    }
}