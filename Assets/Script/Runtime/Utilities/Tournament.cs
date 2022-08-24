using System;

namespace SuperUltra.Container
{

    public class Tournament
    {
        public DateTime endTime;
        public float prizePool;
        
        public Tournament()
        {
            prizePool = -1;
            endTime = new DateTime();
        }
    }
    
}