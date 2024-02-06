using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using BilibiliDM_PluginFramework;
using CommunityToolkit.Mvvm.ComponentModel;
using DMLotteryPlugin.Common;

namespace DMLotteryPlugin.ViewModels
{
    public class MainViewModel : ObservableObject
    {
        /// <summary>
        ///     弹幕关键词
        /// </summary>
        private string _dmKeywords;


        /// <summary>
        ///     是否开始统计
        /// </summary>
        private bool _isCounting;

        private bool _isDrawFinished;

        private string _logger = string.Empty;

        /// <summary>
        ///     抽奖弹幕存储
        /// </summary>
        private ObservableCollection<LotteryDM> _lotteryDmCollection;


        /// <summary>
        ///     抽奖数量
        /// </summary>
        private int _lotteryNum;

        private DMLotteryPlugin _plugin;

        public MainViewModel()
        {
            LotteryDmCollection = new ObservableCollection<LotteryDM>();

            for (var i = 0; i < 10; i++)
                LotteryDmCollection.Add(LotteryDM.Create(DateTime.Now, $"测试用户{i}", $"我是第{i}条弹幕"));

            IsCounting = false;
        }

        public DMLotteryPlugin Plugin
        {
            get => _plugin;
            set => SetProperty(ref _plugin, value);
        }

        public int LotteryNum
        {
            get => _lotteryNum;
            set => SetProperty(ref _lotteryNum, value);
        }

        public string DMKeywords
        {
            get => _dmKeywords;
            set => SetProperty(ref _dmKeywords, value);
        }

        public bool IsCounting
        {
            get => _isCounting;
            set => SetProperty(ref _isCounting, value);
        }

        public bool IsDrawFinished
        {
            get => _isDrawFinished;
            set => SetProperty(ref _isDrawFinished, value);
        }

        public string Logger
        {
            get => _logger;
            set => SetProperty(ref _logger, value);
        }

        public ObservableCollection<LotteryDM> LotteryDmCollection
        {
            get => _lotteryDmCollection;
            set => SetProperty(ref _lotteryDmCollection, value);
        }

        /// <summary>
        ///     处理弹幕事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="args"></param>
        public void OnPluginOnReceivedDanmaku(object sender, ReceivedDanmakuArgs args)
        {
            if (!IsCounting || args.Danmaku.MsgType != MsgTypeEnum.Comment) return;

            var userName = args.Danmaku.UserName;
            var dm = args.Danmaku.CommentText;

            // 弹幕为关键词 + 去重
            if (dm != DMKeywords || LotteryDmCollection.Any(it => it.Name == userName)) return;

            LotteryDmCollection.Add(LotteryDM.Create(DateTime.Now, userName, dm));
            Log($"{userName} 加入了抽奖");
        }

        /// <summary>
        ///     随机干掉一半抽奖者
        /// </summary>
        public void RandomCutHalf()
        {
            if (IsDrawFinished)
                return;

            var totalDmCount = LotteryDmCollection.Count;

            var random = new Random();
            var strBuilder = new StringBuilder();

            for (var i = 0; i <= (totalDmCount - 1) / 2; i++)
            {
                if (totalDmCount - i - LotteryNum <= 0)
                    break;

                var randIndex = random.Next(0, totalDmCount - i);
                strBuilder.Append($"{LotteryDmCollection[randIndex].Name} ");
                LotteryDmCollection.RemoveAt(randIndex);
            }

            if (strBuilder.Length > 0)
            {
                strBuilder.Append("被猫猫吃掉了！");
                Log(strBuilder.ToString());
            }

            CheckDrawStatus();
        }

        /// <summary>
        ///     直接抽出
        /// </summary>
        public void DirectDraw()
        {
            if (IsDrawFinished)
                return;

            if (CheckDrawStatus())
                return; //防止奇怪操作

            var totalDmCount = LotteryDmCollection.Count;

            var random = new Random();
            var strBuilder = new StringBuilder();

            var winnerList = new ObservableCollection<LotteryDM>();

            strBuilder.Append("恭喜这几位B立占用户 ");

            for (var i = 0; i < LotteryNum; i++)
            {
                var randIndex = random.Next(0, totalDmCount - i);

                strBuilder.Append($"{LotteryDmCollection[randIndex].Name} ");
                winnerList.Add(LotteryDmCollection[randIndex]);
                LotteryDmCollection.RemoveAt(randIndex);
            }

            LotteryDmCollection = winnerList;
            Log(strBuilder.ToString());
            IsDrawFinished = true;
        }

        /// <summary>
        ///     重置抽奖机
        /// </summary>
        public void Reset()
        {
            Logger = string.Empty;
            LotteryDmCollection.Clear();
            IsCounting = false;
            IsDrawFinished = false;
        }

        private bool CheckDrawStatus()
        {
            if (LotteryNum < LotteryDmCollection.Count) return false;

            Log($"恭喜这几位B立占用户 {string.Join(" ", LotteryDmCollection.Select(it => it.Name))}");
            IsDrawFinished = true;

            return true;
        }

        private void Log(string msg)
        {
            Logger += $"{msg}\n";
            Plugin.Log(msg);
        }
    }
}