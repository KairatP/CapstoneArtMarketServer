using Capstone.Application.Features.Accounts.Queries.GetProfile;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Capstone.Application.Features.Posts.Queries
{
    public class PostDto
    {
        public PostDto()
        {
            Urls = new List<string>();
            Author = new GetProfileDTO();
        }

        public Guid Id { get; set; }
        public int Price { get; set; }
        public string Description { get; set; }
        public decimal Width { get; set; }
        public decimal Height { get; set; }
        public string Color { get; set; }
        public string Pano { get; set; }
        public List<string> Urls { get; set; }

        public GetProfileDTO Author { get; set; }
    }
}
