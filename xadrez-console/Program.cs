using tabuleiro;
using xadrez_console;
using xadrez;
using System;

internal class Program
{


    private static void Main(string[] args)
    {
        try
        {
            PartidaDeXadrez partida = new PartidaDeXadrez();


            while (!partida.terminada)
            {

                Console.Clear();
                Tela.imprimirTabuleiro(partida.tab);
                Console.WriteLine();

                Console.Write("origem: ");
                Posicao origem = Tela.lerPosicaoXadrez().toPosicao();

                Console.Write("destino: ");
                Posicao destino = Tela.lerPosicaoXadrez().toPosicao();

                partida.executaMovimento(origem, destino);


            }
        }

        catch (TabuleiroExeption ex)
        {
            Console.WriteLine(ex.Message);
        }
    }
}