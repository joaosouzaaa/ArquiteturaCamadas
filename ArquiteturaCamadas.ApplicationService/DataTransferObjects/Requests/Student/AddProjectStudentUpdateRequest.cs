namespace ArquiteturaCamadas.ApplicationService.DataTransferObjects.Requests.Student
{
    public sealed class ProjectStudentRelationShipArgument
    {
        public required int StudentId { get; set; }
        public required int ProjectId { get; set; }
    }
}
