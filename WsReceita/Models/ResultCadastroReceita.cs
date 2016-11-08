using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WsReceita.Models
{
    public class ResultCadastroReceita
    {
        public ResultCadastroReceita()
        {
        }

        public ResultCadastroReceita(string msg, Receita receita)
        {
            this.Msg = msg;
            this.CPF = receita.CPF;
            this.CRM = receita.CRM;
            this.DataCadastro = DateTime.Now;
            this.NumeroReceita = receita.NumReceita;
        }

        public string Msg { get; set; }
        public string CPF { get; set; }
        public string CRM { get; set; }
        public DateTime DataCadastro { get; set; }
        public int NumeroReceita { get; set; }
    }
}