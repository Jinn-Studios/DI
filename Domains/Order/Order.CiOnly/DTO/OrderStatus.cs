﻿using JinnDev.Order.Enums;

namespace JinnDev.OrderCI
{
    public static class OrderStatusDTO
    {
        public static OrderStatus CTM(this Order.Data.Enums.OrderStatus source) => OrderStatus.Calculated;
    }
}