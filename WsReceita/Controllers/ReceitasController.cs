﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Description;
using WsReceita.Context;
using WsReceita.Models;

namespace WsReceita.Controllers
{
    public class ReceitasController : ApiController
    {
        private Context.Context db = new Context.Context();
        
        [Route("CancelarReceitaMedica")]
        [HttpPost]
        public string CancelarReceitaMedica(NumeroReceita numeroReceita)
        {
            var receita = this.ObterReceitaMedica(numeroReceita);
            if (receita == null)
                return "Não existe receita com esse número.";

            if (receita.Utilizada)
                return "Receita não pode ser cancelada porque já foi utilizada.";

            if (receita.Cancelada)
                return "Receita já foi cancelada.";

            receita.Cancelada = true;

            try
            {
                db.SaveChanges();
            }
            catch (Exception ex)
            {
                return "Erro ao atualizar os dados." + ex.Message;
            }

            return "Receita cancelada com sucesso";
        }

        [Route("UtilizarReceitaMedica")]
        [HttpPost]
        public string UtilizarReceitaMedica(NumeroReceita numeroReceita)
        {
            var receita = this.ObterReceitaMedica(numeroReceita);
            if (receita == null)
                return "Não existe receita com esse número.";
            
            if (receita.Cancelada)
                return "Receita não pode ser utilizada porque já foi cancelada.";

            if (receita.Utilizada)
                return "Receita já foi utilizada.";
            
            receita.Utilizada = true;
            try
            {
                db.SaveChanges();
            }
            catch (Exception ex)
            {
                return "Erro ao atualizar os dados." + ex.Message;
            }
            
            return "Receita utilizada com sucesso";
        }

        [Route("ObterReceitaMedica")]
        [HttpPost]
        public Receita ObterReceitaMedica(NumeroReceita numeroReceita)
        {
            Receita receita = db.Receita.Find(numeroReceita.Numero);
            
            if (receita == null)
                return null;
            
            receita.Medico = db.Medico.ToList().Where(y => y.Crm == receita.Crm).FirstOrDefault();
            receita.Paciente = db.Paciente.ToList().Where(y => y.Cpf == receita.Cpf).FirstOrDefault();
            receita.ItensReceita = db.Item.ToList().Where(y => y.NumReceita == receita.NumReceita).ToList();

            return receita;
        }

        [Route("CadastrarReceitaMedica")]
        [HttpPost]
        public ResultCadastroReceita CadastrarReceitaMedica(Receita receita)
        {
            var listaMedico = db.Medico.ToList<Medico>();
            var medico = listaMedico.Where(x => x.Crm == receita.Medico.Crm &&
                                            x.Nome == receita.Medico.Nome);

            if (medico.Count() == 0)
                db.Medico.Add(receita.Medico);
            else
                receita.Medico = medico.FirstOrDefault();

            var paciente = db.Paciente.Where(x => x.Cpf == receita.Paciente.Cpf &&
                                            x.Nome == receita.Paciente.Nome);
            if (paciente.Count() == 0)
                db.Paciente.Add(receita.Paciente);
            else
                receita.Paciente = paciente.FirstOrDefault();

            db.Receita.Add(receita);

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateException ex)
            {
                if (ReceitaExists(receita.NumReceita))
                {
                    return new ResultCadastroReceita("Erro ao cadastrar receita: receita já existe no sistema.", receita);
                }
                else
                {
                    return new ResultCadastroReceita("Erro ao cadastrar receita: " + ex.Message, receita);
                }
            }

            return new ResultCadastroReceita("Cadastrado com sucesso.", receita);
        }

        private bool ReceitaExists(int id)
        {
            return db.Receita.Count(e => e.NumReceita == id) > 0;
        }

    }
}