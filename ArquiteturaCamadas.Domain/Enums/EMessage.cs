using System.ComponentModel;

namespace ArquiteturaCamadas.Domain.Enums
{
    public enum EMessage
    {
        [Description("{0} need to be filled")]
        Required,

        [Description("Field {0} allows {1} chars")]
        MoreExpected,

        [Description("{0} not found")]
        NotFound,

        [Description("An unexpected error happened")]
        UnexpectedError,

        [Description("Format invalid for image")]
        InvalidImageFormat,

        [Description("{0} has to be greater than {1}")]
        GreaterThan
    }
}
