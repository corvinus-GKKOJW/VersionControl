using mikroszimulacio.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace mikroszimulacio
{
    public partial class Form1 : Form
    {
        List<Person> Population = new List<Person>();
        List<BirthProb> BirthProbabilities = new List<BirthProb>();
        List<DeathProb> DeathProbabilities = new List<DeathProb>();

        Random rng = new Random(1234);

        int lastYear;

        List<int> malePop = new List<int>();
        List<int> femalePop = new List<int>();

        public Form1()
        {
            InitializeComponent();
        }

        public List<Person> GetPopulation(string csvpath)
        {
            List<Person> population = new List<Person>();
            using (StreamReader sr = new StreamReader(csvpath, Encoding.Default))
            {
                while (!sr.EndOfStream)
                {
                    var line = sr.ReadLine().Split(';');
                    population.Add(new Person()
                    {
                        BirthYear = int.Parse(line[0]),
                        Gender = (Gender)Enum.Parse(typeof(Gender), line[1]),
                        NumberOfChildren = int.Parse(line[2])
                    });
                }
            }

            return population;
        }

        public List<BirthProb> GetBirthProbs(string csvpath)
        {
            List<BirthProb> birthProbabilities = new List<BirthProb>();
            using (StreamReader sr = new StreamReader(csvpath, Encoding.Default))
            {
                while (!sr.EndOfStream)
                {
                    var line = sr.ReadLine().Split(';');
                    birthProbabilities.Add(new BirthProb()
                    {
                        Age = int.Parse(line[0]),
                        NumberOfChildren = int.Parse(line[1]),
                        BirthProbability = double.Parse(line[2])
                    });
                }
            }

            return birthProbabilities;
        }

        public List<DeathProb> GetDeathProbs(string csvpath)
        {
            List<DeathProb> deathProbabilities = new List<DeathProb>();
            using (StreamReader sr = new StreamReader(csvpath, Encoding.Default))
            {
                while (!sr.EndOfStream)
                {
                    var line = sr.ReadLine().Split(';');
                    deathProbabilities.Add(new DeathProb()
                    {
                        Gender = (Gender)Enum.Parse(typeof(Gender), line[0]),
                        Age = int.Parse(line[1]),
                        DeathProbability = double.Parse(line[2])
                    });
                }
            }

            return deathProbabilities;
        }

        private void SimStep(int year, Person person)
        {
            if (!person.IsAlive) return;

            byte age = (byte)(year - person.BirthYear);

            double pDeath = (from x in DeathProbabilities
                             where x.Gender == person.Gender && x.Age == age
                             select x.DeathProbability).FirstOrDefault();
            
            if (rng.NextDouble() <= pDeath) person.IsAlive = false;

            if (person.IsAlive && person.Gender == Gender.Female)
            {
                double pBirth = (from x in BirthProbabilities
                                 where x.Age == age
                                 select x.BirthProbability).FirstOrDefault();
                if (rng.NextDouble() <= pBirth)
                {
                    Person newborn = new Person();
                    newborn.BirthYear = year;
                    newborn.NumberOfChildren = 0;
                    newborn.Gender = (Gender)(rng.Next(1, 3));
                    Population.Add(newborn);
                }
            }
        }

        public void Simulation()
        {
            Population = GetPopulation(@"C:\Windows\Temp\nép.csv");
            BirthProbabilities = GetBirthProbs(@"C:\Windows\Temp\születés.csv");
            DeathProbabilities = GetDeathProbs(@"C:\Windows\Temp\halál.csv");

            for (int year = 2005; year <= 2024; year++)
            {
                for (int i = 0; i < Population.Count; i++)
                {
                    SimStep(year, Population[i]);
                }

                int NumberOfMales = (from x in Population
                                     where x.Gender == Gender.Male && x.IsAlive
                                     select x).Count();
                int NumberOfFemales = (from x in Population
                                       where x.Gender == Gender.Female && x.IsAlive
                                       select x).Count();
                //Console.WriteLine(string.Format("Év:{0} Fiúk:{1} Lányok:{2}", year, NumberOfMales, NumberOfFemales));

                malePop.Add(NumberOfMales);
                femalePop.Add(NumberOfFemales);

                //teszt
                //textBox1.Text = (string.Format("Év:{0} Fiúk:{1} Lányok:{2}", year, NumberOfMales, NumberOfFemales));
            }
        }

        private void buttonStart_Click(object sender, EventArgs e)
        {
            richTextBox1.Clear();
            malePop.Clear();
            femalePop.Clear();
            Simulation();
            DisplayResults();
        }

        private void numericUpDown1_ValueChanged(object sender, EventArgs e)
        {
            lastYear = Convert.ToInt32(numericUpDown1.Value);
        }

        private void buttonBrowse_Click(object sender, EventArgs e)
        {
            using (OpenFileDialog ofd = new OpenFileDialog())
            {
                ofd.InitialDirectory = "c:\\Windows\\Temp";
                ofd.Filter = "csv files (*.csv)|*.csv|All files (*.*)|*.*";
                if (ofd.ShowDialog() == DialogResult.OK)
                {
                    fileTextBox.Text = ofd.FileName;
                }
            }
        }

        public void DisplayResults()
        {
            for (int i = 2004; i < lastYear; i++)
            {
                richTextBox1.Text = "Szimulációs év: " + i + "\n" + "\t" + "Fiúk: " + malePop + "\n" + "\t" + "Lányok: " + femalePop;
            }
        }
    }
}
