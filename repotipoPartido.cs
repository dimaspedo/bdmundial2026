using System.Data;
using Dapper;

namespace Mundial.Persistencia;

public class RepoTipoPartido : RepoDapper, IRepoTipoPartido
{
    private static readonly string _query =
        @"SELECT  idTipoPartido,
                  tipoPartido
          FROM TipoPartido";

    private static readonly string _queryDetalle =
        string.Concat(_query,
                      @" WHERE idTipoPartido = @unId");

    public RepoTipoPartido(IDbConnection conexion)
        : base(conexion) { }

    public void AltaTipoPartido(TipoPartido tipoPartido)
    {
        var parametros = new DynamicParameters();

        parametros.Add("unIdTipoPartido", direction: ParameterDirection.Output);
        parametros.Add("unTipoPartido", tipoPartido.TipoPartido);

        _conexion.Execute("altaTipoPartido",
                          parametros,
                          commandType: CommandType.StoredProcedure);

        tipoPartido.IdTipoPartido = parametros.Get<byte>("unIdTipoPartido");
    }

    public TipoPartido? Detalle(byte id)
    {
        var tipo = _conexion.QueryFirstOrDefault<TipoPartido>(
            _queryDetalle,
            new { unId = id });

        return tipo;
    }

    public IEnumerable<TipoPartido> Obtener()
    {
        var tipos = _conexion.Query<TipoPartido>(_query);
        return tipos;
    }
}
