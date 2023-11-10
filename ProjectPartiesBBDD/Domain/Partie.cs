using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectPartiesBBDD.Domain
{
    internal class Partie
    {
        #region Propiedades

        private string _acronym;
        public string Acronym
        {
            get { return _acronym; }
            set { _acronym = value; }
        }

        private string _name;
        public string Name
        {
            get { return _name; }
            set { _name = value; }
        }

        private string _president;
        public string President
        {
            get { return _president; }
            set { _president = value; }
        }

        private int _validVot;
        public int ValidVot
        {
            get { return _validVot; }
            set { _validVot = value; }
        }

        private int _seat;
        public int Seat
        {
            get { return _seat; }
            set { _seat = value; }
        }

        #endregion

        private void OnPropertyChange(string propertyName)
        {
            if (PropertyChanged != null) PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
