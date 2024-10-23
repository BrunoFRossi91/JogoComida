namespace JogoComida.Models
{
    public class Prato
    {
        public string Nome { get; set; }
        public string Categoria { get; set; }
        public string Adjetivo { get; set; }

        public Prato(string nome, string categoria, string adjetivo = "")
        {
            Nome = nome;
            Categoria = categoria;
            Adjetivo = adjetivo;
        }
    }
}
