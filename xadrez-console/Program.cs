using tabuleiro;
using xadrez_console;
using xadrez;
using xadrez_console.xadrez;

internal class Program
{
    private static void Main(string[] args)
    {

        PosicaoXadrez pos = new PosicaoXadrez('c', 7);
        Console.WriteLine(pos);


        Console.WriteLine(pos.toPosicao());




    }
}