using Capstone.Domain.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Capstone.Domain.Entities
{
    public class PostFile : BaseEntity
    {
        public Guid PostId { get; set; }
        public Guid FileId { get; set; }

        public File File { get; set; }
        public Post Post { get; set; }
    }
}
