using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CheckersLogic
{
    public class Board
    {
        private GamePiece[,] m_BoardMatrix;
        // $G$ CSS-999 (-5) This member should be a readonly member
        private int m_ColAndRowSize;

        public void CreateTheBoard(int i_ColAndRowSize)
        {
            m_BoardMatrix = new GamePiece[i_ColAndRowSize, i_ColAndRowSize];
            m_ColAndRowSize = i_ColAndRowSize;
        }

        public void SetGamePieces(out List<GamePiece> i_TopPlayerPieces, out List<GamePiece> i_BotPlayerPieces)
        {
            Point creatingLocation;
            i_TopPlayerPieces = new List<GamePiece>();
            i_BotPlayerPieces = new List<GamePiece>();
            int rowsToFill = (m_ColAndRowSize - 2) / 2;

            for (int i = 0; i < rowsToFill; i++)
            {
                for (int j = (i + 1) % 2; j < m_ColAndRowSize; j += 2)
                {
                    creatingLocation.m_X = i;
                    creatingLocation.m_Y = j;
                    m_BoardMatrix[i, j] = new GamePiece(eTeam.TopSide, creatingLocation);
                    i_TopPlayerPieces.Add(m_BoardMatrix[i, j]);
                    creatingLocation.m_X = (m_ColAndRowSize - 1) - i;
                    creatingLocation.m_Y = (m_ColAndRowSize - 1) - j;
                    m_BoardMatrix[(m_ColAndRowSize - 1) - i, (m_ColAndRowSize - 1) - j] = new GamePiece(eTeam.BottomSide, creatingLocation);
                    i_BotPlayerPieces.Add(m_BoardMatrix[(m_ColAndRowSize - 1) - i, (m_ColAndRowSize - 1) - j]);
                }
            }
        }

        public GamePiece GetPiece(Point i_Point)
        {
            return m_BoardMatrix[i_Point.m_X, i_Point.m_Y];
        }

        public GamePiece GetPiece(int i_X, int i_Y)
        {
            Point helper;

            helper.m_X = i_X;
            helper.m_Y = i_Y;

            return GetPiece(helper);
        }

        public GamePiece[,] GetBoardMatrix()
        {
            return m_BoardMatrix;
        }

        public void RemovePiece(Point i_Point) // called by KillPiece of GameLogic
        {
            m_BoardMatrix[i_Point.m_X, i_Point.m_Y] = null;
        }

        public void MovePiece(Point i_From, Point i_To)
        {
            m_BoardMatrix[i_From.m_X, i_From.m_Y].SetPoint(i_To); // update the point in the GamePiece class
            m_BoardMatrix[i_To.m_X, i_To.m_Y] = m_BoardMatrix[i_From.m_X, i_From.m_Y];
            m_BoardMatrix[i_From.m_X, i_From.m_Y] = null;
        }

        public int GetSize() 
        { 
            return m_ColAndRowSize; 
        }

        public bool CheckIfPointInRange(Point i_Point)
        {
            bool inRange;
            if (i_Point.m_X < 0 || i_Point.m_X >= m_ColAndRowSize)
            {
                inRange = false;
            }
            else if (i_Point.m_Y < 0 || i_Point.m_Y >= m_ColAndRowSize)
            {
                inRange = false;
            }
            else
            {
                inRange = true;
            }

            return inRange;
        }
    }
}
