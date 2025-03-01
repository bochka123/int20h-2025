namespace Int20h2025.Common.Models.Ai
{
    public class SystemMethodInfo
    {
        public string SystemName { get; set; } = string.Empty;
        public ServiceMethodInfo[] Methods { get; set; } = null!;
        public override string ToString()
        {
            var methods = string.Join("\n\n", Methods.Select(m => $"{m.MethodName}({string.Join(", ", m.Parameters.Select(p => $"{p.Name}: {p.Type}{(p.Name.EndsWith("?") ? " = None" : string.Empty)}"))})\n\n{m.Description}"));

            return $@"System name: {SystemName}

{methods}";
        }
    }

    public class ServiceMethodInfo
    {
        public string MethodName { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public ParameterInfo[] Parameters { get; set; } = null!;
    }

    public class ParameterInfo
    {
        public string Name { get; set; } = string.Empty;
        public string Type { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
    }
}
