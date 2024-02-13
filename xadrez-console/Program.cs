using tabuleiro;
using xadrez_console;
using xadrez;

internal class Program
{
    private static void Main(string[] args)
    {
        try
        {
            Tabuleiro tab = new(8, 8);


            tab.colocarPeca(new Torre(tab, Cor.preta), new Posicao(0, 0));
            tab.colocarPeca(new Torre(tab, Cor.preta), new Posicao(1, 9));
            tab.colocarPeca(new Rei(tab, Cor.preta), new Posicao(0, 2));



            Tela.imprimirTabuleiro(tab);
        }

        catch (TabuleiroExeption ex)
        {
            Console.WriteLine(ex.Message);
        }
    }
}