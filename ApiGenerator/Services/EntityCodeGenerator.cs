using ApiGenerator.InputModels;
using System.Text;
using ApiGenerator.Common;
using ApiGenerator.Managers;

namespace ApiGenerator.Services
{
    public class EntityCodeGenerator
    {
        public string GenerateEntity(EntityDefinition entity, string projectName)
        {
            var entityName = entity.EntityName;
            var entityNamespace = $"{projectName}.Domain.Entities";

            var propertiesCode = new StringBuilder();
            foreach (var prop in entity.Properties)
            {
                string cSharpType = prop.Type.GetDescription();
                propertiesCode.AppendLine($"        public {cSharpType} {prop.Name} {{ get; set; }}");
            }
            var template = $@"
using {projectName}.Domain.Common; 

namespace {entityNamespace}
{{
    public class {entityName}
    {{
{propertiesCode}
    }}
}}";
            return template.Trim();
        }
    }
}