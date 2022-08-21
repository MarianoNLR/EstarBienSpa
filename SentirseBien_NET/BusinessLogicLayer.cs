using SentirseBien_NET.Modelos;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SentirseBien_NET
{
    public class BusinessLogicLayer
    {
        DataAccessLayer _dataAccessLayer = new DataAccessLayer();

        public DataSet obtenerClientes()
        {
            return _dataAccessLayer.obtenerClientes();
        }


        public DataSet obtenerProfesionales()
        {
            return _dataAccessLayer.obtenerProfesionales();
        }

        public DataSet obtenerPagos()
        {
            return _dataAccessLayer.obtenerPagos();
        }

        public DataSet obtenerCitas(string rolActual)
        {
            return _dataAccessLayer.obtenerCitas(rolActual);
        }

        public Paciente iniciarSesion(string email, string password)
        {
            return _dataAccessLayer.IniciarSesion(email, password);
        }

        public bool agregarPersonal(Paciente nuevo) 
        {
           return _dataAccessLayer.AgregarPersonal(nuevo);
        }

        public void DarDeBaja(string email)
        {
            _dataAccessLayer.DarDeBaja(email);
        }

        public DataSet obtenerLineasFactura(int idfactura)
        {
            return _dataAccessLayer.obtenerLineasFactura(idfactura);
        }


    }
}
