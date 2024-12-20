﻿using VizitConsole.Models;
using VizitConsole.Repositories;

namespace VizitConsole.Services
{
    internal class AppointmentService
    {
        private AppointmentRepository _repository;
        public string? Message { get; set; }
        public AppointmentService(string connectionString)
        {
            _repository = new AppointmentRepository(connectionString);
        }
        public async Task<List<(TimeSpan start, TimeSpan end, string status)>> GetBusyTime(int masterId)
        {
            List<Appointment> appointments = await _repository.GetAllAppointments(masterId);

            List<(TimeSpan start, TimeSpan end, string status)> busyTime =
                appointments.Select(a =>
                {
                    TimeSpan start = a.Date.TimeOfDay;
                    TimeSpan end = start + a.Duration;
                    string status = "Busy";
                    return (start, end, status);
                }).ToList();
            return busyTime;
        }
        public async Task MakeAppointment(Appointment appointment)
        {
            if (appointment == null || appointment.User_id <= 0)
            {
                throw new ArgumentException("Error AppointmentService, MakeAppointment: appointment must be appointment.User_id> 0 && appointment != null");
            }
            else
            {
                int masterId = appointment.Master_id;
                var busyTime = new List<(TimeSpan start, TimeSpan end, string status)>(await GetBusyTime(masterId));

                var newTimeStart = appointment.Date.TimeOfDay;
                var newTimeEnd = appointment.Date.TimeOfDay + appointment.Duration;

                bool timeOccupited = busyTime.Any(busy => newTimeStart < busy.end && newTimeEnd > busy.start);
                if (timeOccupited)
                {
                    Message = $"Appointment on: {appointment.Date.TimeOfDay} Can't be made: Time interval is busy ";
                }
                else
                {
                    await _repository.InsertAppointment(appointment);
                    Message = $"Appointment Date: {appointment.Date} is Created";
                }
            }
        }
        public async Task CancelAppointment(int id)
        {
            Appointment appointment = await _repository.GetAppointment(id);
            appointment.Cancellation = true;
            appointment.Duration = new TimeSpan(0, 0, 0);

            await _repository.UpdateAppointment(appointment);
        }
        public async Task CancelAppointment(DateTime date, Master master, User user) 
        {
            Appointment appointment = await _repository.GetAppointment(date, master, user);
            if (appointment != null)
            {
                appointment.Cancellation = true;
                appointment.Duration = new TimeSpan(0, 0, 0);

                await _repository.UpdateAppointment(appointment);
            }
            else 
            {
                throw new KeyNotFoundException("Appointment not found.");
            }
        }
    }
}
