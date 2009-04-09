using System;
using System.Collections.Generic;
using System.Text;

namespace HDGraph.Interop
{
    public abstract class ToolProviderBase
    {
        public static EnvironmentTarget GetEnvironmentType()
        {
            return EnvironmentTarget.WindowsXp; // TODO
        }

        private static ToolProviderBase current;

        public static ToolProviderBase Current
        {
            get
            {
                if (current == null)
                {
                    EnvironmentTarget env = GetEnvironmentType();
                    switch (env)
                    {
                        case EnvironmentTarget.WindowsXp:
                        case EnvironmentTarget.WindowsVista:
                            current = new Windows.WindowsToolProvider();
                            break;
                        case EnvironmentTarget.Linux:
                            throw new NotImplementedException();
                            break;
                        case EnvironmentTarget.Mac:
                            throw new NotImplementedException();
                            break;
                        case EnvironmentTarget.Unknown:
                            throw new NotImplementedException();
                            break;
                        default:
                            break;
                    }
                }
                return current;
            }
        }

        public abstract List<PathWithIcon> ListFavoritPath();
    }
}
