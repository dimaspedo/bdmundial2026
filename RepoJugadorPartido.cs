using System.Data;
using Dapper;

namespace Mundial.Persistencia;

public class RepoJugadorPartido : RepoDapper, IRepoJugadorPartido
{
    private static readonly string _query =
        @"SELECT  idJugador,
                  idPartido,
                  idReemplazo,
                  ingreso,
                  ingresoAdicionado,
                  egreso,
                  egresoAdicionado
          FROM JugadorPartido";

    private static readonly string _queryDetalle =
        string.Concat(_query,
                      @" WHERE idJugador = @unIdJugador
                         AND idPartido = @unIdPartido");

    public RepoJugadorPartido(IDbConnection conexion)
        : base(conexion) { }

    public void AltaJugadorPartido(JugadorPartido jugadorPartido)
    {
        var parametros = new DynamicParameters();

        parametros.Add("unIdJugador", jugadorPartido.IdJugador);
        parametros.Add("unIdPartido", jugadorPartido.IdPartido);
        parametros.Add("unIdReemplazo", jugadorPartido.IdReemplazo);
        parametros.Add("unIngreso", jugadorPartido.Ingreso);
        parametros.Add("unIngresoAdicionado", jugadorPartido.IngresoAdicionado);
        parametros.Add("unEgreso", jugadorPartido.Egreso);
        parametros.Add("unEgresoAdicionado", jugadorPartido.EgresoAdicionado);

        _conexion.Execute("altaJugadorPartido",
                          parametros,
                          commandType: CommandType.StoredProcedure);
    }

    public JugadorPartido? Detalle(short idJugador, byte idPartido)
    {
        return _conexion.QueryFirstOrDefault<JugadorPartido>(
            _queryDetalle,
            new
            {
                unIdJugador = idJugador,
                unIdPartido = idPartido
            });
    }

    public IEnumerable<JugadorPartido> Obtener()
    {
        return _conexion.Query<JugadorPartido>(_query);
    }
}
