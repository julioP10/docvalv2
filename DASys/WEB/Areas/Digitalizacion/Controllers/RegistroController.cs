using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Entidad;
using Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace WEB.Areas.Digitalizacion.Controllers
{
    [Area("Digitalizacion")]
    public class RegistroController : Controller
    {
        private readonly IDigitalizacion _Digitalizacion;
        private readonly IColaborador _Colaborador;
        private readonly IVehiculo _Vehiculo;
        private readonly IEmpresa _Empresa;
        private readonly ILogger<RegistroController> _logger;
        public RegistroController(IDigitalizacion Digitalizacion, IColaborador Colaborador, IVehiculo Vehiculo, IEmpresa Empresa, ILogger<RegistroController> logger)
        {
            _Digitalizacion = Digitalizacion;
            _Colaborador = Colaborador;
            _Vehiculo = Vehiculo;
            _Empresa = Empresa;
            _logger = logger;
        }
        public IActionResult Index()
        {
            return View();
        }
        public IActionResult Empresa()
        {
            EmpresaFilterDto objEmpresaFilterDto = new EmpresaFilterDto();
            return PartialView(objEmpresaFilterDto);
        }
        public IActionResult Colaborador()
        {
            ColaboradorFilterDto objColaboradorFilterDto = new ColaboradorFilterDto();
            return PartialView(objColaboradorFilterDto);
        }

        public IActionResult Maquinaria()
        {
            MaquinariaFilterDto objMaquinariaFilterDto = new MaquinariaFilterDto();
            return PartialView(objMaquinariaFilterDto);
        }
        public IActionResult Vehiculo()
        {
            VehiculoFilterDto objVehiculoFilterDto = new VehiculoFilterDto();
            return PartialView(objVehiculoFilterDto);
        }
    }

}