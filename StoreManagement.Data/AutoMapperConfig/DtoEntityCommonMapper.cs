using AutoMapper;
using StoreManagement.Data.DTO;
using StoreManagement.Data.Model;
using StoreManagement.Data.ViewModel.Authenticate;
using System;
using System.Collections.Generic;
using System.Text;

namespace StoreManagement.Data.AutoMapperConfig
{
    public class DtoEntityCommonMapper : Profile
    {
        public DtoEntityCommonMapper()
        {
            #region Enity To Dto

            CreateMap<User, UserDto>();

            #endregion

            #region Dto to Entity

            CreateMap<UserDto, User>();

            #endregion

            CreateMap<UserDto, LoginReturnViewModel>();

            CreateMap<User, LoginReturnViewModel>();
        }
    }
}
