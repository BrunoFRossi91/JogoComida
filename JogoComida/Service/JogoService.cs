using JogoComida.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace JogoComida.Service
{
    public class JogoService
    {
        private List<Prato> pratos;

        public JogoService()
        {
            pratos = new List<Prato>
            {
                new Prato("Lasanha", "Massa", null),
                new Prato("Bolo de Cenoura", "Doce", null),
            };
        }

        public void Jogar()
        {
            bool jogarNovamente = true;
            bool primeiraExecucao = true;

            while (jogarNovamente)
            {
                if (primeiraExecucao && !FazerPergunta("\nVou adivinhar seu prato favorito, ok?"))
                {
                    Console.WriteLine("\nOk, até mais!");
                    break;
                }

                bool pratoAdivinhado = false;

                // Iterar pelas categorias
                foreach (var categoria in ObterCategoriasDistintas())
                {
                    if (PerguntarCategoria(categoria))
                    {
                        var pratosDaCategoria = ObterPratosPorCategoria(categoria);

                        // Perguntar sobre os pratos da categoria
                        pratoAdivinhado = PerguntarSobrePratos(pratosDaCategoria);

                        if (pratoAdivinhado)
                        {
                            break; // Sai do loop de categorias se acertou
                        }

                        // Caso não tenha acertado o prato
                        string novoPrato = ObterNomeValido("\nQual prato você pensou?");
                        string adjetivo = ObterNomeValido($"\n{novoPrato} é ... mais {pratosDaCategoria.FirstOrDefault()?.Nome ?? "prato anterior"} não?");
                        AdicionarPrato(novoPrato, categoria, adjetivo);
                        pratoAdivinhado = true; // Prato foi adicionado
                        break;
                    }
                }

                // Se não acertou, perguntar por nova categoria e prato
                if (!pratoAdivinhado)
                {
                    AdicionarNovaCategoria();
                }

                // Pergunta se quer jogar novamente
                jogarNovamente = FazerPergunta("\nVocê quer jogar novamente?");
                primeiraExecucao = false;
            }
        }

        // Métodos auxiliares

        private List<string> ObterCategoriasDistintas()
        {
            return pratos.Select(p => p.Categoria).Distinct().ToList();
        }

        private List<Prato> ObterPratosPorCategoria(string categoria)
        {
            return pratos
                .Where(p => p.Categoria == categoria)
                .OrderByDescending(p => p.Adjetivo)
                .ToList();
        }

        private bool PerguntarCategoria(string categoria)
        {
            return FazerPergunta($"\nO prato que você pensou é {categoria}?");
        }

        private bool PerguntarSobrePratos(List<Prato> pratosDaCategoria)
        {
            foreach (var prato in pratosDaCategoria)
            {
                if (prato.Adjetivo != null)
                {
                    if (FazerPergunta($"\nO prato que você pensou é {prato.Adjetivo}?") && PerguntarPrato(prato.Nome))
                    {
                        Console.WriteLine("\nAcertei!");
                        return true;
                    }
                }
                else if (PerguntarPrato(prato.Nome))
                {
                    Console.WriteLine("\nAcertei!");
                    return true;
                }
            }
            return false;
        }

        private bool PerguntarPrato(string nome)
        {
            return FazerPergunta($"\nO prato que você pensou é {nome}?");
        }

        private void AdicionarPrato(string nome, string categoria, string adjetivo)
        {
            pratos.Add(new Prato(nome, categoria, adjetivo));
            Console.WriteLine($"\nPrato {nome} salvo na categoria {categoria} com adjetivo '{adjetivo}'.");
        }

        private void AdicionarNovaCategoria()
        {
            string novaCategoria = ObterNomeValido("\nNão consegui adivinhar! Qual o tipo de prato que você pensou?");
            string pratoVinculado = ObterNomeValido($"\nQual prato do tipo {novaCategoria} você gosta?");
            AdicionarPrato(pratoVinculado, novaCategoria, null);
        }

        private bool FazerPergunta(string pergunta)
        {
            string resposta;
            do
            {
                Console.WriteLine(pergunta);
                resposta = Console.ReadLine().ToLower();

                if (resposta != "sim" && resposta != "não")
                {
                    Console.WriteLine("\nPor favor, responda apenas com 'sim' ou 'não'.");
                }
            } while (resposta != "sim" && resposta != "não");

            return resposta == "sim";
        }

        private string ObterNomeValido(string mensagem)
        {
            string nome;
            do
            {
                Console.WriteLine(mensagem);
                nome = Console.ReadLine();
            } while (string.IsNullOrWhiteSpace(nome) || !Regex.IsMatch(nome, @"^[a-zA-Z\s]+$"));

            return nome;
        }
    }
}
