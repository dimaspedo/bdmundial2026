using System.Data;
using Dapper;

namespace Mundial.Persistencia;

public class RepoPosicion : RepoDapper, IRepoPosicion
{
    private static readonly string _query =
        @"SELECT  idPosicion,
                  posicion
          FROM Posicion";

    private static readonly string _queryDetalle =
        string.Concat(_query,
                      @" WHERE idPosicion = @unId");

    public RepoPosicion(IDbConnection conexion)
        : base(conexion) { }

    public void AltaPosicion(Posicion posicion)
    {
        var parametros = new DynamicParameters();

        parametros.Add("unIdPosicion", direction: ParameterDirection.Output);
        parametros.Add("unaPosicion", posicion.Posicion);

        _conexion.Execute("altaPosicion",
                          parametros,
                          commandType: CommandType.StoredProcedure);

        posicion.IdPosicion = parametros.Get<byte>("unIdPosicion");
    }

    public Posicion? Detalle(byte id)
    {
        var posicion = _conexion.QueryFirstOrDefault<Posicion>(
            _queryDetalle,
            new { unId = id });

        return posicion;
    }

    public IEnumerable<Posicion> Obtener()
    {
        var posiciones = _conexion.Query<Posicion>(_query);
        return posiciones;
    }
}
