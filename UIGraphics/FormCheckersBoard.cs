using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

public delegate void PlayFunctionDelegate(CheckersLogic.Point from, CheckersLogic.Point to);

public delegate void ForfeitGameDelegate();

namespace UIGraphics
{
    public partial class FormCheckersBoard : Form
    {
        public event PlayFunctionDelegate Played;

        public event ForfeitGameDelegate Forfeit;

        private const int defaultSpaceSize = 50;
        private PieceButton[,] m_ButtonsGamePieces;
        private CheckersLogic.GamePiece[,] m_GameBoard;
        private PieceButton m_FromPiece;         
        private bool m_IsFormRunning = true;
        private bool m_IsUserForfeiting = true;

        public FormCheckersBoard(CheckersLogic.GamePiece[,] i_GameBoard, CheckersLogic.GameInfo i_GameInfo)
        {
            m_GameBoard = i_GameBoard;
            InitializeComponent();
            InitializeNameAndScoreLabels(i_GameInfo);
            setWindowSizeAccordingToNumberOfPiecesAndNameLabels(i_GameInfo);
            initializePiecesButtons(i_GameInfo.BoardSize);
        }

        public bool IsFormRunning
        {
            set
            { 
                m_IsFormRunning = value;
            }
        }

        public bool IsUserForfeiting
        {
            set 
            {
                m_IsUserForfeiting = value;
            }
        }

        private void setWindowSizeAccordingToNumberOfPiecesAndNameLabels(CheckersLogic.GameInfo i_GameInfo)
        {
            const int leftSide = defaultSpaceSize * 2;
            const int rightSide = defaultSpaceSize * 2;
            const int topSide = defaultSpaceSize * 2;
            const int botSide = defaultSpaceSize * 2;
            const int ButtonSize = 50;
            this.AutoSize = false;
            this.Width = leftSide + rightSide + (ButtonSize * i_GameInfo.BoardSize);
            this.Height = topSide + botSide + (ButtonSize * (i_GameInfo.BoardSize + 1));
            if(labelPlayer2Score.Right + defaultSpaceSize > this.Width)
            {
                this.Width = labelPlayer2Score.Right + defaultSpaceSize;
            }
        }

        public void ChangeGameBoard(CheckersLogic.GamePiece[,] i_GameBoard)
        {
            m_GameBoard = i_GameBoard;
        }

        private void InitializeNameAndScoreLabels(CheckersLogic.GameInfo i_GameInfo)
        {
            labelPlayer1Name.Text = i_GameInfo.PlayerOneName;
            labelPlayer1Score.Text = "0";
            labelPlayer1Score.Left = labelPlayer1Name.Right + defaultSpaceSize;
            labelPlayer2Name.Left = labelPlayer1Score.Right + (2 * defaultSpaceSize);
            labelPlayer2Name.Text = i_GameInfo.PlayerTwoName;
            labelPlayer2Score.Text = "0";
            labelPlayer2Score.Left = labelPlayer2Name.Right + defaultSpaceSize;
        }

        private void initializePiecesButtons(int i_BoardSize)
        {
            bool isCurrentTileBlack = true;
            bool changeColorCombinationEveryNewLine = false;
            const int moveXPixels = defaultSpaceSize * 2;

            if (i_BoardSize % 2 == 0)
            {
                changeColorCombinationEveryNewLine = true;
            }

            m_ButtonsGamePieces = new PieceButton[i_BoardSize, i_BoardSize];
            for(int i = 0; i < i_BoardSize; i++)
            {
                for (int j = 0; j < i_BoardSize; j++)
                {                   
                    m_ButtonsGamePieces[i, j] = new PieceButton(i, j, moveXPixels, defaultSpaceSize);
                    m_ButtonsGamePieces[i, j].Text = convertGameMatrixCellIntoChar(m_ButtonsGamePieces[i, j].PiecePoint).ToString();
                    m_ButtonsGamePieces[i, j].Click += ButtonsPiecesClick;
                    if (isCurrentTileBlack)
                    {
                        m_ButtonsGamePieces[i, j].BackColor = Color.Gray;
                        m_ButtonsGamePieces[i, j].Enabled = false;
                        isCurrentTileBlack = false;
                    }
                    else
                    {
                        isCurrentTileBlack = true;
                    }

                    this.Controls.Add(m_ButtonsGamePieces[i, j]);
                }

                if(changeColorCombinationEveryNewLine)
                {
                    if(isCurrentTileBlack)
                    {
                        isCurrentTileBlack = false;
                    }
                    else
                    {
                        isCurrentTileBlack = true;
                    }
                }
            }
        }

        private void ButtonsPiecesClick(object sender, EventArgs e)
        {
            if(m_FromPiece == null)
            {
                m_FromPiece = sender as PieceButton;
                m_FromPiece.BackColor = Color.LightBlue;
            }
            else if (m_FromPiece == sender as PieceButton)
            {
                m_FromPiece.BackColor = Button.DefaultBackColor;
                m_FromPiece = null;
            }
            else
            {
                m_FromPiece.BackColor = Button.DefaultBackColor;
                OnPlay(m_FromPiece.PiecePoint, (sender as PieceButton).PiecePoint);
                m_FromPiece = null;
            }
        }

        protected virtual void OnPlay(CheckersLogic.Point i_From, CheckersLogic.Point i_To)
        {
            Played.Invoke(i_From, i_To);
        }

        public void UpdateAllButtons()
        {
            int boardSize = (int)Math.Sqrt(m_GameBoard.Length);
           
            for (int i = 0; i < boardSize; i++)
            {
                for (int j = 0; j < boardSize; j++)
                {                    
                    m_ButtonsGamePieces[i, j].Text = convertGameMatrixCellIntoChar(m_ButtonsGamePieces[i, j].PiecePoint).ToString();
                }
            }
        }

        private char convertGameMatrixCellIntoChar(CheckersLogic.Point i_BoardCell)
        {
            char convertedBoardCellPiece;

            if (m_GameBoard[i_BoardCell.m_X, i_BoardCell.m_Y] == null)
            {
                convertedBoardCellPiece = ' ';
            }
            // $G$ NTT-999 (-5) You should have used constants \ enum here
            else if (m_GameBoard[i_BoardCell.m_X, i_BoardCell.m_Y].GetTeam() == CheckersLogic.eTeam.BottomSide)
            {
                if (m_GameBoard[i_BoardCell.m_X, i_BoardCell.m_Y].IsKing())
                {
                    convertedBoardCellPiece = 'K';
                }
                else
                {
                    convertedBoardCellPiece = 'X';
                }
            }
            else if (m_GameBoard[i_BoardCell.m_X, i_BoardCell.m_Y].IsKing())
            {
                convertedBoardCellPiece = 'U';
            }
            else
            {
                convertedBoardCellPiece = 'O';
            }

            return convertedBoardCellPiece;
        }

        public void UpdatePlayersScore(string i_Player1Score, string i_Player2Score)
        {
            labelPlayer1Score.Text = i_Player1Score;
            labelPlayer2Score.Text = i_Player2Score;
        }

        public void ShowMessageBoxToUser(string i_MessageToUser)
        {
            MessageBox.Show(i_MessageToUser, string.Empty, MessageBoxButtons.OK);
        }

        public bool ShowMessageBoxToUserWithYesAndNo(string i_MessageToUser)
        {
            bool isUserAnswerYes = false;
            DialogResult result;

            result = MessageBox.Show(i_MessageToUser, string.Empty, MessageBoxButtons.YesNo);
            if(result == System.Windows.Forms.DialogResult.Yes)
            {
                isUserAnswerYes = true;
            }

            return isUserAnswerYes;
        }    
       
        private void FormCheckersBoard_FormClosing(object sender, FormClosingEventArgs e)
        {
            if(m_IsUserForfeiting)
            {
                OnForfeiting();
            }

            if (m_IsFormRunning)
            {
                e.Cancel = true;
            }
            else
            {
                e.Cancel = false;
            }

            m_IsUserForfeiting = true;
        }

        protected virtual void OnForfeiting()
        {
            if(Forfeit != null)
            {
                Forfeit.Invoke();
            }
        }
    }
}
