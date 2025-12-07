using ApiGenerator.InputModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApiGenerator.Services;

public class ControllerGenerator
{
    public string GenerateController(EntityDefinition entity, string projectName)
    {
        var sb = new StringBuilder();
        var entityName = entity.EntityName;
        var pluralEntityName = entityName + "s";
        var entityType = entityName;
        var repositoryInterface = $"I{entityName}Repository"; // Örn: IProductRepository
        var repositoryVariableName = $"_{entityName.ToLower()}Repository"; // Örn: _productRepository
        var creationDto = $"Create{entityName}Dto"; // Oluşturma için DTO adı (varsayım)
        var returnDto = $"{entityName}Dto"; // Dönüş için DTO adı (varsayım)

        // Şablon yapısı
        sb.AppendLine("using Microsoft.AspNetCore.Mvc;");
        sb.AppendLine("using Microsoft.EntityFrameworkCore;");
        sb.AppendLine($"using {projectName}.Domain.Entities;"); // Entity'i kullanmak için
        sb.AppendLine($"using {projectName}.Domain.Repositories;"); // Repository'i kullanmak için
        sb.AppendLine();
        sb.AppendLine($"namespace {projectName}.Api.Controllers");
        sb.AppendLine("{");
        sb.AppendLine($"    [Route(\"api/[controller]\")]");
        sb.AppendLine("    [ApiController]");
        sb.AppendLine($"    public class {pluralEntityName}Controller : ControllerBase");
        sb.AppendLine("    {");
        sb.AppendLine($"        private readonly {repositoryInterface} {repositoryVariableName};");
        sb.AppendLine();

        // Constructor (Repository Enjeksiyonu)
        sb.AppendLine($"        public {pluralEntityName}Controller({repositoryInterface} {repositoryVariableName.Substring(1)})");
        sb.AppendLine("        {");
        sb.AppendLine($"            {repositoryVariableName} = {repositoryVariableName.Substring(1)};");
        sb.AppendLine("        }");
        sb.AppendLine();

        // GET ALL Metodu
        sb.AppendLine("        [HttpGet]");
        sb.AppendLine("        public async Task<IActionResult> GetAll()");
        sb.AppendLine("        {");
        sb.AppendLine($"            // Veritabanından tüm Entity'leri çekme (Mapping DTO'ya uygulama katmanında yapılır) ");
        sb.AppendLine($"            var list = await {repositoryVariableName}.GetAllAsync();");
        sb.AppendLine($"            // Normalde burada AutoMapper ile List<Entity> -> List<{returnDto}> dönüşümü yapılır");
        sb.AppendLine("            return Ok(list);");
        sb.AppendLine("        }");
        sb.AppendLine();

        // GET BY ID Metodu
        sb.AppendLine("        [HttpGet(\"{id}\")]");
        sb.AppendLine("        public async Task<IActionResult> GetById(int id)");
        sb.AppendLine("        {");
        sb.AppendLine($"            var entity = await {repositoryVariableName}.GetByIdAsync(id);");
        sb.AppendLine("            if (entity == null)");
        sb.AppendLine("            {");
        sb.AppendLine("                return NotFound();");
        sb.AppendLine("            }");
        sb.AppendLine("            return Ok(entity);");
        sb.AppendLine("        }");
        sb.AppendLine();

        // POST (CREATE) Metodu
        sb.AppendLine("        [HttpPost]");
        sb.AppendLine($"        public async Task<IActionResult> Create({entityType} model)"); // DTO yerine direkt Entity'i kabul etme varsayımı
        sb.AppendLine("        {");
        sb.AppendLine($"            var newEntity = await {repositoryVariableName}.AddAsync(model);");
        sb.AppendLine($"            return CreatedAtAction(nameof(GetById), new {{ id = newEntity.Id }}, newEntity);");
        sb.AppendLine("        }");
        sb.AppendLine();

        // PUT (UPDATE) Metodu
        sb.AppendLine("        [HttpPut(\"{id}\")]");
        sb.AppendLine($"        public async Task<IActionResult> Update(int id, {entityType} model)");
        sb.AppendLine("        {");
        sb.AppendLine("            if (id != model.Id)");
        sb.AppendLine("            {");
        sb.AppendLine("                return BadRequest();");
        sb.AppendLine("            }");
        sb.AppendLine($"            await {repositoryVariableName}.UpdateAsync(model);");
        sb.AppendLine("            return NoContent();");
        sb.AppendLine("        }");
        sb.AppendLine();

        // DELETE Metodu
        sb.AppendLine("        [HttpDelete(\"{id}\")]");
        sb.AppendLine("        public async Task<IActionResult> Delete(int id)");
        sb.AppendLine("        {");
        sb.AppendLine($"            var entity = await {repositoryVariableName}.GetByIdAsync(id);");
        sb.AppendLine("            if (entity == null)");
        sb.AppendLine("            {");
        sb.AppendLine("                return NotFound();");
        sb.AppendLine("            }");
        sb.AppendLine($"            await {repositoryVariableName}.DeleteAsync(entity);");
        sb.AppendLine("            return NoContent();");
        sb.AppendLine("        }");

        sb.AppendLine("    }");
        sb.AppendLine("}");

        return sb.ToString();
    }
}
