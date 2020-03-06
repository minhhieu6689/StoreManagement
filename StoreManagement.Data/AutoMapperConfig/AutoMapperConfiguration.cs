using AutoMapper;
using System;
using System.Collections.Generic;
using System.Text;

namespace StoreManagement.Data.AutoMapperConfig
{
    public static class AutoMapperConfiguration
    {
        public static void Config()
        {
            Mapper.Initialize(cfg => {
                cfg.AddProfile(new DtoEntityCommonMapper());
            });
        }
    }
}
