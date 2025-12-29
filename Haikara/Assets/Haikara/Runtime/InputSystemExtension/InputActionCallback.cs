#if HAIKARA_IS_EXISTS_INPUTSYSTEM

using System;

namespace Haikara.Runtime.InputSystemExtension
{
    [AttributeUsage(validOn: AttributeTargets.Method)]
    public class InputActionCallbackAttribute : Attribute
    {
        public string ActionNameOrId { get; }
        public InputType InputType;
        public bool ThrowIfNotFound { get; set; } = false;

        public InputActionCallbackAttribute(string actionNameOrId, InputType inputType)
        {
            ActionNameOrId = actionNameOrId;
            InputType = inputType;
        }
    }
}

#endif