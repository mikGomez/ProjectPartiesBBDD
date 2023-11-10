using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Data;
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
        private ObservableCollection<Partie> _parties;
        private String _acronym = "";
        private String _name = "";
        private String _president = "";
        private int _validVot = 0;
        private int _seat = 0;
        #endregion

        #region OBJETOS
        public ObservableCollection<Partie> Partie
        {
            get { return _parties; }
            set
            {
                _parties = value;
                OnPropertyChange("users");
            }
        }
        public String userName
        {
            get { return _user; }
            set
            {
                _user = value;
                OnPropertyChange("user");
            }
        }
        public String mail
        {
            get { return _mail; }
            set
            {
                _mail = value;
                OnPropertyChange("mail");
            }
        }

        public String age
        {
            get { return _age; }
            set
            {
                _age = value;
                OnPropertyChange("age");
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
            String SQL = $"INSERT INTO usuarios (usuario, mail, edad) VALUES ('{userName}','{mail}', '{age}');";
            //usaremos las clases de la librería de MySQL para ejecutar queries
            //Instalar el paqueta MySQL.Data
            MySQLDataManagement.ExecuteNonQuery(SQL, cnstr);
        }

        public void UpdateUser()
        {
            String SQL = $"UPDATE usuarios SET mail = '{mail}', edad = '{age}' WHERE usuario = '{userName}';";
            MySQLDataManagement.ExecuteNonQuery(SQL, cnstr);
        }

        public void LoadUsers()
        {
            String SQL = $"SELECT usuario, mail, edad FROM usuarios;";
            DataTable dt = MySQLDataManagement.LoadData(SQL, cnstr);
            if (dt.Rows.Count > 0)
            {
                if (Parties == null) Parties = new ObservableCollection<Partie>();
                foreach (DataRow i in dt.Rows)
                {
                    Parties.Add(new Partie
                    {
                        _acronym = i[0].ToString(),
                        _name = i[0].ToString(),
                        _president = i[0].ToString(),
                        _validVot = i[0].ToString(),
                        _seat = i[0].ToString()
                    });
                }
            }
            dt.Dispose();
        }
    }
}
