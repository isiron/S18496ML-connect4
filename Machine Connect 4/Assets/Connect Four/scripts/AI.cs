using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System.Text;

public class AI : MonoBehaviour {

    private const int DEEPNESS = 4;
        int Strength = 30000;

        MinMax alg = new MinMax();

        public AI()
        {
            Strength = 40000;
            alg.SetStrength(Strength);
        }

        // use minmax to find best move
        public int TakeTurn(Board board)
        {
        // represents the best move value (starts low)
            float best = float.MinValue;
            int bestColumn = -1;
            List<int> moves = board.FindMoves();
            // loop through each column and check the value of the move
            foreach (int col in moves)
            {
                float value = FindValue(board, col, 0);
                // if the column has a better value for the AI than the current best move
                // use that column as the best
                if (value > best)
                {
                    best = value;
                    bestColumn = col;
                }
            }
            return bestColumn;
        }

        // finds the value of a specific move in a column
        // depth is how many moves ahead the alg is looking
        public float FindValue(Board board, int col, int depth)
        {
            // check if the move will win the game
            
            WinState? win = board.CheckWinState();

            // if the game is going to end with the move
            if (win != null)
            {
                // check if it will end with draw
                if (win == WinState.TIE)
                    return 0f;
                // return 1 (best) for win, and -1 (worst) for lose
                //else if (win == WinState.BLACKWIN && Game1.AIColor == BoardState.BLACK)
                else if (win == WinState.BLACKWIN && board.CurrentPlayer == BoardState.BLACK)
                    return 1f;
                //else if (win == WinState.REDWIN && Game1.AIColor == BoardState.RED)
                else if (win == WinState.REDWIN && board.CurrentPlayer == BoardState.RED)
                    return 1f;
                else
                    return -1f;
            }

            // if we have looked forward the maximum amount
            // return the value of the move
            if (depth == DEEPNESS)
            {
                // MCScore
                int newStrength = Convert.ToInt32(Strength / ((double)Math.Pow(7, DEEPNESS)));
                alg.SetStrength(newStrength);

                return alg.FindDeepValue(board, col);
            }

            //newBoard.MakeMove(col);
            board.MakeMove(col);

            // Get the possible moves for the newBoard (the next move would be players)
            List<int> moves = board.FindMoves(); //newBoard.FindMoves();

            // start looking into depther moves
            float value = float.MinValue;
            foreach (int col2 in moves)
                value = Math.Max(value, -1f * FindValue(board, col2, depth + 1));

            // remove the last move made so it doesnt stay permanent
            board.Revert(col);
            return value;
        }


    }