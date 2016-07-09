﻿using Microsoft.Win32;
using System;
using System.Configuration;
using WeifenLuo.WinFormsUI.Docking.Configuration;

namespace WeifenLuo.WinFormsUI.Docking
{
    public static class PatchController
    {
        public static bool? EnableAll { get; set; }

        private static bool? _highDpi;

        public static bool EnableHighDpiSupport
        {
            get
            {
                if (_highDpi != null)
                {
                    return _highDpi.Value;
                }

                if (EnableAll != null)
                {
                    return (_highDpi = EnableAll).Value;
                }

                var section = ConfigurationManager.GetSection("dockPanelSuite") as PatchSection;
                if (section != null)
                {
                    if (section.EnableAll != null)
                    {
                        return (_highDpi = section.EnableAll).Value;
                    }

                    return (_highDpi = section.EnableHighDpi).Value;
                }

                var environment = Environment.GetEnvironmentVariable("DPS_EnableHighDpi");
                if (!string.IsNullOrEmpty(environment))
                {
                    var enable = false;
                    if (bool.TryParse(environment, out enable))
                    {
                        return (_highDpi = enable).Value;
                    }
                }

                {
                    var key = Registry.CurrentUser.OpenSubKey(@"Software\DockPanelSuite");
                    if (key != null)
                    {
                        var pair = key.GetValue("EnableHighDpi");
                        if (pair != null)
                        {
                            var enable = false;
                            if (bool.TryParse(pair.ToString(), out enable))
                            {
                                return (_highDpi = enable).Value;
                            }
                        }
                    }
                }

                {
                    var key = Registry.LocalMachine.OpenSubKey(@"Software\DockPanelSuite");
                    if (key != null)
                    {
                        var pair = key.GetValue("EnableHighDpi");
                        if (pair != null)
                        {
                            var enable = false;
                            if (bool.TryParse(pair.ToString(), out enable))
                            {
                                return (_highDpi = enable).Value;
                            }
                        }
                    }
                }

                return true;
            }

            set
            {
                _highDpi = value;
            }
        }
    }
}