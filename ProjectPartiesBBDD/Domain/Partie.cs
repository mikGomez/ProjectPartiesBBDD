using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectPartiesBBDD.Domain
{
    public class Partie
    {
        #region Propiedades
        public event PropertyChangedEventHandler? PropertyChanged;
        private String _acronym = "";
        private String _name = "";
        private String _president = "";
        private int _validVot = 0;
        private int _seat = 0;
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

        private void OnPropertyChange(string propertyName)
        {
            if (PropertyChanged != null) PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
