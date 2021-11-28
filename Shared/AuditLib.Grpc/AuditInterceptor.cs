using Audit.Core;
using DTO;
using Grpc.Core;
using Grpc.Core.Interceptors;
using Microsoft.Extensions.Configuration;
using MongoDB.Bson;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text.Json;
using System.Threading.Tasks;

namespace AuditLib.Grpc
{
    public class AuditInterceptor : Interceptor
    {
        private const string AuditScopeKey = "__private_AuditScope__";
        private readonly ConfigAuditLog _configAudit;
        public AuditInterceptor(IConfiguration configuration)
        {
            _configAudit = configuration.GetSection(ConfigAuditLog.AuditLog).Get<ConfigAuditLog>();
        }

        public override async Task<TResponse> UnaryServerHandler<TRequest, TResponse>(TRequest request, ServerCallContext context, UnaryServerMethod<TRequest, TResponse> continuation)
        {
            var traceId = context.RequestHeaders.FirstOrDefault(h => h.Key.Equals("TraceId", StringComparison.OrdinalIgnoreCase))?.Value;
            var clientIp = context.RequestHeaders.FirstOrDefault(h => h.Key.Equals("ClientIp", StringComparison.OrdinalIgnoreCase))?.Value;
            var httpContext = context.GetHttpContext();
            var objectExtraField = new
            {
                Request = JsonSerializer.Serialize(request),
                Headers = new
                {
                    TraceId = traceId,
                    ClientIp = clientIp
                }
            };
            bool isAudit = _configAudit.ServiceUnAudit == null || !_configAudit.ServiceUnAudit.Any(c => context.Method.Contains(c));
            using ManualAuditScope scopeInstance = new(context.Method, objectExtraField, isAudit);
            if (isAudit)
            {
                httpContext.Items[AuditScopeKey] = scopeInstance.auditScope;
                scopeInstance.auditScope.Event.Environment.CallingMethodName = context.Method;
            }
            try
            {
                var response = await base.UnaryServerHandler(request, context, continuation);
                if (scopeInstance != null && scopeInstance.auditScope != null)
                {
                    var data = response.ToBsonDocument();
                    var dotNetObj = BsonTypeMapper.MapToDotNetValue(data);
                    scopeInstance.auditScope.SetCustomField("Response", JsonSerializer.Serialize(dotNetObj));
                    //_auditLog.SetAuditEntityInfo();
                    var listEntityAudit = httpContext.Items.Where(c => c.Key.ToString().StartsWith("AuditEntityInfo_")).ToList();
                    if (listEntityAudit != null && listEntityAudit.Count > 0)
                    {
                        var _ListEntityInfo = new List<object>();
                        foreach (var entity in listEntityAudit)
                        {
                            _ListEntityInfo.Add(entity.Value);
                        }
                        if (_ListEntityInfo.Count > 0)
                        {
                            scopeInstance.auditScope.SetCustomField("EntityInfo", _ListEntityInfo);
                        }
                    }
                }
                return response;
            }
            catch (SqlException e)
            {
                scopeInstance?.auditScope.CommentAudit($"An SQL error occured when calling {context.Method}. Exception:{e}");
                Status status;

                if (e.Number == -2)
                {
                    status = new Status(StatusCode.DeadlineExceeded, "SQL timeout");
                }
                else
                {
                    status = new Status(StatusCode.Internal, "SQL error");
                }
                throw new RpcException(status);
            }
            catch (RpcException ex)
            {
                scopeInstance?.auditScope.CommentAudit($"An error occured when calling {context.Method}. RpcException: {ex.Message}");
                throw;
            }
            catch (Exception e)
            {
                scopeInstance?.auditScope.CommentAudit($"An error occured when calling {context.Method}. Exception:{e.Message}");
                throw new RpcException(Status.DefaultCancelled, e.Message);
            }
            finally
            {
                if (scopeInstance != null && scopeInstance.auditScope != null)
                {
                    try
                    {
                        _ = scopeInstance.End();
                    }
                    catch (Exception ex)
                    {
                        scopeInstance?.auditScope.CommentAudit($"scopeInstance SaveTo MongoDB Exception > {ex.Message}");
                    }
                }
            }
        }
    }
    public class ManualAuditScope : IDisposable
    {
        private IntPtr pointer;
        private bool disposed = false;
        public AuditScope auditScope;
        private readonly string EventType;
        public ManualAuditScope(string eventType, object ExtraField, bool isAudit = false)
        {
            if (isAudit)
            {
                EventType = eventType;
                auditScope = AuditScope.Create(new AuditScopeOptions
                {
                    EventType = EventType,
                    ExtraFields = ExtraField,
                    CreationPolicy = EventCreationPolicy.Manual,
                    //AuditEvent = new AuditEntityChangeInfo()
                });
            }
            pointer = Marshal.AllocHGlobal(1024);
        }

        public async Task End()
        {
            // Save the event
            await auditScope.SaveAsync();
            auditScope.Discard();
        }
        public void Dispose()
        {
            if (disposed)
                return;
            Marshal.FreeHGlobal(pointer);
            pointer = IntPtr.Zero;
            GC.SuppressFinalize(this);
            disposed = true;
        }
        ~ManualAuditScope()
        {
            Dispose();
        }
    }

    //public class AuditEntityChangeInfo : AuditEvent
    //{
    //    [JsonProperty(NullValueHandling = NullValueHandling.Ignore, Order = 10)]
    //    public List<EntityFrameworkEventExtend> EntityChangeInfo { get; set; }
    //}

}
