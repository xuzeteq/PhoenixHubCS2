using backend.Application.Dtos.Feature;
using backend.Domain.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace backend.Application.Mappings
{
    public static class FeatureMapping
    {
        public static FeatureResponseDto ToDto(this Feature feature)
        {
            return new FeatureResponseDto
            {
                Id = feature.Id,
                Title = feature.Title,
                Description = feature.Description,
                Icon = feature.Icon,
                CreatedAt = feature.CreatedAt
            };
        }
    }
}
