﻿using MassTransit.EntityFrameworkCoreIntegration.Mappings;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;


namespace SagaStateMachineWorkerService.Models
{
    public class OrderStateMap : SagaClassMap<OrderStateInstance>
    {
        protected override void Configure(EntityTypeBuilder<OrderStateInstance> entity, ModelBuilder model)
        {
            entity.Property(x => x.BuyyerId).HasMaxLength(256);
            entity.Property(x => x.TotalPrice).HasColumnType("decimal(18,2)");
        }

    }
}
