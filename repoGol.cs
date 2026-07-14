using System.Data;
using Dapper;

namespace Mundial.Persistencia;

public class RepoGol : RepoDapper, IRepoGol
{
    private static readonly string _query =
        @"SELECT  idPartido,
                  minuto,
                  adicionado,
                  idJugador,
                  enContra
          FROM Gol";

    private static readonly string _queryDetalle =
        string.Concat(_query,
                      @" WHERE idPartido = @unIdPartido
                         AND minuto = @unMinuto
                         AND adicionado <=> @unAdicionado");

    public RepoGol(IDbConnection conexion)
        : base(conexion) { }

    public void AltaGol(Gol gol)
    {
        var parametros = new DynamicParameters();

        parametros.Add("unIdPartido", gol.IdPartido);
        parametros.Add("unMinuto", gol.Minuto);
        parametros.Add("unAdicionado", gol.Adicionado);
        parametros.Add("unIdJugador", gol.IdJugador);
        parametros.Add("unEnContra", gol.EnContra);

        _conexion.Execute("altaGol",
                          parametros,
                          commandType: CommandType.StoredProcedure);
    }

    public Gol? Detalle(byte idPartido, byte minuto, byte? adicionado)
    {
        return _conexion.QueryFirstOrDefault<Gol>(
            _queryDetalle,
            new
            {
                unIdPartido = idPartido,
                unMinuto = minuto,
                unAdicionado = adicionado
            });
    }

    public IEnumerable<Gol> Obtener()
    {
        return _conexion.Query<Gol>(_query);
    }
}
