using System;
using System.ComponentModel;
using System.Drawing;
using System.Numerics;
using System.Reflection;
using Core;
using Core.Drawing.GUI;

namespace QueuingSystem
{
    public class Application : Window
    {
        private Panel _currentPanel;
        private Panel? _previousPanel;

        private SystemType _systemType;

        public Application()
        {
            Settings.Title = "Queuing System";
            Settings.Color = Color.Black;
            Settings.VWidth = Settings.Width;
            Settings.VHeight = Settings.Height;

            var exitButton = new Button(new SizeF(135, 25), "Exit")
            {
                OnButtonPressed = Close
            };
            

            var mainLeftPanel = new Panel(this, new SizeF(Settings.VWidth / 3f, Settings.VHeight))
            {
                Padding = new Vector2(10)
            };
            var withQueuePanel = new Panel(this, new SizeF(Settings.VWidth / 3f, Settings.VHeight))
            {
                Padding = new Vector2(10)
            };
            var chooseChannelPanel = new Panel(this, new SizeF(Settings.VWidth / 3f, Settings.VHeight))
            {
                Padding = new Vector2(10)
            };
            var inputDataPanel = new Panel(this, new SizeF(Settings.VWidth / 3f, Settings.VHeight))
            {
                Padding = new Vector2(10)
            }; 
            
            var backButton = new Button(new SizeF(135, 25), "Back")
            {
                OnButtonPressed = () =>
                {
                    if (_previousPanel != null)
                        _currentPanel = _previousPanel;
                }
            };
            
            mainLeftPanel.Add(
                new Label("Queuing System"),
                new Button(new SizeF(135, 25), "With queue")
                {
                    OnButtonPressed = () =>
                    {
                        _previousPanel = _currentPanel;
                        _currentPanel = withQueuePanel;
                    }
                },
                new Button(new SizeF(135, 25), "With rejects")
                {
                    OnButtonPressed = () =>
                    {
                        _previousPanel = _currentPanel;
                        _systemType = SystemType.WithRejects;
                        _currentPanel = chooseChannelPanel;
                    }
                },
                exitButton.Copy()
            );
            
            withQueuePanel.Add(
                new Label("Queuing System"),
                new Button(new SizeF(135, 25), "Limited queue")
                {
                    OnButtonPressed = () =>
                    {
                        _previousPanel = _currentPanel;
                        _systemType = SystemType.WithQueueLimited;
                        _currentPanel = chooseChannelPanel;
                    }
                },
                new Button(new SizeF(135, 25),"Unlimited queue")
                {
                    OnButtonPressed = () =>
                    {
                        _previousPanel = _currentPanel;
                        _systemType = SystemType.WithQueueUnlimited;
                        _currentPanel = chooseChannelPanel;
                    }
                },
                backButton.Copy(),
                exitButton.Copy()
            );

            chooseChannelPanel.Add(
                new Label("Queuing System"),
                new Button(new SizeF(135, 25), "One chanel")
                {
                    OnButtonPressed = () =>
                    {
                        switch (_systemType)
                        {
                            case SystemType.WithQueueLimited:
                                _systemType = SystemType.WithQueueLimitedOneChannel;
                                break;
                            case SystemType.WithQueueUnlimited:
                                _systemType = SystemType.WithQueueUnlimitedOneChannel;
                                break;
                            case SystemType.WithRejects:
                                _systemType = SystemType.WithRejectsOneChannel;
                                break;
                        }
                        
                        _previousPanel = mainLeftPanel;
                        _currentPanel = inputDataPanel;
                    }
                },
                new Button(new SizeF(135, 25),"Several chanel")
                {
                    OnButtonPressed = () =>
                    {
                        switch (_systemType)
                        {
                            case SystemType.WithQueueLimited:
                                _systemType = SystemType.WithQueueLimitedSeveralChannel;
                                break;
                            case SystemType.WithQueueUnlimited:
                                _systemType = SystemType.WithQueueUnlimitedSeveralChannel;
                                break;
                            case SystemType.WithRejects:
                                _systemType = SystemType.WithRejectsSeveralChannels;
                                break;
                        }

                        _previousPanel = mainLeftPanel;
                        _currentPanel = inputDataPanel;
                    }
                },
                backButton.Copy(),
                exitButton.Copy()
            );
            
            inputDataPanel.Add(
                new Label($"Queuing System:"),
                backButton.Copy(),
                exitButton.Copy()
            );

            _currentPanel = mainLeftPanel;
        }
        
        protected override void Update(float deltaTime)
        {
            Settings.Title = $"Queuing System: {GetFPS()}";
            _currentPanel.Update(deltaTime);
        }

        protected override void Draw()
        {
            _currentPanel.Draw();
        }
    }
}