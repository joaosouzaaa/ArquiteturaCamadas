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

        [Description("{0} age has to be greater than 18 years")]
        InvalidAge,

        [Description("An unexpected error happened")]
        UnexpectedError,

        [Description("Invalid Credencials")]
        InvalidCredencials,

        [Description("Your transaction failed with status code {0}")]
        FailedTransaction,

        [Description("Format invalid for image")]
        InvalidImageFormat
    }
}
