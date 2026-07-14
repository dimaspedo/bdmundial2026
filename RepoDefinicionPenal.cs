using System.Data;
using Dapper;

namespace Mundial.Persistencia;

public class RepoDefinicionPenal : RepoDapper, IRepoDefinicionPenal
{
    private static readonly string _query =
        @"SELECT  idPartido,
                  idJugador,
                  turno,
                  acierto
          FROM DefinicionPenal";

    private static readonly string _queryDetalle =
        string.Concat(_query,
                      @" WHERE idPartido = @unIdPartido
                         AND turno = @unTurno");

    public RepoDefinicionPenal(IDbConnection conexion)
        : base(conexion) { }

    public void AltaDefinicionPenal(DefinicionPenal definicionPenal)
    {
        var parametros = new DynamicParameters();

        parametros.Add("unIdPartido", definicionPenal.IdPartido);
        parametros.Add("unIdJugador", definicionPenal.IdJugador);
        parametros.Add("unTurno", definicionPenal.Turno);
        parametros.Add("unAcierto", definicionPenal.Acierto);

        _conexion.Execute("altaDefinicionPenal",
                          parametros,
                          commandType: CommandType.StoredProcedure);
    }

    public DefinicionPenal? Detalle(byte idPartido, byte turno)
    {
        return _conexion.QueryFirstOrDefault<DefinicionPenal>(
            _queryDetalle,
            new
            {
                unIdPartido = idPartido,
                unTurno = turno
            });
    }

    public IEnumerable<DefinicionPenal> Obtener()
    {
        return _conexion.Query<DefinicionPenal>(_query);
    }
}
