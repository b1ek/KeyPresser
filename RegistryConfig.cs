using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Text;

namespace KeyPresser
{
    static class RegistryConfig
    {
        //static RegistryKey root = Registry.CurrentUser.OpenSubKey(@"\SOFTWARE\blek\KeyPresser", true);
        const string rootPath = @"SOFTWARE\blek\KeyPresser";

        static public RegistryKey root
        {
            get
            {
                RegistryKey key = Registry.CurrentUser.OpenSubKey(rootPath, true);
                if (key == null)
                {
                    return Registry.CurrentUser.CreateSubKey(rootPath, true);
                }
                return key;
            }
        }

        static public int x1
        {
            get
            {
                int val = (int) root.GetValue("x1", 0);
                if (val == null) { root.SetValue("x1", 0);return 0; }
                return val;
            }
            set
            {
                root.SetValue("x1", value);
            }
        }
        static public int y1
        {
            get
            {
                int val = (int)root.GetValue("y1", 0);
                if (val == null) { root.SetValue("y1", 0); return 0; }
                return val;
            }
            set
            {
                root.SetValue("y1", value);
            }
        }
        static public int x2
        {
            get
            {
                int val = (int)root.GetValue("x2", 0);
                if (val == null) { root.SetValue("x2", 0); return 0; }
                return val;
            }
            set
            {
                root.SetValue("x2", value);
            }
        }
        static public int y2
        {
            get
            {
                int val = (int) root.GetValue("y2", 0);
                if (val == null) { root.SetValue("y2", 0); return 0; }
                return val;
            }
            set
            {
                root.SetValue("y2", value);
            }
        }
    }
}
