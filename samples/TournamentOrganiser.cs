using System;
using System.Text;
using System.IO;

namespace CodeSubmission
{
    class TournamentOrganiser
    {
        public void GenerateMatches(int playerNumber)
        {
            StringBuilder sb = new StringBuilder();
            StreamWriter wr;
            int realPlayers = playerNumber;

            // if odd number of players, add a virtual player
            if (playerNumber % 2 == 1)
            {
                playerNumber++;
            }
            int[] players = new int[playerNumber];
            for (int i = 0; i < playerNumber; i++)
            {
                players[i] = i + 1;
            }

            int separator = playerNumber / 2;

            // number of times players should play against each other. In our case, it's 2
            int battleNumber = 2;
            int RoundMult = 0;

            for (int bo = 0; bo < battleNumber; bo++)
            {
                for (int rounds = 0; rounds < playerNumber - 1; rounds++)
                {
                    wr = new StreamWriter($"Round_{rounds + 1 + RoundMult}.txt");
                    sb.Append($"Round {rounds + 1 + RoundMult}:");
                    sb.Append(Environment.NewLine);
                    for (int i = 0; i < separator; i++)
                    {
                        int player1 = players[separator - i - 1];
                        int player2 = players[separator + i];

                        // if odd number of players skip writing match with virtual player
                        if (realPlayers != playerNumber && (player1 == playerNumber || player2 == playerNumber)) continue;

                        // for return match, inverse write order to change starting player
                        if (bo % 2 == 0) 
                        {
                            sb.Append($"player {player1} vs player {player2}");
                            sb.Append(Environment.NewLine);
                        }
                        else
                        {
                            sb.Append($"player {player2} vs player {player1}");
                            sb.Append(Environment.NewLine);
                        }
                    }
                    RotatePlayers(players);
                    PrintRoundLayout(wr, sb);
                    sb.Clear();
                }
                RoundMult += (playerNumber - 1);
            }
        }

        //rotate all players but one in a direction that allows everyone to play each other
        public void RotatePlayers(int[] players) 
        {
            int temp = players[players.Length - 2];
            for (int i = players.Length - 2; i > 0; i--)
            {
                players[i] = players[i - 1];
            }
            players[0] = temp;
        }

        // write StringBuilder's content in a .txt file
        // .txt files will be found in the CodeSubmission\CodeSubmission\bin\Debug folder
        public void PrintRoundLayout(StreamWriter sw, StringBuilder sb) 
        {
            sw.WriteLine(sb);
            sw.Close();
        }
    }
}
