using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace TaskManagerMVC.Models
{
    public class Task
    {
        
        public int TaskId { get; set; }
        

        public string Description { get; set; }
        public bool? CompletionStatus { get; set; }
        public DateTime DueDate { get; set; }
    }
}
