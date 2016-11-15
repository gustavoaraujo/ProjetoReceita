namespace WsReceita.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Initial : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Item",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        NumReceita = c.Int(nullable: false),
                        RegAnvisa = c.Int(nullable: false),
                        Instrucao = c.String(unicode: false),
                        Nome = c.String(unicode: false),
                        Uso = c.String(unicode: false),
                        ContraIndicacao = c.String(unicode: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Receita", t => t.NumReceita, cascadeDelete: true)
                .Index(t => t.NumReceita);
            
            CreateTable(
                "dbo.Receita",
                c => new
                    {
                        NumReceita = c.Int(nullable: false),
                        Data = c.DateTime(nullable: false, precision: 0),
                        Crm = c.String(maxLength: 128, storeType: "nvarchar"),
                        Cpf = c.String(maxLength: 128, storeType: "nvarchar"),
                        Utilizada = c.Boolean(nullable: false),
                        Cancelada = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.NumReceita)
                .ForeignKey("dbo.Medico", t => t.Crm)
                .ForeignKey("dbo.Paciente", t => t.Cpf)
                .Index(t => t.Crm)
                .Index(t => t.Cpf);
            
            CreateTable(
                "dbo.Medico",
                c => new
                    {
                        Crm = c.String(nullable: false, maxLength: 128, storeType: "nvarchar"),
                        Nome = c.String(unicode: false),
                        Usuario_IdUsuario = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Crm)
                .ForeignKey("dbo.Usuario", t => t.Usuario_IdUsuario)
                .Index(t => t.Usuario_IdUsuario);
            
            CreateTable(
                "dbo.Usuario",
                c => new
                    {
                        IdUsuario = c.Int(nullable: false, identity: true),
                        Login = c.String(unicode: false),
                        Senha = c.String(unicode: false),
                        Crm = c.String(unicode: false),
                    })
                .PrimaryKey(t => t.IdUsuario);
            
            CreateTable(
                "dbo.Paciente",
                c => new
                    {
                        Cpf = c.String(nullable: false, maxLength: 128, storeType: "nvarchar"),
                        Nome = c.String(unicode: false),
                    })
                .PrimaryKey(t => t.Cpf);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Receita", "Cpf", "dbo.Paciente");
            DropForeignKey("dbo.Medico", "Usuario_IdUsuario", "dbo.Usuario");
            DropForeignKey("dbo.Receita", "Crm", "dbo.Medico");
            DropForeignKey("dbo.Item", "NumReceita", "dbo.Receita");
            DropIndex("dbo.Medico", new[] { "Usuario_IdUsuario" });
            DropIndex("dbo.Receita", new[] { "Cpf" });
            DropIndex("dbo.Receita", new[] { "Crm" });
            DropIndex("dbo.Item", new[] { "NumReceita" });
            DropTable("dbo.Paciente");
            DropTable("dbo.Usuario");
            DropTable("dbo.Medico");
            DropTable("dbo.Receita");
            DropTable("dbo.Item");
        }
    }
}
