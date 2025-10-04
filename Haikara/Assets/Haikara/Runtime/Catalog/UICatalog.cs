using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

namespace Haikara.Runtime.Catalog
{
    public class UICatalog : ScriptableObject
    {
        public static string AssetName => nameof(UICatalog);
        private static UICatalog? _instance;

        public static UICatalog Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = Resources.Load<UICatalog>(AssetName);

                    if (_instance == null)
                    {
                        throw new Exception("UICatalog not found");
                    }
                }

                return _instance;
            }
        }

        [SerializeField] private UIAssets<VisualTreeAsset> uxmlAssets = new();
        public UIAssets<VisualTreeAsset> UxmlAssets => uxmlAssets;

        [SerializeField] private UIAssets<StyleSheet> styleAssets = new();
        public UIAssets<StyleSheet> StyleAssets => styleAssets;
    }
}