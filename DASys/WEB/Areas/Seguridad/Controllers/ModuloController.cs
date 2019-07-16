﻿using System;
using Entidad;
using Interfaces;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Serilog;

namespace WEB.Areas.Seguridad.Controllers
{
    [Area("Seguridad")]
    public class ModuloController : Controller
    {
        private readonly IModulo _Modulo;
        public ModuloController(IModulo Modulo)
        {
            _Modulo = Modulo;
        }
        #region Metodos publicos
        #region Vistas
        public IActionResult Index()
        {
            return View();
        }


        public IActionResult Registrar()
        {
            ModuloConsultaDto objModuloConsultaDto = new ModuloConsultaDto();
            return PartialView("Registrar", objModuloConsultaDto);
        }


        [HttpPost]
        public IActionResult Eliminar([FromForm] ModuloConsultaDto objModulo)
        {
            var jsonResponseDto = new JsonResponseDto();
            try
            {
                var result = _Modulo.EliminarModulo(objModulo.IdModulo, objModulo.Accion);
                var mensaje = result.Split(":");
                jsonResponseDto.Type = mensaje[0];
                jsonResponseDto.IsValid = true;
                jsonResponseDto.Mensaje = mensaje[1];
            }
            catch (Exception ex)
            {
                Log.Error(ex.Message);
                jsonResponseDto.Type = Constante.Warning.ToLower();
                jsonResponseDto.IsValid = false;
                jsonResponseDto.Mensaje = ex.Message.ToString();
            }
            return Json(jsonResponseDto);
        }

        [HttpPost]
        public IActionResult Actualizar([FromForm] ModuloConsultaDto objModulo)
        {
            var jsonResponseDto = new JsonResponseDto();
            try
            {

                objModulo = _Modulo.ConsultaModulo(objModulo);
                if (objModulo != null)
                {
                    return PartialView("Actualizar", objModulo);
                }
                else
                {
                    //Log.Error(response.Content.ToString());
                    jsonResponseDto.Type = Constante.Warning.ToLower();
                    jsonResponseDto.IsValid = false;
                    jsonResponseDto.Mensaje = "Error en al consulta";
                }
            }
            catch (Exception ex)
            {
                Log.Error(ex.Message);
                jsonResponseDto.Type = Constante.Warning.ToLower();
                jsonResponseDto.IsValid = false;
                jsonResponseDto.Mensaje = ex.Message.ToString();
            }
            return Json(jsonResponseDto);
        }
        [HttpPost]
        public IActionResult Consultar([FromForm] ModuloConsultaDto objModulo)
        {
            var jsonResponseDto = new JsonResponseDto();
            try
            {

                objModulo = _Modulo.ConsultaModulo(objModulo);
                if (objModulo != null)
                {
                    return PartialView("Consultar", objModulo);
                }
                else
                {
                    //Log.Error(response.Content.ToString());
                    jsonResponseDto.Type = Constante.Warning.ToLower();
                    jsonResponseDto.IsValid = false;
                    jsonResponseDto.Mensaje = "Error en al consulta";
                }
            }
            catch (Exception ex)
            {
                Log.Error(ex.Message);
                jsonResponseDto.Type = Constante.Warning.ToLower();
                jsonResponseDto.IsValid = false;
                jsonResponseDto.Mensaje = ex.Message.ToString();
            }
            return Json(jsonResponseDto);
        }
        #endregion
        [HttpPost]
        public IActionResult Mantenimiento([FromForm] Modulo objModulo)
        {
            var jsonResponseDto = new JsonResponseDto();
            try
            {
                var message = (objModulo.IdModulo == null) ? Constante.registroExitoso : Constante.actualizacionExitoso;
                objModulo.IdModulo = (objModulo.IdModulo == null) ? "" : objModulo.IdModulo;
                var response = _Modulo.MantenimientoModulo(objModulo);
                if (response == 2)
                {
                    jsonResponseDto.Type = Constante.Error.ToLower();
                    jsonResponseDto.IsValid = false;
                    jsonResponseDto.Mensaje = Constante.registroError;
                }
                else if (response > 0)
                {
                    jsonResponseDto.Type = Constante.Success.ToLower();
                    jsonResponseDto.IsValid = true;
                    jsonResponseDto.Mensaje = Constante.registroExitoso;
                }
                else
                {
                    // Log.Error(response.Content.ToString());
                    jsonResponseDto.Type = Constante.Warning.ToLower();
                    jsonResponseDto.IsValid = false;
                    jsonResponseDto.Mensaje = Constante.registroError;
                }
            }
            catch (Exception ex)
            {
                Log.Error(ex.Message);
                jsonResponseDto.Type = Constante.Warning.ToLower();
                jsonResponseDto.IsValid = false;
                jsonResponseDto.Mensaje = ex.Message.ToString();
            }
            return Json(jsonResponseDto);
        }
        [HttpPost]
        public IActionResult Listar(DataTableModel<ModuloFilterDto> dataTableModel)
        {
            var jsonResponse = new JsonResponseDto() { Type = Constante.Error };

            try
            {
                FormatDataTable(dataTableModel);
                var jsonResponseDto = new JsonResponseDto() { Type = Constante.Error };

                var paginationParameter = new PaginationParameter
                {
                    Start = dataTableModel.start,
                    AmountRows = dataTableModel.length,
                    WhereFilter = dataTableModel.whereFilter,
                    OrderBy = dataTableModel.orderBy
                };

                List<ModuloPaginationDto> lstCampania = _Modulo.PaginadoModulo(paginationParameter);
                dataTableModel.data = lstCampania;
                if (lstCampania.Count > 0)
                {
                    dataTableModel.recordsTotal = lstCampania[0].Cantidad;
                    dataTableModel.recordsFiltered = dataTableModel.recordsTotal;
                }
            }


            catch (Exception ex)
            {
                Log.Error(ex.Message.ToString());
                jsonResponse.Mensaje = ex.Message;
            }
            return Json(dataTableModel);
        }

        #endregion
        #region metodos privados
        private void FormatDataTable(DataTableModel<ModuloFilterDto> dataTableModel)
        {
            for (int i = 0; i < dataTableModel.order.Count; i++)
            {
                var columnIndex = dataTableModel.order[0].column;
                var columnDir = dataTableModel.order[0].dir.ToUpper();
                var column = dataTableModel.columns[columnIndex].data;
                dataTableModel.orderBy = (" [" + column + "] " + columnDir + " ");
            }

            dataTableModel.whereFilter = "WHERE P.Estado != '' ";


            if (dataTableModel.filter != null)
            {
                if (!string.IsNullOrWhiteSpace(dataTableModel.filter.NombreSearch))
                    dataTableModel.whereFilter += (" AND UPPER(P.Nombre) LIKE '%'+'" + dataTableModel.filter.NombreSearch.ToUpper() + "'+'%'");
            }
        }
        #endregion
    }
}