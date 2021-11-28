using Audit.Core;
using Serilog;
using System;

namespace AuditLib
{
    public static class CustomAuditScope
    {
        public static void CommentAudit(this AuditScope scope, string message, params object[] args)
        {
            try
            {
                if (!string.IsNullOrEmpty(message))
                {
                    var commentMessage = $"{DateTime.Now:HH:mm:ss.fff}-{message.Replace("{", "{{").Replace("}", "}}")}";
                    if (args == null)
                        scope?.Comment(commentMessage);
                    else
                        scope?.Comment(commentMessage, args);
                }
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Audit Comment Exception");
            }
        }
    }
    
}
