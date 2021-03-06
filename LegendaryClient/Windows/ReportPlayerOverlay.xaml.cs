﻿#region

using System;
using System.Windows;
using LegendaryClient.Logic;
using LegendaryClient.Properties;
using PVPNetConnect.RiotObjects.Platform.Harassment;
using System.Windows.Documents;
using System.Windows.Controls;

#endregion

namespace LegendaryClient.Windows
{
    /// <summary>
    ///     Interaction logic for ReportPlayerOverlay.xaml
    /// </summary>
    public partial class ReportPlayerOverlay
    {
        private double summonerID, gameID;
        private Button reportButton;

        public ReportPlayerOverlay(double summonerID, double gameID, string summonerName, Button reportButton)
        {
            InitializeComponent();
            Change();
            this.summonerID = summonerID;
            this.gameID = gameID;
            this.reportButton = reportButton;
            PlayerNameLabel.Content = summonerName;
        }

        public void Change()
        {
            var themeAccent = new ResourceDictionary
            {
                Source = new Uri(Settings.Default.Theme)
            };
            Resources.MergedDictionaries.Add(themeAccent);
        }

        private void ExitButton_Click(object sender, RoutedEventArgs e)
        {
            Client.OverOverlayContainer.Visibility = Visibility.Hidden;
        }

        private async void SendReport_Click(object sender, RoutedEventArgs e)
        {
            TextRange text = new TextRange(ReportText.Document.ContentStart, ReportText.Document.ContentEnd);
            HarassmentReport report = new HarassmentReport()
            {
                GameID = gameID,
                ReportingSummonerID = summonerID,
                ReportSource = "GAME",
                SummonerID = Client.LoginPacket.AllSummonerData.Summoner.SumId,
                Comment = text.Text
            };
            string reportReason = string.Empty;
            switch (ReportCategoryComboBox.SelectedItem as string)
            {
                case "Griefing: Assisting Enemy Team":
                    reportReason = "ASSIST_ENEMY_TEAM";
                    break;
                case "Griefing: Intentionally Feeding":
                    reportReason = "INTENT_FEED";
                    break;
                case "Harassment: Offensive Language":
                    reportReason = "OFFENSIVE_LANGUAGE";
                    break;
                case "Harassment: Verbal Abuse":
                    reportReason = "VERBAL_ABUSE";
                    break;
                case "Inappropriate Name":
                    reportReason = "INAPPROPRIATE_NAME";
                    break;
                case "Leaving the Game/AFK":
                    reportReason = "LEAVE_OR_AFK";
                    break;
                case "Negative Attitude":
                    reportReason = "NEGATIVE_ATTITUDE";
                    break;
                case "Refusing to Communicate with Team":
                    reportReason = "REFUSE_COMMUNICATE";
                    break;
                case "Spamming": //what is this?
                    reportReason = "";
                    break;
                case "Unskilled Player":
                    reportReason = "UNSKILLED_PLAYER";
                    break;
            }
            report.Offense = reportReason;
            await Client.PVPNet.ReportPlayer(report);
            reportButton.IsEnabled = false;
            Client.OverOverlayContainer.Visibility = Visibility.Hidden;
        }
    }
}