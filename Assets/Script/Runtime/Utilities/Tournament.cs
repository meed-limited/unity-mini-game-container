using System;

namespace SuperUltra.Container
{
    public enum TournamentStatus
    {
        Pending = 1,
        Started = 2,
        Pause = 3,
        End = 4
    }

    public class Tournament
    {
        public DateTime endTime;
        public float prizePool;
        public TournamentStatus status;
        
        public Tournament()
        {
            prizePool = -1;
            endTime = new DateTime();
            status = TournamentStatus.Pending;
        }

        public bool IsValid()
        {
            bool hasPrize = prizePool > 0;
            bool hasBegin = status == TournamentStatus.Started;
            return hasPrize && hasBegin;
        }

    }
    
}