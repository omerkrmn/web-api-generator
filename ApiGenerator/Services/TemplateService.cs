using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace ApiGenerator.Services;
public class TemplateService
{
    private readonly string _templateBasePath;

    public TemplateService(string templateBasePath)
    {
        _templateBasePath = templateBasePath;
    }

    public string GenerateContent(TemplateType type, Dictionary<string, string> replacements)
    {
        string fileName = type.GetFileName();
        string templatePath = Path.Combine(_templateBasePath, fileName);

        if (!File.Exists(templatePath))
            throw new FileNotFoundException($"Şablon dosyası bulunamadı: {fileName}");

        string content = File.ReadAllText(templatePath);

        foreach (var replacement in replacements)
        {
            content = content.Replace(replacement.Key, replacement.Value);
        }

        return content;
    }

    public string BuildPropertiesCode(List<(string Type, string Name)> properties)
    {
        var sb = new StringBuilder();
        foreach (var prop in properties)
        {
            sb.AppendLine($"        public {prop.Type} {prop.Name} {{ get; set; }}");
        }
        return sb.ToString().TrimEnd();
    }
}
public enum TemplateType
{
    Entity,
    DbContext,
    IGenericRepository,
    GenericRepository,
    IEntityRepository,
    EntityRepository,
    Controller,
    RepoManagerProperty,
    RepoManagerField,
    RepoManagerCtor,
    IRepositoryManager,
    RepositoryManager,
    Program,
    AppSettings,
    LaunchSettings // Yeni eklendi
}

public static class TemplateTypeExtensions
{
    public static string GetFileName(this TemplateType type)
    {
        return type switch
        {
            TemplateType.Entity => "EntityClassTemplate.cstemp",
            TemplateType.IEntityRepository => "IEntityRepositoryTemplate.cstemp",
            TemplateType.EntityRepository => "EntityRepositoryTemplate.cstemp",
            TemplateType.Controller => "ControllerClassTemplate.cstemp",
            TemplateType.RepoManagerProperty => "RepoManager_Property.cstemp",
            TemplateType.RepoManagerField => "RepoManager_Field.cstemp",
            TemplateType.RepoManagerCtor => "RepoManager_Ctor_Assignment.cstemp",
            TemplateType.IGenericRepository => "IGenericRepositoryTemplate.cstemp",
            TemplateType.GenericRepository => "GenericRepositoryTemplate.cstemp",
            TemplateType.IRepositoryManager => "IRepositoryManagerTemplate.cstemp",
            TemplateType.RepositoryManager => "RepositoryManagerTemplate.cstemp",
            TemplateType.DbContext => "AppDbContextTemplate.cstemp",
            TemplateType.Program => "ProgramTemplate.cstemp",
            TemplateType.AppSettings => "AppSettingsTemplate.cstemp",
            TemplateType.LaunchSettings => "LaunchSettingsTemplate.cstemp", // Yeni eklendi
            _ => throw new ArgumentOutOfRangeException(nameof(type), type, null)
        };
    }
}