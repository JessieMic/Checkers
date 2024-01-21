using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CheckersLogic
{
    public class GameLogic
    {
        private Board m_gameBoard;
        private eTeam m_TeamsTurn;
        private eTeam m_WinnerOfRound;
        private List<GamePiece> m_TopPlayerGamePiecesList;
        private List<GamePiece> m_BottomPlayerGamePiecesList;
        private int m_TopPlayerPoints = 0;
        private int m_BottomPlayerPoints = 0;
        private bool m_IsTherePieceThatCanEat;
        private Point m_PointOfGamePieceThatNeedToEatAgain;
        private bool m_DoesPieceHaveAnotherEat = false;
        private Point m_RecentToPoint;
        private Point m_RecentFromPoint;

        public eTeam WinnerOfGame
        {
            get { return m_WinnerOfRound; }
            set { m_WinnerOfRound = value; }
        }

        public void InitiateANewGame(int i_BoardSize)
        {
            m_gameBoard = new Board();
            m_gameBoard.CreateTheBoard(i_BoardSize);
            m_gameBoard.SetGamePieces(out m_TopPlayerGamePiecesList, out m_BottomPlayerGamePiecesList);
            m_TeamsTurn = eTeam.BottomSide;
        }

        public GamePiece[,] GetBoardMatrix()
        {
            return m_gameBoard.GetBoardMatrix();
        }

        public eTeam GetCurrentPlayerTeam() 
        { 
            return m_TeamsTurn; 
        }

        public eTeam GetNotCurrentPlayerTeam()
        {
            eTeam NotCurrentTeam;

            if (m_TeamsTurn == eTeam.BottomSide)
            {
                NotCurrentTeam = eTeam.TopSide;
            }
            else
            {
                NotCurrentTeam = eTeam.BottomSide;
            }

            return NotCurrentTeam;
        }

        public GamePiece GetPieceAtPoint(Point i_p)
        {
            return m_gameBoard.GetPiece(i_p);
        }

        public bool CheckIfBoardSizeCorrect(int i_Size)
        {
            // $G$ NTT-999 (-3) You should use enum for the board sizes
            if (i_Size == 6 || i_Size == 8 || i_Size == 10)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public int GetPointsForTeam(eTeam i_Team)
        {
            int pointsOfTeam;

            if (i_Team == eTeam.BottomSide)
            {
                pointsOfTeam = m_BottomPlayerPoints;
            }
            else
            {
                pointsOfTeam = m_TopPlayerPoints;
            }

            return pointsOfTeam;
        }

        public eResponseToUI Play(bool i_IsItPcTurnToMakeAMove, Point i_From, Point i_To)
        {
            eResponseToUI response;
            GamePiece thePiecePlaying;

            if (i_IsItPcTurnToMakeAMove)
            {
                response = getPcNextMove(ref i_From, ref i_To);
            }
            else
            {
                response = validateAndGetAllowedAction(i_From, i_To, GetCurrentPlayerTeam());
            }
            
            if(response != eResponseToUI.BadMove)
            {
                thePiecePlaying = m_gameBoard.GetPiece(i_From);
                if (response == eResponseToUI.EatSucceeded || response == eResponseToUI.PcEatSucceeded)
                {
                    eatAndHopPiece(thePiecePlaying, i_To);
                }
                else if (response == eResponseToUI.MoveSucceeded || response == eResponseToUI.PcMoveSucceeded)
                {
                    movePiece(thePiecePlaying, i_To);
                }

                lookForAndMakeKings();
                if (response == eResponseToUI.EatSucceeded || response == eResponseToUI.PcEatSucceeded)
                {
                    MoveOptions optionsForTheSpecificPiece = getValidMovesForPiece(m_gameBoard.GetPiece(i_To));
                    if (optionsForTheSpecificPiece.CanEat())
                    {
                        if (response == eResponseToUI.PcEatSucceeded)
                        {
                            response = eResponseToUI.waitingForAnotherPcEat;
                        }
                        else
                        {
                            response = eResponseToUI.WaitingForAnotherEat;
                        }

                        m_PointOfGamePieceThatNeedToEatAgain = i_To;
                        m_DoesPieceHaveAnotherEat = true;
                    }
                }

                if (response != eResponseToUI.WaitingForAnotherEat && response != eResponseToUI.waitingForAnotherPcEat 
                    && response != eResponseToUI.WrongPieceToEatAgain)
                {
                    checkAndChangeResponseIfGameEnded(ref response);
                    if (response == eResponseToUI.GameEndedWithAWin)
                    {
                        CountPointsForPlayers();
                    }

                    endTurn();
                }
            }
            else if (response == eResponseToUI.BadMove && m_IsTherePieceThatCanEat)
            {
                response = eResponseToUI.UserDidntPickAnEatingOption;
            }

            m_RecentToPoint = i_To;
            m_RecentFromPoint = i_From;

            return response;
        }

        public Point RecentPlayerToPoint
        {
            get { return m_RecentToPoint; }
        }

        public Point RecentPlayerFromPoint
        {
            get { return m_RecentFromPoint; }
        }

        private eResponseToUI getPcNextMove(ref Point o_PointToMoveFrom, ref Point o_PointToMoveTo)
        {
            eResponseToUI response = eResponseToUI.BadMove;
            bool theOnlyPlayIsToEat = false;
            List<MoveOptions> pcOptions;
            int IndexOfAMoveFromPcOption;
            Random randomIndex = new Random();
            MoveOptions optionsForTheRandomPiece;

            pcOptions = getValidMovesForTeam(m_TeamsTurn);
            if (pcOptions.Count() > 0)
            {
                theOnlyPlayIsToEat = pcOptions[0].CanEat();
                if (m_DoesPieceHaveAnotherEat)
                {
                    IndexOfAMoveFromPcOption = pcOptions.FindIndex(option => option.m_Piece.GetPoint() == m_PointOfGamePieceThatNeedToEatAgain);
                    m_DoesPieceHaveAnotherEat = false;
                }
                else
                {
                    IndexOfAMoveFromPcOption = randomIndex.Next(0, pcOptions.Count());
                }

                optionsForTheRandomPiece = pcOptions[IndexOfAMoveFromPcOption];
                if (theOnlyPlayIsToEat)
                {
                    o_PointToMoveTo = optionsForTheRandomPiece.GetThePointOfARandomEatMove();
                    response = eResponseToUI.PcEatSucceeded;
                }
                else
                {
                    o_PointToMoveTo = optionsForTheRandomPiece.GetThePointOfARandomMove();
                    response = eResponseToUI.PcMoveSucceeded;
                }

                o_PointToMoveFrom = optionsForTheRandomPiece.m_Piece.GetPoint();
            }
            else
            {
                response = eResponseToUI.PcHasNoMoreMoves;
            }

            return response;
        }

        private void checkAndChangeResponseIfGameEnded(ref eResponseToUI io_response)
        {
            List<MoveOptions> OtherTeamOptions = getValidMovesForTeam(GetNotCurrentPlayerTeam());
            List<MoveOptions> CurrentTeamOptions;

            if (OtherTeamOptions.Count == 0)
            {
                CurrentTeamOptions = getValidMovesForTeam(GetCurrentPlayerTeam());
                if (CurrentTeamOptions.Count == 0)
                {
                    io_response = eResponseToUI.Tie; // both players cant play
                }
                else
                {
                    io_response = eResponseToUI.GameEndedWithAWin;
                    m_WinnerOfRound = GetCurrentPlayerTeam();
                }
            }
        }

        private eResponseToUI validateAndGetAllowedAction(Point i_From, Point i_To, eTeam i_Team)
        {
            eResponseToUI response = eResponseToUI.BadMove;
            int indexOfPieceOptionsInList;
            List<MoveOptions> playerOptions = getValidMovesForTeam(i_Team);
            MoveOptions optionsForTheSpecificPiece;
            bool theOnlyPlayIsToEat = false;

            if(m_DoesPieceHaveAnotherEat && m_PointOfGamePieceThatNeedToEatAgain != i_From)
            {
                response = eResponseToUI.WrongPieceToEatAgain;
            }
            else
            {
                indexOfPieceOptionsInList = playerOptions.FindIndex(option => option.m_Piece.GetPoint() == i_From);
                if (indexOfPieceOptionsInList >= 0)
                {
                    theOnlyPlayIsToEat = playerOptions[0].CanEat();
                    optionsForTheSpecificPiece = playerOptions[indexOfPieceOptionsInList];
                    if (theOnlyPlayIsToEat && optionsForTheSpecificPiece.CheckIfCanEatToPoint(i_To))
                    {
                        response = eResponseToUI.EatSucceeded;
                    }
                    else if (!theOnlyPlayIsToEat && optionsForTheSpecificPiece.CheckIfCanMoveToPoint(i_To))
                    {
                        response = eResponseToUI.MoveSucceeded;
                    }
                }

                if(m_DoesPieceHaveAnotherEat)
                {
                    m_DoesPieceHaveAnotherEat = false;
                }
            }          

            return response;
        }

        private MoveOptions getValidMovesForPiece(GamePiece i_Piece)
        {
            MoveOptions myOptions = new MoveOptions();
            Point from = i_Piece.GetPoint();
            Point moveLeftPoint, moveRightPoint, eatLeftPoint, eatRightPoint;

            myOptions.m_Piece = i_Piece;
            moveLeftPoint.m_Y = from.m_Y - 1;
            moveRightPoint.m_Y = from.m_Y + 1;
            eatLeftPoint.m_Y = from.m_Y - 2;
            eatRightPoint.m_Y = from.m_Y + 2;
            if (i_Piece.IsKing() || i_Piece.GetTeam() == eTeam.BottomSide) 
            {
                moveLeftPoint.m_X = moveRightPoint.m_X = from.m_X - 1;
                eatLeftPoint.m_X = eatRightPoint.m_X = from.m_X - 2;
                if (m_gameBoard.CheckIfPointInRange(moveLeftPoint) && m_gameBoard.GetPiece(moveLeftPoint) == null)
                {
                    myOptions.m_CanMoveUpLeft = true;
                }
                else if (m_gameBoard.CheckIfPointInRange(eatLeftPoint) && checkIfCanEat(i_Piece, eatLeftPoint))
                {
                    myOptions.m_CanEatUpLeft = true;
                }

                if (m_gameBoard.CheckIfPointInRange(moveRightPoint) && m_gameBoard.GetPiece(moveRightPoint) == null)
                {
                    myOptions.m_CanMoveUpRight = true;
                }
                else if (m_gameBoard.CheckIfPointInRange(eatRightPoint) && checkIfCanEat(i_Piece, eatRightPoint))
                {
                    myOptions.m_CanEatUpRight = true;
                }
            }

            if (i_Piece.IsKing() || i_Piece.GetTeam() == eTeam.TopSide) 
            {
                moveLeftPoint.m_X = moveRightPoint.m_X = from.m_X + 1;
                eatLeftPoint.m_X = eatRightPoint.m_X = from.m_X + 2;
                if (m_gameBoard.CheckIfPointInRange(moveLeftPoint) && m_gameBoard.GetPiece(moveLeftPoint) == null)
                {
                    myOptions.m_CanMoveDownLeft = true;
                }
                else if (m_gameBoard.CheckIfPointInRange(eatLeftPoint) && checkIfCanEat(i_Piece, eatLeftPoint))
                {
                    myOptions.m_CanEatDownLeft = true;
                }

                if (m_gameBoard.CheckIfPointInRange(moveRightPoint) && m_gameBoard.GetPiece(moveRightPoint) == null)
                {
                    myOptions.m_CanMoveDownRight = true;
                }
                else if (m_gameBoard.CheckIfPointInRange(eatRightPoint) && checkIfCanEat(i_Piece, eatRightPoint))
                {
                    myOptions.m_CanEatDownRight = true;
                }
            }

            return myOptions;
        }

        private List<MoveOptions> getValidMovesForTeam(eTeam i_Team)
        {
            List<GamePiece> piecesOfTeam;
            List<MoveOptions> eatOptions = new List<MoveOptions>();
            List<MoveOptions> moveOptions = new List<MoveOptions>();
            List<MoveOptions> canPlayOptions;
            MoveOptions currentOptins;
            
            m_IsTherePieceThatCanEat = false;
            if (i_Team == eTeam.BottomSide)
            {
                piecesOfTeam = m_BottomPlayerGamePiecesList;
            }
            else
            {
                piecesOfTeam = m_TopPlayerGamePiecesList;
            }

            foreach (GamePiece currentPiece in piecesOfTeam)
            {
                currentOptins = getValidMovesForPiece(currentPiece);
                if (currentOptins.CanEat())
                {
                    eatOptions.Add(currentOptins);
                    m_IsTherePieceThatCanEat = true;
                }
                else if (currentOptins.CanMove())
                {
                    moveOptions.Add(currentOptins);
                }
            }

            if (eatOptions.Count > 0)
            {
                canPlayOptions = eatOptions;
            }
            else
            {
                canPlayOptions = moveOptions;
            }

            return canPlayOptions;
        }

        private void killPiece(GamePiece i_PieceToKill)
        {
            m_gameBoard.RemovePiece(i_PieceToKill.GetPoint());
            if (i_PieceToKill.GetTeam() == eTeam.TopSide)
            {
                m_TopPlayerGamePiecesList.Remove(i_PieceToKill);
            }
            else
            {
                m_BottomPlayerGamePiecesList.Remove(i_PieceToKill);
            }
        }

        private void movePiece(GamePiece i_Piece, Point i_To)
        {
            m_gameBoard.MovePiece(i_Piece.GetPoint(), i_To);
        }

        private void eatAndHopPiece(GamePiece i_Piece, Point i_To)
        {
            Point piecePoint = i_Piece.GetPoint();
            GamePiece pieceToDelete = m_gameBoard.GetPiece(Point.GetPointBetweenPoints(piecePoint, i_To));

            killPiece(pieceToDelete);
            m_gameBoard.MovePiece(piecePoint, i_To);
        }

        private bool checkIfCanEat(GamePiece i_Piece, Point i_To)
        {
            bool LegitEatingMove = false;
            Point piecePoint = i_Piece.GetPoint();
            Point eatenPoint = Point.GetPointBetweenPoints(piecePoint, i_To);
            GamePiece gettingEatenPiece;

            gettingEatenPiece = m_gameBoard.GetPiece(eatenPoint);
            if (gettingEatenPiece != null && m_gameBoard.GetPiece(i_To) == null)
            {
                if (i_Piece.GetTeam() != gettingEatenPiece.GetTeam())
                {
                    LegitEatingMove = true;
                }
            }

            return LegitEatingMove;
        }

        private void lookForAndMakeKings()
        {
            GamePiece currentPiece;
            int boardSize = m_gameBoard.GetSize();

            for (int j = 0; j < boardSize; j++)
            {
                currentPiece = m_gameBoard.GetPiece(0, j);
                if (currentPiece != null)
                {
                    if ((currentPiece.GetTeam() == eTeam.BottomSide) && !currentPiece.IsKing())
                    {
                        currentPiece.MakeKing();
                    }
                }

                currentPiece = m_gameBoard.GetPiece(boardSize - 1, j);
                if (currentPiece != null)
                {
                    if ((currentPiece.GetTeam() == eTeam.TopSide) && !currentPiece.IsKing())
                    {
                        currentPiece.MakeKing();
                    }
                }
            }
        }

        public void CountPointsForPlayers()
        {
            int topPiecesCount = 0;
            int bottomPiecesCount = 0;

            foreach (GamePiece currentPiece in m_TopPlayerGamePiecesList)
            {
                if(currentPiece.IsKing())
                {
                    topPiecesCount += 4;
                }
                else               
                {
                    topPiecesCount++;
                }
            }

            foreach (GamePiece currentPiece in m_BottomPlayerGamePiecesList)
            {
                if (currentPiece.IsKing())
                {
                    bottomPiecesCount += 4;
                }
                else
                {
                    bottomPiecesCount++;
                }
            }

            if (m_WinnerOfRound == eTeam.TopSide)
            {
                m_TopPlayerPoints += topPiecesCount - bottomPiecesCount;
            }
            else
            {
                m_BottomPlayerPoints += bottomPiecesCount - topPiecesCount;
            }
        }

        private void endTurn()
        {
            if (m_TeamsTurn == eTeam.BottomSide)
            {
                m_TeamsTurn = eTeam.TopSide;
            }
            else
            {
                m_TeamsTurn = eTeam.BottomSide;
            }
        }
    }
}