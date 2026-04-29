using System.Data;
using Health.Infrastructure.Adapters.Postgres;
using Microsoft.EntityFrameworkCore;

namespace Health.Infrastructure.Repositories;

public sealed class DbSession
{
    public AppDbContext Context { get; }
    public DapperSession Dapper { get; }

    public DbSession(AppDbContext context, DapperSession dapperSession)
    {
        Context = context;
        Dapper = dapperSession;

        // Garantia de segurança: 
        // Ao instanciar a sessão, verificar se a conexão do EF está aberta
        // para que o Dapper não encontre o "cano" fechado.
        var connection = Context.Database.GetDbConnection();
        if (connection.State != ConnectionState.Open)
        {
            connection.Open();
        }
    }

    // Atalhos para os repositórios usarem via _session.Connection
    public IDbConnection Connection => Dapper.Connection;
    public IDbTransaction? Transaction => Dapper.Transaction;
}