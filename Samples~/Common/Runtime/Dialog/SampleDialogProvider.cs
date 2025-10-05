using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Haikara.Runtime;
using Haikara.Runtime.Util;
using Haikara.Runtime.View;
using UnityEngine.UIElements;

namespace Haikara.Samples.Common.Runtime.Dialog
{
    #nullable enable
    public class SampleDialogProvider : SimpleSingleton<SampleDialogProvider>
    {
        /// <summary>
        /// If a VisualElement with the 'dialog-root__content-viewport' class exists,
        /// the dialog's view is added to that element.
        /// </summary>
        private const string ContentsViewportClassName = "dialog-root__content-viewport";

        private IHaikaraView? _currentDialogView;
        private HaikaraUIContext? _uiContext;

        private string _dialogRootUxmlGuid = string.Empty;
        private string _backdropUxmlGuid = string.Empty;

        public void Initialize(HaikaraUIContext uiContext, string dialogRootUxmlGuid, string backdropUxmlGuid = "")
        {
            _uiContext = uiContext;
            _dialogRootUxmlGuid = dialogRootUxmlGuid;
            _backdropUxmlGuid = backdropUxmlGuid;
        }

        private readonly Dictionary<Type, string> _viewModelTypeToContentsGuid = new();

        public void RegisterDialog<TViewModel>(string uxmlGuid)
        {
            if (_viewModelTypeToContentsGuid.TryGetValue(typeof(TViewModel), out _))
            {
                throw new Exception(HaikaraLogUtil.GetMessage("Same view type cant register to DialogProvider"));
            }

            _viewModelTypeToContentsGuid.TryAdd(typeof(TViewModel), uxmlGuid);
        }

        public async void ShowDialog<T>(T viewModel)
        {
            if (_uiContext == null)
            {
                throw new Exception($"{nameof(SampleDialogProvider)} has not initialized.");
            }

            if (!_viewModelTypeToContentsGuid.TryGetValue(typeof(T), out var contentsUxmlGuid))
            {
                throw new Exception($"{typeof(T)} has not registered to DialogProvider");
            }

            var dialogRootParent = _uiContext.GetUiDocumentRootElement();

            // If backdropRoot is specified, instantiate it.
            var backdrop = await LoadBackdropAsync(_uiContext);
            if (backdrop != null)
            {
                dialogRootParent = backdrop.Value.backdropElement;
            }

            // Instantiate DialogRoot
            var (dialogRootView, contentsViewport) =
                await LoadAndInstantiateDialogRootAsync(dialogRootParent: dialogRootParent);

            if (backdrop != null)
            {
                // Add the DialogRoot to the Backdrop's SubView.
                backdrop.Value.backdropView.AddSubView(dialogRootView);
            }

            // Instantiate the dialog's content view and set its DataSource.
            var contentsView =
                InstantiateDialogContents(dialogContentsUxmlGuid: contentsUxmlGuid, contentsViewport: contentsViewport);

            // Add the dialog content to the DialogRoot's SubView.
            dialogRootView.AddSubView(contentsView);

            contentsView.SetDataSource(viewModel);
        }

        private async Task<(IHaikaraView backdropView, VisualElement backdropElement)?> LoadBackdropAsync(
            HaikaraUIContext uiContext
        )
        {
            if (string.IsNullOrWhiteSpace(_backdropUxmlGuid))
            {
                return null;
            }

            var backdropView = ViewProvider.Instance.CreateView(_backdropUxmlGuid);
            if (backdropView == null)
            {
                UnityEngine.Debug.LogError(
                    HaikaraLogUtil.GetMessage($"BackdropView is not found. Guid: {_backdropUxmlGuid}")
                );
                return null;
            }

            var backdropElement = await backdropView.LoadAndAddToAsync(uiContext.GetUiDocumentRootElement());

            return (backdropView, backdropElement);
        }

        private async Task<(IHaikaraView dialogRoot, VisualElement contentsViewport)> LoadAndInstantiateDialogRootAsync(
            VisualElement dialogRootParent)
        {
            var dialogRoot = ViewProvider.Instance.CreateView(_dialogRootUxmlGuid);
            if (dialogRoot == null)
            {
                throw new Exception("Null DialogRoot");
            }

            var dialogRootElement = await dialogRoot.LoadAndAddToAsync(dialogRootParent);
            var contentsViewport = dialogRootElement.Q<VisualElement>(className: ContentsViewportClassName) ??
                                   dialogRootElement;

            return (dialogRoot, contentsViewport);
        }

        private IHaikaraView InstantiateDialogContents(string dialogContentsUxmlGuid, VisualElement contentsViewport)
        {
            var view = ViewProvider.Instance.CreateView(dialogContentsUxmlGuid);
            if (view == null)
            {
                throw new Exception("DialogView not found");
            }

            view.LoadAndAddToAsync(contentsViewport);

            return view;
        }
    }
}