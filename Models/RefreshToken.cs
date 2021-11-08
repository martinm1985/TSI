using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Crud.Models
{
    public class RefreshToken
    {
        [Key]
        public string Token { get; set; }

        public string JwtID { get; set; }

        public DateTime CreationDate { get; set; }

        public DateTime ExpiryDate { get; set; }

        public bool Invalidated { get; set; }

        [ForeignKey("User")]
        public string UserId { get; set; }

        
        public virtual User User { get; set; }

    }
}
