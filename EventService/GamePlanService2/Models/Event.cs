﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace GamePlanService2.Models
{
    public class Event
    {
        [Key]
        public int Id { get; set; }
        public string Description { get; set; }
        public double Lat { get; set; }
        public double Lng { get; set; }
        public bool EmailNotification { get; set; }
        public string Invite { get; set; }
        public double Temp { get; set; }
        public bool Complete { get; set; }

        [DataType(DataType.Date)]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:yyyy-MM-dd}")]
        public DateTime? Date { get; set; }

        public int ToDoListId { get; set; }
        [ForeignKey("ToDoListId")]
        public ToDoList ToDoList { get; set; }
    }
}