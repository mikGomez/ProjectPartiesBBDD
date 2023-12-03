using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProyectoPoblacion.Domain
{
    public class Dates : INotifyPropertyChanged
    {
        private int _pobla = 0;
        private int _absten = 0;
        private int _nullVotes = 0;
        private int _votesV = 0;

        public event PropertyChangedEventHandler? PropertyChanged;

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

        private void OnPropertyChange(string propertyName)
        {
            if (PropertyChanged != null) PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }
        public Dates() { }
        public Dates(int poblation, int absten, int nullVotes)
        {
            this.pobla = poblation;
            this.absten = absten;
            this.nullVotes = nullVotes;
        }
        public void calcularValidos(int nVotes,int votAbs,int pob)
        {
            votesV = pob - nVotes - votAbs;
        }

        public override string? ToString()
        {
            return base.ToString();
        }

    }
}
