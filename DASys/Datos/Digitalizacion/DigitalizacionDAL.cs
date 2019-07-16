using Entidad;
using Interfaces;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using Acces;
namespace Datos
{
    public class DigitalizacionDAL: IDigitalizacion
    {
        public string MantenimientoDigitalizacion(Digitalizacion objDigitalizacion)
        {
            string r = "";
            using (SqlConnection conn = DataContext.GetConnection())
            {
                var listaParams = new List<SqlParameter>();
                listaParams.Add(new SqlParameter("@IdPersona", SqlDbType.VarChar) { Direction = ParameterDirection.Input, Value = objDigitalizacion.IdPersona });
                listaParams.Add(new SqlParameter("@FechaVencimiento", SqlDbType.DateTime) { Direction = ParameterDirection.Input, Value = objDigitalizacion.FechaVencimiento });
                listaParams.Add(new SqlParameter("@IdDigitalizacion", SqlDbType.VarChar) { Direction = ParameterDirection.Input, Value = objDigitalizacion.IdDigitalizacion });
                listaParams.Add(new SqlParameter("@IdDocumento", SqlDbType.VarChar) { Direction = ParameterDirection.Input, Value = objDigitalizacion.IdDocumento });
                listaParams.Add(new SqlParameter("@Observacion", SqlDbType.VarChar) { Direction = ParameterDirection.Input, Value = objDigitalizacion.Observacion });
                listaParams.Add(new SqlParameter("@IdEstado", SqlDbType.VarChar) { Direction = ParameterDirection.Input, Value = objDigitalizacion.IdEstado });
                listaParams.Add(new SqlParameter("@Opcion", SqlDbType.VarChar) { Direction = ParameterDirection.Input, Value = objDigitalizacion.Opcion });
                listaParams.Add(new SqlParameter("@IdUsuario", SqlDbType.VarChar) { Direction = ParameterDirection.Input, Value = objDigitalizacion.IdUsuario });
                r = SQLHelper.ExecuteScalar(conn, System.Data.CommandType.StoredProcedure, @"Sp_DigitalizacionMantenimiento", listaParams.ToArray()).ToString();
            }
            return r;
        }
        public int EliminarDigitalizacion(string IdDigitalizacion)
        {
            int r = 0;
            using (SqlConnection conn = DataContext.GetConnection())
            {
                var listaParams = new List<SqlParameter>();
                listaParams.Add(new SqlParameter("@IdDigitalizacion", SqlDbType.VarChar) { Direction = ParameterDirection.Input, Value =IdDigitalizacion });
                r = SQLHelper.ExecuteNonQuery(conn, System.Data.CommandType.StoredProcedure, @"Sp_DigitalizacionEliminar", listaParams.ToArray());
            }
            return r;
        }
        public List<DigitalizacionCorreoDto> ListaDigtalizados(UsuarioActualDto objUsuarioActualDto)
        {
            List<DigitalizacionCorreoDto> retList = new List<DigitalizacionCorreoDto>();
            using (SqlConnection conn = DataContext.GetConnection())
            {
                var listaParams = new List<SqlParameter>();
                listaParams.Add(new SqlParameter("@IdEmpresa", SqlDbType.VarChar) { Direction = ParameterDirection.Input, Value = objUsuarioActualDto.IdEmpresa });
                listaParams.Add(new SqlParameter("@Tipo", SqlDbType.VarChar) { Direction = ParameterDirection.Input, Value = objUsuarioActualDto.DescripcionAdcional });
                SqlDataReader lector = SQLHelper.ExecuteReader(conn, System.Data.CommandType.StoredProcedure, @"Sp_CorreDigitalizacionListado", listaParams.ToArray());
                while (lector.Read())
                {
                    retList.Add(new DigitalizacionCorreoDto
                    {
                        Nombre = lector.IsDBNull(lector.GetOrdinal("Nombre")) ? default(string) : lector.GetString(lector.GetOrdinal("Nombre")),
                        Numero = lector.IsDBNull(lector.GetOrdinal("Numero")) ? default(string) : lector.GetString(lector.GetOrdinal("Numero")),
                        Empresa = lector.IsDBNull(lector.GetOrdinal("Empresa")) ? default(string) : lector.GetString(lector.GetOrdinal("Empresa")),
                        Categoria = lector.IsDBNull(lector.GetOrdinal("Categoria")) ? default(string) : lector.GetString(lector.GetOrdinal("Categoria")),
                        IdEmpresa = lector.IsDBNull(lector.GetOrdinal("IdEmpresa")) ? default(string) : lector.GetString(lector.GetOrdinal("IdEmpresa")),
                        IdPersona = lector.IsDBNull(lector.GetOrdinal("IdPersona")) ? default(string) : lector.GetString(lector.GetOrdinal("IdPersona")),
                        TipoEmpresa = lector.IsDBNull(lector.GetOrdinal("TipoEmpresa")) ? default(string) : lector.GetString(lector.GetOrdinal("TipoEmpresa")),
                        Enviado = lector.IsDBNull(lector.GetOrdinal("Enviado")) ? 0 : lector.GetInt32(lector.GetOrdinal("Enviado")),
                        EstadoDigitalizacion = lector.IsDBNull(lector.GetOrdinal("EstadoDigitalizacion")) ? default(string) : lector.GetString(lector.GetOrdinal("EstadoDigitalizacion")),
                    });
                }
            }
            return retList;
        }

        public List<EntidadesCorreo> ListaEntidadesCorreo(string IdEmpresa, string IdUsuario, string Entidad)
        {
            List<EntidadesCorreo> retList = new List<EntidadesCorreo>();
            using (SqlConnection conn = DataContext.GetConnection())
            {
                var listaParams = new List<SqlParameter>();
                listaParams.Add(new SqlParameter("@IdEmpresa", SqlDbType.VarChar) { Direction = ParameterDirection.Input, Value = IdEmpresa });
                listaParams.Add(new SqlParameter("@IdUsuario", SqlDbType.VarChar) { Direction = ParameterDirection.Input, Value =IdUsuario });
                listaParams.Add(new SqlParameter("@Entidad", SqlDbType.VarChar) { Direction = ParameterDirection.Input, Value = Entidad });
                SqlDataReader lector = SQLHelper.ExecuteReader(conn, System.Data.CommandType.StoredProcedure, @"Sp_EntidadCorreoListado", listaParams.ToArray());
                while (lector.Read())
                {
                    retList.Add(new EntidadesCorreo
                    {
                        Id = lector.IsDBNull(lector.GetOrdinal("Id")) ? default(string) : lector.GetString(lector.GetOrdinal("Id")),
                        IdPersona = lector.IsDBNull(lector.GetOrdinal("IdPersona")) ? default(string) : lector.GetString(lector.GetOrdinal("IdPersona")),
                        Numero = lector.IsDBNull(lector.GetOrdinal("Numero")) ? default(string) : lector.GetString(lector.GetOrdinal("Numero")),
                        Nombre = lector.IsDBNull(lector.GetOrdinal("Nombre")) ? default(string) : lector.GetString(lector.GetOrdinal("Nombre")),
                        Digitalizado = lector.IsDBNull(lector.GetOrdinal("Digitalizado")) ? 0 : lector.GetInt32(lector.GetOrdinal("Digitalizado")),
                        IdEmpresa = lector.IsDBNull(lector.GetOrdinal("IdEmpresa")) ? default(string) : lector.GetString(lector.GetOrdinal("IdEmpresa")),
                        IdEstado = lector.IsDBNull(lector.GetOrdinal("IdEstado")) ? default(string) : lector.GetString(lector.GetOrdinal("IdEstado")),
                        TotalAprobados = lector.IsDBNull(lector.GetOrdinal("TotalAprobados")) ? 0 : lector.GetInt32(lector.GetOrdinal("TotalAprobados")),
                        TotalDesaprobados = lector.IsDBNull(lector.GetOrdinal("TotalDesaprobados")) ? 0 : lector.GetInt32(lector.GetOrdinal("TotalDesaprobados")),
                        TotalDocumento = lector.IsDBNull(lector.GetOrdinal("TotalDocumento")) ? 0 : lector.GetInt32(lector.GetOrdinal("TotalDocumento")),
                        Mensaje = lector.IsDBNull(lector.GetOrdinal("Mensaje")) ? default(string) : lector.GetString(lector.GetOrdinal("Mensaje")),
                        IdUsuario = lector.IsDBNull(lector.GetOrdinal("IdUsuario")) ? default(string) : lector.GetString(lector.GetOrdinal("IdUsuario")),
                        Enviado = lector.IsDBNull(lector.GetOrdinal("Enviado")) ? 0 : lector.GetInt32(lector.GetOrdinal("Enviado")),
                        IdEmpresaContratante = lector.IsDBNull(lector.GetOrdinal("IdEmpresaContratante")) ? default(string) : lector.GetString(lector.GetOrdinal("IdEmpresaContratante")),
                        IdEmpresaPrincipal = lector.IsDBNull(lector.GetOrdinal("IdEmpresaPrincipal")) ? default(string) : lector.GetString(lector.GetOrdinal("IdEmpresaPrincipal")),
                        Empresa = lector.IsDBNull(lector.GetOrdinal("Empresa")) ? default(string) : lector.GetString(lector.GetOrdinal("Empresa")),
                        Entidad = lector.IsDBNull(lector.GetOrdinal("Entidad")) ? default(string) : lector.GetString(lector.GetOrdinal("Entidad")),
                    });
                }
            }
            return retList;
        }
        public List<DigitalizacionExcelDto> ListaEntidadesDigitalizacionExcel(string IdPersona, string Entidad)
        {
            List<DigitalizacionExcelDto> retList = new List<DigitalizacionExcelDto>();
            using (SqlConnection conn = DataContext.GetConnection())
            {
                var listaParams = new List<SqlParameter>();
                listaParams.Add(new SqlParameter("@IdPersona", SqlDbType.VarChar) { Direction = ParameterDirection.Input, Value = IdPersona });
                listaParams.Add(new SqlParameter("@Entidad", SqlDbType.VarChar) { Direction = ParameterDirection.Input, Value = Entidad });
                SqlDataReader lector = SQLHelper.ExecuteReader(conn, System.Data.CommandType.StoredProcedure, @"Sp_EntidadesDigitalizacionExcel", listaParams.ToArray());
                while (lector.Read())
                {
                    retList.Add(new DigitalizacionExcelDto
                    {
                        Codigo = lector.IsDBNull(lector.GetOrdinal("Codigo")) ? default(string) : lector.GetString(lector.GetOrdinal("Codigo")),
                        Numero = lector.IsDBNull(lector.GetOrdinal("Numero")) ? default(string) : lector.GetString(lector.GetOrdinal("Numero")),
                        Empresa = lector.IsDBNull(lector.GetOrdinal("Empresa")) ? default(string) : lector.GetString(lector.GetOrdinal("Empresa")),
                        Categoria = lector.IsDBNull(lector.GetOrdinal("Categoria")) ? default(string) : lector.GetString(lector.GetOrdinal("Categoria")),
                        Documento = lector.IsDBNull(lector.GetOrdinal("Documento")) ? default(string) : lector.GetString(lector.GetOrdinal("Documento")),
                        Observacion = lector.IsDBNull(lector.GetOrdinal("Observacion")) ? default(string) : lector.GetString(lector.GetOrdinal("Observacion")),
                        Obligatorio = lector.IsDBNull(lector.GetOrdinal("Obligatorio")) ? default(string) : lector.GetString(lector.GetOrdinal("Obligatorio")),
                        Vencimiento = lector.IsDBNull(lector.GetOrdinal("Vencimiento")) ? default(string) : lector.GetString(lector.GetOrdinal("Vencimiento")),
                        Estado = lector.IsDBNull(lector.GetOrdinal("Estado")) ? default(string) : lector.GetString(lector.GetOrdinal("Estado")),
                    });
                }
            }
            return retList;
        }

    }
}
