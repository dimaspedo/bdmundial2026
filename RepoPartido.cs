using System.Data;
using Dapper;

namespace Mundial.Persistencia;

public class RepoPartido : RepoDapper, IRepoPartido
{
    private static readonly string _query =
        @"SELECT  idPartido,
                  idTipoPartido,
                  idLocal,
                  idVisitante,
                  idEstadio,
                  fecha,
                  golesLocales,
                  golesVisitantes,
                  duracion
          FROM Partido";

    private static readonly string _queryDetalle =
        string.Concat(_query,
                      @" WHERE idPartido = @unId");

    public RepoPartido(IDbConnection conexion)
        : base(conexion) { }

    public void AltaPartido(Partido partido)
    {
        var parametros = new DynamicParameters();

        parametros.Add("unIdPartido", direction: ParameterDirection.Output);
        parametros.Add("unIdTipoPartido", partido.IdTipoPartido);
        parametros.Add("unIdLocal", partido.IdLocal);
        parametros.Add("unIdVisitante", partido.IdVisitante);
        parametros.Add("unIdEstadio", partido.IdEstadio);
        parametros.Add("unaFecha", partido.Fecha);
        parametros.Add("unosGolesLocales", partido.GolesLocales);
        parametros.Add("unosGolesVisitantes", partido.GolesVisitantes);
        parametros.Add("unaDuracion", partido.Duracion);

        _conexion.Execute("altaPartido",
                          parametros,
                          commandType: CommandType.StoredProcedure);

        partido.IdPartido = parametros.Get<byte>("unIdPartido");
    }

    public Partido? Detalle(byte id)
    {
        var partido = _conexion.QueryFirstOrDefault<Partido>(
            _queryDetalle,
            new { unId = id });

        return partido;
    }

    public IEnumerable<Partido> Obtener()
    {
        var partidos = _conexion.Query<Partido>(_query);
        return partidos;
    }
}
