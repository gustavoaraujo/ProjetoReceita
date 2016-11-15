using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;

namespace WsReceita.Models
{
    [Table("Receita")]
    [DataContract(IsReference = true)]
    public class Receita
    {
        [Key]
        [DataMember]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int NumReceita { get; set; }

        [DataMember]
        public DateTime Data { get; set; }
        
        public string Crm { get; set; }
        [DataMember]
        [ForeignKey("Crm")]
        public Medico Medico { get; set; }
        
        public string Cpf { get; set; }
        [DataMember]
        [ForeignKey("Cpf")]
        public Paciente Paciente { get; set; }

        [DataMember]
        public List<Item> ItensReceita { get; set; }

        [DataMember]
        public bool Utilizada { get; set; }

        [DataMember]
        public bool Cancelada { get; set; }

    }
}