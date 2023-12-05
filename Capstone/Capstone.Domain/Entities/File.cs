using Capstone.Domain.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Capstone.Domain.Entities
{
    public class File : AuditableBaseEntity
    {
        public string Name { get; set; }
        public string Path { get; set; }
    }
}
