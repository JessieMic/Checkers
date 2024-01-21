using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace UIGraphics
{
    public class PieceButton : System.Windows.Forms.Button
    {
        private readonly int m_X;
        private readonly int m_Y;

        public CheckersLogic.Point PiecePoint
        {
            get
            {
                CheckersLogic.Point point;
                point.m_X = m_X;
                point.m_Y = m_Y;
                return point;
            }
        }

        public PieceButton(int i_X, int i_Y, int i_DistanceFromLeftUpperScreen, int i_ButtonSize)
        {
            m_X = i_X;
            m_Y = i_Y;
            Name = string.Format("Piece:{0},{1}", i_X, i_Y);
            Enabled = true;
            AutoSize = true;
            Height = i_ButtonSize;
            Width = i_ButtonSize;
            Left = (i_Y * i_ButtonSize) + i_DistanceFromLeftUpperScreen;
            Top = (i_X * i_ButtonSize) + i_DistanceFromLeftUpperScreen;
        }
    }
}
