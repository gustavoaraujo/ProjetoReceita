﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;

namespace WsReceita.Models
{
    [Table("Paciente")]
    public class Paciente
    {
        public Paciente()
        {
            Receitas = new List<Receita>();
        }

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        [DataMember]
        public string Cpf { get; set; }
        [DataMember]
        public string Nome { get; set; }
        [DataMember]
        public List<Receita> Receitas { get; set; }
    }
}