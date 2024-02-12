using tabuleiro;
using xadrez_console;
using xadrez;

Tabuleiro tab = new(8, 8);


tab.colocarPeca(new Torre(tab, Cor.preta), new Posicao(0, 0));
tab.colocarPeca(new Torre(tab, Cor.preta), new Posicao(1, 3));
tab.colocarPeca(new Rei(tab, Cor.preta), new Posicao(2, 4));
Tela.imprimirTabuleiro(tab);