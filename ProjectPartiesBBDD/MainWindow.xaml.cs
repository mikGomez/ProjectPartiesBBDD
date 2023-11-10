using ProjectPartiesBBDD.Domain;
using ProjectPartiesBBDD.ViewModel;
using ProyectoPoblacion.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace ProjectPartiesBBDD
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public Dates poblation { get; set; }
        const System.String POBLACION_MADRID = "6.921.267";
        private UserModelView modelView = new UserModelView();

        public MainWindow()
        {
            InitializeComponent();
            List<Partie> parties = modelView.listPart.ToList();
            DataContext = modelView;
            //Cargamos los datos existentes en la BDD
            modelView.LoadUsers();
            Loaded += calPoblation;
            tvAbsten.TextChanged += calPoblation;
            poblation = new Dates();
            this.DataContext = poblation;
            dgvParties.ItemsSource = parties.getListPartie();
            dgvParties2.ItemsSource = parties.getListPartie();
            txtPresident.TextChanged += txtName_changed;
            txtAcronym.TextChanged += txtName_changed;
            txtName.TextChanged += txtName_changed;
        }

        //Funcion por si cambian los text
        private void txtName_changed(object sender, TextChangedEventArgs e)
        {
            if (!(txtAcronym.Text.Equals("") || txtName.Text.Equals("") || txtPresident.Text.Equals("")) && dgvParties.Items.Count <= 10)
            {
                btnSave.Foreground = new SolidColorBrush(Colors.White);
                btnSave.IsEnabled = true;
            }
            else
            {
                btnSave.Foreground = new SolidColorBrush(Colors.Black);
                btnSave.IsEnabled = false;
            }

        }
        //Calcular votos nulos
        private void calPoblation(object sender, RoutedEventArgs e)
        {
            const int CONSTANTE = 20;
            tvPobla.Text = POBLACION_MADRID;
            tvPobla.IsReadOnly = true;
            int pob = int.Parse(POBLACION_MADRID.Replace(".", ""));
            if (int.TryParse(tvAbsten.Text.Replace(".", ""), out int absten) || tvAbsten.Text == "")
            {
                if (!(tvAbsten.Text == ""))
                {
                    if (pob >= absten)
                    {
                        int nulos = (pob - absten) / CONSTANTE;
                        tvNullVotes.Text = nulos.ToString();
                        tvNullVotes.IsReadOnly = true;
                    }
                    else
                    {
                        MessageBox.Show("Number Absten is bigger than number Poblation", "Number", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                }
            }
            else
            {
                MessageBox.Show("Enter numbers", "Number", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
        //Guardar numero
        private void savePob(object sender, RoutedEventArgs e)
        {
            if (int.TryParse(tvPobla.Text.Replace(".", ""), out int po) && int.TryParse(tvAbsten.Text.Replace(".", ""), out int abs) && int.TryParse(tvNullVotes.Text.Replace(".", ""), out int nuV))
            {
                poblation = new Dates(po, abs, nuV);
                poblation.calcularValidos(nuV, abs, po);
                MessageBox.Show("Guardado\n", "Save", MessageBoxButton.OK, MessageBoxImage.Information);
                Control.SelectedIndex = 1;
                if (dgvParties.Items.Count == 11)
                {
                    int cont = 1;
                    foreach (Partie partie in parties.getListPartie())
                    {
                        parties.calculateValidVotes(partie, cont, poblation.votesValid);
                        cont++;
                    }
                    dgvParties2.Items.Refresh();
                }
            }
            else
            {
                MessageBox.Show("Enter numbers", "Number", MessageBoxButton.OK, MessageBoxImage.Error);
            }

        }
        //Para saber cual esta seleccionado
        private void dgvParties_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (dgvParties.SelectedItem != null)
            {
                btnDelete.Foreground = new SolidColorBrush(Colors.White);
                btnDelete.IsEnabled = true;
            }
            else
            {
                btnDelete.Foreground = new SolidColorBrush(Colors.Black);
                btnDelete.IsEnabled = false;
            }

        }
    }
}
