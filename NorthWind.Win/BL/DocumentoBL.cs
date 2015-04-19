using NorthWind.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NorthWind.Win.BL
{
    public class DocumentoBL
    {
        public decimal SubTotal { get; set; }

        public decimal IGV
        {
            get { return SubTotal * (decimal)0.18; }

        }

        public decimal Total
        {
            get { return SubTotal + IGV; }
        }
        public List<ItemBE> oDetalle = new List<ItemBE>();
        public void AgregarDetalle(ItemBE oItem)
        {
            SubTotal += oItem.Total;

            ItemBE iProducto = null;

            if (oDetalle.Count>0)
            {
                iProducto = (from item in oDetalle.ToArray()
                                    where item.Producto.CodProducto == oItem.Producto.CodProducto
                                    select item).SingleOrDefault();
            }

            if (iProducto != null)
            {
                iProducto.Cantidad += oItem.Cantidad;
            }
            else 
            {
                oItem.Item = oDetalle.Count + 1;
                oDetalle.Add(oItem);
            }
            //La cantidad de productos que se va llenando
            
        }

        public List<ItemBE> GetDetalle()
        {
            return oDetalle;
        }


    }
}
