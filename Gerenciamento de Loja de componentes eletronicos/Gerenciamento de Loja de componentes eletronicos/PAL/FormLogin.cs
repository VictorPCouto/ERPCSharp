using System; // Biblioteca básica para funcionalidades gerais
using System.Data.SqlClient; // Biblioteca para trabalhar com banco de dados SQL Server
using System.Windows.Forms; // Biblioteca para criação de interfaces gráficas no Windows Forms

namespace Gerenciamento_de_Loja_de_componentes_eletronicos.PAL
{
    public partial class FormLogin : Form
    {
        // Construtor do formulário (inicializa os componentes gráficos)
        public FormLogin()
        {
            InitializeComponent();
        }

        // Método para limpar os campos de entrada de texto
        private void EmptyBox()
        {
            txtUsername.Clear(); // Limpa o campo de nome de usuário
            txtPassword.Clear(); // Limpa o campo de senha
        }

        // Evento de clique para fechar a janela
        private void picClose_Click(object sender, EventArgs e)
        {
            Close(); // Fecha o formulário
        }

        // Evento de clique para mostrar a senha digitada
        private void picShow_Click(object sender, EventArgs e)
        {
            // Se o ícone "Mostrar" estiver visível
            if (picShow.Visible == true)
            {
                txtPassword.UseSystemPasswordChar = false; // Mostra o texto da senha
                picShow.Visible = false; // Oculta o ícone "Mostrar"
                picHide.Visible = true; // Exibe o ícone "Ocultar"
            }
        }

        // Evento de clique para ocultar a senha digitada
        private void picHide_Click(object sender, EventArgs e)
        {
            // Se o ícone "Ocultar" estiver visível
            if (picHide.Visible == true)
            {
                txtPassword.UseSystemPasswordChar = true; // Oculta o texto da senha
                picShow.Visible = true; // Exibe o ícone "Mostrar"
                picHide.Visible = false; // Oculta o ícone "Ocultar"
            }
        }

        // Evento de clique para validar o login
        private void btnLogIn_Click(object sender, EventArgs e)
        {
            // Verifica se o campo de nome de usuário está vazio
            if (string.IsNullOrWhiteSpace(txtUsername.Text))
            {
                MessageBox.Show("Por favor, digite seu nome de usuário.", "Informação", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return; // Sai do método
            }

            // Verifica se o campo de senha está vazio
            if (string.IsNullOrWhiteSpace(txtPassword.Text))
            {
                MessageBox.Show("Por favor, digite sua senha.", "Informação", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return; // Sai do método
            }

            // Define a string de conexão com o banco de dados
            string connectionString = "Server=COUTODESKTOP;Database=CSMS;Trusted_Connection=True;";

            try
            {
                // Cria uma conexão com o banco de dados
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open(); // Abre a conexão

                    // Comando SQL para verificar o nome de usuário e senha
                    string query = "SELECT COUNT(*) FROM Users WHERE Users_Name = @username AND Users_Password = @password";

                    // Cria um comando SQL com a consulta e a conexão
                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        // Adiciona os parâmetros para evitar SQL Injection
                        command.Parameters.AddWithValue("@username", txtUsername.Text.Trim());
                        command.Parameters.AddWithValue("@password", txtPassword.Text.Trim());

                        // Executa a consulta e retorna o número de correspondências
                        int count = (int)command.ExecuteScalar();

                        if (count > 0) // Se houver correspondência
                        {
                            MessageBox.Show("Login bem-sucedido!", "Sucesso", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            FormMain formMain = new FormMain(); // Abre a tela principal
                            formMain.ShowDialog();
                            EmptyBox(); // Limpa os campos após o login
                        }
                        else // Se o login falhar
                        {
                            MessageBox.Show("Senha ou usuário incorreto(s).", "Erro", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        }
                    }
                }
            }
            catch (SqlException sqlEx) // Erros específicos do SQL Server
            {
                MessageBox.Show($"Erro ao conectar ao banco de dados: {sqlEx.Message}", "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            catch (Exception ex) // Outros erros gerais
            {
                MessageBox.Show($"Ocorreu um erro: {ex.Message}", "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
