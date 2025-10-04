using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Haikara.Runtime.View;
using UnityEngine.UIElements;

namespace Haikara.Runtime.Catalog
{
    public class RuntimeUICatalog
    {
        private static RuntimeUICatalog? _instance;

        public static RuntimeUICatalog Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new RuntimeUICatalog();
                    _instance.Initialize(UICatalog.Instance);
                }

                return _instance;
            }
        }

        public UICollection<VisualTreeAsset> UxmlUICollection { get; } = new();
        public UICollection<StyleSheet> UssUICollection { get; } = new();


        private void Initialize(UICatalog uiCatalog)
        {
            UxmlUICollection.Initialize(uiCatalog.UxmlAssets);
            UssUICollection.Initialize(uiCatalog.StyleAssets);
        }
        
        public VisualTreeAsset? LoadVisualTreeAsset(IHaikaraView view)
        {
            var id = view.GetGuid();
            return UxmlUICollection.LoadOrIncrementUIAsset(id);
        }
        
        public async Task<VisualTreeAsset?> LoadVisualTreeAssetAsync(IHaikaraView view)
        {
            var id = view.GetGuid();
            return await UxmlUICollection.LoadOrIncrementUIAssetAsync(id);
        }

        public async void ReleaseVisualTreeAsset(IHaikaraView view)
        {
            var id = view.GetGuid();
            await UxmlUICollection.ReleaseOrDecrementUIAssetAsync(id);
        }
        
        public StyleSheet? LoadStyleSheet(string id)
        {
            return UssUICollection.LoadOrIncrementUIAsset(id);
        }

        public async Task<StyleSheet?> LoadStyleSheetAsync(string id)
        {
            return await UssUICollection.LoadOrIncrementUIAssetAsync(id);
        }
        
        public async void ReleaseStyleSheet(string id)
        {
            await UssUICollection.ReleaseOrDecrementUIAssetAsync(id);
        }
    }
}