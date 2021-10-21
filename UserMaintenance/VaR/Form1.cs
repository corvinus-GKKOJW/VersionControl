﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using VaR.entities;

namespace VaR
{
    public partial class Form1 : Form
    {
        PortfolioEntities context = new PortfolioEntities();
        List<Tick> Ticks;

        List<PortfolioItem> Portfolio = new List<PortfolioItem>();

        List<decimal> Nyereségek = new List<decimal>();

        public Form1()
        {
            InitializeComponent();
            Ticks = context.Ticks.ToList();
            dataGridView1.DataSource = Ticks;

            CreatePortfolio();

            int intervallum = 30;
            DateTime kezdőDátum = (from x in Ticks select x.TradingDay).Min();
            DateTime záróDátum = new DateTime(2016, 12, 30);
            TimeSpan z = záróDátum - kezdőDátum;
            for (int i = 0; i < z.Days - intervallum; i++)
            {
                decimal ny = GetPortfolioValue(kezdőDátum.AddDays(i + intervallum)) - GetPortfolioValue(kezdőDátum.AddDays(i));
                Nyereségek.Add(ny);
                Console.WriteLine(i + " " + ny);
            }

            var nyereségekRendezve = (from x in Nyereségek
                                      orderby x
                                      select x).ToList();
            MessageBox.Show(nyereségekRendezve[nyereségekRendezve.Count() / 5].ToString());

            SaveToFile();
        }

        public void CreatePortfolio()
        {
            Portfolio.Add(new PortfolioItem() { Index = "OTP", Volume = 10 });

            //vagy lehetne így is hozzáadni:
            /*PortfolioItem p = new PortfolioItem();
            p.Index = "OTP";
            p.Volume = 10;
            Portfolio.Add(p);*/

            Portfolio.Add(new PortfolioItem() { Index = "ZWACK", Volume = 10 });
            Portfolio.Add(new PortfolioItem() { Index = "ELMU", Volume = 10 });

            dataGridView2.DataSource = Portfolio;
        }

        public decimal GetPortfolioValue(DateTime date)
        {
            decimal value = 0;
            foreach (var item in Portfolio)
            {
                var last = (from x in Ticks
                            where item.Index == x.Index.Trim() && date <= x.TradingDay
                            select x).First();
                value += (decimal)last.Price * item.Volume;
            }
            return value;
        }

        public void SaveToFile()
        {
            SaveFileDialog sfd = new SaveFileDialog();
            sfd.Filter = "Text files (*.txt)|*.txt|All files (*.*)|*.*";
            if (sfd.ShowDialog()==DialogResult.OK)
            {
                using (StreamWriter sw = new StreamWriter(sfd.FileName))
                {
                    sw.WriteLine("Időszak\tNyereség");
                    for (int i = 0; i < Nyereségek.Count; i++)
                    {
                        sw.WriteLine((i + 1).ToString() + "\t" + Nyereségek[i]);
                    }
                }
            }
        }
    }
}
