using Audit.Core;
using Microsoft.AspNetCore.Http;
using System;
using System.Runtime.InteropServices;

namespace AuditLib.Grpc
{
    public class AuditCore : IDisposable
    {
        private IntPtr pointer;
        private bool disposed = false;
        private readonly IHttpContextAccessor _context;
        public AuditCore(IHttpContextAccessor context)
        {
            _context = context;
            pointer = Marshal.AllocHGlobal(1024);
        }
        private AuditScope instance = null;
        private readonly object padlock = new();
        private const string AuditScopeKey = "__private_AuditScope__";
        public AuditScope CurrentScope
        {
            get
            {
                if (instance == null)
                {
                    lock (padlock)
                    {
                        if (instance == null)
                        {
                            instance = _context.HttpContext?.Items[AuditScopeKey] as AuditScope;
                        }
                    }
                }
                return instance;
            }
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
        ~AuditCore()
        {
            Dispose();
        }
    }
}
