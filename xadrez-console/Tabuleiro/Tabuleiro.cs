﻿

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

    }
}