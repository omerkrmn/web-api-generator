using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApiGenerator.InputModels;

public class EntityDefinition
{
    /// <summary>
    /// bu entitynin ismi ve dosya ismi olarak kullanılacak
    /// </summary>
    public string EntityName { get; set; }
    /// <summary>
    /// property tanımlarının listesi <br/>
    /// <code>
    /// int, double, string, datetime, bool, decimal, float, long, short, byte, char
    /// </code>
    /// </summary>
    public List<PropertyDefinition> Properties { get; set; } = new List<PropertyDefinition>();
}
