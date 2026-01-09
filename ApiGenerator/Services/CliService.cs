using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApiGenerator.Services;

public class CliService
{
    private readonly string _projectFolderPath;
    private readonly string _projectName;

    public CliService(string projectFolderPath, string projectName)
    {
        _projectFolderPath = projectFolderPath;
        _projectName = projectName;
    }
    public void RunProject()
    {
        Process.Start(new ProcessStartInfo
        {
            FileName = "cmd.exe",
            Arguments = "/c dotnet run",
            WorkingDirectory = _projectFolderPath,
            UseShellExecute = true
        });

        Thread.Sleep(3000);
        Process.Start(new ProcessStartInfo
        {
            FileName = "http://localhost:5000/swagger/index.html",
            UseShellExecute = true
        });
    }
    public void ExecuteMigrations()
    {
        RunCommand("dotnet ef migrations add InitialCreate --output-dir Data/Migrations", _projectFolderPath);

        RunCommand("dotnet ef database update", _projectFolderPath);
    }
    private void RunCommand(string command, string workingDirectory)
    {
        if (!Directory.Exists(workingDirectory))
        {
            Directory.CreateDirectory(workingDirectory);
        }

        var processInfo = new ProcessStartInfo("cmd.exe", "/c " + command)
        {
            WorkingDirectory = workingDirectory,
            CreateNoWindow = true,
            UseShellExecute = false,
            RedirectStandardOutput = true,
            RedirectStandardError = true
        };

        using var process = Process.Start(processInfo);
        process?.WaitForExit();

        if (process?.ExitCode != 0)
        {
            string error = process?.StandardError.ReadToEnd();
            throw new Exception($"Komut hatası: {command}\nDetay: {error}");
        }
    }

    public void InitializeProject(bool includeSwagger)
    {
        Directory.CreateDirectory(_projectFolderPath);

        RunCommand($"dotnet new webapi -n {_projectName} -o . --force --framework net9.0", _projectFolderPath);

        RunCommand("dotnet add package Microsoft.EntityFrameworkCore.SqlServer -v 9.0.*", _projectFolderPath);
        RunCommand("dotnet add package Microsoft.EntityFrameworkCore.Design -v 9.0.*", _projectFolderPath);
        RunCommand("dotnet add package Microsoft.EntityFrameworkCore.Tools -v 9.0.*", _projectFolderPath);

        if (includeSwagger)
        {
            RunCommand("dotnet add package Swashbuckle.AspNetCore --version 7.2.0", _projectFolderPath);
        }

        CreateProjectFolders();
    }

    private void CreateProjectFolders()
    {
        string[] folders = { "Models", "Repositories", "Controllers", "Services", "Data" };
        foreach (var folder in folders)
        {
            string path = Path.Combine(_projectFolderPath, folder);
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }
        }
    }
}