using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Configuration;

namespace CapaDatos
{
#pragma warning disable S1118 // Utility classes should not have public constructors
    public class Conexion
#pragma warning restore S1118 // Utility classes should not have public constructors
    {
        public static string cn = ConfigurationManager.ConnectionStrings["cadena"].ToString();

    }
}
