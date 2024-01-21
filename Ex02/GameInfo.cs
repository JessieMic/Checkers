namespace CheckersLogic
{
    using System;
    using System.Text;

    public class GameInfo
    {
        // $G$ CSS-999 This member shoul be a readonly member
        // $G$ CSS-004 (-5) Bad static member name (should be s_CamelCased)
        public static int m_BoardSize;
        private string m_PlayerOneName;
        private eTeam m_PlayerOneTeam;
        private string m_PlayerTwoName;
        private bool m_IsPlayerTwoAComputer = false;
        private eTeam m_PlayerTwoTeam;

        public eTeam PlayerOneTeam
        {
            get { return m_PlayerOneTeam; }
            set { m_PlayerOneTeam = value; }
        }

        public string PlayerOneName
        {
            get { return m_PlayerOneName; }
            set { m_PlayerOneName = value; }
        }

        public eTeam PlayerTwoTeam
        {
            get { return m_PlayerTwoTeam; }
            set { m_PlayerTwoTeam = value; }
        }

        public string PlayerTwoName
        {
            get { return m_PlayerTwoName; }
            set { m_PlayerTwoName = value; }
        }

        public bool IsPlayerTwoComputer
        {
            get { return m_IsPlayerTwoAComputer; }
            set { m_IsPlayerTwoAComputer = value; }
        }

        public int BoardSize
        {
            get { return m_BoardSize; }
            set { m_BoardSize = value; }
        }
    }
}