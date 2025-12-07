using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApiGenerator.Managers;

public class DependencyManager
{
    private void RunDotNetCommand(string arguments)
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
                CreateNoWindow = true
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

    public void AddLibrary(string projectPath, string libraryName, string version = "")
    {
        // dotnet add <PROJE_YOLU> package <PAKET_ADI> --version <VERSİYON>
        string versionArg = string.IsNullOrEmpty(version) ? "" : $"--version \"{version}\"";
        string arguments = $@"add ""{projectPath}"" package ""{libraryName}"" {versionArg}";
        RunDotNetCommand(arguments);
    }

    public void AddProjectReference(string sourceProjectPath, string targetProjectPath)
    {
        // dotnet add <KAYNAK_PROJE_YOLU> reference <HEDEF_PROJE_YOLU>
        string arguments = $@"add ""{sourceProjectPath}"" reference ""{targetProjectPath}""";
        RunDotNetCommand(arguments);
    }
}