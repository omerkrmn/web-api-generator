using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApiGenerator.InputModels;

public class EntityDefinition
{

    public string EntityName { get; set; }
    public List<PropertyDefinition> Properties { get; set; } = new List<PropertyDefinition>();
}
