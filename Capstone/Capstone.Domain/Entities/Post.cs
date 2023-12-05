using Capstone.Domain.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Capstone.Domain.Entities
{
    public class Post : AuditableBaseEntity
    {
        public Post()
        {
            Pictures = new List<PostFile>();
        }

        public int Price { get; set; }
        public string Description { get; set; }
        public decimal Width { get; set; }
        public decimal Height { get; set; }
        public string Color { get; set; }
        public string Pano { get; set; }
        public Guid? CountryId { get; set; }
        public Guid? CityId { get; set; }


        [ForeignKey("CreatedBy")]
        public User Author { get; set; }

        public ICollection<PostFile> Pictures { get; set; }
    }
}
