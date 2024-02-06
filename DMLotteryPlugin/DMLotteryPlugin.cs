using System;
using System.Windows;
using BilibiliDM_PluginFramework;
using DMLotteryPlugin.Views;
using DispatcherPriority = System.Windows.Threading.DispatcherPriority;

namespace DMLotteryPlugin
{
    public class DMLotteryPlugin : DMPlugin
    {
        private MainView _mainViewWindow;

        public DMLotteryPlugin()
        {
            PluginAuth = "ForDream丶梦空";
            PluginName = "弹幕抽奖工具";
            PluginCont = "bilibili@ForDream丶梦空";
            PluginVer = "v0.0.1";
            PluginDesc = "只有抽奖功能的工具";
            ReceivedDanmaku += DMLotteryPlugin_ReceivedDanmaku;
            base.Start();
        }

        private void DMLotteryPlugin_ReceivedDanmaku(object sender, ReceivedDanmakuArgs e)
        {
            _mainViewWindow?.Dispatcher.BeginInvoke(DispatcherPriority.Normal,
                new Action(() => { _mainViewWindow.ViewModel.OnPluginOnReceivedDanmaku(sender, e); }));
        }

        private void InitWindow()
        {
            if (_mainViewWindow != null)
            {
                _mainViewWindow.Show();
                return;
            }

            _mainViewWindow = new MainView
            {
                ViewModel =
                {
                    Plugin = this
                }
            };

            _mainViewWindow.Closing += (sender, args) =>
            {
                _mainViewWindow.Visibility = Visibility.Hidden;
                args.Cancel = true;
            };

            _mainViewWindow.InitializeComponent();
            _mainViewWindow.Show();
        }

        public override void Admin()
        {
            InitWindow();
            base.Admin();
        }

        public override void Start()
        {
            base.Start();
        }

        public override void Stop()
        {
            base.Stop();
        }
    }
}