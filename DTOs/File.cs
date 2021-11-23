using Crud.Models;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Crud.DTOs
{
    public class FileDto
    {
    
        public record File
        {
            public string Data { get; set; }
            public string Extension { get; set; }
        }

    }
}
