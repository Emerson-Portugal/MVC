﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

using CapaEntidad;
using CapaNegocio;

using System.IO;
using System.Web.Services.Description;

namespace CapaPresentacionTienda.Controllers
{
    public class TiendaController : Controller
    {
        // GET: Tienda
        public ActionResult Index()
        {
            return View();
        }


        public ActionResult DetalleProducto(int idproducto = 0)
        {
            bool conversion;

            var oProducto = new CN_Producto().Listar().Find(p => p.IdProducto == idproducto);

            if (oProducto != null)
            {
                oProducto.Base64 = CN_Recursos.ConvertirBase64(Path.Combine(oProducto.RutaImagen, oProducto.NombreImagen), out conversion);
                oProducto.Extension = Path.GetExtension(oProducto.NombreImagen);
            }

            return View(oProducto);
        }





        [HttpGet]
        public JsonResult ListaCategorias()
        {
            List<Categoria> lista;
            lista = new CN_Categoria().Listar();
            return Json(new { data = lista }, JsonRequestBehavior.AllowGet);
        }





        [HttpPost]
        public JsonResult ListarMarcaporCategoria(int idcategoria)
        {
            List<Marca> lista;
            lista = new CN_Marca().ListarMarcaporCategoria(idcategoria);
            return Json(new { data = lista }, JsonRequestBehavior.AllowGet);
        }


        [HttpPost]
        public JsonResult ListarProducto(int idcategoria, int idmarca) {
            bool conversion;

            var lista = new CN_Producto().Listar().Select(p => new Producto()
            {
                IdProducto = p.IdProducto,
                Nombre = p.Nombre,
                Descripcion = p.Descripcion,
                oMarca = p.oMarca,
                oCategoria = p.oCategoria,
                Precio = p.Precio,
                Stock = p.Stock,
                RutaImagen = p.RutaImagen,
                Base64 = CN_Recursos.ConvertirBase64(Path.Combine(p.RutaImagen, p.NombreImagen), out conversion),
                Extension = Path.GetExtension(p.NombreImagen),
                Activo = p.Activo
            }).Where(p =>
                p.oCategoria.IdCategoria == (idcategoria == 0 ? p.oCategoria.IdCategoria : idcategoria) &&
                p.oMarca.IdMarca == (idmarca == 0 ? p.oMarca.IdMarca : idmarca) &&
                p.Stock > 0 && p.Activo
              ).ToList();


            var jsonresult = Json(new { data = lista }, JsonRequestBehavior.AllowGet);
            jsonresult.MaxJsonLength = int.MaxValue;

            return jsonresult;


        }


        [HttpPost]
        public JsonResult AgregarCarrito(int idproducto)
        {

            int idcliente = ((Cliente)Session["Cliente"]).IdCliente;
            bool existe = new CN_Carrito().ExisteCarrito(idcliente, idproducto);
            bool respuesta = false;
            string mensaje  = string.Empty;
            if (existe)
            {
                mensaje = "El producto ya existe en el carrito";
            }
            else
            {
                respuesta = new CN_Carrito().OperacionCarrito(idcliente, idproducto, true, out mensaje);
            }
            return Json(new {respuesta =respuesta, mensaje= mensaje },JsonRequestBehavior.AllowGet);

        }
        [HttpGet]
        public JsonResult CantidadEnCarrito()
        {
            int idcliente = ((Cliente)Session["Cliente"]).IdCliente;
            int cantidad = new CN_Carrito().CantidadEnCarrito(idcliente);
            return Json(new { cantidad =cantidad }, JsonRequestBehavior.AllowGet);


        }

        [HttpPost]
        public JsonResult ListarProductosCarrito()
        {
            int idcliente = ((Cliente)Session["Cliente"]).IdCliente;

            bool conversion;

            var oLista = new CN_Carrito().ListarProducto(idcliente).Select(oc => new Carrito()
            {
                oProducto = new Producto()
                {
                    IdProducto = oc.oProducto.IdProducto, 
                    Nombre = oc.oProducto.Nombre,
                    oMarca =oc.oProducto.oMarca,
                    Precio = oc.oProducto.Precio,
                    RutaImagen = oc.oProducto.RutaImagen,
                    Base64 =CN_Recursos.ConvertirBase64 (Path.Combine(oc.oProducto.RutaImagen,oc.oProducto.NombreImagen), out conversion),
                    Extension = Path.GetExtension(oc.oProducto.NombreImagen)




                },
                Cantidad = oc.Cantidad

            }).ToList();

            return Json(new { data = oLista }, JsonRequestBehavior.AllowGet);

        }
        [HttpPost]
        public JsonResult OperacionCarrito(int idproducto,bool sumar)
        {

            int idcliente = ((Cliente)Session["Cliente"]).IdCliente;
            bool respuesta = false;
            string mensaje = string.Empty;
            respuesta = new CN_Carrito().OperacionCarrito(idcliente, idproducto, true, out mensaje);

            return Json(new { respuesta = respuesta, mensaje = mensaje }, JsonRequestBehavior.AllowGet);

        }
        [HttpPost]
        public JsonResult EliminarCarrito(int idproducto)
        {
            int idcliente = ((Cliente)Session["Cliente"]).IdCliente;
            bool respuesta = false;
            string mensaje = string.Empty;
            respuesta = new CN_Carrito().EliminarCarrito(idcliente, idproducto);

            return Json(new { respuesta = respuesta, mensaje = mensaje }, JsonRequestBehavior.AllowGet);


        }

    }
}