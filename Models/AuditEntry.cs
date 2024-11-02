using Microsoft.EntityFrameworkCore.ChangeTracking;
using Newtonsoft.Json;

namespace EmployeesManagement.Models
{
    public class AuditEntry
    {
        public EntityEntry Entry { get; set; }
        public string UserId { get; set; }
        public string TableName { get; set; }
        public Dictionary<string, object> KeyValues { get; set; } = new Dictionary<string, object>();
        public Dictionary<string, object> OldValues { get; set; } = new Dictionary<string, object>();
        public Dictionary<string, object> NewValues { get; set; } = new Dictionary<string, object>();
        public AuditType AuditType { get; set; }
        public List<string> ChangedColumns = new List<string>();
        
        public AuditEntry(EntityEntry entityEntry)
        {
            Entry = entityEntry;
        }
        public Audit ToAudit()
        {
            var audit = new Audit();
            audit.UserId = UserId;
            audit.AuditType = AuditType.ToString();
            audit.TableName = TableName;
            audit.DateTime = DateTime.Now;
            audit.PrimaryKey = JsonConvert.SerializeObject(KeyValues);
            audit.OldValues = OldValues.Count != 0 ? JsonConvert.SerializeObject(OldValues) : null;
            audit.NewValues = NewValues.Count != 0 ? JsonConvert.SerializeObject(NewValues) : null;
            audit.AffectedCoulmns = ChangedColumns.Count != 0 ? JsonConvert.SerializeObject(ChangedColumns) : null;
            return audit;
        }
    }
}
