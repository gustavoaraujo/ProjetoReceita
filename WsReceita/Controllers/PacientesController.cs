using System;
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
    public class PacientesController : ApiController
    {
        private Context.Context db = new Context.Context();

        [Route("ObterPacientes")]
        [HttpGet]
        public List<Paciente> GetPaciente()
        {
            var pacientes = db.Paciente.ToList();

            pacientes.ForEach(p => p.Receitas.AddRange(db.Receita.AsNoTracking<Receita>().ToList().Where(x => x.Cpf == p.Cpf).Distinct()));
            
            return pacientes;
        }

        [Route("CadastrarPaciente")]
        [HttpPost]
        public string CadastrarPaciente(Paciente p)
        {
            if (!ModelState.IsValid)
            {
                return "Falha técnica ao cadastrar paciente. Consulte o suporte.";
            }

            if (this.PacienteExists(p.Cpf))
            {
                return "Erro ao cadastrar paciente: paciente já cadastrado";
            }

            db.Paciente.Add(p);

            try
            {
                db.SaveChanges();
            }
            catch (Exception ex)
            {
                if (PacienteExists(p.Cpf))
                {
                    return "Erro ao cadastrar paciente: paciente já existe no sistema.";
                }
                else
                {
                    return "Erro ao cadastrar paciente: " + ex.Message;
                }
            }

            return "Cadastrado com sucesso.";
        }
        private bool PacienteExists(string login)
        {
            var usuario = db.Paciente.Where(x => x.Cpf == login);

            return usuario.Count() > 0;
        }

    }
}