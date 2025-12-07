using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApiGenerator.Managers
{
    public class TemplateProcessor
    {
        private const string TemplatesFolder = "Templates";

        public string ProcessTemplate(string templateFileName, string entityName, string projectName)
        {
            string baseDirectory = AppDomain.CurrentDomain.BaseDirectory;

            string templatePath = Path.Combine(baseDirectory, TemplatesFolder, templateFileName);

            if (!File.Exists(templatePath))
            {
                throw new FileNotFoundException($"Şablon dosyası bulunamadı! Lütfen '{templateFileName}' dosyasının şu dizinde olduğundan emin olun:\n{templatePath}", templatePath);
            }

            string template = File.ReadAllText(templatePath);

            template = template.Replace("##PROJECT_NAME##", projectName);
            template = template.Replace("##ENTITY_NAME##", entityName);
            template = template.Replace("##ENTITY_NAME_LOWER##", entityName.ToLower());

            return template;
        }
    }

}
