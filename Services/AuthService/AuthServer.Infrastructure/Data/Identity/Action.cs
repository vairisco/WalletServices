using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace AuthService.Infrastructure.Data.Identity
{
    public class Action
    {
        [Key]
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        [ForeignKey("Module")]
        public int? ModuleId { get; set; }
        public virtual Module Module { get; set; }
        [ForeignKey("IdentityRole")]
        public string RoleId { get; set; }
        public IdentityRole IdentityRole { get; set; }
    }
}
