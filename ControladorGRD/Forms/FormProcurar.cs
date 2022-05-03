﻿using System;
using MySql.Data.MySqlClient;
using System.Windows.Forms;
using ControladorGRD.Entities;

namespace ControladorGRD.Forms
{
    public partial class FormProcurar : Form
    {
        public int? id_contatoSelecionado = null;
        FormCadastroDoc FormCadastroDoc;
        public string numero, rev, os, obs, data;
        TextBox txtRev;
        string[] dados = new string[5];

        public FormProcurar(FormCadastroDoc FormCadastroDoc, TextBox txtRev)
        {
            InitializeComponent();
            this.FormBorderStyle = FormBorderStyle.FixedSingle;
            this.FormCadastroDoc = FormCadastroDoc;
            this.txtRev = txtRev;
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            try
            {
                ConnectSQL.Connect();

                id_contatoSelecionado = ConnectSQL.SearchID(txtProcurar.Text.ToUpper());


                if (id_contatoSelecionado != null)
                {
                    dados = ConnectSQL.Values((int)id_contatoSelecionado);
                    numero = dados[0];
                    rev = dados[1];
                    os = dados[2];
                    obs = dados[3];
                    data = dados[4];

                    ConnectSQL.cmd.CommandText = $"SELECT pend FROM documento WHERE numero='{numero}'";
                    MySqlDataReader reader = ConnectSQL.cmd.ExecuteReader();
                    reader.Read();
                    int pend = Int32.Parse(reader.GetString(0));
                    reader.Close();
                    if (pend > 0)
                    {
                        txtRev.Enabled = false;
                    }
                    else
                    {
                        txtRev.Enabled = true;
                    }

                    FormCadastroDoc.Preencher(id_contatoSelecionado, numero, rev, os, obs, data);

                    //se o id for encontrado, colocar os valores nas caixas de textos
                    this.Close();
                }
                else
                {
                    MessageBox.Show("Não encontrado");
                }
                
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            finally
            {
                ConnectSQL.conexao.Close();
                
            }


        }
    }
}
