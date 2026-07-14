using System.Data;
using Dapper;

namespace Mundial.Persistencia;

public class RepoJugador : RepoDapper, IRepoJugador
{
    private static readonly string _query =
        @"SELECT  idJugador,
                  idPais,
                  idPosicion,
                  nombre,
                  apellido,
                  nacimiento,
                  numCamiseta
          FROM Jugador";

    private static readonly string _queryDetalle =
        string.Concat(_query,
                      @" WHERE idJugador = @unId");

    public RepoJugador(IDbConnection conexion)
        : base(conexion) { }

    public void AltaJugador(Jugador jugador)
    {
        var parametros = new DynamicParameters();

        parametros.Add("unIdJugador", direction: ParameterDirection.Output);
        parametros.Add("unIdPais", jugador.IdPais);
        parametros.Add("unIdPosicion", jugador.IdPosicion);
        parametros.Add("unNombre", jugador.Nombre);
        parametros.Add("unApellido", jugador.Apellido);
        parametros.Add("unaFechaNacimiento", jugador.Nacimiento);
        parametros.Add("unNumeroCamiseta", jugador.NumCamiseta);

        _conexion.Execute("altaJugador",
                          parametros,
                          commandType: CommandType.StoredProcedure);

        jugador.IdJugador = parametros.Get<short>("unIdJugador");
    }

    public Jugador? Detalle(short id)
    {
        var jugador = _conexion.QueryFirstOrDefault<Jugador>(
            _queryDetalle,
            new { unId = id });

        return jugador;
    }

    public IEnumerable<Jugador> Obtener()
    {
        var jugadores = _conexion.Query<Jugador>(_query);
        return jugadores;
    }
}
