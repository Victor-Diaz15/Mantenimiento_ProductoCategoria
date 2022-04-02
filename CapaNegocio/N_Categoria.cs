using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CapaEntidad;
using CapaDatos;

namespace CapaNegocio
{
    public class N_Categoria
    {
        //objeto de la clase Categoria
        D_Categoria Datos = new D_Categoria();

        //Listando categorias
        public List<E_Categoria> ListandoCategorias()
        {
            return Datos.ListarCategorias();
        }

        //Listando categoria
        public List<E_Categoria> ListandoCategoria(string buscar)
        {
            return Datos.ListarCategoria(buscar);
        }

        //Insertando categoria
        public void AgregandoCategoria(E_Categoria Categoria)
        {
            Datos.AgregarCategoria(Categoria);
        }

        //Editando categoria
        public void EditandoCategoria(E_Categoria Categoria)
        {
            Datos.EditarCategoria(Categoria);
        }

        //Eliminando categoria
        public void EliminandoCategoria(E_Categoria Categoria)
        {
            Datos.EliminarCategoria(Categoria);
        }
    }
}
