using System.Data;
using Health.Infrastructure.Adapters.Postgres;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;

namespace Health.Infrastructure.Repositories;

public sealed class DapperSession
{
    private readonly AppDbContext _context;

    public DapperSession(AppDbContext context)
    {
        _context = context;
    }

    // O Dapper usará esta conexão, que é a mesma que o EF já abriu
    public IDbConnection Connection => _context.Database.GetDbConnection();

    // Se o EF abrir uma transação, o Dapper a enxergará aqui instantaneamente
    public IDbTransaction? Transaction => _context.Database.CurrentTransaction?.GetDbTransaction();
}