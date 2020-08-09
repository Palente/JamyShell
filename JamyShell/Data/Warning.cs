using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace JamyShell.Data
{
    [Table("warnings")]
    public class Warning
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public ulong WarnId { get; set; }
        public ulong AuthorId { get; set; }
        public ulong VictimeId { get; set; }
        public string Reason { get; set; }
        public DateTime Created { get; set; }
    }
}
