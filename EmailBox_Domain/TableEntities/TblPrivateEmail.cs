using Microsoft.VisualBasic.FileIO;
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
    [Table("TblPrivateEmail")]
    public class TblPrivateEmail
    {
        [Key]
        public long PrivateEmailID { get; set; }

        public long Sender_UserId { get; set; }
        public long ReciverId_UserId { get; set; }
        public string? EmailTitle { get; set; }
        public string? EmailBody { get; set; }
        public DateTime? CreatedDateTime { get; set; }
        public string? FIlePath { get; set; }
        public string? FileType { get; set; }
        public bool? IsFile { get; set; }
        public bool? IsDeleted { get; set; }
        public long? BoxID { get; set; }

    }
}
