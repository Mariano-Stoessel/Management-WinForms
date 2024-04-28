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
        private List<Imagen> listaImagenes;
        int indiceMaximo;
        int indiceActual;

        public VentanaListaDeArticulos()
        {
            InitializeComponent();
            indiceMaximo = 0;
            indiceActual = 0;
        }
        private void VentanaListaDeArticulos_Load(object sender, EventArgs e)
        {
            LecturaCategoria LecturaCategoria = new LecturaCategoria();
            LecturaMarca LecturaMarca = new LecturaMarca();
            try
            {
                cargarDatos();
                cargarCampos();
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.ToString());
            }
        }

        private void btnVolver_Click(object sender, EventArgs e)
        {

            this.Dispose();
        }

        private void cargarDatos()
        {
            try
            {
                LecturaArticulo lecturaArticulo = new LecturaArticulo();
                listaLecturaArticulos = lecturaArticulo.listar();
                dgvListaArticulos.DataSource = lecturaArticulo.listar();

                LecturaImagen imgBD = new LecturaImagen();
                indiceMaximo = imgBD.maximoImagen(listaLecturaArticulos[0].Id);

                ocultarColumnas();
                cargarImagen(listaLecturaArticulos[0].Id);
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.ToString());
            }

        }

        private void dgvListaArticulos_SelectionChanged(object sender, EventArgs e)
        {
            if(dgvListaArticulos.CurrentCell != null)
            {
                indiceActual = 0;
                Articulo seleccionado = (Articulo)dgvListaArticulos.CurrentRow.DataBoundItem;
                cargarImagen(seleccionado.Id);
            }
        }

        private void ocultarColumnas()
        {
            dgvListaArticulos.Columns["Id"].Visible = false;
            dgvListaArticulos.Columns["ImagenUrl"].Visible = false;
        }

        private void cargarImagen(int Id)
        {
            try
            {
                LecturaImagen lecturaImagen = new LecturaImagen();
                listaImagenes = lecturaImagen.listar(Id);

                pbxImagenUrl.Load(listaImagenes[indiceActual].ImagenUrl);
            }
            catch (Exception)
            {

                pbxImagenUrl.Load("https://storage.googleapis.com/proudcity/mebanenc/uploads/2021/03/placeholder-image.png");
            }
        }

        private void btnAgregar_Click(object sender, EventArgs e)
        {
            frmAgregarArticulo alta = new frmAgregarArticulo();
            alta.ShowDialog();
            cargarDatos();
        }

        private void btnEditar_Click(object sender, EventArgs e)
        {
            Articulo seleccionado = (Articulo)dgvListaArticulos.CurrentRow.DataBoundItem;
            frmAgregarArticulo editar = new frmAgregarArticulo(seleccionado);
            editar.ShowDialog();
            cargarDatos();
        }

        private void btnEliminar_Click(object sender, EventArgs e)
        {
            LecturaArticulo lecturaArticulo = new LecturaArticulo();
            try
            {
                DialogResult respuesta = MessageBox.Show("¿Está seguro que desea eliminar el artículo?", "Eliminar Artículo", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
                if(respuesta == DialogResult.Yes)
                {
                    Articulo seleccionado = (Articulo)dgvListaArticulos.CurrentRow.DataBoundItem;
                    lecturaArticulo.eliminarArticulo(seleccionado.Id);

                    MessageBox.Show("Artículo eliminado correctamente");
                    cargarDatos();
                }
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.ToString());
            }
        }

        private void btnBuscar_Click(object sender, EventArgs e)
        {
            filtrarArticulo();
        }

        private void txtBuscar_TextChanged(object sender, EventArgs e)
        {
            filtrarArticulo();
        }

        private void filtrarArticulo()
        {
            List<Articulo> listaFiltrada;
            string filtro = txtBuscar.Text;
            if (filtro.Length >= 2)
            {
                listaFiltrada = listaLecturaArticulos.FindAll(x => x.Codigo.ToUpper().Contains(filtro.ToUpper()) || x.Nombre.ToLower().Contains(filtro.ToLower()));
            }
            else
            {
                listaFiltrada = listaLecturaArticulos;
            }
            dgvListaArticulos.DataSource = null;
            dgvListaArticulos.DataSource = listaFiltrada;
            ocultarColumnas();
        }

        private void cbxOrdenar_SelectedIndexChanged(object sender, EventArgs e)
        {
            List<Articulo> listaFiltrada;
            string opcion = cbxOrdenar.SelectedItem.ToString();
            if(opcion == "Menor Código")
            {
                listaFiltrada = listaLecturaArticulos.OrderBy(x => x.Codigo).ToList();
                txtBuscar.Text = "";
            }
            else
            {
                listaFiltrada = listaLecturaArticulos.OrderByDescending(x => x.Codigo).ToList();
                txtBuscar.Text = "";
            }
            dgvListaArticulos.DataSource = null;
            dgvListaArticulos.DataSource = listaFiltrada;
            ocultarColumnas();
        }

        private void cbxCampo_SelectedIndexChanged(object sender, EventArgs e)
        {
            string opcion = cbxCampo.SelectedItem.ToString();
            if (opcion == "Código")
            {
                cbxCriterio.Items.Clear();
                cbxCriterio.Items.Add("Mayor que");
                cbxCriterio.Items.Add("Menor que");
                cbxCriterio.Items.Add("Igual a");
            }
            else
            {
                cbxCriterio.Items.Clear();
                cbxCriterio.Items.Add("Comienza con");
                cbxCriterio.Items.Add("Termina con");
                cbxCriterio.Items.Add("Contiene");
            }
        }

        private void btnAvanzado_Click(object sender, EventArgs e)
        {
            LecturaArticulo lecturaArticulo = new LecturaArticulo();
            try
            {
                string campo = cbxCampo.SelectedItem.ToString();
                string criterio = cbxCriterio.SelectedItem.ToString();
                string filtro = txtAvanzado.Text;
                dgvListaArticulos.DataSource = lecturaArticulo.filtrarArticulo(campo, criterio, filtro);
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.ToString());
            }
        }

        private void btnLimpiar_Click(object sender, EventArgs e)
        {
            cbxCampo.Items.Clear();
            cbxCriterio.Items.Clear();
            cbxOrdenar.Items.Clear();
            txtBuscar.Text = "";
            txtAvanzado.Text = "";

            cargarDatos();
            cargarCampos();
        }
        private void cargarCampos()
        {
            cbxOrdenar.Items.Add("Menor Código");
            cbxOrdenar.Items.Add("Mayor Código");

            cbxCampo.Items.Add("Código");
            cbxCampo.Items.Add("Nombre");
            cbxCampo.Items.Add("Marca");
            cbxCampo.Items.Add("Categoría");
        }

        private void btnDown_Click(object sender, EventArgs e)
        {
            if (indiceActual < indiceMaximo)
            {
                indiceActual++;
            }
            Articulo seleccionado = (Articulo)dgvListaArticulos.CurrentRow.DataBoundItem;
            cargarImagen(seleccionado.Id);
        }

        private void btnUp_Click(object sender, EventArgs e)
        {          
            indiceActual--;
            if (indiceActual < 1) indiceActual = 0;
            Articulo seleccionado = (Articulo)dgvListaArticulos.CurrentRow.DataBoundItem;
            cargarImagen(seleccionado.Id);
        }
    }
}