using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NorthWind.Entity
{
    public class TbCategoriaBE
    {
        public Int32 CodCategoria { get; set; }
        public string NomCategoria { get; set; }

        public override string ToString()
        {
            return NomCategoria;
        }

    }
}
