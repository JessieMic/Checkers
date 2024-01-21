using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CheckersLogic
{
    public struct MoveOptions
    {
        public GamePiece m_Piece;
        public bool m_CanEatUpLeft;
        public bool m_CanEatUpRight;
        public bool m_CanEatDownLeft;
        public bool m_CanEatDownRight;
        public bool m_CanMoveUpLeft;
        public bool m_CanMoveUpRight;
        public bool m_CanMoveDownLeft;
        public bool m_CanMoveDownRight;

        public bool CanMove()
        {
            return m_CanMoveDownLeft || m_CanMoveDownRight || m_CanMoveUpLeft || m_CanMoveUpRight;
        }

        public bool CanEat()
        {
            return m_CanEatDownLeft || m_CanEatDownRight || m_CanEatUpLeft || m_CanEatUpRight;
        }

        public bool CheckIfCanMoveToPoint(Point i_To)
        {
            bool canMove;

            int row = m_Piece.GetPoint().m_X - i_To.m_X;
            int col = m_Piece.GetPoint().m_Y - i_To.m_Y;
            if (row == 1 && col == 1)
            {
                canMove = m_CanMoveUpLeft;
            }
            else if (row == 1 && col == -1)
            {
                canMove = m_CanMoveUpRight;
            }
            else if (row == -1 && col == 1)
            {
                canMove = m_CanMoveDownLeft;
            }
            else if (row == -1 && col == -1)
            {
                canMove = m_CanMoveDownRight;
            }
            else
            {
                canMove = false;
            }

            return canMove;
        }

        public bool CheckIfCanEatToPoint(Point i_To)
        {
            bool canMove;

            int row = m_Piece.GetPoint().m_X - i_To.m_X;
            int col = m_Piece.GetPoint().m_Y - i_To.m_Y;
            if (row == 2 && col == 2)
            {
                canMove = m_CanEatUpLeft;
            }
            else if (row == 2 && col == -2)
            {
                canMove = m_CanEatUpRight;
            }
            else if (row == -2 && col == 2)
            {
                canMove = m_CanEatDownLeft;
            }
            else if (row == -2 && col == -2)
            {
                canMove = m_CanEatDownRight;
            }
            else
            {
                canMove = false;
            }

            return canMove;
        }

        public Point GetThePointOfARandomEatMove()
        {
            Point pointOfRandomMove = new Point();
            Point pointOfGamePiece = m_Piece.GetPoint();
            bool haveWeFoundALegalMove = false;
            Random randomMove = new Random();
            int randomMovePicked;

            while(!haveWeFoundALegalMove)
            {
                haveWeFoundALegalMove = true;
                randomMovePicked = randomMove.Next(1, 5);
                if(randomMovePicked == 1 && m_CanEatUpRight)
                {
                    pointOfRandomMove.m_X = pointOfGamePiece.m_X - 2;
                    pointOfRandomMove.m_Y = pointOfGamePiece.m_Y + 2;
                }
                else if (randomMovePicked == 2 && m_CanEatUpLeft)
                {
                    pointOfRandomMove.m_X = pointOfGamePiece.m_X - 2;
                    pointOfRandomMove.m_Y = pointOfGamePiece.m_Y - 2;
                }
                else if (randomMovePicked == 3 && m_CanEatDownLeft)
                {
                    pointOfRandomMove.m_X = pointOfGamePiece.m_X + 2; 
                    pointOfRandomMove.m_Y = pointOfGamePiece.m_Y - 2;
                }
                else if (randomMovePicked == 4 && m_CanEatDownRight)
                {
                    pointOfRandomMove.m_X = pointOfGamePiece.m_X + 2;
                    pointOfRandomMove.m_Y = pointOfGamePiece.m_Y + 2;
                }
                else
                {
                    haveWeFoundALegalMove = false;
                }
            }

            return pointOfRandomMove;
        }

        public Point GetThePointOfARandomMove()
        {
            Point pointOfRandomMove = new Point();
            Point pointOfGamePiece = m_Piece.GetPoint();
            bool haveWeFoundALegalMove = false;
            Random randomMove = new Random();
            int randomMovePicked;

            while (!haveWeFoundALegalMove)
            {
                haveWeFoundALegalMove = true;
                randomMovePicked = randomMove.Next(1, 5);
                if (randomMovePicked == 1 && m_CanMoveUpRight)
                {
                    pointOfRandomMove.m_X = pointOfGamePiece.m_X - 1;
                    pointOfRandomMove.m_Y = pointOfGamePiece.m_Y + 1;
                }
                else if (randomMovePicked == 2 && m_CanMoveUpLeft)
                {
                    pointOfRandomMove.m_X = pointOfGamePiece.m_X - 1;
                    pointOfRandomMove.m_Y = pointOfGamePiece.m_Y - 1;
                }
                else if (randomMovePicked == 3 && m_CanMoveDownLeft)
                {
                    pointOfRandomMove.m_X = pointOfGamePiece.m_X + 1;
                    pointOfRandomMove.m_Y = pointOfGamePiece.m_Y - 1;
                }
                else if (randomMovePicked == 4 && m_CanMoveDownRight)
                {
                    pointOfRandomMove.m_X = pointOfGamePiece.m_X + 1;
                    pointOfRandomMove.m_Y = pointOfGamePiece.m_Y + 1;
                }
                else
                {
                    haveWeFoundALegalMove = false;
                }             
            }

            return pointOfRandomMove;
        }
    }
}
