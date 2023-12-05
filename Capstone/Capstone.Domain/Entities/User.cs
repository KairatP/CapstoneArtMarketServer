using Capstone.Domain.Entities.Identity;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Capstone.Domain.Entities
{

    public class User : IdentityUser<Guid>
    {
        public User()
        {
            Posts = new List<Post>();
        }

        public string Name { get; set; }
        public int Code { get; set; }
        public Guid? CountryId { get; set; }
        public Guid? CityId { get; set; }
        public Guid? AvatarId { get; set; }

        public Country Country { get; set; }
        public City City { get; set; }
        public File Avatar { get; set; }

        public virtual ICollection<UserClaim> Claims { get; set; }
        public virtual ICollection<UserLogin> Logins { get; set; }
        public virtual ICollection<UserToken> Tokens { get; set; }
        public virtual ICollection<UserRole> UserRoles { get; set; }

        public virtual ICollection<Post> Posts { get; set; }
    }
}
