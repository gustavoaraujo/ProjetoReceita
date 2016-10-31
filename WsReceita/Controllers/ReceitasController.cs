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
    public class ReceitasController : ApiController
    {
        private Context.Context db = new Context.Context();

        // GET: api/Receitas
        //public List<Receita> GetReceita()
        //{
        //    var receita = db.Receita.ToList();

        //    receita.ForEach(x => 
        //    {
        //        x.Medico = db.Medico.ToList().Where(y => y.CRM == x.CRM).FirstOrDefault();
        //        x.Paciente = db.Paciente.ToList().Where(y => y.CPF == x.CPF).FirstOrDefault();
        //        x.ItensReceita = db.Item.ToList().Where(y => y.NumReceita == x.NumReceita).ToList();
        //    });

        //    return receita;
        //}

        // GET: api/Receitas/5
        [ResponseType(typeof(Receita))]
        public IHttpActionResult GetReceita(int id)
        {
            Receita receita = db.Receita.Find(id);
            receita.Medico = db.Medico.ToList().Where(y => y.CRM == receita.CRM).FirstOrDefault();
            receita.Paciente = db.Paciente.ToList().Where(y => y.CPF == receita.CPF).FirstOrDefault();
            receita.ItensReceita = db.Item.ToList().Where(y => y.NumReceita == receita.NumReceita).ToList();

            if (receita == null)
            {
                return NotFound();
            }

            return Ok(receita);
        }

        // PUT: api/Receitas/5
        [ResponseType(typeof(void))]
        public IHttpActionResult PutReceita(int id, Receita receita)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != receita.NumReceita)
            {
                return BadRequest();
            }

            db.Entry(receita).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ReceitaExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return StatusCode(HttpStatusCode.NoContent);
        }

        // POST: api/Receitas
        //[ResponseType(typeof(ResultCadastroReceita))]
        public ResultCadastroReceita PostReceita(Receita receita)
        {
            if (!ModelState.IsValid)
            {
                return new ResultCadastroReceita("Falha técnica ao cadastrar receita. Consulte o suporte.", receita);
            }

            var listaMedico = db.Medico.ToList<Medico>();
            var medico = listaMedico.Where(x => x.CRM == receita.Medico.CRM &&
                                            x.Nome == receita.Medico.Nome);

            if (medico == null)
                db.Medico.Add(receita.Medico);
            else
                receita.Medico = medico.FirstOrDefault();

            var paciente = db.Paciente.Where(x => x.CPF == receita.Paciente.CPF &&
                                            x.Nome == receita.Paciente.Nome);
            if (paciente == null)
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

        // DELETE: api/Receitas/5
        [ResponseType(typeof(Receita))]
        public IHttpActionResult DeleteReceita(int id)
        {
            Receita receita = db.Receita.Find(id);
            if (receita == null)
            {
                return NotFound();
            }

            db.Receita.Remove(receita);
            db.SaveChanges();

            return Ok(receita);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool ReceitaExists(int id)
        {
            return db.Receita.Count(e => e.NumReceita == id) > 0;
        }
    }
}