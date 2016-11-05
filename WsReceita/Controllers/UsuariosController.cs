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
    public class UsuariosController : ApiController
    {
        private Context.Context db = new Context.Context();

        [Route("CadastrarUsuario")]
        [HttpPost]
        public string CadastrarUsuario(Usuario usuario)
        {
            if (!ModelState.IsValid)
            {
                return "Falha técnica ao cadastrar usuário. Consulte o suporte.";
            }

            if (this.UsuarioExists(usuario.Login))
            {
                var medico = db.Usuarios.Where(x => x.CRM == usuario.CRM).FirstOrDefault();

                if (medico != null)
                    return "Erro ao cadastrar usuário: Médico já possui login de usuário.";
                else
                    return "Erro ao cadastrar usuário: Usuário já cadastrado";
            }
            else
            {
                if (usuario.CRM != null)
                {
                    var medico = db.Medico.Add(usuario.Medico);
                    if (medico == null)
                        return "Erro ao cadastrar usuário: CRM inválido.";
                }
            }

            db.Usuarios.Add(usuario);

            try
            {
                db.SaveChanges();
            }
            catch (Exception ex)
            {
                if (UsuarioExists(usuario.Login))
                {
                    return "Erro ao cadastrar usuário: usuário já existe no sistema.";
                }
                else
                {
                    return "Erro ao cadastrar usuário: " + ex.Message;
                }
            }

            return "Cadastrado com sucesso.";
        }

        private bool UsuarioExists(string login)
        {
            var usuario = db.Usuarios.Where(x => x.Login == login);

            return usuario.Count() > 0;
        }

        private bool MedicoExists(string crm)
        {
            var medicos = db.Medico.Where(x => x.CRM == crm);

            return medicos.Count() > 0;
        }

        [Route("LogarUsuario")]
        [HttpPost]
        public Usuario LogarUsuario(Usuario u)
        {
            Usuario userLogin = db.Usuarios.Where(x => x.Login == u.Login && x.Senha == u.Senha).FirstOrDefault();

            if (userLogin == null)
                return null;

            userLogin.Medico = db.Medico.Where(x => x.CRM == userLogin.CRM).FirstOrDefault();

            var listaReceitas = db.Receita.Where(x => x.CRM == userLogin.CRM).ToList();

            var rc = new ReceitasController();

            listaReceitas.ForEach(x =>
            {
                var receita = rc.ObterReceitaMedica(new NumeroReceita() { Numero = x.NumReceita });
                userLogin.Medico.Receitas.Add(receita);
            });

            return userLogin;
        }
    }
}