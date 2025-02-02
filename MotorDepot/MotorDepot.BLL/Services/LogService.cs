﻿using MotorDepot.BLL.BusinessModels;
using MotorDepot.BLL.Infrastructure;
using MotorDepot.BLL.Infrastructure.Mappers;
using MotorDepot.BLL.Interfaces;
using MotorDepot.BLL.Models;
using MotorDepot.DAL.Interfaces;
using MotorDepot.Shared.Enums;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using MotorDepot.Shared;

namespace MotorDepot.BLL.Services
{
    public class LogService : ILoggerService
    {
        private readonly IUnitOfWork _database;

        public LogService(IUnitOfWork unitOfWork)
        {
            _database = unitOfWork;
        }

        public void Log(LogEventDto logEvent)
        {
            _database.Logger.Log(logEvent.ToEntity());
        }

        public IEnumerable<LogEventDto> GetLogs(LogType logType)
        {
            return _database.Logger.GetLogs()
                .Where(x => x.LogTypeLookupId == logType)
                .ToList()
                .ToDto();
        }

        public OperationStatus<LogEventDto> GetLogById(int? id)
        {
            if (id == null)
                throw new ArgumentNullException(nameof(id));

            var log = _database.Logger.GetLog(id);

            if (log == null)
                return new OperationStatus<LogEventDto>("Log doesn't exist", HttpStatusCode.NotFound, false);

            return new OperationStatus<LogEventDto>("Ok", log.ToDto(), true);
        }

        public IEnumerable GetLogTypes()
        {
            var parser = new EnumParser<LogType>().Parse();

            return parser;
        }
    }
}
