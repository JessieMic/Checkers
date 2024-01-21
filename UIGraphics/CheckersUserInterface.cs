using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CheckersLogic;

namespace UIGraphics
{
    public class CheckersUserInterface
    {
        private FormGameSetup m_WindowSetupMenu;
        private FormCheckersBoard m_WindowGameBoard;
        private GameLogic m_GameLogic;
        private GameInfo m_GameInfo;    
 
        public CheckersUserInterface()
        {
            m_GameInfo = new GameInfo();
            m_WindowSetupMenu = new FormGameSetup(m_GameInfo);
            m_GameLogic = new GameLogic();
        }

        public void Run()
        {
            m_WindowSetupMenu.ShowDialog();
            if (m_WindowSetupMenu.DialogResult == System.Windows.Forms.DialogResult.OK)
            {
                m_GameLogic.InitiateANewGame(m_GameInfo.BoardSize);
                m_WindowGameBoard = new FormCheckersBoard(m_GameLogic.GetBoardMatrix(), m_GameInfo);
                m_WindowGameBoard.Forfeit += new ForfeitGameDelegate(this.forfeitWithResponseHandler);
                m_WindowGameBoard.Played += new PlayFunctionDelegate(this.playACheckersMove);
                m_WindowGameBoard.ShowDialog();
            }
        }

        private void playACheckersMove(CheckersLogic.Point i_From, CheckersLogic.Point i_To)
        {
            eResponseToUI response = m_GameLogic.Play(false, i_From, i_To);
            responseHandler(response);
        }

        private void responseHandler(eResponseToUI i_Response)
        {
            switch (i_Response)
            {
                case eResponseToUI.BadMove:
                    m_WindowGameBoard.ShowMessageBoxToUser("Invalid move, please try again");
                    break;
                case eResponseToUI.UserDidntPickAnEatingOption:
                    m_WindowGameBoard.ShowMessageBoxToUser("If there's an option for a player to take down an enemy piece, player must choose it");
                    break;
                case eResponseToUI.UserForfeit:
                    m_GameLogic.CountPointsForPlayers();
                    askForAnotherGameWithCustomMessege(string.Format("{0} Has forfeit the game! {1} Wins!", getCurrentPlayerName(), getNotCurrentPlayerName()));
                    break;         
                case eResponseToUI.GameEndedWithAWin:
                    askForAnotherGameWithCustomMessege(string.Format("It's a Win for {0}!", getWinnerName()));
                    break;
                case eResponseToUI.Tie:
                    askForAnotherGameWithCustomMessege("It's a tie!");
                    break;
                case eResponseToUI.WrongPieceToEatAgain:
                    m_WindowGameBoard.ShowMessageBoxToUser("Player next move must capture an enemy piece with the same piece he used before.");
                    break;
                default: // EatSucceeded or MoveSucceeded or WaitingForAnotherEat,  or PcEatSucceeded PcMoveSucceeded waitingForAnotherPcEat.
                    m_WindowGameBoard.UpdateAllButtons();
                    m_WindowGameBoard.Update();
                    if ((m_GameInfo.IsPlayerTwoComputer && m_GameLogic.GetCurrentPlayerTeam() == eTeam.TopSide) || i_Response == eResponseToUI.waitingForAnotherPcEat)
                    {
                        CheckersLogic.Point point = new CheckersLogic.Point();
                        System.Threading.Thread.Sleep(500);
                        i_Response = m_GameLogic.Play(true, point, point);
                        m_WindowGameBoard.UpdateAllButtons();
                        responseHandler(i_Response);
                    }

                    break;
            }
        }

        public void forfeitWithResponseHandler()
        {
            responseHandler(eResponseToUI.UserForfeit);
        }

        private string getWinnerName()
        {
            string nameOfWinner;

            if (m_GameLogic.WinnerOfGame == eTeam.BottomSide)
            {
                nameOfWinner = m_GameInfo.PlayerOneName;
            }
            else
            {
                nameOfWinner = m_GameInfo.PlayerTwoName;
            }

            return nameOfWinner;
        }

        private string getCurrentPlayerName()
        {
            string nameOfPlayer;

            if (m_GameLogic.GetCurrentPlayerTeam() == eTeam.BottomSide)
            {
                nameOfPlayer = m_GameInfo.PlayerOneName;
            }
            else
            {
                nameOfPlayer = m_GameInfo.PlayerTwoName;
            }

            return nameOfPlayer;
        }

        private string getNotCurrentPlayerName()
        {
            string nameOfPlayer;

            if (m_GameLogic.GetNotCurrentPlayerTeam() == eTeam.BottomSide)
            {
                nameOfPlayer = m_GameInfo.PlayerOneName;
            }
            else
            {
                nameOfPlayer = m_GameInfo.PlayerTwoName;
            }

            return nameOfPlayer;
        }

        private void askForAnotherGameWithCustomMessege(string i_Messege)
        {
            if(m_WindowGameBoard.ShowMessageBoxToUserWithYesAndNo(string.Format("{0}! {1}Do you want another game?", i_Messege, Environment.NewLine)))
            {
                startAnotherGame();
            }
            else
            {               
                m_WindowGameBoard.IsUserForfeiting = false;               
                m_WindowGameBoard.ShowMessageBoxToUser("Bye bye.");
                m_WindowGameBoard.IsFormRunning = false;
                m_WindowGameBoard.Close();
            }
        }

        private void startAnotherGame()
        {
            int pointsForPlayer1;
            int pointsForPlayer2;

            pointsForPlayer1 = m_GameLogic.GetPointsForTeam(eTeam.BottomSide);
            pointsForPlayer2 = m_GameLogic.GetPointsForTeam(eTeam.TopSide);
            m_WindowGameBoard.UpdatePlayersScore(pointsForPlayer1.ToString(), pointsForPlayer2.ToString());
            m_GameLogic.InitiateANewGame(m_GameInfo.BoardSize);
            m_WindowGameBoard.ChangeGameBoard(m_GameLogic.GetBoardMatrix());
            m_WindowGameBoard.UpdateAllButtons();
        }
    }
}
