using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace GamePlanService2.Models
{
    public class ToDoList
    {
        [Key]
        public int Id { get; set; }
        public string Title { get; set; }
        public List<Event> Events { get; set; }
    }
}