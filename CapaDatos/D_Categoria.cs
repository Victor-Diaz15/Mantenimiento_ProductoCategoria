using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using System.Configuration;
using CapaEntidad;
using System.Data;

namespace CapaDatos
{
    public class D_Categoria
    {
        //Connection String
        SqlConnection conexion = new SqlConnection(ConfigurationManager.ConnectionStrings["conexion"].ConnectionString);
    
        //Methods CRUD

        //Listando las categorias
        public List<E_Categoria> ListarCategorias()
        {
            SqlDataReader dataReader;
            SqlCommand cm = new SqlCommand("SP_SELECTALLCATEGORIAS", conexion);
            cm.CommandType = CommandType.StoredProcedure;
            conexion.Open();

            dataReader = cm.ExecuteReader();
            List<E_Categoria> listar = new List<E_Categoria>();
            while (dataReader.Read())
            {
                listar.Add(new E_Categoria
                { 
                    IdCategoria = dataReader.GetInt32(0),
                    CodigoCategoria = dataReader.GetString(1),
                    Nombre = dataReader.GetString(2),
                    Descripcion = dataReader.GetString(3)
                });
            }
            conexion.Close();
            dataReader.Close();
            return listar;
        }
        //Listando las categorias por sus nombres
        public List<E_Categoria> ListarCategoria(string buscar)
        {
            SqlDataReader dataReader;
            SqlCommand cm = new SqlCommand("SP_BUSCARCATEGORIA", conexion);
            cm.CommandType = CommandType.StoredProcedure;
            conexion.Open();

            cm.Parameters.AddWithValue("@BUSCAR", buscar);
            dataReader = cm.ExecuteReader();
            List<E_Categoria> listar = new List<E_Categoria>();
            while (dataReader.Read())
            {
                listar.Add(new E_Categoria
                {
                    IdCategoria = dataReader.GetInt32(0),
                    CodigoCategoria = dataReader.GetString(1),
                    Nombre = dataReader.GetString(2),
                    Descripcion = dataReader.GetString(3)
                });
            }
            conexion.Close();
            dataReader.Close();
            return listar;
        }
        //Agregando categoria 
        public void AgregarCategoria(E_Categoria Categoria)
        {
            SqlCommand cm = new SqlCommand("SP_INSERTARCATEGORIA", conexion);
            cm.CommandType = CommandType.StoredProcedure;
            conexion.Open();

            cm.Parameters.AddWithValue("@NOMBRE", Categoria.Nombre);
            cm.Parameters.AddWithValue("@DESCRIPCION", Categoria.Descripcion);

            cm.ExecuteNonQuery();
            conexion.Close();
        }
        //editando categoria
        public void EditarCategoria(E_Categoria Categoria)
        {
            SqlCommand cm = new SqlCommand("SP_EDITARCATEGORIA", conexion);
            cm.CommandType = CommandType.StoredProcedure;
            conexion.Open();

            cm.Parameters.AddWithValue("@IDCATEGORIA", Categoria.IdCategoria);
            cm.Parameters.AddWithValue("@NOMBRE", Categoria.Nombre);
            cm.Parameters.AddWithValue("@DESCRIPCION", Categoria.Descripcion);

            cm.ExecuteNonQuery();
            conexion.Close();
        }
        //Eliminando categoria
        public void EliminarCategoria(E_Categoria Categoria)
        {
            SqlCommand cm = new SqlCommand("SP_ELIMINARCATEGORIA", conexion);
            cm.CommandType = CommandType.StoredProcedure;
            conexion.Open();

            cm.Parameters.AddWithValue("@IDCATEGORIA", Categoria.IdCategoria);
            
            cm.ExecuteNonQuery();
            conexion.Close();
        }
    }
}
