using CapaDatos;
using CapaEntidad;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CapaNegocio
{
#pragma warning disable S101 // Types should be named in PascalCase
    public class CN_Cliente
#pragma warning restore S101 // Types should be named in PascalCase
    {
        private readonly CD_Cliente objCapaDato = new CD_Cliente();


        public int Registrar(Cliente obj, out string Mensaje)
        {

            Mensaje = string.Empty;


            if (string.IsNullOrEmpty(obj.Nombres) || string.IsNullOrWhiteSpace(obj.Nombres))
            {
                Mensaje = "El nombre del cliente no puede ser vacio";
            }
            else if (string.IsNullOrEmpty(obj.Apellidos) || string.IsNullOrWhiteSpace(obj.Apellidos))
            {
                Mensaje = "El apellido del cliente no puede ser vacio";
            }
            else if (string.IsNullOrEmpty(obj.Correo) || string.IsNullOrWhiteSpace(obj.Correo))
            {
                Mensaje = "El correo del cliente no puede ser vacio";
            }


            if (string.IsNullOrEmpty(Mensaje))
            {
                return objCapaDato.Registrar(obj, out Mensaje);


            }
            else
            {

                return 0;
            }
        }




           

            public List<Cliente> Listar()
            {
                return objCapaDato.Listar();
            }
        public Cliente ObtenerUsuario(string correo, string clave)
        {
            return Listar().SingleOrDefault(item => item.Correo == correo && item.Clave == clave);
        }

        public bool ValidarUsuario(string correo, string clave, out int idCliente)
        {
            idCliente = 0;
            return objCapaDato.ValidarUsuario(correo, clave, out idCliente);
        }
        public Cliente ObtenerClientePorId(int idCliente)
        {
            // Llama al método correspondiente en la capa de datos para obtener el cliente por su ID
            return objCapaDato.ObtenerClientePorId(idCliente);
        }
    }
            


    }


 

