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
                try
                {
                    Console.Clear();
                    Tela.imprimirPartida(partida);

                    Console.WriteLine();

                    Console.Write("origem: ");
                    Posicao origem = Tela.lerPosicaoXadrez().toPosicao();
                    partida.ValidarPosicaoOrigem(origem);

                    bool[,] posicoesPossiveis = partida.tab.peca(origem).movimentosPossiveis();
                   
                    Console.Clear();
                    Tela.imprimirTabuleiro(partida.tab, posicoesPossiveis);

                    Console.WriteLine();
                    Console.Write("destino: ");
                    Posicao destino = Tela.lerPosicaoXadrez().toPosicao();
                    partida.ValidarPosicoaDestino(origem, destino);

                    partida.realizaJogada(origem, destino);
                }
                catch (TabuleiroExeption e) 
                {
                    Console.WriteLine(e.Message);
                    Console.ReadLine();
                }
                

            }
        }

        catch (TabuleiroExeption ex)
        {
            Console.WriteLine(ex.Message);
        }
    }
}