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
        private HashSet<Peca> pecas;
        private HashSet<Peca> PecasCapturada;
        public Cor jogadorDaVez { get; private set; }


        public PartidaDeXadrez()
        {
            tab = new Tabuleiro(8, 8);
            turno = 1;
            jogadorDaVez = Cor.branca;
            ColocarPecas();
            pecas = new HashSet<Peca>();
            PecasCapturada = new HashSet<Peca>();
            terminada = false;
        }

        public void executaMovimento(Posicao origem, Posicao destino)
        {
            Peca p = tab.retiraPeca(origem);
            p.incrementarQteMovimentos();
            Peca pecaCapturada = tab.retiraPeca(destino);
            tab.colocarPeca(p, destino);
            if (pecaCapturada != null)
            {
                PecasCapturada.Add(pecaCapturada);
            }

        }

        public void realizaJogada(Posicao origem, Posicao destino)
        {
            executaMovimento(origem, destino);
            turno++;
            trocaJogador();

        }

        public HashSet<Peca> PecasCapturadas(Cor cor)
        {
            HashSet<Peca> aux = new HashSet<Peca>();
            foreach (Peca x in PecasCapturada)
            {
                if (x.cor == cor)
                {
                    aux.Add(x);
                }
            }
            return aux;
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

        public void ValidarPosicoaDestino(Posicao origem, Posicao destino)
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

        public void ColocarNovaPeca(char coluna, int linha, Peca peca)
        {
            tab.colocarPeca(peca, new PosicaoXadrez(coluna, linha).toPosicao());
            if (pecas !=  null) { pecas.Add(peca); }
        }

        private void ColocarPecas()
        {
            ColocarNovaPeca('c', 1, new Torre(tab, Cor.branca));
            ColocarNovaPeca('c', 2, new Torre(tab, Cor.branca));
            ColocarNovaPeca('d', 2, new Torre(tab, Cor.branca));
            ColocarNovaPeca('e', 1, new Torre(tab, Cor.branca));
            ColocarNovaPeca('e', 2, new Torre(tab, Cor.branca));
            ColocarNovaPeca('d', 1, new Rei(tab, Cor.branca));

            ColocarNovaPeca('c', 8, new Torre(tab, Cor.preta));
            ColocarNovaPeca('c', 7, new Torre(tab, Cor.preta));
            ColocarNovaPeca('d', 7, new Torre(tab, Cor.preta));
            ColocarNovaPeca('e', 8, new Torre(tab, Cor.preta));
            ColocarNovaPeca('e', 7, new Torre(tab, Cor.preta));
            ColocarNovaPeca('d', 8, new Rei(tab, Cor.preta));

        }

        public HashSet<Peca> pecasEmJogo(Cor cor)
        {
            HashSet<Peca> aux = new HashSet<Peca>();
            foreach (Peca x in pecas)
            {
                if (x.cor == cor)
                {
                    aux.Add(x);
                }
            }
            aux.ExceptWith(PecasCapturadas(cor));
            return aux;
        }
    }
}
