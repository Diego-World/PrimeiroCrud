using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

 using MySql.Data.MySqlClient;

namespace WFDotNetCoreGravarDadosMySQL
{



    public partial class Form1 : Form
    {

        private MySqlConnection Conexao;
        private String data_source = "datasource=localhost;username=root;password=1234;database=db_agenda";

        private int ?id_contato_selecionado = null;



        public Form1()
        {
            InitializeComponent();

            lstContatos.View = View.Details;
            lstContatos.LabelEdit = true;
            lstContatos.AllowColumnReorder = true;
            lstContatos.FullRowSelect = true;
            lstContatos.GridLines = true;




            lstContatos.Columns.Add("ID", 30, HorizontalAlignment.Left);
            lstContatos.Columns.Add("Nome", 150, HorizontalAlignment.Left);
            lstContatos.Columns.Add("Email", 150, HorizontalAlignment.Left);
            lstContatos.Columns.Add("Telefone", 150, HorizontalAlignment.Left);

            carregar_Contatos();


        }

        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                Conexao = new MySqlConnection(data_source);

                Conexao.Open();


                MySqlCommand cmd = new MySqlCommand();

                cmd.Connection = Conexao;


                if(id_contato_selecionado == null)
                {
                    cmd.CommandText = "INSERT INTO contato (nome, email, telefone) " +
                    "VALUES " +
                    "(@Nome, @Email, @Telefone) ";

                    cmd.Parameters.AddWithValue("@Nome", txtNome.Text);
                    cmd.Parameters.AddWithValue("@Email", txtEmail.Text);
                    cmd.Parameters.AddWithValue("@Telefone", txtTelefone.Text);

                    cmd.Prepare();
                    cmd.ExecuteNonQuery();


                    MessageBox.Show("Contato inserido com sucesso!",
                                   "Sucesso!", MessageBoxButtons.OK,
                                   MessageBoxIcon.Information);

                }
                else
                {
                    // Atulização

                    cmd.CommandText = "UPDATE contato SET nome=@nome, " +
                        "email=@email, " +
                        "Telefone=@Telefone " +
                        "WHERE " +
                        "id=@id ";

                    cmd.Parameters.AddWithValue("@Nome", txtNome.Text);
                    cmd.Parameters.AddWithValue("@Email", txtEmail.Text);
                    cmd.Parameters.AddWithValue("@Telefone", txtTelefone.Text);
                    cmd.Parameters.AddWithValue("@id", id_contato_selecionado);

                    cmd.Prepare();
                    cmd.ExecuteNonQuery();


                    MessageBox.Show("Contato Atualizado com sucesso!",
                                   "Sucesso!", MessageBoxButtons.OK,
                                   MessageBoxIcon.Information);
                }

                zerar_formulario();

                carregar_Contatos();

            }
            catch (MySqlException ex)
            {

                MessageBox.Show("Erro " + ex.Number + " ocorreu: " + ex.Message,
                                "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);

            }
            catch (Exception ex)
            {
                MessageBox.Show("ocorreu: " + ex.Message,
                                "Erro", MessageBoxButtons.OK,
                                MessageBoxIcon.Error);
            }
            finally
            {
                Conexao.Close();
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            try
            {

                Conexao = new MySqlConnection(data_source);

                Conexao.Open();


                MySqlCommand cmd = new MySqlCommand();

                cmd.Connection = Conexao;
                cmd.CommandText = "SELECT * FROM contato WHERE nome LIKE @query OR email LIKE @query";

                cmd.Parameters.AddWithValue("@query", "%" + txtBuscar.Text + "%");
                cmd.Prepare();




                MySqlDataReader reader = cmd.ExecuteReader();

                lstContatos.Items.Clear();

                while (reader.Read())
                {
                    string[] row =
                    {
                        reader.GetString(0),
                        reader.GetString(1),
                        reader.GetString(2),
                        reader.GetString(3),
                    };

                    lstContatos.Items.Add(new ListViewItem(row));
                }


            }
            catch (MySqlException ex)
            {

                MessageBox.Show("Erro " + ex.Number + " ocorreu: " + ex.Message,
                                "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);

            }
            catch (Exception ex)
            {
                MessageBox.Show("ocorreu: " + ex.Message,
                                "Erro", MessageBoxButtons.OK,
                                MessageBoxIcon.Error);
            }
            finally
            {
                Conexao.Close();
            }
        }

        private void carregar_Contatos()
        {
            try
            {

                Conexao = new MySqlConnection(data_source);

                Conexao.Open();


                MySqlCommand cmd = new MySqlCommand();

                cmd.Connection = Conexao;
                cmd.CommandText = "SELECT * FROM contato ORDER BY id DESC ";
                cmd.Prepare();


                MySqlDataReader reader = cmd.ExecuteReader();

                lstContatos.Items.Clear();

                while (reader.Read())
                {
                    string[] row =
                    {
                        reader.GetString(0),
                        reader.GetString(1),
                        reader.GetString(2),
                        reader.GetString(3),
                    };

                    lstContatos.Items.Add(new ListViewItem(row));
                }


            }
            catch (MySqlException ex)
            {

                MessageBox.Show("Erro " + ex.Number + " ocorreu: " + ex.Message,
                                "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);

            }
            catch (Exception ex)
            {
                MessageBox.Show("ocorreu: " + ex.Message,
                                "Erro", MessageBoxButtons.OK,
                                MessageBoxIcon.Error);
            }
            finally
            {
                Conexao.Close();
            }
        }

        private void lstContatos_ItemSelectionChanged(object sender, ListViewItemSelectionChangedEventArgs e)
        {
            ListView.SelectedListViewItemCollection itens_selecionados = lstContatos.SelectedItems;

            foreach (ListViewItem item in itens_selecionados)
            {
                id_contato_selecionado = Convert.ToInt32(item.SubItems[0].Text);

                txtNome.Text = item.SubItems[1].Text;
                txtEmail.Text = item.SubItems[2].Text;
                txtTelefone.Text = item.SubItems[3].Text;

                button3.Visible = true;

            }
        }

        private void txtNovo_Click(object sender, EventArgs e)
        {
            zerar_formulario();
        }

        private void zerar_formulario()
        {
            id_contato_selecionado = null;

            txtNome.Text = String.Empty;
            txtEmail.Text = "";
            txtTelefone.Text = "";
            txtNome.Focus();


            button3.Visible = false;
        }




        private void toolStripMenuItem1_Click(object sender, EventArgs e)
        {

            excluir_contato();        


        }

        private void button3_Click(object sender, EventArgs e)
        {

           excluir_contato();

        }

        private void excluir_contato()
        {
            try
            {




                DialogResult conf = MessageBox.Show("Tem Certeza que deseja exluir o registro?",
                                        "Ops, tem certeza?",
                                         MessageBoxButtons.YesNo,
                                         MessageBoxIcon.Warning);


                if (conf == DialogResult.Yes)
                {

                    Conexao = new MySqlConnection(data_source);

                    Conexao.Open();


                    MySqlCommand cmd = new MySqlCommand();

                    cmd.Connection = Conexao;

                    cmd.CommandText = "DELETE FROM contato " + "WHERE " + "id=@id ";

                    cmd.Parameters.AddWithValue("@id", id_contato_selecionado);

                    cmd.Prepare();
                    cmd.ExecuteNonQuery();

                    MessageBox.Show("Contato excluido com sucesso!",
                                   "Sucesso!", MessageBoxButtons.OK,
                                   MessageBoxIcon.Information);

                    carregar_Contatos();

                    zerar_formulario();

                }







            }
            catch (MySqlException ex)
            {

                MessageBox.Show("Erro " + ex.Number + " ocorreu: " + ex.Message,
                                "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);

            }
            catch (Exception ex)
            {

                MessageBox.Show("ocorreu: " + ex.Message,
                                 "Erro", MessageBoxButtons.OK,
                                 MessageBoxIcon.Error);


            }
            finally
            {

                Conexao.Close();
            }

            //Demorou mais veio
        }


    }
}