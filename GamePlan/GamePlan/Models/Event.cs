﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace GamePlan.Models
{
    public class Event
    {
        [Key]
        public int Id { get; set; }
        public string Description { get; set; }
        public double Lat { get; set; }
        public double Lng { get; set; }

        [DataType(DataType.Date)]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:yyyy-MM-dd}")]
        public DateTime? Date { get; set; }


        //[ForeignKey("ToDoList")]
        //public int ToDoListId { get; set; }
        //public virtual ToDoList ToDoList { get; set; }
    }
}