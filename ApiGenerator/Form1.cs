using ApiGenerator.Common;
using ApiGenerator.Constants;
using ApiGenerator.InputModels;
using ApiGenerator.Services;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;

namespace ApiGenerator;

public partial class Form1 : Form
{
    private List<Tuple<ComboBox, TextBox>> propertyControls = new List<Tuple<ComboBox, TextBox>>();
    private int yLocation = 10;


    public Form1()
    {
        InitializeComponent();
        this.Load += new EventHandler(Form1_Load);
    }

    private string GetCSharpTypeName(PrimitiveType type)
    {
        return type switch
        {
            PrimitiveType.Int => "int",
            PrimitiveType.Long => "long",
            PrimitiveType.Decimal => "decimal",
            PrimitiveType.Bool => "bool",
            PrimitiveType.String => "string",
            PrimitiveType.DateTime => "DateTime",
            _ => type.ToString()
        };
    }


    private List<PropertyDefinition> CollectPropertyDefinitions()
    {
        return propertyControls
            .Select(cp => new PropertyDefinition
            {
                Name = cp.Item2.Text.Trim(),
                Type = (PrimitiveType)cp.Item1.SelectedItem
            })
            .Where(p => !string.IsNullOrWhiteSpace(p.Name))
            .ToList();
    }


    private void Form1_Load(object sender, EventArgs e)
    {
        btnSelectFolder.Click += btnSelectFolder_Click;
        btnAddProperty.Click += btnAddProperty_Click;
        btnGenerate.Click += btnGenerate_Click;

        AddPropertyRow("Id", PrimitiveType.Int, true);
        AddPropertyRow("Name", PrimitiveType.String, false);
    }

    private void btnSelectFolder_Click(object sender, EventArgs e)
    {
        using (var fbd = new FolderBrowserDialog())
        {
            DialogResult result = fbd.ShowDialog();
            if (result == DialogResult.OK && !string.IsNullOrWhiteSpace(fbd.SelectedPath))
            {
                txtProjectPath.Text = fbd.SelectedPath;
            }
        }
    }

    private void AddPropertyRow(string defaultName = "", PrimitiveType defaultType = PrimitiveType.String, bool isReadOnly = false)
    {
        ComboBox cmbType = new ComboBox();
        cmbType.DataSource = Enum.GetValues(typeof(PrimitiveType));
        cmbType.SelectedItem = defaultType;
        cmbType.Location = new Point(10, yLocation);
        cmbType.Width = 100;
        cmbType.Enabled = !isReadOnly;

        TextBox txtName = new TextBox();
        txtName.Text = defaultName;
        txtName.Location = new Point(120, yLocation);
        txtName.Width = 150;
        txtName.ReadOnly = isReadOnly;

        pnlProperties.Controls.Add(cmbType);
        pnlProperties.Controls.Add(txtName);

        propertyControls.Add(new Tuple<ComboBox, TextBox>(cmbType, txtName));
        yLocation += 30;
    }

    private void btnAddProperty_Click(object sender, EventArgs e)
    {
        AddPropertyRow();
    }

    private void btnGenerate_Click(object sender, EventArgs e)
    {
        if (string.IsNullOrWhiteSpace(txtProjectName.Text) ||
            string.IsNullOrWhiteSpace(txtProjectPath.Text) ||
            string.IsNullOrWhiteSpace(txtEntityName.Text))
        {
            MessageBox.Show("Lütfen tüm alanları doldurun.", "Uyarı", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            return;
        }

        try
        {
            string projectName = txtProjectName.Text.Trim();
            string projectRootPath = Path.Combine(txtProjectPath.Text, projectName);
            string entityName = txtEntityName.Text.Trim();
            entityName = char.ToUpper(entityName[0]) + entityName.Substring(1);
            bool isSwaggerSelected = chkIncludeSwagger.Checked;

            if (!Directory.Exists(projectRootPath))
            {
                Directory.CreateDirectory(projectRootPath);
            }
            else
            {
                var result = MessageBox.Show("Bu klasör zaten mevcut. Üzerine yazılsın mı?", "Onay", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (result == DialogResult.No) return;
            }

            var cliService = new CliService(projectRootPath, projectName);
            cliService.InitializeProject(includeSwagger: isSwaggerSelected);

            string templateDir = Path.Combine(Application.StartupPath, "Templates");
            var templateService = new TemplateService(templateDir);

            var rawProperties = CollectPropertyDefinitions();
            var propertyList = rawProperties.Select(p => (GetCSharpTypeName(p.Type), p.Name)).ToList();
            string propsCode = templateService.BuildPropertiesCode(propertyList);

            var replacements = new Dictionary<string, string>
         {
             { "##PROJECT_NAME##", projectName },
             { "##ENTITY_NAME##", entityName },
             { "##PROPERTIES##", propsCode },
             { "##ENTITY_NAME_LOWER##", entityName.ToLower() }
         };

            if (isSwaggerSelected)
            {
                replacements.Add("##SWAGGER_SERVICES##", "builder.Services.AddEndpointsApiExplorer();\r\nbuilder.Services.AddSwaggerGen();");

                string middleware = "if (app.Environment.IsDevelopment())\r\n" +
                                    "{\r\n" +
                                    "    app.UseSwagger();\r\n" +
                                    "    app.UseSwaggerUI();\r\n" +
                                    "}";
                replacements.Add("##SWAGGER_MIDDLEWARE##", middleware);
            }
            else
            {
                replacements.Add("##SWAGGER_SERVICES##", "");
                replacements.Add("##SWAGGER_MIDDLEWARE##", "");
            }

            File.WriteAllText(Path.Combine(projectRootPath, "Models", $"{entityName}.cs"),
                templateService.GenerateContent(TemplateType.Entity, replacements));

            File.WriteAllText(Path.Combine(projectRootPath, "Data", "AppDbContext.cs"),
                templateService.GenerateContent(TemplateType.DbContext, replacements));

            string propertiesPath = Path.Combine(projectRootPath, "Properties");
            if (!Directory.Exists(propertiesPath)) Directory.CreateDirectory(propertiesPath);

            File.WriteAllText(Path.Combine(propertiesPath, "launchSettings.json"),
                templateService.GenerateContent(TemplateType.LaunchSettings, replacements));


            File.WriteAllText(Path.Combine(projectRootPath, "Repositories", "IGenericRepository.cs"),
                templateService.GenerateContent(TemplateType.IGenericRepository, replacements));

            File.WriteAllText(Path.Combine(projectRootPath, "Repositories", "GenericRepository.cs"),
                templateService.GenerateContent(TemplateType.GenericRepository, replacements));

            File.WriteAllText(Path.Combine(projectRootPath, "Repositories", $"I{entityName}Repository.cs"),
                templateService.GenerateContent(TemplateType.IEntityRepository, replacements));

            File.WriteAllText(Path.Combine(projectRootPath, "Repositories", $"{entityName}Repository.cs"),
                templateService.GenerateContent(TemplateType.EntityRepository, replacements));

            File.WriteAllText(Path.Combine(projectRootPath, "Repositories", "IRepositoryManager.cs"),
                templateService.GenerateContent(TemplateType.IRepositoryManager, replacements));

            File.WriteAllText(Path.Combine(projectRootPath, "Repositories", "RepositoryManager.cs"),
                templateService.GenerateContent(TemplateType.RepositoryManager, replacements));

            File.WriteAllText(Path.Combine(projectRootPath, "Controllers", $"{entityName}sController.cs"),
                templateService.GenerateContent(TemplateType.Controller, replacements));

            File.WriteAllText(Path.Combine(projectRootPath, "Program.cs"),
                templateService.GenerateContent(TemplateType.Program, replacements));

            File.WriteAllText(Path.Combine(projectRootPath, "appsettings.json"),
                templateService.GenerateContent(TemplateType.AppSettings, replacements));

            if (chkAutoMigrate.Checked)
            {
                Cursor = Cursors.WaitCursor;
                cliService.ExecuteMigrations();
                Cursor = Cursors.Default;
            }

            var runResult = MessageBox.Show("Proje ve Veritabanı hazır! Başlatılsın mı?", "Başarılı", MessageBoxButtons.YesNo);
            if (runResult == DialogResult.Yes)
            {
                cliService.RunProject();
            }
            System.Diagnostics.Process.Start("explorer.exe", projectRootPath);
        }
        catch (Exception ex)
        {
            MessageBox.Show($"Hata: {ex.Message}", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }

    private void chkIncludeSwagger_CheckedChanged(object sender, EventArgs e)
    {

    }
}