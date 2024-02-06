using System;

namespace DMLotteryPlugin.Common
{
    public class LotteryDM
    {
        public DateTime ReceiveTime { get; set; }
        public string Name { get; set; }
        public string Comment { get; set; }

        public static LotteryDM Create(DateTime receiveTime, string name, string comment)
        {
            return new LotteryDM { ReceiveTime = receiveTime, Name = name, Comment = comment };
        }
    }
}
