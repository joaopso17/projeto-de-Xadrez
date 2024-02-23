using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using tabuleiro;
using xadrez_console.xadrez;

namespace xadrez
{
    internal class PartidaDeXadrez
    {
        public HashSet<Peca> capturadas { get; private set; } = new HashSet<Peca>();
        public HashSet<Peca> pecas { get; private set; } = new HashSet<Peca>();
        public Cor jogadorDaVez { get; private set; }
        public bool terminada { get; private set; }
        public Tabuleiro tab { get; private set; }
        public bool Xeque { get; private set; }
        public int turno { get; private set; }

        public PartidaDeXadrez()
        {
            tab = new Tabuleiro(8, 8);
            turno = 1;
            jogadorDaVez = Cor.branca;
            ColocarPecas();
            terminada = false;
            Xeque = false;
        }

        public Peca executaMovimento(Posicao origem, Posicao destino)
        {
            Peca p = tab.retiraPeca(origem);
            p.incrementarQteMovimentos();
            Peca pecaCapturada = tab.retiraPeca(destino);
            tab.colocarPeca(p, destino);
            if (pecaCapturada != null)
            {
                capturadas.Add(pecaCapturada);
            }
            // #jogadaespecial roque pequeno
            if (p is Rei && destino.coluna == origem.coluna + 2)
            {
                Posicao origemT = new Posicao(origem.linha, origem.coluna + 3);
                Posicao destinoT = new Posicao(origem.linha, origem.coluna + 1);
                Peca T = tab.retiraPeca(origemT);
                T.incrementarQteMovimentos();
                tab.colocarPeca(T, destinoT);
            }

            // #jogadaespecial roque grande
            if (p is Rei && destino.coluna == origem.coluna - 2)
            {
                Posicao origemT = new Posicao(origem.linha, origem.coluna - 4);
                Posicao destinoT = new Posicao(origem.linha, origem.coluna - 1);
                Peca T = tab.retiraPeca(origemT);
                T.incrementarQteMovimentos();
                tab.colocarPeca(T, destinoT);
            }
            return pecaCapturada;
        }

        public void desfazMovimeto(Posicao origem, Posicao destino, Peca pecaCapturada)
        {
            Peca p = tab.retiraPeca(destino);
            p.decrementarQteMovimentos();
            if (pecaCapturada != null)
            {
                tab.colocarPeca(pecaCapturada, destino);
                capturadas.Remove(pecaCapturada);
            }   
            tab.colocarPeca(p, origem);

            // #jogadaespecial roque pequeno
            if (p is Rei && destino.coluna == origem.coluna + 2)
            {
                Posicao origemT = new Posicao(origem.linha, origem.coluna + 3);
                Posicao destinoT = new Posicao(origem.linha, origem.coluna + 1);
                Peca T = tab.retiraPeca(destinoT);
                T.decrementarQteMovimentos();
                tab.colocarPeca(T, origemT);
            }

            // #jogadaespecial roque grande
            if (p is Rei && destino.coluna == origem.coluna - 2)
            {
                Posicao origemT = new Posicao(origem.linha, origem.coluna - 4);
                Posicao destinoT = new Posicao(origem.linha, origem.coluna - 1);
                Peca T = tab.retiraPeca(destinoT);
                T.decrementarQteMovimentos();
                tab.colocarPeca(T, origemT);
            }
        }

        public void realizaJogada(Posicao origem, Posicao destino)
        {
            Peca pecaCapturada = executaMovimento(origem, destino);

            if (estaEmXeque(jogadorDaVez))
            {
                desfazMovimeto(origem, destino, pecaCapturada);
                throw new TabuleiroExeption("Não pode se colocar em Xeque!");
            }
            if (estaEmXeque(adversaria(jogadorDaVez)))
            {
                Xeque = true;
            }
            else { Xeque = false; }
            if (estaEmXequemate(adversaria(jogadorDaVez)))
            {
                terminada = true;
            }
            else
            {
                turno++;
                trocaJogador();
            }
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
            if (!tab.peca(origem).podeMovimentar(destino))
            {
                throw new TabuleiroExeption("Posição de destino invalida!");
            }
        }

        private Cor adversaria(Cor cor)
        {
            if (cor == Cor.branca)
            {
                return Cor.preta;
            }
            else
            {
                return Cor.branca;
            }
        }

        private Peca rei(Cor cor)
        {
            foreach (Peca x in pecasEmJogo(cor))
            {
                if (x is Rei)
                {
                    return x;
                }
            }
            return null;
        }

        public bool estaEmXeque(Cor cor)
        {
            Peca R = rei(cor);
            if (R == null)
            {
                throw new TabuleiroExeption("Não tem rei dessa cor " + cor + " no tabuleiro");
            }
            foreach (Peca x in pecasEmJogo(adversaria(cor)))
            {
                bool[,] mat = x.movimentosPossiveis();
                if (mat[R.posicao.linha, R.posicao.coluna]) { return true; }
            }
            return false;
        }

        public bool estaEmXequemate(Cor cor)
        {
            if (!estaEmXeque(cor))
            {
                return false;
            }
            foreach (Peca x in pecasEmJogo(cor))
            {
                bool[,] mat = x.movimentosPossiveis();
                for (int i = 0; i < tab.linha; i++)
                {
                    for (int j = 0; j < tab.coluna; j++)
                    {
                        if (mat[i, j])
                        {
                            Posicao origem = x.posicao;
                            Posicao destito = new Posicao(i, j);
                            Peca pecaCapturadas = executaMovimento(origem, destito);
                            bool testeXeque = estaEmXeque(cor);
                            desfazMovimeto(origem, destito, pecaCapturadas);
                            if (!testeXeque) { return false; }
                        }
                    }
                }
            }
            return true;
        }

        private void trocaJogador()
        {
            if (jogadorDaVez == Cor.branca)
            {
                jogadorDaVez = Cor.preta;
            }
            else { jogadorDaVez = Cor.branca; }
        }

        public HashSet<Peca> pecasCapturadas(Cor cor)
        {
            HashSet<Peca> aux = new HashSet<Peca>();
            foreach (Peca x in capturadas)
            {
                if (x.cor == cor)
                {
                    aux.Add(x);
                }
            }
            return aux;
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
            aux.ExceptWith(pecasCapturadas(cor));
            return aux;
        }

        public void colocarNovaPeca(char coluna, int linha, Peca peca)
        {
            tab.colocarPeca(peca, new PosicaoXadrez(coluna, linha).toPosicao());
            pecas.Add(peca);
        }

        private void ColocarPecas()
        {
            
            colocarNovaPeca('a', 1, new Torre(tab, Cor.branca));
            colocarNovaPeca('b', 1, new Cavalo(tab, Cor.branca));
            colocarNovaPeca('c', 1, new Bispo(tab, Cor.branca));
            colocarNovaPeca('d', 1, new Dama(tab, Cor.branca));
            colocarNovaPeca('e', 1, new Rei(tab, Cor.branca, this));
            colocarNovaPeca('f', 1, new Bispo(tab, Cor.branca));
            colocarNovaPeca('g', 1, new Cavalo(tab, Cor.branca));
            colocarNovaPeca('h', 1, new Torre(tab, Cor.branca));
            colocarNovaPeca('a', 2, new Peao(tab, Cor.branca));
            colocarNovaPeca('b', 2, new Peao(tab, Cor.branca));
            colocarNovaPeca('c', 2, new Peao(tab, Cor.branca));
            colocarNovaPeca('d', 2, new Peao(tab, Cor.branca));
            colocarNovaPeca('e', 2, new Peao(tab, Cor.branca));
            colocarNovaPeca('f', 2, new Peao(tab, Cor.branca));
            colocarNovaPeca('g', 2, new Peao(tab, Cor.branca));
            colocarNovaPeca('h', 2, new Peao(tab, Cor.branca));

            colocarNovaPeca('a', 8, new Torre(tab, Cor.preta));
            colocarNovaPeca('b', 8, new Cavalo(tab, Cor.preta));
            colocarNovaPeca('c', 8, new Bispo(tab, Cor.preta));
            colocarNovaPeca('d', 8, new Dama(tab, Cor.preta));
            colocarNovaPeca('e', 8, new Rei(tab, Cor.preta, this));
            colocarNovaPeca('f', 8, new Bispo(tab, Cor.preta));
            colocarNovaPeca('g', 8, new Cavalo(tab, Cor.preta));
            colocarNovaPeca('h', 8, new Torre(tab, Cor.preta));
            colocarNovaPeca('a', 7, new Peao(tab, Cor.preta));
            colocarNovaPeca('b', 7, new Peao(tab, Cor.preta));
            colocarNovaPeca('c', 7, new Peao(tab, Cor.preta));
            colocarNovaPeca('d', 7, new Peao(tab, Cor.preta));
            colocarNovaPeca('e', 7, new Peao(tab, Cor.preta));
            colocarNovaPeca('f', 7, new Peao(tab, Cor.preta));
            colocarNovaPeca('g', 7, new Peao(tab, Cor.preta));
            colocarNovaPeca('h', 7, new Peao(tab, Cor.preta));

        }

    }
}
