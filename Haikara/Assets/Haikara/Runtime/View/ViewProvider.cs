using System;
using System.Collections.Generic;
using System.Linq;
using Haikara.Runtime.Catalog;
using Haikara.Runtime.Util;
using UnityEngine.UIElements;

namespace Haikara.Runtime.View
{
    public class ViewProvider
    {
        private static ViewProvider? _instance;
        public static ViewProvider Instance => _instance ??= new ViewProvider();

        private readonly Dictionary<string, Func<IHaikaraView>> _uxmlGuidToViewCreator = new();

        public void Register(string guid, Func<IHaikaraView> creator)
        {
            if (string.IsNullOrEmpty(guid))
            {
                return;
            }

            _uxmlGuidToViewCreator.TryAdd(guid, creator);
        }

        public void UnregisterAll()
        {
            _uxmlGuidToViewCreator.Clear();
        }

        public IHaikaraView? CreateView(string guid)
        {
            if (!_uxmlGuidToViewCreator.TryGetValue(guid, out var creator))
            {
                UnityEngine.Debug.LogWarning(HaikaraLogUtil.GetMessage($"View is not installed. Guid: {guid}"));
                return null;
            }

            var view = creator.Invoke();
            return view;
        }

        public IEnumerable<string> GetRegisteredGuids()
        {
            return _uxmlGuidToViewCreator.Select(kvp => kvp.Key);
        }
    }
}