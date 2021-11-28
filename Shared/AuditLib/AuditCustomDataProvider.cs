using Audit.Core;
using Audit.Core.Providers;
using Audit.MongoDB.Providers;
using DTO;
using Serilog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace AuditLib
{
    public class AuditCustomDataProvider : AuditDataProvider
    {
        private readonly MongoDataProvider _mongoDataProvider = new();
        private readonly FileDataProvider _fileDataProvider = new();
        private bool _mongoAlive;
        private Timer _timerCheckConnection;
        public AuditCustomDataProvider(ConfigAuditLog configAuditLog)
        {
            _mongoDataProvider.ConnectionString = configAuditLog.ConnectionString;
            _mongoDataProvider.Database = configAuditLog.Database;
            _mongoDataProvider.Collection = configAuditLog.Collection;
            CheckConnection();
            _timerCheckConnection = new Timer(CheckConnection, null, TimeSpan.FromMinutes(1), TimeSpan.FromMinutes(1));

            static string funcDirectory(AuditEvent a)
            {
                return $"{AppContext.BaseDirectory}/AuditLog/{DateTime.Now:ddMMyyyy}";
            }
            _fileDataProvider.DirectoryPathBuilder = funcDirectory;
        }

        private void CheckConnection(object state = null)
        {
            try
            {
                _mongoDataProvider.TestConnection();
                _mongoAlive = true;
            }
            catch (Exception ex)
            {
                Log.Error(ex.Message);
                _mongoAlive = false;
            }
        }

        // async implementation:
        public override async Task<object> InsertEventAsync(AuditEvent auditEvent)
        {
            try
            {
                if (_mongoAlive)
                    return await _mongoDataProvider.InsertEventAsync(auditEvent);
                else
                    return await _fileDataProvider.InsertEventAsync(auditEvent);
            }
            catch (Exception ex)
            {
                Log.Error(ex.Message);
                return await _fileDataProvider.InsertEventAsync(auditEvent);
                //return null;
            }
        }

        public override object InsertEvent(AuditEvent auditEvent)
        {
            try
            {
                if (_mongoAlive)
                    return _mongoDataProvider.InsertEvent(auditEvent);
                else
                    return _fileDataProvider.InsertEvent(auditEvent);
            }
            catch (Exception ex)
            {
                Log.Error(ex.Message);
                return _fileDataProvider.InsertEvent(auditEvent);
            }
        }
        public override void ReplaceEvent(object eventId, AuditEvent auditEvent)
        {
            _mongoDataProvider.ReplaceEvent(eventId, auditEvent);
        }

        private void SerializeExtraFields(AuditEvent auditEvent)
        {
            foreach (var k in auditEvent.CustomFields.Keys.ToList())
            {
                auditEvent.CustomFields[k] = Serialize(auditEvent.CustomFields[k]);
            }
        }
        public override object Serialize<T>(T value)
        {
            return _mongoDataProvider.Serialize<T>(value);
        }
        #region Events Query        
        /// <summary>
        /// Returns an IQueryable that enables querying against the audit events stored on Azure Document DB.
        /// </summary>
        public IQueryable<AuditEvent> QueryEvents()
        {
            return _mongoDataProvider.QueryEvents();
        }
        /// <summary>
        /// Returns an IQueryable that enables querying against the audit events stored on Azure Document DB, for the audit event type given.
        /// </summary>
        /// <typeparam name="T">The AuditEvent type</typeparam>
        public IQueryable<T> QueryEvents<T>() where T : AuditEvent
        {
            return _mongoDataProvider.QueryEvents<T>();
        }
        #endregion
    }


}
