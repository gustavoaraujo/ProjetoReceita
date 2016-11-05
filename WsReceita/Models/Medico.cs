using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;

namespace WsReceita.Models
{
    [Table("Medico")]
    public class Medico
    {
        [Key]
        [DataMember]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public string CRM { get; set; }
        [DataMember]
        public string Nome { get; set; }
        [DataMember]
        public List<Receita> Receitas { get; set; }
        public Usuario Usuario { get; set; }
    }
}