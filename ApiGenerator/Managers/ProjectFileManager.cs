using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApiGenerator.Managers;

public class ProjectFileManager
{
    private void RunDotNetCommand(string arguments, string workingDirectory)
    {
        var process = new Process
        {
            StartInfo = new ProcessStartInfo
            {
                FileName = "dotnet",
                Arguments = arguments,
                RedirectStandardOutput = true,
                RedirectStandardError = true,
                UseShellExecute = false,
                CreateNoWindow = true,
                WorkingDirectory = workingDirectory
            }
        };
        process.Start();
        string error = process.StandardError.ReadToEnd();
        process.WaitForExit();

        if (process.ExitCode != 0)
        {
            throw new Exception($"CLI Komutu Başarısız: dotnet {arguments}\nHata: {error}");
        }
    }

    public void CreateSolution(string solutionPath, string solutionName)
    {
        Directory.CreateDirectory(solutionPath);
        RunDotNetCommand($"new sln --name {solutionName}", solutionPath);
    }

    public void CreateApiProject(string solutionPath, string projectNamePrefix)
    {
        // API projesi ve 3 Class Library projesi oluşturulur
        RunDotNetCommand($"new webapi -n {projectNamePrefix}.Api", solutionPath);
        RunDotNetCommand($"new classlib -n {projectNamePrefix}.Domain", solutionPath);
        RunDotNetCommand($"new classlib -n {projectNamePrefix}.Infrastructure", solutionPath);

        // Solution'a projeleri ekle
        RunDotNetCommand($"sln add {projectNamePrefix}.Api/{projectNamePrefix}.Api.csproj", solutionPath);
        RunDotNetCommand($"sln add {projectNamePrefix}.Domain/{projectNamePrefix}.Domain.csproj", solutionPath);
        RunDotNetCommand($"sln add {projectNamePrefix}.Infrastructure/{projectNamePrefix}.Infrastructure.csproj", solutionPath);
    }
}
