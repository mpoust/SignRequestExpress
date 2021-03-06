﻿////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
/*
 * CIT498 - Senior Project - Fall 2018
 * 
 * FileName: MappingProfile.cs
 * Author: Michael Poust
		   mbp3@pct.edu
 * Created On: 9/16/2018
 * Last Modified: 9/20/2018
 * Description: This class inherits from AutoMapper and defines how database entity objects are mapped to their corresponding resource objects.
 *  This boilerplate mapping of all resource objects is reduced through the use of this AutoMapper package.
 * 
 * References:
 *   
 * (c) Michael Poust, 2018
 */
////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

using AutoMapper;
using SignRequestExpressAPI.Entities;
using SignRequestExpressAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SignRequestExpressAPI.Infrastructure
{
    public class MappingProfile : Profile
    {
        // In constructor we define which properties get mapped over between entity objects and resource objects
        public MappingProfile()
        {
            Mapper.Initialize(cfg => {
                cfg.CreateMap<AccountEntity, Account>().ForMember(dest => dest.Self, opt => opt.MapFrom(src =>
                    Link.To(nameof(Controllers.AccountsController.GetAccountByIdAsync), new { accountId = src.Id })));

                cfg.CreateMap<AccountContactEntity, AccountContact>().ForMember(dest => dest.Self, opt => opt.MapFrom(src =>
                    Link.To(nameof(Controllers.AccountContactsController.GetAccountContactByIdAsync), new { accountContactId = src.Id })));

                cfg.CreateMap<TemplateEntity, Template>().ForMember(dest => dest.Self, opt => opt.MapFrom(src =>
                    Link.To(nameof(Controllers.TemplatesController.GetTemplateByIdAsync), new { templateId = src.Id })));
                /*
                cfg.CreateMap<UserEntity, User>().ForMember(dest => dest.Self, opt => opt.MapFrom(src =>
                    Link.To(nameof(Controllers.UsersController.GetUserByIdAsync), new { userId = src.Id })));
                */

                /*
                cfg.CreateMap<UserEntity, User>().ForMember(dest => dest.Self, opt => opt.MapFrom(src =>
                   Link.To(nameof(Controllers.UsersController.GetUserById),
                   new { userId = src.Id })));
                */

                cfg.CreateMap<BrandEntity, Brand>().ForMember(dest => dest.Self, opt => opt.MapFrom(src =>
                    Link.To(nameof(Controllers.BrandsController.GetBrandByIdAsync), new { brandId = src.Id })));

                cfg.CreateMap<BrandStandardsEntity, BrandStandards>().ForMember(dest => dest.Self, opt => opt.MapFrom(src =>
                    Link.To(nameof(Controllers.BrandStandardsController.GetBrandStandardbyIdAsync), new { brandStandardId = src.Id })));


                
                /* // Commenting this out makes the return of request not return the form fields underneath each and every request
                cfg.CreateMap<RequestEntity, Request>()
                    .ForMember(dest => dest.Self, opt => opt.MapFrom(src => 
                        Link.To(nameof(Controllers.RequestsController.GetRequestByIdAsync), new { requestId = src.Id })))
                    .ForMember(dest => dest.Submit, opt => opt.MapFrom(src =>
                        FormMetadata.FromModel(
                            new RequestForm
                            {
                                NeededDate = src.NeededDate
                            },
                            Link.ToForm(
                                nameof(Controllers.RequestsController.SubmitRequestAsync),
                                new { requestId = src.Id },
                                Link.PostMethod,
                                Form.CreateRelation))));
                */

                cfg.CreateMap<RequestEntity, Request>()
                    .ForMember(dest => dest.Self, opt => opt.MapFrom(src =>
                        Link.To(nameof(Controllers.RequestsController.GetRequestByIdAsync), new { requestId = src.Id })));
                    

            });

            CreateMap<UserEntity, User>()
                .ForMember(dest => dest.Self, opt => opt.MapFrom(src =>
                    Link.To(nameof(Controllers.UsersController.GetUserById),
                    new { userId = src.Id })));

        }
    }
}
