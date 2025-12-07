using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApiGenerator.Utilities;

public class FileUpdater
{
    public void InsertContent(string filePath, string contentToInsert, string hookLine, bool insertBefore = true)
    {
        if (!File.Exists(filePath))
        {
            throw new FileNotFoundException($"Güncellenecek dosya bulunamadı: {filePath}");
        }

        var lines = File.ReadAllLines(filePath).ToList();
        int index = -1;

        for (int i = 0; i < lines.Count; i++)
        {
            if (lines[i].Contains(hookLine))
            {
                index = i;
                break;
            }
        }

        if (index == -1)
        {
            throw new InvalidOperationException($"'{hookLine}' kancası dosyada bulunamadı. Güncelleme yapılamadı.");
        }

        if (insertBefore)
        {
            lines.Insert(index, contentToInsert);
        }
        else
        {
            lines.Insert(index + 1, contentToInsert);
        }

        File.WriteAllLines(filePath, lines);
    }
}
