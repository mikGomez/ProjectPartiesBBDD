using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Data;
using System.Windows.Controls;
using ProjectPartiesBBDD.DB;
using ProjectPartiesBBDD.Domain;

namespace ProjectPartiesBBDD.ViewModel
{
    public class UserModelView : INotifyPropertyChanged
    {
        #region VARIABLES
        public event PropertyChangedEventHandler? PropertyChanged;

        //Declaro la constante para la conexión a la BDD
        private const String cnstr = "server=localhost;uid=miguel;pwd=miguel;database=parties";
        //Modelo de la lista de registros a mostrar
        private ObservableCollection<Partie> _party;
        private String _acronym = "";
        private String _name = "";
        private String _president = "";
        private int _validVot = 0;
        private int _seat = 0;
        #endregion

        #region OBJETOS
        public ObservableCollection<Partie> listPart
        {
            get { return _party; }
            set
            {
                _party = value;
                OnPropertyChange("party");
            }
        }  
        public String acronym
        {
            get { return _acronym; }
            set
            {
                _acronym = value;
                OnPropertyChange("acronym");
            }
        }
        public String name
        {
            get { return _name; }
            set
            {
                _name = value;
                OnPropertyChange("name");
            }
        }

        public String president
        {
            get { return _president; }
            set
            {
                _president = value;
                OnPropertyChange("president");
            }
        }
        public int validVot
        {
            get { return _validVot; }
            set
            {
                _validVot = value;
                OnPropertyChange("validVot");
            }
        }
        public int seat
        {
            get { return _seat; }
            set
            {
                _seat = value;
                OnPropertyChange("seat");
            }
        }
        #endregion

        //Método que se encarga de actualizar las propiedades en cada cambio
        private void OnPropertyChange(string propertyName)
        {
            if (PropertyChanged != null) PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }

        public void NewUser()
        {
            String SQL = $"INSERT INTO party (acronym, name, president, validVot, seat) VALUES ('{acronym}','{name}', '{president}','{validVot}', '{seat}');";
            //usaremos las clases de la librería de MySQL para ejecutar queries
            //Instalar el paqueta MySQL.Data
            MySQLDataManagement.ExecuteNonQuery(SQL, cnstr);
        }

        public void UpdateUser()
        {
            String SQL = $"UPDATE party SET acronym = '{acronym}', president = '{president}, 'validVot = '{validVot}', seat = '{seat}' WHERE name = '{name}';";
            MySQLDataManagement.ExecuteNonQuery(SQL, cnstr);
        }
        public void DeleteUser()
        {
            String SQL = $"Delete FROM party WHERE name = '{name}';";
            MySQLDataManagement.ExecuteNonQuery(SQL, cnstr);
        }

        public void LoadUsers()
        {
            String SQL = $"SELECT usuario, mail, edad FROM usuarios;";
            DataTable dt = MySQLDataManagement.LoadData(SQL, cnstr);
            if (dt.Rows.Count > 0)
            {
                if (listPart == null) listPart = new ObservableCollection<Partie>();
                foreach (DataRow i in dt.Rows)
                {
                    listPart.Add(new Partie
                    {
                        acronym = i[0].ToString(),
                        name = i[0].ToString(),
                        president = i[0].ToString(),
                        validVot = int.Parse((string)i[0]),
                        seat = int.Parse((string)i[0])
                    });
                }
            }
            dt.Dispose();
        }
    }
}
