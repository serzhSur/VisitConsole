﻿
namespace ConsoleSchedule.Models
{
    internal class  Master
    {
        public int Id { get; set; }
        public string Name { get; set; } = "undefined";
        public string Speciality { get; set; }
        public TimeSpan Day_interval { get; set; } 
        public TimeSpan Start_Day_Time { get; set; }=new TimeSpan(8,00,0);    
        public TimeSpan End_Day_Time { get; set; }=new TimeSpan(22,00,0);

        public Master() { }
        public Master(string name,string speciality, TimeSpan dayInterval, TimeSpan startDayTime, TimeSpan endDayTime) 
        { 
            Name = name;
            Speciality = speciality;
            Day_interval = dayInterval;
            Start_Day_Time = startDayTime;
            End_Day_Time = endDayTime;
        }
    }
}
