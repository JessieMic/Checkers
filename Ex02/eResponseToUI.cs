using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CheckersLogic
{
    public enum eResponseToUI
    {
        BadMove,
        EatSucceeded,
        MoveSucceeded,
        WaitingForAnotherEat,
        Tie,
        UserDidntPickAnEatingOption,
        PcEatSucceeded,
        PcMoveSucceeded,
        GameEndedWithAWin,
        waitingForAnotherPcEat,
        WrongPieceToEatAgain,
        UserWantsToKeepPlaying,
        ComputerTurnToPlay,
        PcHasNoMoreMoves,
        UserForfeit
    }
}
