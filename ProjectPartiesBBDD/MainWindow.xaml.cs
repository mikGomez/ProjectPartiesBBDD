using ProjectPartiesBBDD.Domain;
using ProjectPartiesBBDD.ViewModel;
using ProyectoPoblacion.Domain;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
            DataContext = modelView;
            //Cargamos los datos existentes en la BDD
            modelView.LoadUsers();
            Loaded += calPoblation;
            tvAbsten.TextChanged += calPoblation;
            poblation = new Dates();
            this.DataContext = poblation;
            dgvParties.ItemsSource = modelView.listPart;
            dgvParties2.ItemsSource = modelView.listPart;
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
        // Guardar partidos
        private void btnSave_Click(object sender, RoutedEventArgs e)
        {
            if (!(txtAcronym.Text.Equals("") || txtName.Text.Equals("") || txtPresident.Text.Equals("")))
            {

                if (modelView.listPart == null)
                {
                    modelView.listPart = new ObservableCollection<Partie>();
                }
                //Para que no agregue un partido igual
                if (modelView.listPart.Any(p => p.name == txtName.Text))
                {
                    MessageBox.Show("Ya existe ese partido", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    return; 
                }
                modelView.listPart.Add(new Partie
                {
                    acronym = txtAcronym.Text,
                    name = txtName.Text,
                    president = txtPresident.Text,
                    validVot = 0,
                    seat = 0
                });

                // Agregamos nuevo partidp
                modelView.NewUser();

                txtAcronym.Text = "";
                txtName.Text = "";
                txtPresident.Text = "";

                btnSave.Foreground = new SolidColorBrush(Colors.Black);
                btnSave.IsEnabled = false;

                // Agregamos votos en blanco si tenemos 10 partidos
                if (modelView.listPart.Count == 10)
                {
                    modelView.listPart.Add(new Partie
                    {
                        acronym = "VB",
                        name = "Votos blanco",
                        president = "Ninguno",
                        validVot = 0,
                        seat = 0
                    });
                    // Agregamos votos en blanco a la base de datos
                    modelView.NewUser();
                }

                // Como agregamos en la ult posicion votos en blanco tendremos 11 y activara la nueva pestaña
                if (modelView.listPart.Count == 11)
                {
                    Control.SelectedIndex = 2;
                    item2.IsEnabled = true;
                    item3.IsEnabled = true;
                    dgvParties2.Items.Refresh();
                }
            }
        }
        private void btnDelete_Click(object sender, RoutedEventArgs e)
        {
            if (btnDelete.IsEnabled)
            {
                // Si no la creamos da error, he encontrado que salta error por un tema de seguridad
                List<Partie> selectedParties = dgvParties.SelectedItems.Cast<Partie>().ToList();

                foreach (Partie partie in selectedParties)
                {
                    modelView.name = partie.name;
                    modelView.DeleteUser();
                }
                // Si existe votos en blanco que lo borre, para que sea en la ultima fila donde se coloque
                Partie lastParty = modelView.listPart.LastOrDefault();
                if (lastParty != null && lastParty.name.Equals("Votos blanco"))
                {
                    modelView.name = lastParty.name;
                    modelView.DeleteUser();
                }

                if (dgvParties.Items.Count < 11)
                {
                    item3.IsEnabled = false;
                }
            }
        }
        //Boton para simular los escaños
        private void btnSimulate_(object sender, RoutedEventArgs e)
        {
            
        }

    }
}
