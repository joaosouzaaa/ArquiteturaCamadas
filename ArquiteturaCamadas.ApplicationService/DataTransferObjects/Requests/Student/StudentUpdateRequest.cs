﻿using ArquiteturaCamadas.ApplicationService.DataTransferObjects.Enums;
using ArquiteturaCamadas.ApplicationService.DataTransferObjects.Requests.Person;

namespace ArquiteturaCamadas.ApplicationService.DataTransferObjects.Requests.Student
{
    public sealed class StudentUpdateRequest : PersonUpdateRequest
    {
        public ESchoolDivisionRequest SchoolDivision { get; set; }
    }
}
