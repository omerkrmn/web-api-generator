using ApiGenerator.InputModels;
using System.Text;
using ApiGenerator.Common; // PrimitiveType enum'u burada varsayılır.

namespace ApiGenerator.Services
{
    public class EntityCodeGenerator
    {
        // PrimitiveType enum'ını uygun C# tip string'ine çeviren yardımcı metot
        private string GetCSharpTypeName(PrimitiveType type)
        {
            switch (type)
            {
                case PrimitiveType.Int: return "int";
                case PrimitiveType.Long: return "long";
                case PrimitiveType.Decimal: return "decimal";
                case PrimitiveType.Bool: return "bool";
                case PrimitiveType.String: return "string";
                case PrimitiveType.DateTime: return "DateTime";
                default: return type.ToString().ToLower(); // Varsayılan olarak küçük harf
            }
        }

        public string GenerateEntity(EntityDefinition entity, string projectName)
        {
            var entityName = entity.EntityName;

            // 🚩 DÜZELTME 1: Dinamik proje adını kullanıyoruz.
            var entityNamespace = $"{projectName}.Domain.Entities";

            var propertiesCode = new StringBuilder();
            foreach (var prop in entity.Properties)
            {
                // 🚩 DÜZELTME 2: Enum tipini C# string'ine çeviriyoruz.
                string cSharpType = GetCSharpTypeName(prop.Type);
                propertiesCode.AppendLine($"        public {cSharpType} {prop.Name} {{ get; set; }}");
            }

            var template = $@"
using {projectName}.Domain.Common; // Eğer BaseEntity kullanılıyorsa

namespace {entityNamespace}
{{
    public class {entityName} : BaseEntity 
    {{
{propertiesCode}
    }}
}}";
            return template.Trim();
        }
    }
}