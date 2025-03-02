namespace Int20h2025.Common.Models.Ai
{
    public class SystemMethodInfo
    {
        public string SystemName { get; set; } = string.Empty;
        public ServiceMethodInfo[] Methods { get; set; } = null!;
        public override string ToString()
        {
            var methods = string.Join("\n\n", Methods.Select(m =>
            {
                var parameters = string.Join(", ", m.Parameters.Select(p =>
                {
                    var type = p.IsRequired ? p.Type : $"{p.Type}?";
                    return $"{p.Name}: {type}";
                }));
                return $"{m.MethodName}({parameters})\n\n{m.Description}";
            }));

            return $"System name: {SystemName}\n\n{methods}";
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
        public bool IsRequired { get; set; } = true;
    }
}
