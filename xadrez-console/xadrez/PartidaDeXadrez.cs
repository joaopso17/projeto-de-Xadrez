using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using tabuleiro;
using xadrez_console.xadrez;

namespace xadrez
{
    internal class PartidaDeXadrez
    {
        public bool terminada { get; private set; }
        public Tabuleiro tab { get; private set; }
        public int turno { get; private set; }

        public Cor jogadorDaVez { get; private set; }


        public PartidaDeXadrez()
        {
            tab = new Tabuleiro(8, 8);
            turno = 1;
            jogadorDaVez = Cor.branca;
            ColocarPeca();
            terminada = false;
        }

        public void executaMovimento(Posicao origem, Posicao destino)
        {
            Peca p = tab.retiraPeca(origem);
            p.incrementarQteMovimentos();
            Peca pecaCapturada = tab.retiraPeca(destino);
            tab.colocarPeca(p, destino);

        }

        public void realizaJogada(Posicao origem, Posicao destino)
        {
            executaMovimento(origem, destino);
            turno++;
            trocaJogador();

        }

        public void ValidarPosicaoOrigem(Posicao pos)
        {
            if (tab.peca(pos) == null)
            {
                throw new TabuleiroExeption("Não existe peça na posição escolhida!");
            }
            if (jogadorDaVez != tab.peca(pos).cor)
            {
                throw new TabuleiroExeption("A peça escolhida não é sua! ");
            }
            if (!tab.peca(pos).existeMovimentosPossiveis())
            {
                throw new TabuleiroExeption("Não a movimentos possiveis para a peça escolhida!");
            }
        }
        
        public void ValidarPosicoaDestino (Posicao origem, Posicao destino)
        {
            if (!tab.peca(origem).podeMoverPara(destino))
            {
                throw new TabuleiroExeption("Posição de destino invalida!");
            }
        }

        private void trocaJogador()
        {
            if (jogadorDaVez == Cor.branca)
            {
                jogadorDaVez = Cor.preta;
            }
            else { jogadorDaVez = Cor.branca; }
        }

        private void ColocarPeca()
        {
            tab.colocarPeca(new Torre(tab, Cor.branca), new PosicaoXadrez('c', 1).toPosicao());
            tab.colocarPeca(new Torre(tab, Cor.branca), new PosicaoXadrez('c', 2).toPosicao());
            tab.colocarPeca(new Torre(tab, Cor.branca), new PosicaoXadrez('d', 2).toPosicao());
            tab.colocarPeca(new Torre(tab, Cor.branca), new PosicaoXadrez('e', 1).toPosicao());
            tab.colocarPeca(new Torre(tab, Cor.branca), new PosicaoXadrez('e', 2).toPosicao());
            tab.colocarPeca(new Rei(tab, Cor.branca), new PosicaoXadrez('d', 1).toPosicao());

            tab.colocarPeca(new Torre(tab, Cor.preta), new PosicaoXadrez('c', 8).toPosicao());
            tab.colocarPeca(new Torre(tab, Cor.preta), new PosicaoXadrez('c', 7).toPosicao());
            tab.colocarPeca(new Torre(tab, Cor.preta), new PosicaoXadrez('d', 7).toPosicao());
            tab.colocarPeca(new Torre(tab, Cor.preta), new PosicaoXadrez('e', 8).toPosicao());
            tab.colocarPeca(new Torre(tab, Cor.preta), new PosicaoXadrez('e', 7).toPosicao());
            tab.colocarPeca(new Rei(tab, Cor.preta), new PosicaoXadrez('d', 8).toPosicao());

        }
    }
}
