using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Utilis;
using Datos;
using HtmlAgilityPack;
using Microsoft.AspNetCore.Mvc;
using WEB.Core;
using Entidad;

namespace WEB.Controllers
{
    public class ConsultaSunatController : Controller
    {

        private static string URL_CONSULTA_RUC = "http://e-consultaruc.sunat.gob.pe/cl-ti-itmrconsruc/jcrS00Alias";
        private static string URL_CAPTCHA_RANDOM = "http://e-consultaruc.sunat.gob.pe/cl-ti-itmrconsruc/captcha?accion=random";
        private static CookieContainer cookie = new CookieContainer();

        [HttpPost]
        public JsonResult ConsultaRuc (string ruc)
        {
            var jsonResponseDto = new JsonResponseDto();
            try
            {
                UtilsDAL utilsDAL = new UtilsDAL();
                var random = ConsultarURL(URL_CAPTCHA_RANDOM);
                var url = URL_CONSULTA_RUC + "?accion=consPorRuc&nroRuc=" + ruc + "&numRnd=" + random + "&tipdoc=";

                var html = ConsultarURL(url);
                ConsultaRuc data = ObtenerValoresHTML(html);
                jsonResponseDto.data = data;
                jsonResponseDto.IsValid = true;
                jsonResponseDto.Mensaje = ruc;
                jsonResponseDto.Type = Constante.Success.ToLower();

                return Json(jsonResponseDto);
            }
            catch (Exception ex)
            {
                jsonResponseDto.data = null;
                jsonResponseDto.IsValid = false;
                jsonResponseDto.Mensaje = ruc;
                jsonResponseDto.Type = Constante.Warning.ToLower();

                return Json(jsonResponseDto);
            }
        }

        private string ConsultarURL(string url)
        {
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
            request.CookieContainer = cookie;

            HttpWebResponse response = (HttpWebResponse)request.GetResponse();
            var stream = response.GetResponseStream();
            var reader = new StreamReader(stream);
            string receiveContent = reader.ReadToEnd();
            reader.Close();

            return receiveContent;
        }

        private ConsultaRuc ObtenerValoresHTML(string html)
        {
            ConsultaRuc data = new ConsultaRuc();
            HtmlDocument pageDocument = new HtmlDocument();
            pageDocument.LoadHtml(html);

            List<List<string>> table = pageDocument.DocumentNode.SelectSingleNode("//table[@class='form-table']")
                .Descendants("tr")
                .Skip(1)
                .Where(tr => tr.Elements("td").Count() > 1)
                .Select(tr => tr.Elements("td").Select(td => td.InnerText.Trim()).ToList())
                .ToList();

            var rucRazonSocial = table.Where(e => e[0] == "N&uacute;mero de RUC:").ToList()[0][1];
            // var ruc = rucRazonSocial.Split("- ")[0];
            data.Nombre = rucRazonSocial.Split("- ")[1];
            data.NombreComercial = table.Where(e => e[0] == "Nombre Comercial:").ToList()[0][1];
            var domicilio = table.Where(e => e[0] == "Direcci&oacute;n del Domicilio Fiscal:").ToList()[0][1];
            mapDireccion(data, domicilio);

            return data;
        }

        private void mapDireccion(ConsultaRuc data, string direccion)
        {
            string dir = "", dep = "", prov = "", dist = "";

            string[] items = direccion.Split("-");
            if (items.Length != 3)
            {
                if (direccion.Trim() == "-")
                {
                    dir = "";
                }
            }
            else
            {
                items = items.Select(item => item.Trim()).ToArray();
                var words = items[0].Split(" ");
                (int, string) mapDep = mapDepartamento(words.Last());

                dep = mapDep.Item2;
                prov = items[1];
                dist = items[2];
                var i = words.Take(words.Length - mapDep.Item1).ToArray();
                dir = string.Join(" ", i);
            }

            data.Direccion = dir;
            data.Departamento = dep != "" ? dep : "LIMA";
            data.Provincia = prov != "" ? prov : "LIMA";
            data.Distrito = dist != "" ? dist : "LIMA";
        }

        private (int, string) mapDepartamento(string departamento)
        {
            int palabras = 1;

            switch (departamento.ToUpper())
            {
                case "DIOS":
                    palabras = 3;
                    departamento = "MADRE DE DIOS";
                    break;
                case "MARTIN":
                    palabras = 2;
                    departamento = "SAN MARTIN";
                    break;
                case "LIBERTAD":
                    palabras = 2;
                    departamento = "LA LIBERTAD";
                    break;
            }
            return (palabras, departamento);
        }
    }
}