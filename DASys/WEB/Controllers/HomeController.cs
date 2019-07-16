using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WEB.Models;

namespace WEB.Controllers
{

    public class HomeController : BaseController
    {
        public HomeController(IServiceProvider serviceProvider)
                   : base(serviceProvider)
        {

        }
        public IActionResult Index()
        {
            //SendEmail();
            if (ModulosActuales() == null)
            {
                return Redirect("/Login/Index");

            }
            else
            {
                HttpContext.Session.SetString("_perfil", GetPerfil());
                ViewBag.Menu = ModulosActuales();
                ViewBag.Opciones = OpcionesActuales();
                ViewBag.UsuarioActual = UsuarioActual();
                ViewBag.OpcionesHijo = OpcionesHijoActuales();
                ViewBag.TipoEmpresa = (GetTipoEmpresa() == "" || GetTipoEmpresa() == null) ? "SUPER" : GetTipoEmpresa();
                ViewBag.TipoPerfil = GetPerfil();
                return View();
            }
        }


        public IActionResult About()
        {
            ViewData["Message"] = "Your application description page.";

            return View();
        }

        public IActionResult Contact()
        {
            ViewData["Message"] = "Your contact page.";

            return View();
        }

        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        public class EnPersona
        {
            public string IdPersona { get; set; }
            public string Apellidos { get; set; }
            public string Nombres { get; set; }
        }
        public string ObtenerCadenaSql(string consulta)
        {
            string resultado;
            using (SqlConnection cn = new SqlConnection(""))
            {
                SqlCommand cmd = new SqlCommand(consulta, cn);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                //cmd.Parameters.AddWithValue("@IdPersona", id);
                cn.Open();
                object obj = cmd.ExecuteScalar();
                resultado = obj == null ? "" : obj.ToString();
            }
            return resultado;
        }
        public void demo(string id){

            string resultado = ObtenerCadenaSql("Usp_");
            resultado = "PE0000000000093|martinez lopez|CARLOS ¬PE0000000000098|dd dd|dd¬PE0000000000099|dd dd|dd¬PE0000000000121|Martinez castro|miguel¯PE0000000000095|calderon torres|marcos ¬PE0000000000100|aaaa aaaa|aaa¬PE0000000000094|perales torres|luis¬PE0000000000120|Pilllaca Pilllaca|Nelson¯PE0000000000096|martinez martinez|carlos gabriel¬PE0000000000097|ap am|na¬PE0000000000101|bbbb bbbb|bbbb¬PE0000000000104|eeee ee|eeee¬PE0000000000105|FFFF FFF|FFFF¬PE0000000000106|ddd ddd|ddd";
            List<EnPersona> lEnPersonaPrincipal = new List<EnPersona>();
            List<EnPersona> lEnPersonaContratista = new List<EnPersona>();
            List<EnPersona> lEnPersonaSubContratista = new List<EnPersona>();
            EnPersona oEnPersona;
            string[] listas = resultado.Split('¯');
            string[] listaPrincipal = listas[0].Split('¬');
            string[] campos;
            string idPersona;
            string apellidos;
            string nombre;
            int i;

            for (i = 0; i < listaPrincipal.Length; i++)
            {
                oEnPersona = new EnPersona();
                campos = listaPrincipal[i].Split('|');
                oEnPersona.IdPersona = campos[0];
                oEnPersona.Apellidos = campos[1];
                oEnPersona.Nombres = campos[2];
                lEnPersonaPrincipal.Add(oEnPersona);
            }

            string[] listaContratista = listas[1].Split('¬');
            for (i = 0; i < listaContratista.Length; i++)
            {
                oEnPersona = new EnPersona();
                campos = listaContratista[i].Split('|');
                oEnPersona.IdPersona = campos[0];
                oEnPersona.Apellidos = campos[1];
                oEnPersona.Nombres = campos[2];
                lEnPersonaPrincipal.Add(oEnPersona);
                lEnPersonaContratista.Add(oEnPersona);
            }

            string[] listaSubContratista = listas[2].Split('¬');
            for (i = 0; i < listaContratista.Length; i++)
            {
                oEnPersona = new EnPersona();
                campos = listaSubContratista[i].Split('|');
                oEnPersona.IdPersona = campos[0];
                oEnPersona.Apellidos = campos[1];
                oEnPersona.Nombres = campos[2];
                lEnPersonaPrincipal.Add(oEnPersona);
                lEnPersonaContratista.Add(oEnPersona);
                lEnPersonaSubContratista.Add(oEnPersona);
            }



        }
    }
}
