using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Practice.Foundation.Infrastructure.DemoTypes
{
    public class ExcelData
    {
        public List<string> Columns { get; set; } = new List<string>();
        public Dictionary<string, List<string>> Data { get; set; } = new Dictionary<string, List<string>>();
    }
}
