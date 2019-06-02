﻿using Projecto_Parcial1.BLL;
using Projecto_Parcial1.Entidades;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Projecto_Parcial1
{
    public partial class Valor_Inventario : Form
    {
        public Valor_Inventario()
        {
            InitializeComponent();
        }

       

        private void RefreshButton_Click(object sender, EventArgs e)
        {
            


            Mostrar();
        }



        private void Mostrar()
        {
           
            int r = 0; //do while


            for (int i = 1; i < 1000; i++)
            {
                Producto producto = new Producto();
              

                producto = ProductoBLL.Buscar(i);

            

                if (producto != null)
                {
                    r += Convert.ToInt32(producto.Existen * producto.Costo);
                }
                else
                {
                    break;
                }

            }


            ValorInventarioLabel.Text = Convert.ToString(r);



        }


    }
}
