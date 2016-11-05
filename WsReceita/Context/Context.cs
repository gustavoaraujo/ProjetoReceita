using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;
using WsReceita.Models;

namespace WsReceita.Context
{
    public class Context : DbContext
    {
        public DbSet<Item> Item { get; set; }
        public DbSet<Medico> Medico { get; set; }
        public DbSet<Paciente> Paciente { get; set; }
        public DbSet<Receita> Receita { get; set; }
        public DbSet<Usuario> Usuarios { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Usuario>()
           .HasOptional(a => a.Medico)
           .WithRequired(s => s.Usuario);

        }

        public Context() : base("ReceitaDB")
        {
            //this.Database.Connection.ConnectionString = @"Data Source = (LocalDB)\MSSQLLocalDB; AttachDbFilename = C:\Users\Cliente\Documents\Visual Studio 2015\Projects\ProjetoReceita\WsReceita\App_Data\ReceitaDB.mdf; Integrated Security = True";
        }
    }
}