using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace EmailBox_Domain.TableEntities
{
    [Table("TblEmailBox")]
    public class TblEmailBox
    {
        [Key]
        public long BoxId { get; set; }
        public long Sender_UserId { get; set; }
        public long ReciverId_UserId { get; set; }
        public string? EmailTitle { get; set; }
        public string? LastEmailBody { get; set; }
        public DateTime? DateTime { get; set; }
        public string? SenderName { get; set; }
        public string? ReciverName { get; set; }


    }
}
