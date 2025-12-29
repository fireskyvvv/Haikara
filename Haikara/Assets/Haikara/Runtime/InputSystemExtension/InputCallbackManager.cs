#if HAIKARA_IS_EXISTS_INPUTSYSTEM
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Haikara.Runtime.InputSystemExtension
{
    public class InputCallbackManager : MonoBehaviour
    {
        private class InputActionInfo
        {
            public readonly InputAction Action;
            public readonly Dictionary<InputType, List<Action<InputAction>>> TypeToCallbacks;

            public InputActionInfo(InputAction action)
            {
                Action = action;
                TypeToCallbacks = Enum.GetValues(typeof(InputType)).OfType<InputType>().ToDictionary(
                    x => x,
                    _ => new List<Action<InputAction>>()
                );
            }
        }

        private readonly Dictionary<string, InputActionInfo> _idToActionInfo = new();

        private static InputCallbackManager? _instance;

        public static InputCallbackManager Instance
        {
            get
            {
                if (_instance != null)
                {
                    return _instance;
                }

                var found = FindFirstObjectByType<InputCallbackManager>();
                _instance = found == null
                    ? new GameObject($"[Haikara] {nameof(InputCallbackManager)}")
                        .AddComponent<InputCallbackManager>()
                    : found;

                return _instance;
            }
        }

        private void Update()
        {
            foreach (var kvp in _idToActionInfo)
            {
                var actionInfo = kvp.Value;
                var inputAction = actionInfo.Action;
                foreach (var (inputType, callbacks) in actionInfo.TypeToCallbacks)
                {
                    switch (inputType)
                    {
                        case InputType.InProgress when inputAction.inProgress:
                        case InputType.Triggered when inputAction.triggered:
                            RunCallbacks();
                            break;
                    }

                    continue;

                    void RunCallbacks()
                    {
                        foreach (var callback in callbacks)
                        {
                            callback.Invoke(inputAction);
                        }
                    }
                }
            }
        }

        public void RegisterInputAction(
            string actionNameOrId,
            bool throwIfNotFound,
            InputType inputType,
            Action<InputAction> callback
        )
        {
            if (!_idToActionInfo.TryGetValue(actionNameOrId, out var action))
            {
                action = new InputActionInfo(InputSystem.actions.FindAction(actionNameOrId, throwIfNotFound));
                _idToActionInfo.Add(actionNameOrId, action);
            }

            action.TypeToCallbacks[inputType].Add(callback);
        }

        public static void UnregisterInputAction(
            string actionNameOrId,
            bool throwIfNotFound,
            InputType inputType,
            Action<InputAction> callback
        )
        {
            if (_instance == null)
            {
                return;
            }
            
            if (_instance._idToActionInfo.TryGetValue(actionNameOrId, out var action))
            {
                action.TypeToCallbacks[inputType].Remove(callback);
                if (action.TypeToCallbacks.Count == 0)
                {
                    _instance._idToActionInfo.Remove(actionNameOrId);
                }
            }
        }

        private void OnDestroy()
        {
            foreach (var kvp in _idToActionInfo)
            {
                kvp.Value.Action.Dispose();
            }
        }
    }
}
#endif