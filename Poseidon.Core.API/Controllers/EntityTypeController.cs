using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Query;
using Microsoft.AspNetCore.OData.Routing.Controllers;

namespace Poseidon.Core.API.Controllers;

public class EntityTypeController : ODataController
{
    private Entities.EntityType[] entityTypes = new[]{
        new Entities.EntityType {
            Name = "EntityType",
            Properties = new List<Entities.EntityProperty>{
                new Entities.EntityProperty{
                    Name = "Name",
                    Type = Entities.PropetyType.Text
                }
            }
        }
    };


    [EnableQuery]
    public IActionResult Get()
    {
        return Ok(this.entityTypes.AsQueryable());
    }

    [EnableQuery]
    public IActionResult Get(Guid key)
    {
        return Ok(this.entityTypes.FirstOrDefault(e => e.Id == key));
    }

    [EnableQuery]
    public IActionResult GetName(Guid key)
    {
        return Ok(this.entityTypes.FirstOrDefault(e => e.Id == key)?.Name);
    }
}