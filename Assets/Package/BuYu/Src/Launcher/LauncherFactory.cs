using UnityEngine;
using System.Collections.Generic;

namespace BuYu
{
    public class LauncherFactory
    {
        public static Launcher CreateLauncher(byte launcherType, bool valid, byte seat, byte rateIndx)
        {
            return new Launcher(launcherType, valid, seat, rateIndx);
        }
    }
}
