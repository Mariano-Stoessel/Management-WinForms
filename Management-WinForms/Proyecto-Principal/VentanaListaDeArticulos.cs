﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Dominio;
using LecturaDatos;

namespace Proyecto_Principal
{
    public partial class VentanaListaDeArticulos : Form
    {
        private List<Articulo> listaLecturaArticulos;

        public VentanaListaDeArticulos()
        {
            InitializeComponent();
        }
        private void VentanaListaDeArticulos_Load(object sender, EventArgs e)
        {
            CargarDatos();
        }

        private void btnVolver_Click(object sender, EventArgs e)
        {

            this.Dispose();
        }

        private void CargarDatos()
        {
            try
            {
                LecturaArticulo lecturaArt = new LecturaArticulo();
                listaLecturaArticulos = lecturaArt.listar();
                dgvListaArticulos.DataSource = lecturaArt.listar();

                dgvListaArticulos.Columns["ImagenUrl"].Visible = false;

                CargarImagen(listaLecturaArticulos[0].ImagenUrl);
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.ToString());
            }

        }
        private void CargarImagen(string imagenUrl)
        {
            try
            {
                pbxImagenUrl.Load(imagenUrl);
            }
            catch (Exception ex)
            {

                pbxImagenUrl.Load("https://storage.googleapis.com/proudcity/mebanenc/uploads/2021/03/placeholder-image.png");
            }
        }

        private void dgvListaArticulos_SelectionChanged(object sender, EventArgs e)
        {
            Articulo seleccionado = (Articulo)dgvListaArticulos.CurrentRow.DataBoundItem;
            CargarImagen(seleccionado.ImagenUrl);
        }

        private void btnAgregar_Click(object sender, EventArgs e)
        {
            frmAgregarArticulo alta = new frmAgregarArticulo();
            alta.ShowDialog();
            CargarDatos();
        }
    }
}
