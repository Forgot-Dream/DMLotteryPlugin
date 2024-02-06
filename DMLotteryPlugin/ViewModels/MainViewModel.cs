using System;
using System.Collections.ObjectModel;
using BilibiliDM_PluginFramework;
using CommunityToolkit.Mvvm.ComponentModel;
using DMLotteryPlugin.Common;

namespace DMLotteryPlugin.ViewModels
{
    public class MainViewModel : ObservableObject
    {
        private DMLotteryPlugin _plugin;

        public DMLotteryPlugin Plugin
        {
            get => _plugin;
            set => SetProperty(ref _plugin, value);
        }

        public void OnPluginOnReceivedDanmaku(object sender, ReceivedDanmakuArgs args)
        {
            if (!IsCounting) return;

            var dm = args.Danmaku.CommentText;
            if (dm == DMKeywords)
            {
                LotteryDM.Add(Common.LotteryDM.Create(DateTime.Now, args.Danmaku.UserName, dm));
            }
        }


        public MainViewModel()
        {
            LotteryDM = new ObservableCollection<LotteryDM>();
            IsCounting = false;
        }



        /// <summary>
        /// 抽奖数量
        /// </summary>
        private int _lotteryNum;

        public int LotteryNum
        {
            get => _lotteryNum;
            set => SetProperty(ref _lotteryNum, value);
        }

        /// <summary>
        /// 弹幕关键词
        /// </summary>
        private string _dmKeywords;

        public string DMKeywords
        {
            get => _dmKeywords;
            set => SetProperty(ref _dmKeywords, value);
        }


        /// <summary>
        /// 是否开始统计
        /// </summary>
        private bool _isCounting;

        public bool IsCounting
        {
            get => _isCounting;
            set => SetProperty(ref _isCounting, value);
        }

        /// <summary>
        /// 抽奖弹幕存储
        /// </summary>
        private ObservableCollection<LotteryDM> _lotteryDM;

        public ObservableCollection<LotteryDM> LotteryDM
        {
            get => _lotteryDM;
            set => SetProperty(ref _lotteryDM, value);
        }
    }
}
