using MassTransit.EntityFrameworkCoreIntegration;
using MassTransit.EntityFrameworkCoreIntegration.Mappings;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;

namespace SagaStateMachineWorkerService.Models
{
    public class OrderStateDbContext : SagaDbContext
    {
        public OrderStateDbContext(DbContextOptions options) : base(options)
        {
        }

        // OderStateInstance tablosunun alanlarına kural koyabilmemiz için Configuration tarafında OrderStateMap sınıfını oluşturduk
        protected override IEnumerable<ISagaClassMap> Configurations { get { yield return new OrderStateMap(); } }
    }
}
