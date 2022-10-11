namespace saper1
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            int n = ((int)field_size.Value);
            int k = ((int)bombs.Value);
            if (n*n<=k)
            {
                MessageBox.Show("Слишком много бомб");
            }
            else
            {
                game g = new game(n, k);
                g.Show();
            }
        }
    }
}