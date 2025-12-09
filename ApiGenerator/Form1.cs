using ApiGenerator.Common;
using ApiGenerator.InputModels;
using ApiGenerator.Managers;
using ApiGenerator.Services;
using ApiGenerator.Utilities;
using System.Text;
using System.IO;
using System.Linq;
using System.Drawing;
using ApiGenerator.Constants; 

namespace ApiGenerator;

public partial class Form1 : Form
{
    private List<Tuple<ComboBox, TextBox>> propertyControls = new List<Tuple<ComboBox, TextBox>>();
    private int yLocation = 10;

    private readonly TemplateProcessor _templateProcessor = new TemplateProcessor();
    private readonly FileUpdater _fileUpdater = new FileUpdater();
    private readonly ProjectFileManager _projectManager = new ProjectFileManager();
    private readonly DependencyManager _dependencyManager = new DependencyManager();
    private readonly EntityCodeGenerator _entityCodeGenerator = new EntityCodeGenerator();
    private readonly ControllerGenerator _controllerGenerator = new ControllerGenerator();

    public Form1()
    {
        InitializeComponent();
        this.Load += new EventHandler(Form1_Load);
    }

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
            default: return type.ToString();
        }
    }
    private string ProcessTemplate(string templateName, string entityName, string projectName)
    {
        string template = _templateProcessor.ProcessTemplate(templateName, entityName, projectName);

        if (templateName == "EntityClassTemplate.cstemp")
        {
            var properties = CollectPropertyDefinitions();
            var propertiesCode = new StringBuilder();
            foreach (var prop in properties)
            {
                string cSharpType = GetCSharpTypeName(prop.Type);
                propertiesCode.AppendLine($"        public {cSharpType} {prop.Name} {{ get; set; }}");
            }
            template = template.Replace("##PROPERTIES##", propertiesCode.ToString());
        }
        return template;
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


    private async void btnGenerate_Click(object sender, EventArgs e)
    {
        if (string.IsNullOrWhiteSpace(txtEntityName.Text) || string.IsNullOrWhiteSpace(txtProjectPath.Text) || !Directory.Exists(txtProjectPath.Text))
        {
            MessageBox.Show("Lütfen Entity Adını girin ve geçerli bir Proje Konumu seçin.", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            return;
        }

        try
        {
            var entityDefinition = new EntityDefinition
            {
                EntityName = txtEntityName.Text.Trim(),
                Properties = CollectPropertyDefinitions()
            };

            string entityName = entityDefinition.EntityName.First().ToString().ToUpper() + entityDefinition.EntityName.Substring(1);
            string solutionRootPath = txtProjectPath.Text;
            string projectNamePrefix = entityName + "Api";
            string solutionFilePath = Path.Combine(solutionRootPath, projectNamePrefix + ".sln");
            bool isFirstRun = !File.Exists(solutionFilePath);

            string apiPath = Path.Combine(solutionRootPath, projectNamePrefix + ".Api");
            string domainPath = Path.Combine(solutionRootPath, projectNamePrefix + ".Domain");
            string infrastructurePath = Path.Combine(solutionRootPath, projectNamePrefix + ".Infrastructure");
            string apiCsproj = Path.Combine(apiPath, projectNamePrefix + ".Api.csproj");
            string domainCsproj = Path.Combine(domainPath, projectNamePrefix + ".Domain.csproj");

            string dbContextPath = Path.Combine(infrastructurePath, "Context", "AppDbContext.cs");
            string iRepositoryManagerPath = Path.Combine(domainPath, "Repositories", "IRepositoryManager.cs");
            string repositoryManagerPath = Path.Combine(infrastructurePath, "Repositories", "RepositoryManager.cs");

            if (isFirstRun)
            {
                _projectManager.CreateSolution(solutionRootPath, projectNamePrefix);
                _projectManager.CreateApiProject(solutionRootPath, projectNamePrefix);
                _dependencyManager.AddProjectReference(apiCsproj, domainCsproj);
            }

        
            string entityCode = ProcessTemplate("EntityClassTemplate.cstemp", entityName, projectNamePrefix);
            string entityFilePath = Path.Combine(domainPath, "Entities", entityName + ".cs");
            Directory.CreateDirectory(Path.GetDirectoryName(entityFilePath));
            File.WriteAllText(entityFilePath, entityCode);

            string iRepoCode = ProcessTemplate("IEntityRepositoryTemplate.cstemp", entityName, projectNamePrefix);
            string iRepoFilePath = Path.Combine(domainPath, "Repositories", "I" + entityName + "Repository.cs");
            Directory.CreateDirectory(Path.GetDirectoryName(iRepoFilePath));
            File.WriteAllText(iRepoFilePath, iRepoCode);

            string repoCode = ProcessTemplate("EntityRepositoryTemplate.cstemp", entityName, projectNamePrefix);
            string repoFilePath = Path.Combine(infrastructurePath, "Repositories", entityName + "Repository.cs");
            Directory.CreateDirectory(Path.GetDirectoryName(repoFilePath));
            File.WriteAllText(repoFilePath, repoCode);

            string controllerCode = ProcessTemplate("ControllerClassTemplate.cstemp", entityName, projectNamePrefix);
            string controllerFilePath = Path.Combine(apiPath, "Controllers", entityName + "sController.cs");
            Directory.CreateDirectory(Path.GetDirectoryName(controllerFilePath));
            File.WriteAllText(controllerFilePath, controllerCode);


            string dbSetLine = $"        public DbSet<{entityName}> {entityName}s {{ get; set; }}";
            string iRepoManagerProp = _templateProcessor.ProcessTemplate("IRepositoryManager_Property.cstemp", entityName, projectNamePrefix);
            string repoManagerField = _templateProcessor.ProcessTemplate("RepoManager_Field.cstemp", entityName, projectNamePrefix);
            string repoManagerCtor = _templateProcessor.ProcessTemplate("RepoManager_Ctor_Assignment.cstemp", entityName, projectNamePrefix);
            string repoManagerProp = _templateProcessor.ProcessTemplate("RepoManager_Property.cstemp", entityName, projectNamePrefix);

            _fileUpdater.InsertContent(dbContextPath, dbSetLine, "protected override void OnModelCreating", true);

            _fileUpdater.InsertContent(iRepositoryManagerPath, iRepoManagerProp, "Task SaveAsync()", true);

            _fileUpdater.InsertContent(repositoryManagerPath, repoManagerField, "private readonly AppDbContext", false);
            _fileUpdater.InsertContent(repositoryManagerPath, repoManagerCtor, "public RepositoryManager(AppDbContext context)", false);
            _fileUpdater.InsertContent(repositoryManagerPath, repoManagerProp, "public async Task SaveAsync()", true);

            MessageBox.Show($"'{projectNamePrefix}' projesi ve '{entityName}' Entity kodları başarıyla oluşturuldu.",
                            "İşlem Tamamlandı", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
        catch (Exception ex)
        {
            MessageBox.Show($"Kritik Hata Oluştu:\n{ex.Message}", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }
}