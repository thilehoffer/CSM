using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using CaseloadManager.Data;
using System.Web.Mvc;

namespace CaseloadManager.Models
{
    public class EventListItem
    {
        public Boolean IsComplete {get; set;} 
        public DateTime ScheduledOn { get; set; }
        public int StudentId { get; set; }
        public string OccursIn
        {
            get
            {
                var diff = ScheduledOn - DateTime.Now;
                return diff.Days + " days";
            }
        }
        public string ScheduledOnString
        {
            get 
            {
                return ScheduledOn.ToShortDateString() + " " + ScheduledOn.ToShortTimeString();
            }
        }
        public string StudentName { get; set; }
        public string EventType { get; set; }
        public string Complete { get { return this.IsComplete ? "Yes" : "No"; } }

    }

    public enum EventType
    {
        Evaluation,
        IEP
    }
}