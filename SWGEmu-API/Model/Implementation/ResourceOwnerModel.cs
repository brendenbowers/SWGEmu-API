using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Dynamic;
using Dapper;
using ServiceStack.Text;
using OAuth2.DataModels;

namespace OAuth2.Server.Model
{
    public class ResourceOwnerModel : OAuth2.Server.Model.IResourceOwnerModel
    {
        public ServiceStack.OrmLite.IDbConnectionFactory DBFactory { get; set; } //injected by IOC
        private IDbConnection _DB = null;

        protected IDbConnection Db
        {
            get
            {
                return this._DB ?? (this._DB = ServiceStack.OrmLite.OrmLiteConnectionFactoryExtensions.Open(DBFactory));
            }
        }

        public ResourceOwner GetByID(string ID)
        {
            IEnumerable < dynamic > res = Db.Query("SELECT * FROM ResourceOwner WHERE id = @id", new { id = ID });
            object obj = res.FirstOrDefault();
            if (obj != null)
            {
                IDictionary<string, object> result = (IDictionary<string, object>)obj;
                ResourceOwner owner = new ResourceOwner();
                owner.id = (string)result["id"];
                owner.time = Convert.ToInt64(result["time"]);
                //owner.attributes = ServiceStack.Text.JsonSerializer.DeserializeFromString<Dictionary<string, object[]>>((string)result["attributes"]);
                owner.attributes = ServiceStack.Text.JsonSerializer.DeserializeFromString<Dictionary<string, object[]>>((string)result["attributes"]);
                return owner;
            }

            return null;
        }

        public bool Update(ResourceOwner Owner)
        {
            if (Owner == null || string.IsNullOrWhiteSpace(Owner.id))
            {
                throw new ArgumentException("ResourceOwner Id is invalid", "ToUpdate");
            }

            int res = Db.Execute("UPDATE ResourceOwner SET time = @time, attributes = @attributes WHERE id = @id", new { id = Owner.id, time = DateTime.UtcNow.Millisecond, attributes = Owner.attributes.ToJson() });
            return res == 1;
        }

        public bool Create(ResourceOwner Owner)
        {
            if (Owner == null || string.IsNullOrWhiteSpace(Owner.id))
            {
                throw new ArgumentException("ResourceOwner Id is invalid", "ToUpdate");
            }
            
            int res = Db.Execute("INSERT INTO ResourceOwner (id, time, attributes) VALUES (@id, @time, @attributes)", new { id = Owner.id, time = Owner.time, attributes = Owner.attributes.ToJson() });

            return res == 1;
        }


        public bool CreateOrUpdate(ResourceOwner Owner)
        {
            if (Update(Owner))
                return true;
            return Create(Owner);
        }


        public List<ResourceOwner> GetByIDs(IEnumerable<string> IDs)
        {
            IEnumerable<dynamic> res = Db.Query("SELECT * FROM ResourceOwner WHERE id IN @id", new { id = IDs });
            List<ResourceOwner> owners = new List<ResourceOwner>();
            foreach (object owner in res)
            {
                IDictionary<string, object> result = (IDictionary<string, object>)owner;
                owners.Add(new ResourceOwner
                    {
                        id = (string)result["id"],
                        time = Convert.ToInt64(result["time"]),
                        attributes = ServiceStack.Text.JsonSerializer.DeserializeFromString<Dictionary<string, object[]>>((string)result["attributes"]),
                    });
            }
            return owners;
        }
    }
}