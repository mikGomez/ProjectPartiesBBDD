using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Printing;
using System.Windows.Controls;
using ProjectPartiesBBDD.DB;
using ProjectPartiesBBDD.Domain;
using ProyectoPoblacion.Domain;

namespace ProjectPartiesBBDD.ViewModel
{
    public class PartiesDates : INotifyPropertyChanged
    {
        #region VARIABLES
        public event PropertyChangedEventHandler? PropertyChanged;

        //Declaro la constante para la conexión a la BDD
        private const String cnstr = "server=localhost;uid=miguel;pwd=miguel;database=parties";
        private int _pobla = 0;
        private int _absten = 0;
        private int _nullVotes = 0;
        private int _votesV = 0;
        #endregion

        #region OBJETOS
        public int pobla
        {
            get { return _pobla; }
            set
            {
                if (_pobla != value)
                {
                    _pobla = value;
                    OnPropertyChange("pobla");
                }
            }
        }

        public int absten
        {
            get { return _absten; }
            set
            {
                if (_absten != value)
                {
                    _absten = value;
                    OnPropertyChange("absten");
                }
            }
        }

        public int nullVotes
        {
            get { return _nullVotes; }
            set
            {
                if (_nullVotes != value)
                {
                    _nullVotes = value;
                    OnPropertyChange("nullVotes");
                }
            }
        }

        public int votesV
        {
            get { return _votesV; }
            set
            {
                if (_votesV != value)
                {
                    _votesV = value;
                    OnPropertyChange("votesV");
                }
            }
        }
        #endregion

        //Método que se encarga de actualizar las propiedades en cada cambio
        private void OnPropertyChange(string propertyName)
        {
            if (PropertyChanged != null) PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }
        public void UpadateOrNew()
        {
            String SQL = $"SELECT * FROM datosvotos;";
            DataTable dt = MySQLDataManagement.LoadData(SQL, cnstr);
            if (dt.Rows.Count > 0)
            {
                UpdateUser();
               
            }
            else
            {
               NewUser();
            }
        }
        public void NewUser()
        {
            String SQL = $"INSERT INTO datosvotos (pobla, absten, nullVotes, votesV) VALUES ('{pobla}','{absten}', '{nullVotes}','{votesV}');";
            //usaremos las clases de la librería de MySQL para ejecutar queries
            //Instalar el paqueta MySQL.Data

            MySQLDataManagement.ExecuteNonQuery(SQL, cnstr);
            // Para que cargue si hemos agregao nuevo usuario
            //LoadUsers();
        }
        public void UpdateUser()
        {
            String SQL = $"UPDATE datosvotos SET pobla = '{pobla}',absten = '{absten}', nullVotes = '{nullVotes}',votesV = '{votesV}' WHERE id = '1';";
            MySQLDataManagement.ExecuteNonQuery(SQL, cnstr);
        }
        public void LoadUsers(Dates pobla)
        {
            String SQL = $"SELECT pobla, absten, nullVotes, votesV FROM datosvotos WHERE id = '1';";
            DataTable dt = MySQLDataManagement.LoadData(SQL, cnstr);

            if (dt.Rows.Count > 0)
            {
                pobla.pobla = int.Parse(dt.Rows[0]["pobla"].ToString());
                pobla.absten = int.Parse(dt.Rows[0]["absten"].ToString());
                pobla.nullVotes = int.Parse(dt.Rows[0]["nullVotes"].ToString());
                pobla.votesV = int.Parse(dt.Rows[0]["votesV"].ToString());
            }


            dt.Dispose();
        }
    }
}
