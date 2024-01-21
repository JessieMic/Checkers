using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace UIGraphics
{
    public partial class FormGameSetup : Form
    {
        private CheckersLogic.GameInfo m_GameInfo;

        public FormGameSetup(CheckersLogic.GameInfo i_GameInfo)
        {
            InitializeComponent();
            m_GameInfo = i_GameInfo;
        }

        private void checkboxPlayer2_CheckedChanged(object sender, EventArgs e)
        {
            if((sender as System.Windows.Forms.CheckBox).Checked)
            {
                textBoxPlayer2.Text = string.Empty;
                textBoxPlayer2.Enabled = true;
            }
            else
            {
                textBoxPlayer2.Enabled = false;
                // $G$ NTT-999 (-5) You should have used constants here
                textBoxPlayer2.Text = "[Computer]";
            }
        }

        private void ButtonDone_Click(object sender, EventArgs e)
        {
            bool isUserInputCorrect;

            isUserInputCorrect = showMessageBoxIfUserInputIsIncorrect();
            if(isUserInputCorrect)
            {
                m_GameInfo.PlayerOneName = textBoxPlayer1.Text;
                m_GameInfo.PlayerTwoName = textBoxPlayer2.Text;

                if (checkboxPlayer2.Checked == false)
                {
                    m_GameInfo.IsPlayerTwoComputer = true;
                }
                // $G$ NTT-999 (-3) You should use enum for the board sizes
                if (radioButton6x6.Checked)
                {
                    m_GameInfo.BoardSize = 6;
                }
                else if (radioButton8x8.Checked)
                {
                    m_GameInfo.BoardSize = 8;
                }
                else
                {
                    m_GameInfo.BoardSize = 10;
                }

                this.DialogResult = DialogResult.OK;
                this.Close();
            }
        }
        
        private bool showMessageBoxIfUserInputIsIncorrect()
        {
            string invalidNameMessage;
            string playerWithInvalidName = string.Empty;
            string caption = "Invalid Name Input";
            bool invalidPlayer1Name;
            bool invalidPlayer2Name = false;
            bool isInputCorrect = false;
            bool isTherePlayer2 = checkboxPlayer2.Checked;
            MessageBoxButtons buttonOk = MessageBoxButtons.OK;

            invalidPlayer1Name = checkIfPlayerNameIsCorrectAndSet(textBoxPlayer1.Text);
            if(isTherePlayer2)
            {
                invalidPlayer2Name = checkIfPlayerNameIsCorrectAndSet(textBoxPlayer2.Text);
            }

            if (!invalidPlayer1Name && isTherePlayer2 && !invalidPlayer2Name)
            {
                playerWithInvalidName = "Player 1 and player 2";
            }
            else if(!invalidPlayer1Name)
            {
                playerWithInvalidName = "Player 1";
            }
            else if(isTherePlayer2 && !invalidPlayer2Name)
            {
                playerWithInvalidName = "Player 2";
            }
            else
            {
                isInputCorrect = true;
            }

            if(!isInputCorrect)
            {
                invalidNameMessage = string.Format("Invalid name for {0}, Name should have no spaces and only 1-20 characters, please try again.", playerWithInvalidName);
                MessageBox.Show(invalidNameMessage, caption, buttonOk);
            }

            return isInputCorrect;
        }

        private bool checkIfPlayerNameIsCorrectAndSet(string i_PlayerName)
        {
            bool isUserNameInpurCorrect = false;

            while (!isUserNameInpurCorrect)
            {
                if (i_PlayerName.Length > 20 || i_PlayerName.Length == 0 || i_PlayerName.Contains(" "))
                {
                    break;
                }
                else
                {
                    isUserNameInpurCorrect = true;
                }
            }

            return isUserNameInpurCorrect;
        }
    }
}
