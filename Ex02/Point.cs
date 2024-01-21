using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CheckersLogic
{
    public struct Point
    {
        public int m_X; 
        public int m_Y; 

        public Point(int i_x, int i_y)
        {
            m_X = i_x;
            m_Y = i_y;
        }

        public static Point GetPointBetweenPoints(Point i_From, Point i_To)
        {
            Point thePointInTheMiddle;

            thePointInTheMiddle.m_X = (i_To.m_X + i_From.m_X) / 2;
            thePointInTheMiddle.m_Y = (i_To.m_Y + i_From.m_Y) / 2;

            return thePointInTheMiddle;
        }

        public static bool CheckIfTwoPointsAreDiagonal(Point i_p1, Point i_p2)
        {
            return Math.Abs(i_p1.m_X - i_p2.m_X) == Math.Abs(i_p1.m_Y - i_p2.m_Y);
        }

        public static bool operator ==(Point i_p1, Point i_p2)
        {
            return (i_p1.m_X == i_p2.m_X) && (i_p1.m_Y == i_p2.m_Y);
        }

        public static bool operator !=(Point i_p1, Point i_p2)
        {
            return (i_p1.m_X != i_p2.m_X) || (i_p1.m_Y != i_p2.m_Y);
        }
    }
}
