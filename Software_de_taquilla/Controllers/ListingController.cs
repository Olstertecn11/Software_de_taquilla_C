﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Software_de_taquilla.Views.UserViews;
using Software_de_taquilla.Models.Dao;
using Software_de_taquilla.Models.Dto;
using Software_de_taquilla.Views.UserViews.components;

namespace Software_de_taquilla.Controllers
{
    public class ListingController
    {
        public ListingView view;
        public List<Object> objects = new List<Object>();
        public ListingController(ListingView view)
        {
            this.view = view;
            this.view.Load += new EventHandler(this.buildComponent);
            this.view.combo_cine.SelectedIndexChanged += new EventHandler(this.filtrar);
            this.view.combo_city.SelectedIndexChanged += new EventHandler(this.filtrarCine);
        }

        public void filtrarCine(object sender, EventArgs e)
        {
            this.view.combo_cine.Items.Clear();
            this.view.combo_cine.DataSource = null;
            this.fillCine(this.view.combo_city.SelectedIndex + 1);
        }

        public void filtrar(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(this.view.combo_city.Text))
            {
                this.view.printMessage("Seleccione una ciudad antes"); return;
            }
            this.view.flow_container.Controls.Clear();
            this.fillMovies(this.view.combo_city.SelectedIndex + 1, this.view.combo_cine.SelectedIndex + 1);
        }

        public void fillMovies(int n, int j)
        {

            MovieDao mydao = new MovieDao();
            List<Movie> movies = new List<Movie>();
            if (n == -1) movies = mydao.getMovies();
            else movies = mydao.getMovies(n, j);
            this.objects.Add(this.view.combo_cine.SelectedIndex + 1);
            foreach (Movie movie in movies)
            {
                this.view.flow_container.Controls.Add(new MovieCard(this.objects, movie));
            }
        }


        public void buildComponent(object sender, EventArgs e)
        {
            this.fillCity();
            this.fillCine(-1);
            this.fillMovies(-1, -1);
        }

        public void fillCity()
        {
            CityDao mydao = new CityDao();
            List<City> cts = mydao.getCitys();
            foreach (City c in cts)
            {
                this.view.combo_city.Items.Add(c.name);
            }
        }

        public void fillCine(int n)
        {
            CityDao mydao = new CityDao();
            List<Cine> cts = mydao.getCine();
            foreach (Cine c in cts)
            {
                if (n == -1) this.view.combo_cine.Items.Add(c.lugar);
                else
                {
                    if (c.id_ciudad.Equals(n))
                    {
                        this.view.combo_cine.Items.Add(c.lugar);
                    }
                }
            }
        }
    }
}
