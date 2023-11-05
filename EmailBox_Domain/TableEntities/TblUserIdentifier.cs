using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmailBox_Domain.TableEntities
{
    [Table("TblUserIdentifier")]
    public class TblUserIdentifier
    {
        [Key]
        public long UserIdentifierId { get; set; }
        public string?  Email { get; set; }
        public string? Code { get; set; }
        public DateTime?  DateTime { get; set; }

    }
}
