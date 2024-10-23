using JogoComida.Service;
using System;
using System.Collections.Generic;
using System.Linq;

class Program
{
    static void Main(string[] args)
    {
        JogoService jogoService = new JogoService();
        jogoService.Jogar();

        Console.WriteLine("Pressione qualquer tecla para sair...");
        Console.ReadKey();
    }
}
