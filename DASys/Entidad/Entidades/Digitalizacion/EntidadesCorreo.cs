using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entidad
{
    public class EntidadesCorreo
    {
        public string Id { get; set; }
        public string IdPersona { get; set; }
        public string Numero { get; set; }
        public string Nombre { get; set; }
        public int Digitalizado { get; set; }
        public string IdEmpresa { get; set; }
        public string IdEstado { get; set; }
        public int TotalAprobados { get; set; }
        public int TotalDesaprobados { get; set; }
        public int TotalDocumento { get; set; }
        public string Mensaje { get; set; }
        public string IdUsuario { get; set; }
        public int Enviado { get; set; }
        public string IdEmpresaPrincipal { get; set; }
        public string IdEmpresaContratante { get; set; }
        public string Empresa { get; set; }
        public string Entidad { get; set; }
    }
}
