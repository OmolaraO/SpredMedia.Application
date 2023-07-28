using System;
using System.ComponentModel;

namespace SpredMedia.UserManagement.Model.Enum
{
	public enum ResolutionType
	{
        [Description("480p")]
        StandardDefinition,
        [Description("720p")]
        HighDefinition,
        [Description("1080p")]
        FullHd,
        [Description("2k or 1440")]
        QuadHd,
        [Description("4k or 2160p")]
        UltraHd,
        [Description("8k or 4320p")]
        FullUltraHd
    }
}

