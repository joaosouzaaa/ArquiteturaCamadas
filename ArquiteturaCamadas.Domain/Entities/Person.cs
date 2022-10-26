﻿using ArquiteturaCamadas.Domain.Entities.EntityBase;
using ArquiteturaCamadas.Domain.Enums;
using System.Net.Sockets;

namespace ArquiteturaCamadas.Domain.Entities
{
    public class Person : BaseEntity
    {
        public string Name { get; set; }
        public int Age { get; set; }
        public EGender Gender { get; set; }
    }
}