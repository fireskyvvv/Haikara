using System.Collections.Generic;
using Newtonsoft.Json;

namespace SourceGenerator.CodeTemplates.InputSystemExtension;

public class InputActionData
{
    [JsonProperty("maps")] public List<InputActionMap> Maps { get; set; } = [];
}

public class InputActionMap
{
    [JsonProperty("actions")] public List<Action> Actions { get; set; } = [];
}

public class Action
{
    [JsonProperty("name")] public string Name { get; set; } = string.Empty;
}