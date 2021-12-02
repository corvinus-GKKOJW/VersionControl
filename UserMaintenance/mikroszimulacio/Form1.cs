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

        Random rng = new Random(69420);

        public Form1()
        {
            InitializeComponent();

            Population = GetPopulation(@"C:\Windows\Temp\nép.csv");
            BirthProbabilities = GetBirthProbs(@"C:\Windows\Temp\születés.csv");
            DeathProbabilities = GetDeathProbs(@"C:\Windows\Temp\halál.csv");

            for (int year = 2005; year <= 2024; year++)
            {
                for (int i = 0; i < Population.Count; i++)
                {
                    //sim
                }

                int NumberOfMales = (from x in Population
                                     where x.Gender == Gender.Male && x.IsAlive
                                     select x).Count();
                int NumberOfFemales = (from x in Population
                                       where x.Gender == Gender.Female && x.IsAlive
                                       select x).Count();
                Console.WriteLine(string.Format("Év:{0} Fiúk:{1} Lányok:{2}", year, NumberOfMales, NumberOfFemales));
            }
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
                        Age = int.Parse(line[0]),
                        NumberOfChildren = int.Parse(line[1]),
                        DeathProbability = double.Parse(line[2])
                    });
                }
            }

            return deathProbabilities;
        }
    }
}
