﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;

namespace WsReceita.Models
{
    [Table("Usuario")]
    [DataContract(IsReference = true)]
    public class Usuario
    {
        [Key]
        public int IdUsuario { get; set; }
        [DataMember]
        public string Login { get; set; }
        [DataMember]
        public string Senha { get; set; }
        [ForeignKey("Medico")]
        public string Crm { get; set; }
        [DataMember]
        public Medico Medico { get; set; }
    }
}