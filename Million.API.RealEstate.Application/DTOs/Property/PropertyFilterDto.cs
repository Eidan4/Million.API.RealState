﻿namespace Million.API.RealEstate.Application.DTOs.Property
{
    public class PropertyFilterDto
    {
        public string? Name { get; set; }
        public string? Address { get; set; }
        public decimal? MinPrice { get; set; }
        public decimal? MaxPrice { get; set; }
    }
}