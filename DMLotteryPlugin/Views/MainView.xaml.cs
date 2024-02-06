﻿using System;
using System.Windows;
using DMLotteryPlugin.ViewModels;

namespace DMLotteryPlugin.Views
{
    /// <summary>
    /// UserControl1.xaml 的交互逻辑
    /// </summary>
    public partial class MainView : Window
    {
        public MainViewModel ViewModel = new MainViewModel();
        public MainView()
        {
            InitializeComponent();
            DataContext = ViewModel;
            _setButtonStatus();
        }

        private void StartButton_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(ViewModel.DMKeywords) || ViewModel.LotteryNum <= 0)
            {
                MessageBox.Show("非法的抽奖弹幕设置/抽奖数字设置，请输入有效值");
                return;
            }

            ViewModel.IsCounting = true;
            _setButtonStatus();
        }

        private void _setButtonStatus()
        {
            StartButton.IsEnabled = !ViewModel.IsCounting;
            StopButton.IsEnabled = ViewModel.IsCounting;
            ResetButton.IsEnabled = ViewModel.IsCounting;
            CutHalfButton.IsEnabled = !ViewModel.IsCounting;
            FinishButton.IsEnabled = !ViewModel.IsCounting;
        }

        private void StopButton_OnClick(object sender, RoutedEventArgs e)
        {
            ViewModel.IsCounting = false;
            _setButtonStatus();
        }


        private void CutHalfButton_OnClick(object sender, RoutedEventArgs e)
        {
            var totalDmCount = ViewModel.LotteryDM.Count;

            var random = new Random();

            for (var i = 0; i <=(totalDmCount-1)/2; i++)
            {
                ViewModel.LotteryDM.RemoveAt(random.Next(0, totalDmCount - i));
            }
        }
    }
}