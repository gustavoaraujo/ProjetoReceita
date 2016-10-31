using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;

namespace WsReceita.Models
{
    [Table("Item")]
    public class Item
    {
        [Key]
        public int Id { get; set; }
        public int NumReceita { get; set; }
        [ForeignKey("NumReceita")]
        [DataMember]
        public Receita Receita { get; set; }
        [DataMember]
        public int RegAnvisa { get; set; }
        [DataMember]
        public string Instrucao { get; set; }
        [DataMember]
        public string Nome { get; set; }
        [DataMember]
        public string Uso { get; set; }
        [DataMember]
        public string ContraIndicacao { get; set; }
    }
}