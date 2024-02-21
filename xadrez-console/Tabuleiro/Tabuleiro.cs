

using System.Runtime.InteropServices;
using xadrez;

namespace tabuleiro
{
    internal class Tabuleiro
    {

        public int linha { get; set; }
        public int coluna { get; set; }

        private Peca[,] pecas;


        public Tabuleiro(int linha, int coluna)
        {
            this.linha = linha;
            this.coluna = coluna;
            pecas = new Peca[linha, coluna];
        }

        public Peca peca(int linhas, int colunas)
        {
            return pecas[linhas, colunas];
        }

        public Peca peca(Posicao pos)
        {
            return pecas[pos.linha, pos.coluna];
        }

        public bool existePeca(Posicao pos)
        {
            ValidarPosicao(pos);

            return peca(pos) != null;
        }

        public Peca retiraPeca(Posicao pos)
        {
            if (peca(pos) == null)
            {
                return null;
            }
            Peca aux = peca(pos);
            aux.posicao = null;
            pecas[pos.linha, pos.coluna] = null;
            return aux;
        }

        public void colocarPeca(Peca p, Posicao pos)
        {
            if (existePeca(pos))
            {
                throw new TabuleiroExeption("Ja existe uma peça nessa posição!");
            }
            pecas[pos.linha, pos.coluna] = p;
            p.posicao = pos;

        }

        public bool PosicaoValida(Posicao pos)
        {
            if (pos.linha < 0 || pos.linha >= linha || pos.coluna < 0 || pos.coluna >= coluna)
            {
                return false;
            }
            return true;
        }

        public void ValidarPosicao(Posicao pos)
        {
            if (!PosicaoValida(pos))
            {
                throw new TabuleiroExeption("Posição invalida!");
            }
        }
    }
}