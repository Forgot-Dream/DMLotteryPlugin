using System;
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
        }

        private void DMLotteryPlugin_ReceivedDanmaku(object sender, ReceivedDanmakuArgs e)
        {
            _mainViewWindow?.Dispatcher.BeginInvoke(DispatcherPriority.Normal, new Action(() =>
            {
                _mainViewWindow.ViewModel.OnPluginOnReceivedDanmaku(sender,e);
            }));
        }

        private void InitWindow()
        {
            _mainViewWindow = new MainView
            {
                ViewModel =
                {
                    Plugin = this
                }
            };

            _mainViewWindow.Closed += (sender, args) => _mainViewWindow = null; //关闭窗口重新打开时候会丢失资源
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
            _mainViewWindow?.Close();
            base.Stop();
        }
    }
}
