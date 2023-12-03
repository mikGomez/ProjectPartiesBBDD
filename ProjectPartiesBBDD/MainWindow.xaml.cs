using ProjectPartiesBBDD.DB;
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
using System.Xml.Linq;

namespace ProjectPartiesBBDD
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public Dates poblation;
        const System.String POBLACION_MADRID = "6.921.267";
        private UserModelView modelView = new UserModelView();
        private PartiesDates modelDates = new PartiesDates();

        public MainWindow()
        {
            InitializeComponent();
            poblation = new Dates();
            modelDates.LoadUsers(poblation);
            PreviousData.DataContext = poblation;
            DataContext = modelView;
            modelView.LoadUsers();
            Loaded += calPoblation;
            tvAbsten.TextChanged += calPoblation;  
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
                        tvAbsten.Text = "";
                        tvNullVotes.Text = "";
                    }
                }
            }
            else
            {
                MessageBox.Show("Enter numbers", "Number", MessageBoxButton.OK, MessageBoxImage.Error);
                tvAbsten.Text = "";
                tvNullVotes.Text = "";
            }
        }
        //Guardar numero
        private void savePob(object sender, RoutedEventArgs e)
        {
            if (int.TryParse(tvPobla.Text.Replace(".", ""), out int po) && int.TryParse(tvAbsten.Text.Replace(".", ""), out int abs) && int.TryParse(tvNullVotes.Text.Replace(".", ""), out int nuV))
            {
                poblation = new Dates(po, abs, nuV);
                poblation.calcularValidos(nuV, abs, po);
                modelDates.pobla = po;
                modelDates.absten = abs;
                modelDates.nullVotes = nuV;
                modelDates.votesV = poblation.votesV;
                modelDates.UpadateOrNew();
                MessageBox.Show("Guardado\n", "Save", MessageBoxButton.OK, MessageBoxImage.Information);
                Control.SelectedIndex = 1;
                if (modelView.listPart.Count >= 10)
                {
                    int cont = 1;
                    foreach (Partie partie in modelView.listPart)
                    {
                        calculateValidVotes(partie, cont, poblation.votesV);
                        cont++;
                    }
                    item2.IsEnabled = true;
                    item3.IsEnabled = true;
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
                    acronym = modelView.acronym,
                    name = modelView.name,
                    president = modelView.president,
                    validVot = 0,
                    seat = 0
                });

                // Agregamos nuevo partidp
                modelView.NewUser();

                modelView.acronym = "";
                modelView.name = "";
                modelView.president = "";

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
                    modelView.acronym = "VB";
                    modelView.name = "Votos blanco";
                    modelView.president = "Ninguno";
                    modelView.validVot = 0;
                    modelView.seat = 0;
                    // Agregamos votos en blanco a la base de datos
                    modelView.NewUser();

                    modelView.acronym = "";
                    modelView.name = "";
                    modelView.president = "";
                    int cont = 1;
                    foreach (Partie partie in modelView.listPart)
                    {
                        calculateValidVotes(partie, cont, poblation.votesV);
                        modelView.acronym = partie.acronym;
                        modelView.name = partie.name;
                        modelView.president = partie.president;
                        modelView.validVot = partie.validVot;
                        modelView.seat = partie.seat;
                        modelView.validVot = partie.validVot;
                        modelView.UpdateUser();
                        cont++;
                    }
                    
                }

                // Como agregamos en la ult posicion votos en blanco tendremos 11 y activara la nueva pestaña
                if (modelView.listPart.Count == 11)
                {
                    Control.SelectedIndex = 2;
                    item2.IsEnabled = true;
                    item3.IsEnabled = true;
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
                    modelView.listPart.Remove(partie);
                }
                // Si existe votos en blanco que lo borre, para que sea en la ultima fila donde se coloque
                Partie lastParty = modelView.listPart.LastOrDefault();
                if (lastParty != null && lastParty.name.Equals("Votos blanco"))
                {
                    modelView.name = lastParty.name;
                    modelView.DeleteUser();
                    modelView.listPart.Remove(lastParty);
                }

                modelView.name = "";

                if (dgvParties.Items.Count < 11)
                {
                    item3.IsEnabled = false;
                }
            }
        }
        //Boton para simular los escaños
        private void btnSimulate_(object sender, RoutedEventArgs e)
        {
            const int ESCANIOS = 37;
            List<Partie> partiesClon = new List<Partie>();
            List<Partie> partiesRemove = new List<Partie>();

            foreach (Partie party in modelView.listPart)
            {
                party.seat = 0;
                Partie partyClone = new Partie
                {
                    name = party.name,
                    validVot = party.validVot,
                    seat = party.seat
                };

                if (partyClone.validVot < (0.03 * poblation.votesV))
                {
                    partiesRemove.Add(partyClone);
                }
                else
                {
                    partiesClon.Add(partyClone);
                }
            }

            foreach (Partie partyToRemove in partiesRemove)
            {
                partiesClon.Remove(partyToRemove);
            }
            int aux = 0;
            for (int i = 0; i < ESCANIOS; i++)
            {
                for (int j = 0; j < partiesClon.Count; j++)
                {
                    if (j == 0)
                    {
                        aux = 0;
                    }
                    else
                    {
                        if (partiesClon[j].validVot > partiesClon[aux].validVot)
                        {
                            aux = j;
                        }
                    }
                }
                modelView.listPart[aux].seat += 1;
                int num = modelView.listPart[aux].validVot;
                partiesClon[aux].validVot = num / (modelView.listPart[aux].seat + 1);
                modelView.acronym = modelView.listPart[aux].acronym;
                modelView.name = modelView.listPart[aux].name;
                modelView.president = modelView.listPart[aux].president;
                modelView.validVot = modelView.listPart[aux].validVot;
                modelView.seat = modelView.listPart[aux].seat;
                modelView.validVot = modelView.listPart[aux].validVot;
                modelView.UpdateUser();
            }
        }
        private void calculateValidVotes(Partie p, int opc, int poblationVotesValid)
        {
            switch (opc)
            {
                case 1:
                    p.validVot = (int)Math.Ceiling(poblationVotesValid * 0.3525);
                    break;
                case 2:
                    p.validVot = (int)Math.Ceiling(poblationVotesValid * 0.2475);
                    break;
                case 3:
                    p.validVot = (int)Math.Ceiling(poblationVotesValid * 0.1575);
                    break;
                case 4:
                    p.validVot = (int)Math.Ceiling(poblationVotesValid * 0.1425);
                    break;
                case 5:
                    p.validVot = (int)Math.Ceiling(poblationVotesValid * 0.0375);
                    break;
                case 6:
                    p.validVot = (int)Math.Ceiling(poblationVotesValid * 0.0325);
                    break;
                case 7:
                    p.validVot = (int)Math.Ceiling(poblationVotesValid * 0.015);
                    break;
                case 8:
                    p.validVot = (int)Math.Ceiling(poblationVotesValid * 0.005);
                    break;
                case 9:
                    p.validVot = (int)Math.Ceiling(poblationVotesValid * 0.0025);
                    break;
                case 10:
                    p.validVot = (int)Math.Ceiling(poblationVotesValid * 0.0025);
                    break;
                case 11:
                    p.validVot = (int)Math.Ceiling(poblationVotesValid * 0.005);
                    break;
                default:
                    Console.WriteLine("Opción no válida");
                    break;
            }
        }
    }
}
