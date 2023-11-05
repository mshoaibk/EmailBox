using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmailBox_Domain.ViewModel
{
    public class EmailSendReq
    {
        public string? emailTitle { set; get; }
        public long senderId { set; get; }
        public string? recipientEmail { set; get; }
        public string? emailBody { set; get; }
        public string? type { set; get; }
        public string? filePath { set; get; }
        public string? fileType { set; get; }
        public long? boxId { set; get; }
    }
    public class EmailSendResponse
    {
        public long? privateEmailID { get; set; }
        public long? sender_UserId { get; set; }
        public long? reciverId_UserId { get; set; }
        public string? emailBody { get; set; }
        public DateTime? createdDateTime { get; set; }
        public string? fIlePath { get; set; }
        public string? fileType { get; set; }
        public bool? isFile { get; set; }
        //public bool? IsDeleted { get; set; }
        public long? boxID { get; set; }
        public bool? IsSuccess{ get; set; }
    }
}
