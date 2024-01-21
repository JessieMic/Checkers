using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CheckersLogic
{
    public class GamePiece
    {
        private bool m_IAmKing = false;
        private eTeam m_MyTeam;
        private Point m_Location;

        public GamePiece(eTeam i_team, Point i_location)
        {
            m_MyTeam = i_team;
            m_Location = i_location;
        }

        public eTeam GetTeam() 
        { 
            return m_MyTeam; 
        }

        public void MakeKing() 
        { 
            m_IAmKing = true; 
        }

        public bool IsKing() 
        { 
            return m_IAmKing; 
        }

        public void SetPoint(Point i_Point) 
        { 
            m_Location = i_Point; 
        }

        public Point GetPoint() 
        { 
            return m_Location; 
        }
    }
}